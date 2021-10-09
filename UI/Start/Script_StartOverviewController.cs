using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Playables;

/// <summary>
/// Main controller for Start Screen UI States
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_StartOverviewController : Script_UIState
{
    [SerializeField] private SavedGameState savedGameState;
    [SerializeField] private Script_EventSystemLastSelected savedGameEventSystem;
    
    [SerializeField] private Script_IntroController introController;
    [SerializeField] private Script_StartScreenController startScreenController;
    
    [SerializeField] private CanvasGroup introCanvasGroup;
    [SerializeField] private CanvasGroup savedGameCanvasGroup;
    
    [SerializeField] private CanvasGroup startScreenCanvasGroup;
    [SerializeField] private EventSystem startScreenEventSystem;
    
    [SerializeField] private Script_CanvasGroupController startOptionsCanvasGroup;
    [SerializeField] private Script_CanvasGroupController startScreenTitle;
    [SerializeField] private Script_CanvasGroupController startScreenCTA;
    [SerializeField] private Script_CanvasGroupController settingsCanvasGroup;
    [SerializeField] private Script_CanvasGroupController controlsCanvasGroup;
    [SerializeField] private Button[] settingsButtons;
    
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    
    [SerializeField] private Script_SavedGameViewController savedGameController;
    [SerializeField] private Script_SavedGameSubmenuController submenuController;
    [SerializeField] private Transform continueSubmenu;
    [SerializeField] private Transform newGameSubmenu;
    [SerializeField] private Transform deleteGameSubmenu;
    [SerializeField] private Transform pasteGameSubmenu;
    [SerializeField] private Script_SavedGameSubmenuInputChoice[] continueChoices;
    [SerializeField] private Script_SavedGameSubmenuInputChoice[] newGameChoices;
    [SerializeField] private Script_SavedGameSubmenuInputChoice[] deleteGameChoices;
    [SerializeField] private Script_SavedGameSubmenuInputChoice[] pasteGameChoices;
    [SerializeField] private Transform deleteBanner;
    [SerializeField] private Transform copyBanner;
    [SerializeField] private Transform pasteBanner;
    [SerializeField] private Button[] fileActionButtons;
    [SerializeField] private Button copyBtn;
    [SerializeField] private Script_InventoryAudioSettings audioSettings;
    [SerializeField] PlayableDirector crunchDirector;
    [SerializeField] float crunchTimeDown; /// NOTE: ENSURE THIS MATCHES TIMELINE TIME FOR CRUNCH DOWN
    [SerializeField] private Script_DeathByScreen[] deathByScreens;
    [SerializeField] private Script_BackgroundMusicManager bgmManager;
    [SerializeField] private float playGameOverBGWaitTime;
    private Script_DeathByScreen activeDeathByScreen;
    private Script_SavedGameSubmenuInputChoice[] choices;
     
    private int copiedSlotId;
    
    void OnEnable()
    {
        Script_StartEventsManager.OnExitSubmenu     += ActivateViewState;
        Script_StartEventsManager.OnExitFileActions += DeactivateViewState;

        crunchDirector.stopped += OnCrunchPlayableDone;
    }

    void OnDisable()
    {
        Script_StartEventsManager.OnExitSubmenu     -= ActivateViewState;
        Script_StartEventsManager.OnExitFileActions -= DeactivateViewState;

        crunchDirector.stopped -= OnCrunchPlayableDone;
    }

    // Starts Intro Sequence.
    // Intro Timeline, Full Playthrough > StartScreenStart() via Signal > ActivateStartScreenController() via Signal
    // Intro Timeline, Skip Intro > StartScreenStart() via Signal > ActivateStartScreenController() via Signal
    // From Back, Skip Intro > Manually call StartScreenStart > ActivateStartScreenController()
    public void InitializeIntro(bool isSkip = false)
    {
        startScreenCanvasGroup.gameObject.SetActive(false);
        savedGameCanvasGroup.gameObject.SetActive(false);
        gameOverCanvasGroup.gameObject.SetActive(false);
        
        settingsCanvasGroup.Close();
        controlsCanvasGroup.Close();
        
        savedGameEventSystem.InitializeState();

        introController.gameObject.SetActive(true);
        startScreenController.gameObject.SetActive(false);
        
        introCanvasGroup.gameObject.SetActive(true);
        introController.Play();
        
        if (isSkip)
        {
            introController.SkipToStartScreen();

            // Must manually call functions Timeline calls with Marker,
            // since won't fire marker upon Back button press.
            StartScreenStart();
        }
    }

    // Will be called whenever a confirm key is pressed on Start Options (via Start Screen Controller).
    public void StartOptionsOpen(bool isOpen)
    {
        if (isOpen)
        {
            startOptionsCanvasGroup.Open();
            startScreenCTA.Close();

            Debug.Log("Closing settings and controls canvases");
            settingsCanvasGroup.Close();
            controlsCanvasGroup.Close();
        }
        else
        {
            startOptionsCanvasGroup.Close();
            startScreenCTA.Open();
        }
    }

    // ----------------------------------------------------------------------
    // Timeline Signals
    
    // Signals the Start screen is up. Start Screen Controller should be disabled here.
    // Called from timeline as the starting point upon skipping intro.
    // Start options should be hidden until player presses Start command.
    public void StartScreenStart()
    {
        introController.DisableInput();
        
        startScreenCanvasGroup.gameObject.SetActive(true);
        startScreenEventSystem.gameObject.SetActive(true);
        StartOptionsOpen(false);
        
        introCanvasGroup.gameObject.SetActive(false);

        startScreenController.FadeInTitle();
    }

    // Stops the intro timeline at the Start Screen.
    // Switches control to the Start Screen Controller.
    // Timeline calls this after calling StartScreenStart.
    public void ActivateStartScreenController()
    {
        introController.gameObject.SetActive(false);
        startScreenController.gameObject.SetActive(true);
    }
    
    
    // ----------------------------------------------------------------------
    // Unity Events

    // Start Options / Start Game
    public void ToSavedGames()
    {
        state = UIState.Disabled;
        Script_Start.Main.CrunchTransitionDown();

        StartCoroutine(WaitToSavedGames());
    }

    // Saved Games/Back Button
    public void ToStartScreen(bool isSkipIntro = false)
    {
        state = UIState.Disabled;
        Script_Start.Main.CrunchTransitionDown();
        
        StartCoroutine(WaitToStartScreen());
        IEnumerator WaitToStartScreen()
        {
            yield return new WaitForSeconds(crunchTimeDown);

            // switch when teeth are opening
            InitializeIntro(isSkipIntro);
        }
    }

    public void ToSettings(int buttonIdx)
    {
        // Stop detect keypresses.
        // Also prevents restarting intro sequence from Settings and its child views.
        startScreenController.gameObject.SetActive(false);
        
        startScreenTitle.Close();
        startOptionsCanvasGroup.Close();
        controlsCanvasGroup.Close();

        Debug.Log($"{name} Opening settings canvasGroup");
        settingsCanvasGroup.Open();

        EventSystem.current.SetSelectedGameObject(settingsButtons[buttonIdx].gameObject);
    }

    public void ToControls()
    {
        startScreenController.gameObject.SetActive(false);
        
        startScreenTitle.Close();
        startOptionsCanvasGroup.Close();
        settingsCanvasGroup.Close();

        controlsCanvasGroup.Open();
        
        EventSystem.current.SetSelectedGameObject(controlsCanvasGroup.firstToSelect.gameObject);
    }

    public void BackToStartOptions()
    {
        Debug.Log("BackToStartOptions() Closing settings and controls");
        settingsCanvasGroup.Close();
        controlsCanvasGroup.Close();
        
        Debug.Log($"startScreenTitle.IsFadingIn: {startScreenTitle.IsFadingIn}");
        startScreenController.FadeInTitle();
        
        startScreenCTA.Close();
        startOptionsCanvasGroup.Open();
        
        EventSystem.current.SetSelectedGameObject(startOptionsCanvasGroup.firstToSelect.gameObject);
        
        // If coming back from Settings, reactivate the controller.
        StartCoroutine(WaitSetControllerActive());
        
        IEnumerator WaitSetControllerActive()
        {
            yield return null;

            startScreenController.gameObject.SetActive(true);
        }   
    }

    public void Quit()
    {
          Debug.Log($"{name} Quit called");
          Debug.Break();
          Application.Quit();
    }

    // ----------------------------------------------------------------------

    public void InitializeGameOverState()
    {
        StartCoroutine(WaitToPlayBG());
        IEnumerator WaitToPlayBG()
        {
            yield return new WaitForSeconds(playGameOverBGWaitTime);
            bgmManager.Play(1);
        }
        
        startScreenCanvasGroup.gameObject.SetActive(false);
        savedGameCanvasGroup.gameObject.SetActive(false);
        gameOverCanvasGroup.gameObject.SetActive(false); // Start out with black screen, until Teeth Come in
        savedGameEventSystem.InitializeState();
    }

    public void ToGameOver(Script_GameOverController.DeathTypes deathType)
    {
        switch(deathType)
        {
            case(Script_GameOverController.DeathTypes.ThoughtsOverload):
                Debug.Log("Thoughts Overload Screen");
                activeDeathByScreen = deathByScreens[1];
                break;

            case(Script_GameOverController.DeathTypes.Impaled):
                Debug.Log("Impaled Game Over Screen");
                activeDeathByScreen = deathByScreens[2];
                break;

            case(Script_GameOverController.DeathTypes.DemoOver):
                Debug.Log("Demo Over Game Over Screen");
                activeDeathByScreen = deathByScreens[3];
                break;

            default:
                Debug.LogWarning($"Default Game Over Screen -- you need to implement {deathType}");
                activeDeathByScreen = deathByScreens[0];
                break;
        }

        state = UIState.Disabled;
        Script_Start.Main.CrunchTransitionDown();
        
        StartCoroutine(WaitToGameOver());
        IEnumerator WaitToGameOver()
        {
            yield return new WaitForSeconds(crunchTimeDown);

            // switch when teeth are opening
            gameOverCanvasGroup.GetComponent<Script_GameOverParent>().Setup();
            activeDeathByScreen.gameObject.SetActive(true);
            gameOverCanvasGroup.gameObject.SetActive(true);
        }
    }

    IEnumerator WaitToSavedGames()
    {
        yield return new WaitForSeconds(crunchTimeDown);

        // switch when teeth are opening
        savedGameCanvasGroup.gameObject.SetActive(true);
        startScreenCanvasGroup.gameObject.SetActive(false);
        InitializeSavedGamesState();
    }

    public void OnCrunchPlayableDone(PlayableDirector aDirector)
    {
        // After Crunch Down complete
        if (aDirector.playableAsset == crunchDirector.GetComponent<Script_TimelineController>().timelines[0])
        {
            Script_Start.Main.CrunchTransitionUp();
        }
        // After Crunch Up complete
        else if (aDirector.playableAsset == crunchDirector.GetComponent<Script_TimelineController>().timelines[1])
        {
            // unfreeze game
            state = UIState.Interact;
        }
    }

    public SavedGameState State
    {
        get { return savedGameState; }
        set {
            savedGameState = value;

            HandleButtonsState();
        }
    }
    
    /// <summary>
    /// Enter the saved games selection screen
    /// Called from start game screen, and returning from submenu
    /// </summary>
    public void EnterSavedGamesSelectView()
    {
        submenuController.gameObject.SetActive(false);
        savedGameController.gameObject.SetActive(true);
        savedGameController.RehydrateState();
        
        continueSubmenu.gameObject.SetActive(false);
        newGameSubmenu.gameObject.SetActive(false);
        deleteGameSubmenu.gameObject.SetActive(false);
        pasteGameSubmenu.gameObject.SetActive(false);
    }

    /// <summary>
    /// Called from Delete's OnClick
    /// </summary>
    public void EnterSavedGamesDeleteView()
    {
        State = SavedGameState.Delete;
        savedGameController.InitializeState();
        EnterSavedGamesSelectView();
        ActivateViewState();
    }

    /// <summary>
    /// Eventhandler for exitting submenu
    /// </summary>
    private void ReenterDeleteView()
    {
        ActivateViewState();
    }

    /// <summary>
    /// Called from Copy's OnClick
    /// </summary>
    public void EnterSavedGamesCopyView()
    {
        State = SavedGameState.Copy;
        savedGameController.InitializeState();
        EnterSavedGamesSelectView();
        ActivateViewState();
    }

    /// <summary>
    /// Called from SavedGameTitle's OnClick in COPY STATE
    /// </summary>
    public void HandleEnterPasteView(Script_SavedGameTitle savedGame)
    {
        int slotId = savedGame.GetComponent<Script_Slot>().Id;
        if (savedGame.isRendered)
        {
            copiedSlotId = slotId;
            State = SavedGameState.Paste;

            // initialize with next open slot
            int nextOpenSlotId = slotId + 1;
            Transform[] slots = savedGameController.GetSlots();
            for(int j = 0; j < slots.Length - 1; nextOpenSlotId++, j++)
            {
                if (nextOpenSlotId >= slots.Length) nextOpenSlotId = 0;
                if (!slots[nextOpenSlotId].GetComponent<Script_SavedGameTitle>().isRendered)
                    break;
            }

            savedGameController.InitializeState(nextOpenSlotId);
            EnterSavedGamesSelectView();
            ActivateViewState();
        }
        else
        {
            ErrorSFX();
        }
        
    }
    
    private void ActivateViewState()
    {
        deleteBanner.gameObject.SetActive(false);
        copyBanner.gameObject.SetActive(false);
        pasteBanner.gameObject.SetActive(false);
        
        if (State == SavedGameState.Delete)
            deleteBanner.gameObject.SetActive(true);
        if (State == SavedGameState.Copy)
            copyBanner.gameObject.SetActive(true);
        if (State == SavedGameState.Paste)
            pasteBanner.gameObject.SetActive(true);
    }
    private void DeactivateViewState()
    {
        deleteBanner.gameObject.SetActive(false);
        copyBanner.gameObject.SetActive(false);
        pasteBanner.gameObject.SetActive(false);
    }

    /// <summary>
    /// Called from Saved Game ClickHandler
    /// In each OnClick handler, pass in 
    /// </summary>
    /// <param name="choices"></param>
    public void EnterFileChoices(Script_SavedGameTitle savedGame)
    {
        int slotId = savedGame.GetComponent<Script_Slot>().Id;
        if (savedGame.isRendered)
        {
            choices = continueChoices;
            continueSubmenu.gameObject.SetActive(true);
            // EnterSubmenuSFX();
        }
        else
        {
            choices = newGameChoices;
            newGameSubmenu.gameObject.SetActive(true);
        }
        EnterSubmenuSFX();

        /// Set slot Id in submenu
        foreach (Script_SavedGameSubmenuInputChoice choice in choices)
            choice.Id = slotId;
        
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
        savedGameController.gameObject.SetActive(false);
        submenuController.gameObject.SetActive(true);
    }

    public void ContinueGame(int i)
    {
        state = UIState.Disabled;

        Script_SaveGameControl.saveSlotId = i;
        Script_SceneManager.ToGameScene();
    }

    public void NewGame(int i)
    {
        state = UIState.Disabled;
        
        Script_SaveGameControl.saveSlotId = i;
        Script_SceneManager.ToGameScene();
    }

    public void EnterDeleteFileChoices(Script_SavedGameTitle savedGame)
    {
        int slotId = savedGame.GetComponent<Script_Slot>().Id;
        if (savedGame.isRendered)
        {
            deleteBanner.gameObject.SetActive(false);

            choices = deleteGameChoices;
            deleteGameSubmenu.gameObject.SetActive(true);

            /// Set slot Id in submenu
            foreach (Script_SavedGameSubmenuInputChoice choice in choices)
            {
                choice.Id = slotId;
            }
            
            EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
            savedGameController.gameObject.SetActive(false);
            submenuController.gameObject.SetActive(true);
            
            EnterSubmenuSFX();
            Debug.Log("Enter delete submenu");
        }
        else
        {
            ErrorSFX();
        }
    }

    public void DeleteGame(int i)
    {
        Script_SaveGameControl.saveSlotId = i;
        
        if (Script_SaveGameControl.Delete())
        {
            // update that slot
            savedGameController
                .GetSlotTransform(i)
                .GetComponent<Script_SavedGameTitle>()
                .InitializeState();
        }
        
        // end delete mode
        State = SavedGameState.Start;
        EnterSavedGamesSelectView();
    }

    public void EnterPasteFileChoices(Script_SavedGameTitle savedGame)
    {
        int slotId = savedGame.GetComponent<Script_Slot>().Id;

        // also check if it's the same slot we're copying from
        if (slotId != copiedSlotId)
        {
            pasteBanner.gameObject.SetActive(false);

            /// Only show submenu if overwriting a file

            if (savedGame.isRendered)
            {
                choices = pasteGameChoices;
                pasteGameSubmenu.gameObject.SetActive(true);

                /// Set slot Id in submenu
                foreach (Script_SavedGameSubmenuInputChoice choice in choices)
                {
                    choice.Id = slotId;
                }
                
                EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
                savedGameController.gameObject.SetActive(false);
                submenuController.gameObject.SetActive(true);
                EnterSubmenuSFX();
            }
            else
            {
                CopyGame(slotId);
            }
        }
        else
        {
            ErrorSFX();
        }
    }

    public void CopyGame(int i)
    {
        Script_SaveGameControl.saveSlotId = copiedSlotId;
        
        if (Script_SaveGameControl.Copy(i))
        {
            // update that slot
            savedGameController
                .GetSlotTransform(i)
                .GetComponent<Script_SavedGameTitle>()
                .InitializeState();
        }
        
        // end copy mode
        State = SavedGameState.Start;
        EnterSavedGamesSelectView();
    }

    private bool CheckFullSaveSlots()
    {
        foreach (Transform slot in savedGameController.GetSlots())
        {
            if (!slot.GetComponent<Script_SavedGameTitle>().isRendered)
            {
                return false;
            }
        }
        return true;
    }

    private void HandleButtonsState()
    {
        // disable copy button if slots are full
        bool isSlotsFull = CheckFullSaveSlots();

        if (isSlotsFull)
            copyBtn.GetComponent<Script_ButtonHighlighter>().Activate(false);
        else
            copyBtn.GetComponent<Script_ButtonHighlighter>().Activate(true);
        
        foreach (Button btn in fileActionButtons)
        {
            bool isActive = savedGameState == SavedGameState.Start;

            // don't reactivate copy button if it's disabled bc slots are full
            if (isSlotsFull && isActive && btn == copyBtn)  continue;
            
            btn.GetComponent<Script_ButtonHighlighter>().Activate(isActive);
        }
    }

    private void EnterSubmenuSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.OpenCloseBookHeavy,
            Script_SFXManager.SFX.OpenCloseBookHeavyVol
        );
    }

    private void ErrorSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.UIErrorSFX, Script_SFXManager.SFX.UIErrorSFXVol
        );
    }

    public void InitializeSavedGamesState()
    {
        State = SavedGameState.Start;
        
        EnterSavedGamesSelectView();
        
        deleteBanner.gameObject.SetActive(false);
        copyBanner.gameObject.SetActive(false);
        pasteBanner.gameObject.SetActive(false);
    }

    public void Setup()
    {
        savedGameController.Setup();
        startScreenController.Setup();
    }
}
