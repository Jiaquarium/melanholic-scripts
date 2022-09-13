using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This GameObject should remain active.
/// </summary>
public class Script_SettingsController : MonoBehaviour
{
    public enum States
    {
        Overview = 0,
        Controls = 1,
        Graphics = 2,
        MainMenu = 3,
    }

    public static Script_SettingsController Instance;

    public States state;

    [SerializeField] private bool isThrottledInGame;
    [SerializeField] private float throttleTime;
    
    [SerializeField] private List<Button> initialButtons;
    
    [SerializeField] private UnityEvent OnBackFromOverviewAction;
    
    [SerializeField] private Script_CanvasGroupController overviewCanvasGroup;
    [SerializeField] private Script_CanvasGroupController controlsCanvasGroup;
    [SerializeField] private Script_CanvasGroupController graphicsCanvasGroup;
    [SerializeField] private Script_SettingsGraphicsController graphicsController;
    [SerializeField] private Script_CanvasGroupController bgCanvasGroup;
    [SerializeField] private FadeSpeeds bgFadeSpeed;
    
    [SerializeField] private Script_SettingsInputManager settingsInputManager;
    [SerializeField] private EventSystem settingsEventSystem;

    [SerializeField] private AudioSource audioSource;

    [Header("Rebind Settings")]
    [SerializeField] private List<Script_UIRebindAction> UIRebindActions;
    [SerializeField] private Script_SavedGameSubmenuInputChoice[] resetDefaultsSubmenuChoices;
    [SerializeField] private GameObject resetDefaultsSubmenu;
    [SerializeField] private GameObject onExitResetDefaultsSubmenuActiveObject;
    
    private bool isRebindSubmenu = false;

    private float timer;

    public bool IsThrottledInGame { get => isThrottledInGame; }
    
    void OnEnable()
    {
        Script_MenuEventsManager.OnExitMenu += StartThrottlingInGame;
    }

    void OnDisable()
    {
        Script_MenuEventsManager.OnExitMenu -= StartThrottlingInGame;
    }
    
    void Update()
    {
        if (settingsInputManager.gameObject.activeInHierarchy)
            settingsInputManager.HandleExitInput();
        
        if (isThrottledInGame)
        {
            timer -= Time.unscaledDeltaTime;
            
            if (timer < 0f)
                timer = 0f;
            
            if (timer == 0f)
                isThrottledInGame = false;
        }   
    }
    
    public void OpenOverview(int firstSelectedIdx)
    {
        bgCanvasGroup.FadeIn(bgFadeSpeed.GetFadeTime(), isUnscaledTime: true);
        overviewCanvasGroup.Open();
        controlsCanvasGroup.Close();
        graphicsCanvasGroup.Close();

        settingsEventSystem.gameObject.SetActive(true);
        settingsInputManager.gameObject.SetActive(true);

        settingsEventSystem.SetSelectedGameObject(initialButtons[firstSelectedIdx].gameObject);

        state = States.Overview;
    }

    public void Close(bool isFade = false, Action cb = null)
    {
        // Set to null to ensure when we open back Settings, the OnSelect event is triggered.
        settingsEventSystem.SetSelectedGameObject(null);
        
        // Flush state to prevent opening SFX (initial button specifies an onlySFXTransitionParent
        // so it should always come from null, thus no SFX)
        settingsEventSystem.GetComponent<Script_EventSystemLastSelected>().InitializeState();
        
        settingsEventSystem.gameObject.SetActive(false);
        settingsInputManager.gameObject.SetActive(false);
        
        overviewCanvasGroup.Close();
        controlsCanvasGroup.Close();
        graphicsCanvasGroup.Close();
        
        if (isFade)
            bgCanvasGroup.FadeOut(bgFadeSpeed.GetFadeTime(), cb, isUnscaledTime: true);
        else
            bgCanvasGroup.Close();
    }

    // ------------------------------------------------------------
    // Unity Events
    
    // UI Settings Overview: Controls Button
    public void ToControls()
    {
        overviewCanvasGroup.Close();
        
        controlsCanvasGroup.Open();
        UpdateControlKeyDisplays();

        // Set Controls First Selected active.
        EventSystem.current.SetSelectedGameObject(controlsCanvasGroup.firstToSelect.gameObject);

        EnterMenuSFX();

        state = States.Controls;
    }

    // UI Settings Overview: Graphics Button
    public void ToGraphics()
    {
        overviewCanvasGroup.Close();
        graphicsCanvasGroup.Open();
        
        graphicsController.ToGraphics();

        EnterMenuSFX();
        state = States.Graphics;
    }

    // From UI Back Buttons
    public virtual void Back()
    {
        switch (state)
        {
            case (States.Overview):
                Close(
                    isFade: true,
                    cb: () => {
                        OnBackFromOverviewAction.SafeInvoke();
                    }
                );
                ExitMenuSFX();
                
                // This will only affect in game to prevent
                // player from accidentally spamming Esc.
                StartThrottlingInGame();
                
                break;
            case (States.Controls):
                if (isRebindSubmenu)
                {
                    CloseResetDefaultsSubmenu(isSuccess: false);
                }
                else
                {
                    OpenOverview(0);
                    ExitMenuSFX();
                }
                break;
            case (States.Graphics):
                OpenOverview(1);
                ExitMenuSFX();
                break;
        }
    }

    public void ToTitleScreen()
    {
        Script_TransitionManager.Control.ToTitleScreen();
        Script_TransitionManager.Control.EnterMenuSFX();
    }

    // Controls: Reset All to Defaults
    public void ResetDefaults()
    {
        Script_PlayerInputManager.Instance.SetDefault();

        CloseResetDefaultsSubmenu(isSuccess: true);
    }

    // Reset Default Submenu
    public void CloseResetDefaultsSubmenu(bool isSuccess)
    {
        resetDefaultsSubmenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(onExitResetDefaultsSubmenuActiveObject.gameObject);

        if (isSuccess)
            Script_SFXManager.SFX.PlayUISuccessEdit();
        else
            Script_SFXManager.SFX.PlayExitSubmenuPencil();
        
        isRebindSubmenu = false;
    }
    
    // ------------------------------------------------------------
    // Controls
    
    public void UpdateControlKeyDisplays()
    {
        UIRebindActions.ForEach(rebindKeyUI => {
            rebindKeyUI.UpdateBehavior();
        });
    }

    public void OnStartRebindProcess()
    {
        settingsEventSystem.sendNavigationEvents = false;
    }

    public void OnDoneRebindProcess()
    {
        settingsEventSystem.sendNavigationEvents = true;
    }

    public void OpenResetDefaultsSubmenu()
    {
        isRebindSubmenu = true;
        resetDefaultsSubmenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(resetDefaultsSubmenuChoices[0].gameObject);
        EnterSubmenuSFX();
    }
    
    // ------------------------------------------------------------
    

    protected void EnterMenuSFX()
    {
        audioSource.PlayOneShot(
            Script_SFXManager.SFX.OpenCloseBook,
            Script_SFXManager.SFX.OpenCloseBookVol
        );
    }

    protected void ExitMenuSFX()
    {
        audioSource.PlayOneShot(
            Script_SFXManager.SFX.OpenCloseBookReverse,
            Script_SFXManager.SFX.OpenCloseBookReverseVol
        );
    }

    protected void EnterSubmenuSFX()
    {
        audioSource.PlayOneShot(
            Script_SFXManager.SFX.OpenCloseBookHeavy,
            Script_SFXManager.SFX.OpenCloseBookHeavyVol
        );
    }

    /// <summary>
    /// Disable settings after exitting Menu so Player doesn't accidentally
    /// activate settings.
    /// </summary>
    private void StartThrottlingInGame()
    {
        timer = throttleTime;
        isThrottledInGame = true;
    }

    private void InitialState()
    {
        resetDefaultsSubmenu.SetActive(false);
    }
    
    public virtual void Setup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        
        InitialState();
        
        gameObject.SetActive(true);
        audioSource.gameObject.SetActive(true);
        Close();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_SettingsController))]
    public class Script_SettingsControllerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_SettingsController t = (Script_SettingsController)target;
            
            if (GUILayout.Button("Open Reset Defaults Submenu"))
            {
                t.OpenResetDefaultsSubmenu();
            }
        }
    }
#endif
}
