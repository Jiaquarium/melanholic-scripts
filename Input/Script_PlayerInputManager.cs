using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;
using System.IO;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Should only reference this instance as a Singleton (because Game will actually use
/// Start Scene's Player Input Manager)
/// </summary>
public class Script_PlayerInputManager : MonoBehaviour
{
    public static Script_PlayerInputManager Instance;
    
    [SerializeField] private PlayerInput playerInput;
    
    /// <summary>
    /// When adding addt'l rebindable keys, update this field as well as:
    /// - Save/Load the model
    /// - SettingsController.UIRebindActions
    /// This also affects the exclusion keys during RebindingOperation.
    /// </summary>
    /// <typeparam name="string"></typeparam>
    private List<string> rebindableActionNames = new List<string>(){
        "Interact", // TBD Const_KeyCodes.Interact,
        "Inventory", // TBD Const_KeyCodes.Inventory,
        "MaskEffect", // TBD Const_KeyCodes.MaskEffect,
        "Speed", // TBD Const_KeyCodes.Speed,
    };

    public PlayerInput MyPlayerInput { get => playerInput; }
    public Rewired.Player RewiredInput { get; set; }
    public Rewired.Controller GetLastActiveController => RewiredInput.controllers.GetLastActiveController();

    void OnEnable()
    {
        Rewired.ReInput.ControllerConnectedEvent += JoystickConnected;
    }

    void OnDisable()
    {
        Rewired.ReInput.ControllerConnectedEvent -= JoystickConnected;
    }
    
    void Awake()
    {        
        RewiredInput = Rewired.ReInput.players.GetPlayer(Script_Player.PlayerId);
        
        if (Instance == null)
        {
            // Persist through scenes
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Get all current InputActions for the declared rebindable actions  
    /// </summary>
    public List<InputAction> GetRebindableActions()
    {
        var inputActions = new List<InputAction>();

        foreach (var actionName in rebindableActionNames)
        {
            InputAction inputAction = MyPlayerInput.actions.FindActionMap(Const_KeyCodes.PlayerMap).FindAction(actionName);
            inputActions.Add(inputAction);
        }

        return inputActions;
    }

    /// <summary>
    /// Fetch override paths from the Player Input module and save into settings data
    /// </summary>
    public void Save(Model_SettingsData settingsData)
    {
        // Make List of nulls to populate with override paths, if any
        List<string> overrides = new List<string>(new string[rebindableActionNames.Count]);
        var playerMap = MyPlayerInput.actions.FindActionMap(Const_KeyCodes.PlayerMap);
        
        for (var i = 0; i < rebindableActionNames.Count; i++)
        {
            var actionName = rebindableActionNames[i];
            InputAction inputAction = playerMap.FindAction(actionName);
            
            int controlBindingIndex = GetFirstControlBindingIndex(inputAction);
            var overridePath = inputAction.bindings[controlBindingIndex].overridePath;

            // Check & validate the control path
            var isValidControl = GetKeyboardControl(overridePath) != null;
            
            Dev_Logger.Debug($"GetKeyboardControl: {GetKeyboardControl(overridePath)}, isValidControl: {isValidControl}");

            if (!String.IsNullOrEmpty(overridePath) && isValidControl)
            {
                overrides[i] = overridePath;
                MatchUIActions(actionName, overridePath);
            }
        }
        
        Model_KeyBindsData keyBinds = new Model_KeyBindsData(
            _Interact: overrides[0],
            _Inventory: overrides[1],
            _MaskEffect: overrides[2],
            _Speed: overrides[3]
        );

        settingsData.keyBindsData = keyBinds;
    }

    /// <summary>
    /// Apply saved overrides to the Input module.
    /// 
    /// Note: JsonUtility will take care of building the model with the proper fields (e.g. if a player
    /// changes the Interact field key to "Interactz", it will skip the value).
    /// We use GetKeyboardControl to then validate the given path.
    /// </summary>
    public void Load(Model_SettingsData settingsData)
    {
        Model_KeyBindsData keyBinds = settingsData?.keyBindsData;
        
        if (keyBinds == null)
        {
            Dev_Logger.Debug("Key Rebinds settings null");
            return;
        }
        
        var playerMap = MyPlayerInput.actions.FindActionMap(Const_KeyCodes.PlayerMap);
        
        var keyBindsPaths = new List<string>()
        {
            keyBinds.Interact,
            keyBinds.Inventory,
            keyBinds.MaskEffect,
            keyBinds.Speed
        };
        
        for (var i = 0; i < keyBindsPaths.Count; i++)
        {
            var actionName = rebindableActionNames[i];
            var loadedOverridePath = keyBindsPaths[i];
            InputAction inputAction = playerMap.FindAction(actionName);

            // Check & validate the control path
            var isValidControl = GetKeyboardControl(keyBindsPaths[i]) != null;
            
            Dev_Logger.Debug($"GetKeyboardControl: {GetKeyboardControl(keyBindsPaths[0])}, isValidControl: {isValidControl}");

            if (!String.IsNullOrEmpty(loadedOverridePath) && isValidControl)
            {
                // Apply override to only the very first Control.
                int controlBindingIndex = GetFirstControlBindingIndex(inputAction);
                inputAction.ApplyBindingOverride(
                    controlBindingIndex,
                    new InputBinding { overridePath = loadedOverridePath}
                );

                MatchUIActions(actionName, loadedOverridePath);
            }
        }
    }

    /// <summary>
    /// For some player Actions, the UI should have an up to date equivalent
    /// such as with the following:
    /// - Interact (Player changing Interact / Confirm should expect this also be confirm in UI)
    /// </summary>
    /// <param name="actionName">Interact, MaskEffect, etc.</param>
    /// <param name="path">override path</param>
    private void MatchUIActions(string actionName, string path)
    {
        switch (actionName)
        {
            // TBD case Const_KeyCodes.Interact:
            case "Interact":
                InputAction inputAction = MyPlayerInput.actions.FindActionMap(Const_KeyCodes.UIMap)
                    .FindAction("Submit");
                
                int controlBindingIndex = GetFirstControlBindingIndex(inputAction);
                inputAction.ApplyBindingOverride(
                    controlBindingIndex,
                    new InputBinding { overridePath = path}
                );
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// This will delete the settings file, but on Back, a new one will be created, so
    /// volume data will be kept.
    /// </summary>
    public void SetDefault()
    {
        MyPlayerInput.actions.FindActionMap(Const_KeyCodes.PlayerMap).RemoveAllBindingOverrides();
        MyPlayerInput.actions.FindActionMap(Const_KeyCodes.UIMap).RemoveAllBindingOverrides();
        
        // Delete keybindings file
        if(File.Exists(FilePath))
            File.Delete(FilePath);
        
        UpdateKeyBindingUIs();
    }

    public void UpdateKeyBindingUIs()
    {
        Script_SettingsController.Instance.UpdateControlKeyDisplays();
    }

    public static int GetFirstControlBindingIndex(InputAction inputAction) => 
        inputAction.GetBindingIndexForControl(inputAction.controls[0]);

    // Note: Do not call every frame, ToHumanReadableString's performance is very slow.
    // Note: only returns the first binding control path.
    // https://github.com/UnityTechnologies/InputSystem_Warriors/blob/056e74b1701f3bd2f218a34a46d6d2cc1e167a78/InputSystem_Warriors_Project/Assets/Scripts/Behaviours/UI/UIRebindActionBehaviour.cs#L116
    public string GetHumanReadableBindingPath(string actionName, string actionMap = Const_KeyCodes.PlayerMap)
    {
        var inputAction = MyPlayerInput.actions.FindActionMap(actionMap).FindAction(actionName);
        
        int controlBindingIndex = inputAction.GetBindingIndexForControl(inputAction.controls[0]);
        string path = InputControlPath.ToHumanReadableString(
            inputAction.bindings[controlBindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
        return path.ToUpper();
    }

    public InputControl GetKeyboardControl(string path)
    {
        var control = InputControlPath.TryFindControl(Keyboard.current, path);

        return control;
    }
    
    private string FilePath => $"{Script_SaveGameControl.path}/{Script_Utils.SettingsFile}";

    private void JoystickConnected(Rewired.ControllerStatusChangedEventArgs args)
    {
        // If this is Game scene and Player exists, force setting Deadzones
        if (SceneManager.GetActiveScene().name == Script_SceneManager.GameScene)
        {
            if (Script_Game.Game != null)
            {
                var player = Script_Game.Game.GetPlayer();
                if (player != null)
                    player.SetupRewiredDefaults();
            }
        }
    }

    public void Setup()
    {
        MyPlayerInput.actions.FindActionMap(Const_KeyCodes.PlayerMap).Enable();
        MyPlayerInput.actions.FindActionMap(Const_KeyCodes.UIMap).Enable();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_PlayerInputManager))]
    public class Script_PlayerInputManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_PlayerInputManager t = (Script_PlayerInputManager)target;
            
            if (GUILayout.Button("Set Default"))
            {
                t.SetDefault();
            }
        }
    }
#endif
}
