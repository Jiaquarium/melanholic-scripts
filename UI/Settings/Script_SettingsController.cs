using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Rewired;
using TMPro;

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
        System = 2,
        MainMenu = 3,
    }

    public enum ControlsStates
    {
        Keyboard = 0,
        Joystick = 1,
    }
    
    private const float InputTimeout = 5f;
    private const float WaitBeforeListeningTime = 0.1f;
    public static readonly int ClickTrigger = Animator.StringToHash("click");

    public static Script_SettingsController Instance;

    public States state;
    [SerializeField] private ControlsStates controlsState;

    [SerializeField] private bool isThrottledInGame;
    [SerializeField] private float throttleTime;
    
    [SerializeField] private List<Button> initialButtons;
    
    [SerializeField] private UnityEvent OnBackFromOverviewAction;
    
    [SerializeField] private Script_CanvasGroupController overviewCanvasGroup;
    
    [Space][Header("Controls Settings")][Space]
    [SerializeField] private Script_CanvasGroupController controlsCanvasGroup;
    [SerializeField] private Script_CanvasGroupController keyboardControlsCanvasGroup;
    [SerializeField] private Script_CanvasGroupController joystickControlsCanvasGroup;
    [SerializeField] private Button bottomCommonButton;
    [SerializeField] private Button keyboardLastButton;
    [SerializeField] private Button joystickLastButton;
    [SerializeField] private Button topCommonButton;
    [SerializeField] private Button keyboardFirstButton;
    [SerializeField] private Button joystickFirstButton;
    [SerializeField] private List<Animator> controllerSelectArrows;
    [SerializeField] private TextMeshProUGUI keyboardTMP;
    [SerializeField] private TextMeshProUGUI joystickTMP;
    
    [Space][Header("Graphics Settings")][Space]
    [SerializeField] private Script_CanvasGroupController graphicsCanvasGroup;
    
    [Space][Header("System Settings")][Space]
    [SerializeField] private Script_SettingsSystemController systemController;

    [Space][Header("Rebind Settings")][Space]
    [SerializeField] private List<Script_UIRebindAction> UIRebindActions;
    [SerializeField] private List<Script_UIRebindAction> UIRebindActionsJoystick;
    [SerializeField] private Script_SavedGameSubmenuInputChoice[] resetDefaultsSubmenuChoices;
    [SerializeField] private GameObject resetDefaultsSubmenu;
    [SerializeField] private GameObject onExitResetDefaultsSubmenuActiveObject;

    [Space][Header("Other")][Space]
    [SerializeField] private Script_CanvasGroupController bgCanvasGroup;
    [SerializeField] private FadeSpeeds bgFadeSpeed;
    
    [SerializeField] private Script_SettingsInputManager settingsInputManager;
    [SerializeField] private EventSystem settingsEventSystem;

    [SerializeField] private AudioSource audioSource;

    private InputMapper inputMapper = new InputMapper();
    private InputMapper.ConflictFoundEventData conflictData;
    private bool isRebindSubmenu = false;

    private float timer;

    public ControlsStates MyControlsState => controlsState;
    public bool IsThrottledInGame { get => isThrottledInGame; }
    public Player MyPlayer => ReInput.players.GetPlayer(Script_Player.PlayerId);
    
    private Script_UIRebindAction currentRebindActionButton;
    private bool isInputDisabled;
    
    // Note: When referencing controllerMap or MyController, ALWAYS null check (e.g. call IsControllerConnected)
    private ControllerMap MyControllerMap
    {
        get {
            if (MyController == null)
                return null;
            
            return MyPlayer.controllers.maps.GetMap(
                MyController.type,
                MyController.id,
                Script_PlayerInputManager.Category,
                Script_PlayerInputManager.Layout
            );
        }
    }
    
    public Controller MyController
    {
        get
        {
            return MyPlayer.controllers.GetController(
                controlsState.ControlsStateToControllerType(),
                controlsState == ControlsStates.Keyboard
                    ? Script_PlayerInputManager.KeyboardId
                    : Script_PlayerInputManager.ControllerId
            );
        }
    }

    public bool IsControllerConnectedForState => MyController != null && MyControllerMap != null;

    private Rewired.Controller GetLastActiveController => MyPlayer.controllers.GetLastActiveController();
    
    void OnEnable()
    {
        Script_MenuEventsManager.OnExitMenu += StartThrottlingInGame;

        // Configure mapper options and subscribe to events
        // Timeout after 5 seconds of listening
        inputMapper.options.timeout = InputTimeout;

        // Ignore Mouse X and Y axes
        inputMapper.options.ignoreMouseXAxis = true;
        inputMapper.options.ignoreMouseYAxis = true;
        
        // Ensure to not allow modifiers (A should be the same as a); this will also treat modifier as input
        inputMapper.options.allowKeyboardKeysWithModifiers = false;

        // Set to cancel on Conflict
        inputMapper.options.defaultActionWhenConflictFound = InputMapper.ConflictResponse.Cancel;

        // Subscribe to events
        inputMapper.InputMappedEvent += OnInputMapped;
        inputMapper.StoppedEvent += OnStopped;
        inputMapper.ConflictFoundEvent += OnConflictFound;
    }

    void OnDisable()
    {
        Script_MenuEventsManager.OnExitMenu -= StartThrottlingInGame;

        // Make sure the input mapper is stopped first
        inputMapper.Stop();

        // Unsubscribe from events
        inputMapper.RemoveAllEventListeners();
    }
    
    void Update()
    {
        if (settingsInputManager.gameObject.activeInHierarchy && !isInputDisabled)
            settingsInputManager.HandleExitInput();
        
        if (isThrottledInGame)
        {
            timer -= Time.unscaledDeltaTime;
            
            if (timer < 0f)
                timer = 0f;
            
            if (timer == 0f)
                isThrottledInGame = false;
        }
        
        // Check to see if cursor is on Controller Select
        GameObject currentSelected = settingsEventSystem.currentSelectedGameObject;
        if (currentSelected != null && currentSelected == topCommonButton.gameObject)
            HandleControlsControllerSelect();
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
        Dev_Logger.Debug("ToControls()");

        overviewCanvasGroup.Close();
        
        controlsState = GetLastActiveController.type.ControllerTypeToControlsState();
        RenderControlsUI();
        
        controlsCanvasGroup.Open();
        UpdateControlKeyDisplays();

        // Set ControllerType Select as selected
        EventSystem.current.SetSelectedGameObject(controlsCanvasGroup.firstToSelect.gameObject);

        EnterMenuSFX();

        state = States.Controls;
    }

    private void HandleControlsControllerSelect()
    {
        if (MyPlayer.GetNegativeButtonDown(Const_KeyCodes.RWHorizontal))
        {
            controllerSelectArrows[0].SetTrigger(ClickTrigger);
            EnterMenuSFX();
            SwitchControllerType();
        }
        else if (MyPlayer.GetButtonDown(Const_KeyCodes.RWHorizontal))
        {
            controllerSelectArrows[1].SetTrigger(ClickTrigger);
            EnterMenuSFX();
            SwitchControllerType();
        }

        void SwitchControllerType()
        {
            // Disable controls this frame
            SetNavigationEnabled(false);

            if (controlsState == ControlsStates.Keyboard)
                controlsState = ControlsStates.Joystick;
            else
                controlsState = ControlsStates.Keyboard;

            StartCoroutine(WaitToRenderNewUI());
        }

        IEnumerator WaitToRenderNewUI()
        {
            yield return null;
            
            RenderControlsUI();
            UpdateControlKeyDisplays();
            EventSystem.current.SetSelectedGameObject(controlsCanvasGroup.firstToSelect.gameObject);
            SetNavigationEnabled(true);
        }
    }

    // UI Settings Overview: Graphics Button
    public void ToGraphics()
    {
        overviewCanvasGroup.Close();
        graphicsCanvasGroup.Open();
        
        systemController.ToGraphics();

        EnterMenuSFX();
        state = States.System;
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
            case (States.System):
                OpenOverview(1);
                ExitMenuSFX();
                
                Script_SaveSettingsControl.Instance.Save();
                
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
    
    public void RenderControlsUI()
    {
        switch (controlsState)
        {
            case (ControlsStates.Joystick):
                keyboardControlsCanvasGroup.Close();
                joystickControlsCanvasGroup.Open();
                bottomCommonButton.SetNavigation(newSelectOnUp: joystickLastButton);
                topCommonButton.SetNavigation(newSelectOnDown: joystickFirstButton);
                keyboardTMP.gameObject.SetActive(false);
                joystickTMP.gameObject.SetActive(true);
                break;
            default:
                keyboardControlsCanvasGroup.Open();
                joystickControlsCanvasGroup.Close();
                bottomCommonButton.SetNavigation(newSelectOnUp: keyboardLastButton);
                topCommonButton.SetNavigation(newSelectOnDown: keyboardFirstButton);
                keyboardTMP.gameObject.SetActive(true);
                joystickTMP.gameObject.SetActive(false);
                break;
        }
    }
    
    /// <summary>
    /// Update the RebindActionButton UIs with up-to-date Rewired control map values
    /// </summary>
    public void UpdateControlKeyDisplays()
    {
        UIRebindActions.ForEach(rebindKeyUI => {
            rebindKeyUI.UpdateBehaviorUIText();
        });

        UIRebindActionsJoystick.ForEach(rebindKeyUI => {
            rebindKeyUI.UpdateBehaviorUIText();
        });
    }

    public void OpenResetDefaultsSubmenu()
    {
        isRebindSubmenu = true;
        resetDefaultsSubmenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(resetDefaultsSubmenuChoices[0].gameObject);
        EnterSubmenuSFX();
    }

    public void OnInputFieldClicked(Script_UIRebindAction rebindActionButton)
    {
        Dev_Logger.Debug($"OnInputFieldClicked with rebindActionButton <{rebindActionButton}>");
        
        SetNavigationEnabled(false);
        
        StartCoroutine(StartListeningDelayed());

        IEnumerator StartListeningDelayed()
        {
            // Need to wait realtime, since Settings_Game Time.scale will be 0
            yield return new WaitForSecondsRealtime(WaitBeforeListeningTime);

            // Automatically protects joystick binds from keyboard input and vice-versa
            InputMapper.Context context = new InputMapper.Context()
            {
                actionId = rebindActionButton.ActionId,
                controllerMap = MyControllerMap,
                actionElementMapToReplace = MyControllerMap.GetFirstActionElementMapByMap(rebindActionButton.ActionId)
            };
            
            currentRebindActionButton = rebindActionButton;
            inputMapper.Start(context);
        }
    }
    
    // React to UIRebindAction mapping event
    private void OnInputMapped(InputMapper.InputMappedEventData data)
    {
        Dev_Logger.Debug($"Mapped {data.actionElementMap.actionDescriptiveName} with {data.actionElementMap.elementIdentifierName}");

        Script_SaveSettingsControl.Instance.Save();
        InputMappedSuccessSFX();
    }

    /// <summary>
    /// Default Map Category will check against Map Categories: Default, HotKeys, and UI. UI Extra will be ignored,
    /// allowing Space to be re-binded.
    /// </summary>
    private void OnConflictFound(InputMapper.ConflictFoundEventData data)
    {
        // Store the event data for use in user response
        conflictData = data;

        // In Rewired, protection of controls is handled on the Map Category level. To protect a set of System controls
        // from ever being re-assigned, uncheck the "User Assignable" checkbox in the Map Category
        // https://guavaman.com/projects/rewired/docs/HowTos.html#conflict-checking
        if (data.isProtected)
        {
            // the conflicting assignment was protected and cannot be replaced
            // Display some message to the user asking whether to cancel or add the new assignment.
            // Protected assignments cannot be replaced.
            // ...

            // If key was UICancel, cancel and don't display Error; treat as if exiting the input mapper
            if (MyPlayer.GetButtonDown(Const_KeyCodes.RWUICancel))
                ExitSubmenuSFX();
            // If move key
            else if (
                MyPlayer.GetButtonDown(Const_KeyCodes.RWHorizontal)
                || MyPlayer.GetNegativeButtonDown(Const_KeyCodes.RWHorizontal)
                || MyPlayer.GetButtonDown(Const_KeyCodes.RWVertical)
                || MyPlayer.GetNegativeButtonDown(Const_KeyCodes.RWVertical)
            )
                currentRebindActionButton.DisplayError(Script_UIRebindAction.ErrorTypes.Move);
            else if (
                MyPlayer.GetButtonDown(Const_KeyCodes.RWHotKey1)
                || MyPlayer.GetButtonDown(Const_KeyCodes.RWHotKey2)
                || MyPlayer.GetButtonDown(Const_KeyCodes.RWHotKey3)
                || MyPlayer.GetButtonDown(Const_KeyCodes.RWHotKey4)
                || MyPlayer.GetButtonDown(Const_KeyCodes.RWHotKey5)
                || MyPlayer.GetButtonDown(Const_KeyCodes.RWHotKey6)
                || MyPlayer.GetButtonDown(Const_KeyCodes.RWHotKey7)
                || MyPlayer.GetButtonDown(Const_KeyCodes.RWHotKey8)
                || MyPlayer.GetButtonDown(Const_KeyCodes.RWHotKey9)
            )
                currentRebindActionButton.DisplayError(Script_UIRebindAction.ErrorTypes.HotKey);
            // Otherwise, the conflict we can assume came from a UI event
            else
                currentRebindActionButton.DisplayError(Script_UIRebindAction.ErrorTypes.Menu);

            // ... After the user has made a decision, cancel polling
            conflictData.responseCallback(InputMapper.ConflictResponse.Cancel);
        }
        else
        {
            // Display some message to the user asking whether to cancel, replace the existing,
            // or add the new assignment.
            // ...

            currentRebindActionButton.DisplayError(Script_UIRebindAction.ErrorTypes.Taken);

            // ... After the user has made a decision, cancel polling
            conflictData.responseCallback(InputMapper.ConflictResponse.Cancel);
        }
    }

    // Fires when listening is stopped, either via canceling or conflict.
    private void OnStopped(InputMapper.StoppedEventData data)
    {
        Dev_Logger.Debug($"OnStopped() input mapper stopped");

        if (currentRebindActionButton != null)
            currentRebindActionButton.RebindCompleted();

        StartCoroutine(SetNavigationEnabledNextFrame());

        // Note: also must wait a frame to ensure IsInputDisabled remains disabled for the frame 
        // UICancel ButtonDown happens
        IEnumerator SetNavigationEnabledNextFrame()
        {
            yield return new WaitForSecondsRealtime(WaitBeforeListeningTime);

            SetNavigationEnabled(true);
            Dev_Logger.Debug($"{name} Setting navigation enabled after input mapper cancelled");
        }
    }

    // Update keybinding UI
    public void OnControllerChanged(ControllerStatusChangedEventArgs args)
    {
        // Settings not open
        if (!gameObject.activeInHierarchy)
            return;

        inputMapper.Stop();
        
        if (Script_SaveSettingsControl.Instance != null)
            Script_SaveSettingsControl.Instance.Load();

        RenderControlsUI();
        UpdateControlKeyDisplays();
    }

    private void InputMappedSuccessSFX() => Script_SFXManager.SFX.PlayUISuccessEdit();

    // Rewired Docs say disable UI maps (UI & UI Extra); disabling event system alone works too. This method is safer as
    // EventSystem.sendNavigationEvents prop reloads on scene whereas ControllerMap state remains from scene-to-scene.
    public void SetNavigationEnabled(bool isActive)
    {
        settingsEventSystem.sendNavigationEvents = isActive;
        
        // Set flag to prevent exit handling when input is disabled
        isInputDisabled = !isActive;
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

    protected void ExitSubmenuSFX() => Script_SFXManager.SFX.PlayExitSubmenuPencil();    

    protected void DullErrorSFX() => Script_SFXManager.SFX.PlayDullError();

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
