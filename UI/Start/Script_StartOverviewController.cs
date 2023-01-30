using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Playables;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Main controller for Start Screen UI States
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_StartOverviewController : Script_UIState
{
    [SerializeField] private int startScreenBgm;
    [Tooltip("Wait time after crunch transition to stay in UI Disabled state.")]
    [SerializeField] private float transitionWaitTime;
    
    [SerializeField] private SavedGameState savedGameState;
    [SerializeField] private Script_EventSystemLastSelected savedGameEventSystem;
    
    
    [Header("Simple Intro Settings")]
    [SerializeField] private Script_IntroControllerSimple introControllerSimple;
    // Match Title Fade In Time (~2s)
    [SerializeField] private float defaultInputDisabledTime;
    [SerializeField] private float waitBeforeFadeInCTATime;
    [SerializeField] private float buttonFadeInTime;
    [SerializeField] private float waitBeforeFadeInOnBackFromGame;
    [SerializeField] private float waitBeforeFadeInTitleOnBackFromGame;

    [SerializeField] private float transitionExitFadeTime;
    
    [SerializeField] private Script_IntroController introController;
    [SerializeField] private Script_StartScreenController startScreenController;
    [SerializeField] private Script_StartScreenInputManager startScreenInputManager;
    
    [SerializeField] private CanvasGroup introCanvasGroup;
    [SerializeField] private CanvasGroup savedGameCanvasGroup;
    
    [SerializeField] private CanvasGroup startScreenCanvasGroup;
    [SerializeField] private EventSystem startScreenEventSystem;
    
    [SerializeField] private Script_CanvasGroupController startOptionsCanvasGroup;
    [SerializeField] private Script_CanvasGroupController startScreenTitle;
    [SerializeField] private Script_CanvasGroupController startScreenCTA;
    [SerializeField] private Script_CanvasGroupController demoHeader;
    [SerializeField] private Script_SettingsController settingsController;
    [SerializeField] private Script_CanvasGroupController controlsCanvasGroup;
    
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
    
    [SerializeField] private CanvasGroup fileActionBannersCanvasGroup;
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

    [Header("To Loading Screen Settings")]
    [SerializeField] private Script_CanvasGroupController bgBlack;
    [SerializeField] private Script_CanvasGroupController fader;
    [SerializeField] private float toLoadingBgBlackFadeInTime = 0.5f;
    [SerializeField] private float toLoadingSubmenuFadeOutTime = 0.75f;
    [SerializeField] private float waitBlackScreenBeforeLoadingScreenTime = 0.5f;
    
    private Script_DeathByScreen activeDeathByScreen;
    private Script_SavedGameSubmenuInputChoice[] choices;
     
    private int copiedSlotId;

    private bool isCrunchActive;

    private bool isInitedSimple;
    
    void OnEnable()
    {
        Script_StartEventsManager.OnExitSubmenu     += RefreshFileActionBanners;
        Script_StartEventsManager.OnExitFileActions += RemoveFileActionBanners;

        crunchDirector.stopped += OnCrunchPlayableDone;
    }

    void OnDisable()
    {
        Script_StartEventsManager.OnExitSubmenu     -= RefreshFileActionBanners;
        Script_StartEventsManager.OnExitFileActions -= RemoveFileActionBanners;

        crunchDirector.stopped -= OnCrunchPlayableDone;
    }

    // ----------------------------------------------------------------------
    // Simple Intro Sequence
    public void InitializeIntroSimple(bool isForceInitedSimple = false)
    {
        if (isForceInitedSimple)
            isInitedSimple = true;
        
        startScreenCanvasGroup.gameObject.SetActive(false);
        savedGameCanvasGroup.gameObject.SetActive(false);
        gameOverCanvasGroup.gameObject.SetActive(false);
        
        settingsController.Close();
        controlsCanvasGroup.Close();
        
        savedGameEventSystem.InitializeState();

        introController.gameObject.SetActive(false);
        startScreenController.gameObject.SetActive(false);
        
        introCanvasGroup.gameObject.SetActive(false);

        bgmManager.SetDefault(Const_AudioMixerParams.ExposedBGVolume);

        if (isInitedSimple)
            FadeInTitleScreen(withCTA: false);
        else
            introControllerSimple.Play();

        isInitedSimple = true;
    }
    
    // ----------------------------------------------------------------------
    // For long Intro Sequence
    
    // Starts Intro Sequence.
    // Intro Timeline, Full Playthrough > StartScreenStart() via Signal > ActivateStartScreenController() via Signal
    // Intro Timeline, Skip Intro > StartScreenStart() via call > ActivateStartScreenController() via Signal
    // From Back, Skip Intro > Manually call StartScreenStart > ActivateStartScreenController()
    public void InitializeIntro(bool isSkip = false)
    {
        startScreenCanvasGroup.gameObject.SetActive(false);
        savedGameCanvasGroup.gameObject.SetActive(false);
        gameOverCanvasGroup.gameObject.SetActive(false);
        
        settingsController.Close();
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
            StartScreenStart(isFromBack: true);
        }
        else
        {
            bgmManager.SetDefault(Const_AudioMixerParams.ExposedBGVolume);
            bgmManager.Stop();
        }
    }

    // Will be called whenever a confirm key is pressed on Start Options (via Start Screen Controller).
    public void StartOptionsOpen(bool isFadeIn = false, bool isFadeInDemo = false)
    {
        Dev_Logger.Debug("StartOptionsOpen");
        
        SetupStartOptionsOpen();
        
        if (Const_Dev.IsDemo)
        {
            if (isFadeInDemo)
                demoHeader.FadeIn(buttonFadeInTime);
            else
                demoHeader.Open();
        }
        else
            demoHeader.Close();

        if (isFadeIn)
        {
            startOptionsCanvasGroup.Close();
            startOptionsCanvasGroup.FadeIn(buttonFadeInTime);
        }
        else
        {
            startOptionsCanvasGroup.Open();
        }
    }

    private void SetupStartOptionsOpen()
    {
        startScreenCTA.Close();
        settingsController.Close();
        controlsCanvasGroup.Close();
    }

    // ----------------------------------------------------------------------
    // Timeline Signals
    
    // For Simple Intro, called at end.
    // StartScreenStart will activate input.
    public void FadeInTitleScreen(bool withCTA)
    {
        Dev_Logger.Debug("Fade in Title Screen");
        
        StartScreenStart(!withCTA);
    }
    
    // Signals the Start screen is up. Start Screen Controller should be disabled here.
    // Called from timeline as the starting point upon skipping intro.
    // Start options should be hidden until player presses Start command.
    public void StartScreenStart(bool isFromBack = false)
    {
        Dev_Logger.Debug($"{name} StartScreenStart called isFromBack: {isFromBack}");
        
        introController.DisableInput();
        
        startScreenCanvasGroup.gameObject.SetActive(true);
        startScreenEventSystem.gameObject.SetActive(true);
        introCanvasGroup.gameObject.SetActive(false);

        HandleEntryPoint();
        
        void HandleEntryPoint()
        {
            // As a precaution, ensure the Init Fader is closed
            Script_Start.Main.CloseInitFader();
            
            // If coming from Saved Games > Back or Game Settings > Quit to Main Menu,
            // have start options immediately available.
            if (isFromBack)
            {
                // If coming from Game, we'll need to play BGM
                if (!bgmManager.IsPlaying)
                {
                    Dev_Logger.Debug("BGM was not playing, playing now!");
                    PlayBgm();
                }

                // Must mark CTADone or Start Screen Input Manager will try to activate start options
                // and close other canvases on confirm key.
                // Note: Start Screen Controller does nothing.
                startScreenInputManager.IsCTADone = true;
                startScreenController.gameObject.SetActive(true);
                
                // Fix for Windows Bug (Demo 0.13.0 Build Id 9640356):
                // Windows build fails to show first few frames on new Scene, so fade in Start Options
                // first and wait to fade in Title (visually looks okay for Start Options to not fade in)
                if (Script_Start.startState == Script_Start.StartStates.BackToMainMenu)
                {
                    demoHeader.Close();
                    SetupStartOptionsOpen();
                    StartCoroutine(WaitToFadeInStartScreen());
                }
                else
                {
                    StartOptionsOpen();
                    startScreenController.FadeInTitle(Script_StartScreenController.Type.FromBack);
                }

                Script_Start.startState = Script_Start.StartStates.Start;
            }
            else
            {
                startScreenController.FadeInTitle(Script_StartScreenController.Type.Intro);
                
                startScreenCTA.Close();
                demoHeader.Close();
                
                PlayBgm();
                
                // Give Player some time to look at Title before showing CTA.
                StartCoroutine(WaitToShowCTA());
            }
        }

        IEnumerator WaitToFadeInStartScreen()
        {
            yield return new WaitForSeconds(waitBeforeFadeInOnBackFromGame);
            StartOptionsOpen(isFadeIn: true, isFadeInDemo: true);
            
            yield return new WaitForSeconds(waitBeforeFadeInTitleOnBackFromGame);
            startScreenController.FadeInTitle(Script_StartScreenController.Type.FromBack);
        }

        IEnumerator WaitToShowCTA()
        {
            yield return new WaitForSeconds(waitBeforeFadeInCTATime);

            Dev_Logger.Debug($"Fading in CTA, isFromBack {isFromBack}");
                    
            startScreenCTA.gameObject.SetActive(true);
            startScreenCTA.StartIntervalFader(isFadeIn: true);
            
            if (Const_Dev.IsDemo)
                demoHeader.FadeIn(startScreenCTA.GetComponent<Script_CanvasGroupFadeInterval>().Interval);
            
            startScreenController.gameObject.SetActive(true);
        }
    }

    private void PlayBgm()
    {
        Dev_Logger.Debug("Playing BGM");
        
        bgmManager.SetDefault(Const_AudioMixerParams.ExposedBGVolume);
        bgmManager.Play(startScreenBgm);
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
        if (isCrunchActive)
            return;
        
        state = UIState.Disabled;
        
        Script_Start.Main.CrunchTransitionDown();
        isCrunchActive = true;

        StartCoroutine(WaitToSavedGames());

        IEnumerator WaitToSavedGames()
        {
            yield return new WaitForSeconds(crunchTimeDown);

            // Switch when teeth are opening
            // Ensure to clear event system before to prevent Select Sound
            // possibly clicking
            savedGameEventSystem.InitializeState();
            startScreenCanvasGroup.gameObject.SetActive(false);
            savedGameCanvasGroup.gameObject.SetActive(true);

            InitializeSavedGamesState();

            isCrunchActive = false;
        }
    }

    // From Saved Games > Back Button
    // From Game Over
    public void ToStartScreenNonIntro(bool isSkipIntro = false)
    {
        if (isCrunchActive)
            return;
        
        state = UIState.Disabled;
        
        Script_Start.Main.CrunchTransitionDown();
        isCrunchActive = true;
        
        StartCoroutine(WaitToStartScreen());
        
        IEnumerator WaitToStartScreen()
        {
            yield return new WaitForSeconds(crunchTimeDown);

            // Switch when teeth are opening
            InitializeIntroSimple();

            isCrunchActive = false;
        }
    }

    // From Settings Button
    public void ToSettings(int buttonIdx)
    {
        // Stop detect keypresses.
        // Also prevents restarting intro sequence from Settings and its child views.
        startScreenController.gameObject.SetActive(false);
        
        // Note: Clear Start Screen EventSystem to prevent clicking when coming back from Settings
        // (When navigating to Settings, if not cleared, the Start Option: Settings Button will be the
        // last selected causing a click because that object is not explicitly ignored by Select Sound)
        startScreenEventSystem.GetComponent<Script_EventSystemLastSelected>().InitializeState();
        startScreenEventSystem.gameObject.SetActive(false);
        
        startOptionsCanvasGroup.Close();

        Dev_Logger.Debug($"{name} Opening settings canvasGroup");
        
        settingsController.OpenOverview(buttonIdx);
    }

    // From Settings Controller Back => Close() => onCloseEvent
    public void BackToStartOptions()
    {
        startScreenCTA.Close();
        StartOptionsOpen(isFadeIn: true);
        
        // Reenable start screen EventSystem
        startScreenEventSystem.gameObject.SetActive(true);
        
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
        DisableInput();
        fader.Close();
        fader.FadeIn(FadeSpeeds.MedFast.ToFadeTime(), Application.Quit, isUnscaledTime: true);
    }

    // Also called from Menu button click handlers.
    public void EnterMenuSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.OpenCloseBook,
            Script_SFXManager.SFX.OpenCloseBookVol
        );
    }

    /// <summary>
    /// - Title Screen: End
    /// </summary>
    public void EndGameSFX()
    {
        Script_SFXManager.SFX.PlaySubmitTransitionCancel();
    }

    public void ExitMenuSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.OpenCloseBookReverse,
            Script_SFXManager.SFX.OpenCloseBookReverseVol
        );
    }

    /// <summary>
    /// - Title Screen: Wishlist on Steam
    /// </summary>
    public void WishlistSFX()
    {
        Script_SFXManager.SFX.PlaySubmitTransition();
    }

    /// <summary>
    /// - Title Screen: Wishlist on Steam
    /// </summary>
    public void GoToWishlist()
    {
        Application.OpenURL(Script_Utils.SteamClientStoreURL);
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
                Dev_Logger.Debug("Thoughts Overload Screen");
                activeDeathByScreen = deathByScreens[1];
                break;

            case(Script_GameOverController.DeathTypes.Impaled):
                Dev_Logger.Debug("Impaled Game Over Screen");
                activeDeathByScreen = deathByScreens[2];
                break;

            case(Script_GameOverController.DeathTypes.DemoOver):
                Dev_Logger.Debug("Demo Over Game Over Screen");
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
            StartCoroutine(WaitToInteract());
        }

        IEnumerator WaitToInteract()
        {
            yield return new WaitForSeconds(transitionWaitTime);

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

        Dev_Logger.Debug("Enter Saved Games Select View");
        HoldHighlights(false);
    }

    /// <summary>
    /// Called from Delete's OnClick
    /// </summary>
    public void EnterSavedGamesDeleteView()
    {
        State = SavedGameState.Delete;
        savedGameController.InitializeState();
        EnterSavedGamesSelectView();
        RefreshFileActionBanners();
    }

    /// <summary>
    /// Eventhandler for exitting submenu
    /// </summary>
    private void ReenterDeleteView()
    {
        RefreshFileActionBanners();
    }

    /// <summary>
    /// Called from Copy's OnClick
    /// </summary>
    public void EnterSavedGamesCopyView()
    {
        State = SavedGameState.Copy;
        savedGameController.InitializeState();
        EnterSavedGamesSelectView();
        RefreshFileActionBanners();
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
            RefreshFileActionBanners();
        }
        else
        {
            ErrorSFX();
        }
        
    }
    
    private void RefreshFileActionBanners()
    {
        fileActionBannersCanvasGroup.gameObject.SetActive(true);
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

    private void RemoveFileActionBanners()
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
        
        // Must handle highlighting before setting Event System's Selected Object
        HoldHighlights(true);
        
        if (savedGame.isRendered)
        {
            choices = continueChoices;
            continueSubmenu.gameObject.SetActive(true);
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
        
        ToGameTransition(continueSubmenu);
    }

    public void NewGame(int i)
    {
        state = UIState.Disabled;
        Script_SaveGameControl.saveSlotId = i;
        
        ToGameTransition(newGameSubmenu);
    }

    private void ToGameTransition(Transform submenu)
    {
        // Fade In Black Bg, then Fade Out Submenu
        bgBlack.Close();
        bgBlack.FadeIn(toLoadingBgBlackFadeInTime, () => {
            submenu.GetComponent<Script_CanvasGroupController>().FadeOut(
                toLoadingSubmenuFadeOutTime,
                () => StartCoroutine(WaitToLoadScene())
            );
        });

        IEnumerator WaitToLoadScene()
        {
            yield return new WaitForSeconds(waitBlackScreenBeforeLoadingScreenTime);

            Script_SceneManager.ToGameScene();
        }
    }

    public void EnterDeleteFileChoices(Script_SavedGameTitle savedGame)
    {
        int slotId = savedGame.GetComponent<Script_Slot>().Id;

        if (savedGame.isRendered)
        {
            // Must handle highlighting before setting Event System's Selected Object
            HoldHighlights(true);
            
            choices = deleteGameChoices;
            deleteGameSubmenu.gameObject.SetActive(true);

            // Set slot Id in submenu
            foreach (Script_SavedGameSubmenuInputChoice choice in choices)
            {
                choice.Id = slotId;
            }
            
            EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
            savedGameController.gameObject.SetActive(false);
            submenuController.gameObject.SetActive(true);
            
            EnterSubmenuSFX();
            Dev_Logger.Debug("Enter delete submenu");
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

            Script_SFXManager.SFX.PlayChainWrappingCloseMenuSFX();
        }
        
        // end delete mode
        State = SavedGameState.Start;
        EnterSavedGamesSelectView();
        RefreshFileActionBanners();
    }

    public void EnterPasteFileChoices(Script_SavedGameTitle savedGame)
    {
        int targetSlotId = savedGame.GetComponent<Script_Slot>().Id;

        // Check if it's the same slot we're copying from
        if (targetSlotId != copiedSlotId)
        {
            // Only show submenu if overwriting a file
            if (savedGame.isRendered)
            {
                // Must handle highlighting before setting Event System's Selected Object
                HoldHighlights(true);

                choices = pasteGameChoices;
                pasteGameSubmenu.gameObject.SetActive(true);

                /// Set slot Id in submenu
                foreach (Script_SavedGameSubmenuInputChoice choice in choices)
                {
                    choice.Id = targetSlotId;
                }
                
                EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
                savedGameController.gameObject.SetActive(false);
                submenuController.gameObject.SetActive(true);

                EnterSubmenuSFX();
            }
            else
            {
                CopyGame(targetSlotId);
            }
        }
        else
        {
            ErrorSFX();
        }
    }

    public void CopyGame(int targetSlotId)
    {
        Dev_Logger.Debug("Trying to Copy");
        
        Script_SaveGameControl.saveSlotId = copiedSlotId;
        
        if (Script_SaveGameControl.Copy(copiedSlotId, targetSlotId))
        {
            // update that slot
            savedGameController
                .GetSlotTransform(targetSlotId)
                .GetComponent<Script_SavedGameTitle>()
                .InitializeState();
            
            Script_SFXManager.SFX.PlayTakeNote();
        }
        
        // end copy mode
        State = SavedGameState.Start;
        EnterSavedGamesSelectView();
        RefreshFileActionBanners();
    }

    private void DisableInput()
    {
        EventSystem.current.sendNavigationEvents = false;
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

    private void HoldHighlights(bool isHold)
    {
        Dev_Logger.Debug("On Enter Submenu Hold Highlights");
        savedGameController.HoldHighlights(isHold);
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

    public void BuildSettings()
    {
        startScreenCanvasGroup.gameObject.SetActive(false);
        savedGameCanvasGroup.gameObject.SetActive(false);
        gameOverCanvasGroup.gameObject.SetActive(false);
        introCanvasGroup.gameObject.SetActive(false);
        
        settingsController.Close();
        controlsCanvasGroup.Close();   
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_StartOverviewController))]
    public class Script_StartOverviewControllerTester : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Script_StartOverviewController t = (Script_StartOverviewController)target;

            if (GUILayout.Button("Copy Slot 0 to 1"))
            {
                t.copiedSlotId = 0;
                t.CopyGame(1);
            }
        }
    }
#endif
}
