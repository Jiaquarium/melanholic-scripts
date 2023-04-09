using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using Rewired;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum RWActions
{
    Interact = 0,
    MaskCommand = 1,
    Inventory = 2,
    Speed = 3,
    Mask1 = 4,
    Mask2 = 5,
    Mask3 = 6,
    Mask4 = 7,
    Settings = 8,
    MoveHorizontalAxis = 9,
    MoveVerticalAxis = 10,
    MaskHorizontalAxis = 11,
    MaskVerticalAxis = 12
}

/// <summary>
/// Should only reference this instance as a Singleton (because Game will actually use
/// Start Scene's Player Input Manager)
/// 
/// NOTE: Script Execution Order: this is set before Default runtime
/// </summary>
public class Script_PlayerInputManager : MonoBehaviour
{
    public enum JoystickKnownState
    {
        None = 0,
        GamepadTemplate = 1,
        Unknown = 2
    }
    
    /// <summary>
    /// Note: DefaultJoystickDeadZone must match Default Input Behavior button dead zone (current: 0.25)
    /// Rewired Input Manager for isMoving and handleAnimation checks to work consistently.
    /// </summary>
    public const float DefaultJoystickDeadZone = 0.25f;
    public const float InputMapperUnknownJoystickDeadZone = 0.05f;
    public const float WINDOWS_InputMapperJoystickDeadZone = 0.0f;
    
    public const int KeyboardId = 0;
    public static int ControllerId = 0;

    public const string Category = "Default";
    public const string Layout = "Default";
    public const string uiCategory = "UI";
    // All UI actions to ignore for conflict checking, should go here (e.g. Keyboard space on UI can be overwritten
    // in the default map)
    public const string uiExtraCategory = "UI Extra";
    
    // Only for dev purposes to display ControllerId
    [SerializeField] private int devControllerIdDisplay;
    
    // Note: this is the Category Id shown in Rewired Editor UI, NOT the order in the list
    private const int DefaultMapCategoryId = 0;
    public const string UIExtraMapName = "UI Extra";

    // -------------------------------------------------------------------------------------  
    // Rewired Keyboard Action Ids
    private static List<string> KeyboardRebinds = new List<string>{
        Const_KeyCodes.RWInteract,
        Const_KeyCodes.RWInventory,
        Const_KeyCodes.RWMaskCommand,
        Const_KeyCodes.RWSpeed,
    };

    private static List<string> JoystickRebinds = new List<string>{
        Const_KeyCodes.RWInteract,
        Const_KeyCodes.RWInventory,
        Const_KeyCodes.RWMaskCommand,
        Const_KeyCodes.RWMask1,
        Const_KeyCodes.RWMask2,
        Const_KeyCodes.RWMask3,
        Const_KeyCodes.RWMask4,
        Const_KeyCodes.RWSpeed,
    };

    private static List<string> UnknownJoystickRebinds = new List<string>{
        Const_KeyCodes.RWInteract,
        Const_KeyCodes.RWInventory,
        Const_KeyCodes.RWMaskCommand,
        Const_KeyCodes.RWSpeed,
        Const_KeyCodes.RWHorizontal,
        Const_KeyCodes.RWVertical,
        Const_KeyCodes.RWUnknownControllerSettings,
        Const_KeyCodes.RWUnknownControllerMaskHz,
        Const_KeyCodes.RWUnknownControllerMaskVert,
    };

    // -------------------------------------------------------------------------------------  

    public static Script_PlayerInputManager Instance;

    public JoystickKnownState joystickKnownState;
    public JoystickKnownState lastJoystickKnownState;
     
    public Rewired.Player RewiredInput { get; set; }
    public Rewired.Controller GetLastActiveController => RewiredInput.controllers.GetLastActiveController();
    public bool IsJoystickConnected => ControllerId >= 0;
    public bool IsUnknownJoystick => joystickKnownState == JoystickKnownState.Unknown;

    void OnEnable()
    {
        ReInput.ControllerConnectedEvent += JoystickConnected;
        ReInput.ControllerDisconnectedEvent += JoystickConnected;
    }

    void OnDisable()
    {
        ReInput.ControllerConnectedEvent -= JoystickConnected;
        ReInput.ControllerDisconnectedEvent -= JoystickConnected;
    }
    
    void Awake()
    {        
        RewiredInput = Rewired.ReInput.players.GetPlayer(Script_Player.PlayerId);
        UpdateControllerId(out bool isSwitchedUnknownJoystick, out bool didSwitchJoystickLayout);
        
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

    void OnGUI()
    {
        if (Debug.isDebugBuild)
            devControllerIdDisplay = ControllerId;
    }

    // -------------------------------------------------------------------------------------  
    // Save/Load

    /// <summary>
    /// Save keyboardMapSaveData and joystickMapSaveData into respective fields
    /// </summary>
    public void Save(Model_SettingsData settingsData)
    {
        PlayerSaveData playerData = RewiredInput.GetSaveData(userAssignableMapsOnly: true);
        SaveControllerMaps(RewiredInput, playerData);

        void SaveControllerMaps(Player player, PlayerSaveData playerSaveData)
        {
            foreach (KeyboardMapSaveData keyboardMapSaveData in playerSaveData.keyboardMapSaveData)
            {
                // Only look for Default map (Category Id as shown in Rewired UI, NOT order in list)
                if (keyboardMapSaveData.categoryId == DefaultMapCategoryId)
                {
                    Dev_Logger.Debug($"Keyboard Map Save Data Id: {keyboardMapSaveData.categoryId} {keyboardMapSaveData}");
                    settingsData.rewiredKeyboardMapDefault = keyboardMapSaveData.keyboardMap.ToXmlString();
                }
            }

            if (IsJoystickConnected)
            {
                foreach (JoystickMapSaveData joystickMapSaveData in playerSaveData.joystickMapSaveData)
                {
                    // Only look for Default map (Category Id as shown in Rewired UI, NOT order in list)
                    if (joystickMapSaveData.categoryId == DefaultMapCategoryId)
                    {
                        bool isFirstJoystickSaveData = joystickMapSaveData.joystick == RewiredInput.controllers.Joysticks[0];
                        Dev_Logger.Debug($"isFirstJoystickSaveData: {isFirstJoystickSaveData}");

                        if (!isFirstJoystickSaveData)
                        {
                            Dev_Logger.Debug($"Skipping this controller save data because it is not the first connected");
                            continue;
                        }
                        
                        Dev_Logger.Debug($"Joystick Map Save Data Id: {joystickMapSaveData.categoryId} {joystickMapSaveData}");
                        settingsData.rewiredJoystickMapDefault = joystickMapSaveData.joystickMap.ToXmlString();

                        // Only update if currently using a connected joystick
                        if (
                            joystickKnownState == JoystickKnownState.GamepadTemplate
                            || joystickKnownState == JoystickKnownState.Unknown
                        )
                            settingsData.savedJoystickMapKnownState = joystickKnownState;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Try loading maps for keyboard and joystick.
    /// Note: this should only be called in Start() or later, since ControllerId is set in Awake()
    /// </summary>
    public void Load(Model_SettingsData settingsData)
    {
        try
        {
            LoadKeyboard(settingsData);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{name} Loading Default Keyboard bindings. Failed loading rebinds with the following error: {e}");

            SetKeyboardDefaults();
            UpdateKeyBindingUIs();
        }

        try
        {
            // ControllerId will be set in Awake
            LoadJoystick(settingsData);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{name} Loading Default Joystick bindings. Failed loading rebinds with the following error: {e}");

            SetJoystickDefaults();
            UpdateKeyBindingUIs();
        }
    }

    private void LoadKeyboard(Model_SettingsData settingsData)
    {
        string xml = settingsData.rewiredKeyboardMapDefault;

        if (string.IsNullOrEmpty(xml))
            return;

        ControllerMap controllerMap = ControllerMap.CreateFromXml(ControllerType.Keyboard, xml);
        Controller controller = ReInput.controllers.GetController(ControllerType.Keyboard, KeyboardId);

        if (controllerMap == null || controller == null)
            return;
        
        foreach (string keyboardRebind in KeyboardRebinds)
        {
            int targetActionId = keyboardRebind.RWActionNamesToId();
            HandleReplaceKeybindMapping(targetActionId, controller, controllerMap);
        }
    }

    private void LoadJoystick(Model_SettingsData settingsData)
    {
        string xml = settingsData.rewiredJoystickMapDefault;

        if (string.IsNullOrEmpty(xml))
            return;

        ControllerMap controllerMap = ControllerMap.CreateFromXml(ControllerType.Joystick, xml);
        Controller controller = ReInput.controllers.GetController(ControllerType.Joystick, ControllerId);

        if (controllerMap == null || controller == null)
            return;
        
        // Handle current joystick is known and save data is known
        if (controller.IsJoystickSupported())
        {
            // Check for mismatch in save data (if saved joystick map was for an Unknown joystick)
            if (
                settingsData.savedJoystickMapKnownState
                != Script_PlayerInputManager.JoystickKnownState.GamepadTemplate
            )
            {
                OnJoystickTypeMismatch();
                return;
            }

            foreach (string joystickRebind in JoystickRebinds)
            {
                int targetActionId = joystickRebind.RWActionNamesToId();
                HandleReplaceKeybindMapping(targetActionId, controller, controllerMap);
            }
        }
        else
        {
            // Check for mismatch in save data (if saved joystick map was for an Unknown joystick)
            if (
                settingsData.savedJoystickMapKnownState
                != Script_PlayerInputManager.JoystickKnownState.Unknown
            )
            {
                OnJoystickTypeMismatch();
                return;
            }
            
            foreach (string unknownJoystickRebind in UnknownJoystickRebinds)
            {
                int targetActionId = unknownJoystickRebind.RWActionNamesToId();
                var myControllerMap = RewiredInput.controllers.maps.GetMap(
                    controller.type,
                    controller.id,
                    Category,
                    Layout
                );
                HandleReplaceCreateKeybindMapping(targetActionId, myControllerMap, controllerMap);
            }
        }

        void OnJoystickTypeMismatch()
        {
            SetJoystickTemporaryDefaults();
        }

        void HandleReplaceCreateKeybindMapping(
            int targetActionId,
            ControllerMap myControllerMap,
            ControllerMap loadedControllerMap
        )
        {
            var aemToInjectFrom = loadedControllerMap.GetFirstActionElementMapByMap(targetActionId);

            if (aemToInjectFrom == null)
                return;

            HandleReplaceCreateAssignment(targetActionId, myControllerMap, aemToInjectFrom);
            
            // Handle duplicating keybinds for the UI Extra auto-binds like inputMapper.OnInputMapped does
            HandleUIExtraAutoBinds(
                targetActionId, myControllerMap.controllerType, aemToInjectFrom
            );
        }
    }

    private void HandleReplaceKeybindMapping(
        int targetActionId,
        Controller controller,
        ControllerMap controllerMap
    )
    {
        ActionElementMap aemToReplace = null;
        aemToReplace = RewiredInput.GetFirstActionElementMapByActionId(controller, targetActionId);

        if (aemToReplace == null)
            return;
        
        // Get first AEM with matching ActionId
        foreach (var aem in controllerMap.ElementMapsWithAction(targetActionId))
        {
            if (aem.actionId == targetActionId)
            {
                ReplaceAssignment(controllerMap.controllerType, aem, aemToReplace, aem.actionId);
                
                // Handle duplicating keybinds for the UI Extra auto-binds like inputMapper.OnInputMapped does
                HandleUIExtraAutoBinds(targetActionId, controller.type, aem);
                return;
            }
        }
    }

    public void SetKeybindMappingInversion(
        int targetActionId,
        Controller controller,
        bool invert
    )
    {
        ActionElementMap aemToReplace = RewiredInput.GetFirstActionElementMapByActionId(controller, targetActionId);
        
        if (aemToReplace == null)
            return;

        ActionElementMap newInvertedAem = new ActionElementMap(aemToReplace);
        newInvertedAem.invert = invert;
        ActionElementMap replacedAem = ReplaceAssignment(controller.type, newInvertedAem, aemToReplace, targetActionId);

        HandleUIExtraAutoBinds(targetActionId, controller.type, replacedAem);
    }

    // If the targetActionId is found, replace that ActionElementMap; otherwise, create
    // a new one.
    public void HandleReplaceCreateAssignment(
        int targetActionId,
        ControllerMap controllerMap,
        ActionElementMap aemToInjectFrom
    )
    {
        ActionElementMap aemToReplace = null;
        aemToReplace = controllerMap.GetFirstActionElementMapByMap(targetActionId);

        if (aemToReplace != null)
        {
            ReplaceAssignment(controllerMap.controllerType, aemToInjectFrom, aemToReplace, targetActionId);
        }
        else
        {
            CreateAssignment(controllerMap, aemToInjectFrom, targetActionId);
        }
    }

    private ActionElementMap ReplaceAssignment(
        ControllerType controllerType,
        ActionElementMap aemToInjectFrom,
        ActionElementMap aemToReplace,
        int targetActionId
    )
    {
        ElementAssignment assignment = new ElementAssignment(
            controllerType,
            aemToInjectFrom.elementType,
            aemToInjectFrom.elementIdentifierId,
            aemToInjectFrom.axisRange,
            aemToInjectFrom.keyCode,
            aemToInjectFrom.modifierKeyFlags,
            targetActionId,
            aemToInjectFrom.axisContribution,
            aemToInjectFrom.invert
        );

        // Must declare elementMapId as ActionElementMap's Id you are trying to replace
        // https://guavaman.com/projects/rewired/docs/api-reference/html/Overload_Rewired_ControllerMap_ReplaceElementMap.htm
        assignment.elementMapId = aemToReplace.id;
        bool didReplace = aemToReplace.controllerMap.ReplaceElementMap(assignment, out aemToReplace);
        
        // If any replacement fails, the whole map should fail. Set maps to default.
        if (!didReplace)
            throw new Exception($"Replacing AEM for {aemToReplace.actionDescriptiveName} failed, setting map to default!");

        Dev_Logger.Debug($"ReplaceAssignment didReplace {didReplace} {aemToReplace.actionDescriptiveName} aemToReplace: {aemToReplace}");   
        return aemToReplace;
    }

    private ActionElementMap CreateAssignment(
        ControllerMap controllerMap,
        ActionElementMap aemToInjectFrom,
        int targetActionId
    )
    {
        // Handle unknown controller when creating a new binding
        ElementAssignment assignment = new ElementAssignment(
            controllerMap.controllerType,
            aemToInjectFrom.elementType,
            aemToInjectFrom.elementIdentifierId,
            aemToInjectFrom.axisRange,
            aemToInjectFrom.keyCode,
            aemToInjectFrom.modifierKeyFlags,
            targetActionId,
            aemToInjectFrom.axisContribution,
            aemToInjectFrom.invert
        );

        bool didCreate = controllerMap.CreateElementMap(assignment, out ActionElementMap aemToCreate);

        // If any creation fails, the whole map should fail. Set maps to default.
        if (!didCreate)
            throw new Exception($"Create AEM for targetActionId {targetActionId} failed, setting map to default!");
        
        Dev_Logger.Debug($"CreateAssignment didCreate {didCreate} {aemToCreate.actionDescriptiveName} aemToCreate: {aemToCreate}");
        return aemToCreate;
    }

    /// <summary>
    /// This will delete the settings file, but on Back, a new one will be created, so
    /// volume data will be kept.
    /// </summary>
    public void SetDefault()
    {
        // Delete keybindings file
        if(File.Exists(FilePath))
            File.Delete(FilePath);
        
        SetKeyboardDefaults();
        SetJoystickDefaults();
        
        UpdateKeyBindingUIs();
    }

    public void SetJoystickTemporaryDefaults()
    {
        Dev_Logger.Debug($"{name} Setting joystick map to Default (temporary until saved)");
        SetJoystickDefaults();
    }

    public void HandleUIExtraAutoBinds(
        int targetActionId,
        ControllerType controllerType,
        ActionElementMap aemToInjectFrom
    )
    {
        try
        {
            var playerInputManager = Script_PlayerInputManager.Instance;
            
            // If changing Interact, also change UISubmit in UI Extra map
            if (targetActionId == Const_KeyCodes.RWInteract.RWActionNamesToId())
            {
                // Handle keyboard
                if (controllerType == ControllerType.Keyboard)
                {
                    playerInputManager.HandleReplaceCreateAssignment(
                        Const_KeyCodes.RWUISubmit.RWActionNamesToId(),
                        RewiredInput.controllers.maps.GetMap<KeyboardMap>(
                            Script_PlayerInputManager.KeyboardId,
                            Script_PlayerInputManager.UIExtraMapName,
                            Script_PlayerInputManager.Layout
                        ),
                        aemToInjectFrom
                    );
                }
                // Handle joystick
                else if (controllerType == ControllerType.Joystick)
                {
                    playerInputManager.HandleReplaceCreateAssignment(
                        Const_KeyCodes.RWUISubmit.RWActionNamesToId(),
                        RewiredInput.controllers.maps.GetMap<JoystickMap>(
                            Script_PlayerInputManager.ControllerId,
                            Script_PlayerInputManager.UIExtraMapName,
                            Script_PlayerInputManager.Layout
                        ),
                        aemToInjectFrom
                    );
                }
            }
            // If changing Inventory on Joystick, also change UICancel in UI Extra map
            else if (targetActionId == Const_KeyCodes.RWInventory.RWActionNamesToId())
            {
                if (controllerType == ControllerType.Joystick)
                {
                    playerInputManager.HandleReplaceCreateAssignment(
                        Const_KeyCodes.RWUICancel.RWActionNamesToId(),
                        RewiredInput.controllers.maps.GetMap<JoystickMap>(
                            Script_PlayerInputManager.ControllerId,
                            Script_PlayerInputManager.UIExtraMapName,
                            Script_PlayerInputManager.Layout
                        ),
                        aemToInjectFrom
                    );

                    // Handle auto binding Backspace for both Known and Unknown Joysticks
                    playerInputManager.HandleReplaceCreateAssignment(
                        Const_KeyCodes.RWBackspace.RWActionNamesToId(),
                        RewiredInput.controllers.maps.GetMap<JoystickMap>(
                            Script_PlayerInputManager.ControllerId,
                            Script_PlayerInputManager.UIExtraMapName,
                            Script_PlayerInputManager.Layout
                        ),
                        aemToInjectFrom
                    );
                }
            }
            else if (targetActionId == Const_KeyCodes.RWHorizontal.RWActionNamesToId())
            {
                if (
                    controllerType == ControllerType.Joystick
                    && playerInputManager.IsUnknownJoystick
                )
                {
                    // Handle auto binding UIHorizontal for Unknown Controller
                    playerInputManager.HandleReplaceCreateAssignment(
                        Const_KeyCodes.RWUIHorizontal.RWActionNamesToId(),
                        RewiredInput.controllers.maps.GetMap<JoystickMap>(
                            Script_PlayerInputManager.ControllerId,
                            Script_PlayerInputManager.UIExtraMapName,
                            Script_PlayerInputManager.Layout
                        ),
                        aemToInjectFrom
                    );
                }
            }
            else if (targetActionId == Const_KeyCodes.RWVertical.RWActionNamesToId())
            {
                if (
                    controllerType == ControllerType.Joystick
                    && playerInputManager.IsUnknownJoystick
                )
                {
                    // Handle auto binding UIVertical for Unknown Controller
                    playerInputManager.HandleReplaceCreateAssignment(
                        Const_KeyCodes.RWUIVertical.RWActionNamesToId(),
                        RewiredInput.controllers.maps.GetMap<JoystickMap>(
                            Script_PlayerInputManager.ControllerId,
                            Script_PlayerInputManager.UIExtraMapName,
                            Script_PlayerInputManager.Layout
                        ),
                        aemToInjectFrom
                    );
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{name} Failed to also auto rebind actionId {targetActionId} with the following error: \n{e}");
        }
    }

    // -------------------------------------------------------------------------------------  

    private void SetKeyboardDefaults() => RewiredInput.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);
    
    private void SetJoystickDefaults() => RewiredInput.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);

    public void UpdateKeyBindingUIs()
    {
        Script_SettingsController.Instance.UpdateControlKeyDisplays();
    }
    
    private string FilePath => $"{Script_SaveGameControl.path}/{Script_Utils.SettingsFile}";

    private void JoystickConnected(Rewired.ControllerStatusChangedEventArgs args)
    {
        SetupDefaultJoystickDeadzones();

        bool didChange = UpdateControllerId(out bool isSwitchedUnknownJoystick, out bool didSwitchJoystickLayout);

        Dev_Logger.Debug($"ControllerConnectedEvent: JoystickConnected didChange {didChange} new ControllerId {ControllerId}");

        // Load joystick maps
        if (Script_SaveSettingsControl.Instance != null)
            Script_SaveSettingsControl.Instance.Load();

        // If changed from Joystick Known to Unknown or vise versa, set all to default to avoid tangling bindings
        if (isSwitchedUnknownJoystick)
            Script_PlayerInputManager.Instance.SetJoystickTemporaryDefaults();
        
        // Update rebinding UI and stop input mapper if it was listening
        Script_SettingsController.Instance.OnControllerChanged(args, didSwitchJoystickLayout);
    }

    public void SetupDefaultJoystickDeadzones()
    {
        Dev_Logger.Debug($"Setting joystick deadzones to DEFAULT {DefaultJoystickDeadZone}");
        SetJoystickDeadZone(DefaultJoystickDeadZone);
    }

    public void SetupInputMapperUnknownJoystickDeadzones()
    {
        Dev_Logger.Debug($"Setting joystick deadzones to INPUTMAPPER {InputMapperUnknownJoystickDeadZone}");
        SetJoystickDeadZone(InputMapperUnknownJoystickDeadZone);
    }
    
    /// <summary>
    /// Based on Rewired/Examples ControlRemapping1
    /// </summary>
    private void SetJoystickDeadZone(float newDeadZone)
    {
        if (RewiredInput == null)
            return;
        
        foreach (Joystick joystick in RewiredInput.controllers.Joysticks)
        {
            if (joystick == null)
            {
                Dev_Logger.Debug($"Null joystick {joystick}");
                continue;
            }
            
            CalibrationMap calibrationMap = joystick.calibrationMap;

            if (calibrationMap == null)
                continue;
            
            IList<ControllerElementIdentifier> axisIdentifiers = joystick.AxisElementIdentifiers;
            for (int i = 0; i < axisIdentifiers.Count; i++)
            {
                int axisIndex = joystick.GetAxisIndexById(axisIdentifiers[i].id);
                AxisCalibration axis = calibrationMap.GetAxis(axisIndex);
                axis.deadZone = newDeadZone;
                
                Dev_Logger.Debug($"SET {joystick.name} axisIndex {axisIndex} deadZone to {axis.deadZone}");
            }
        }   
    }

    // Keep controllerId up to date with first joystick only
    public bool UpdateControllerId(out bool isSwitchedUnknownJoystick, out bool didSwitchJoystickLayout)
    {
        int originalId = ControllerId;
        isSwitchedUnknownJoystick = false;
        didSwitchJoystickLayout = false;
        
        if (RewiredInput.controllers.joystickCount > 0)
        {
            Joystick firstJoystick = RewiredInput.controllers.Joysticks[0];
            ControllerId = firstJoystick.id;

            // Update controller state
            lastJoystickKnownState = joystickKnownState;
            if (firstJoystick.IsJoystickSupported())
                joystickKnownState = JoystickKnownState.GamepadTemplate;
            else
                joystickKnownState = JoystickKnownState.Unknown;
        }
        else
        {
            ControllerId = -1;
            lastJoystickKnownState = joystickKnownState;
            joystickKnownState = JoystickKnownState.None;
        }

        // Signal that there's been a switch from Unknown/Known joystick and the joystick rebind screen changed layout
        if (
            (joystickKnownState == JoystickKnownState.GamepadTemplate && lastJoystickKnownState == JoystickKnownState.Unknown)
            || (joystickKnownState == JoystickKnownState.Unknown && lastJoystickKnownState == JoystickKnownState.GamepadTemplate)
        )
        {
            isSwitchedUnknownJoystick = true;
            didSwitchJoystickLayout = true;
        }
        // Signal the joystick rebind screen changed layout
        else if (
            (joystickKnownState == JoystickKnownState.Unknown && lastJoystickKnownState == JoystickKnownState.None)
            || (joystickKnownState == JoystickKnownState.None && lastJoystickKnownState == JoystickKnownState.Unknown)    
        )
        {
            didSwitchJoystickLayout = true;
        }

        return ControllerId != originalId;
    }

    public static void SetRewiredUIMapsEnabled(bool isEnabled)
    {
        var rewiredInput = ReInput.players.GetPlayer(Script_Player.PlayerId);
        rewiredInput.controllers.maps.SetMapsEnabled(isEnabled, uiCategory);
        rewiredInput.controllers.maps.SetMapsEnabled(isEnabled, uiExtraCategory);
    }

    public void Setup()
    {
        SetupDefaultJoystickDeadzones();   
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

            if (GUILayout.Button("Set Controller Dead Zone 1f"))
            {
                t.SetupDefaultJoystickDeadzones();
            }

            if (GUILayout.Button("Set Controller Dead Zone 0f"))
            {
                t.SetupDefaultJoystickDeadzones();
            }

            if (GUILayout.Button("Set Mask Vert Invert True"))
            {
                t.SetKeybindMappingInversion(
                    Const_KeyCodes.RWVertical.RWActionNamesToId(),
                    ReInput.controllers.GetController(ControllerType.Joystick, ControllerId),
                    true
                );
            }
        }
    }
#endif
}
