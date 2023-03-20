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
}

/// <summary>
/// Should only reference this instance as a Singleton (because Game will actually use
/// Start Scene's Player Input Manager)
/// 
/// NOTE: Script Execution Order: this is set before Default runtime
/// </summary>
public class Script_PlayerInputManager : MonoBehaviour
{
    /// <summary>
    /// Note: DefaultJoystickDeadZone needs to be the same as Input Behavior dead zone for buttons in
    /// Rewired Input Manager for isMoving and handleAnimation checks to work consistently.
    /// </summary>
    public static float DefaultJoystickDeadZone = 0.4f;
    
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

    // -------------------------------------------------------------------------------------  

    public static Script_PlayerInputManager Instance;
    
    public Rewired.Player RewiredInput { get; set; }
    public Rewired.Controller GetLastActiveController => RewiredInput.controllers.GetLastActiveController();
    public bool IsJoystickConnected => ControllerId >= 0;

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
        UpdateControllerId();
        
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

            foreach (JoystickMapSaveData joystickMapSaveData in playerSaveData.joystickMapSaveData)
            {
                // Only look for Default map (Category Id as shown in Rewired UI, NOT order in list)
                if (joystickMapSaveData.categoryId == DefaultMapCategoryId)
                {
                    Dev_Logger.Debug($"Joystick Map Save Data Id: {joystickMapSaveData.categoryId} {joystickMapSaveData}");
                    settingsData.rewiredJoystickMapDefault = joystickMapSaveData.joystickMap.ToXmlString();
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
            Debug.LogError($"{name} Keyboard keybinds loading failed with the following error: {e}");

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
            Debug.LogError($"{name} Joystick keybinds loading failed with the following error: {e}");

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
            ReplaceKeybindMapping(targetActionId, controller, controllerMap);
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
        
        foreach (string joystickRebind in JoystickRebinds)
        {
            int targetActionId = joystickRebind.RWActionNamesToId();
            ReplaceKeybindMapping(targetActionId, controller, controllerMap);
        }
    }

    private void ReplaceKeybindMapping(
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
                ElementAssignment assignment = new ElementAssignment(
                    controllerMap.controllerType,
                    aem.elementType,
                    aem.elementIdentifierId,
                    aem.axisRange,
                    aem.keyCode,
                    aem.modifierKeyFlags,
                    aem.actionId,
                    aem.axisContribution,
                    aem.invert
                );

                // Must declare elementMapId as ActionElementMap's Id you are trying to replace
                // https://guavaman.com/projects/rewired/docs/api-reference/html/Overload_Rewired_ControllerMap_ReplaceElementMap.htm
                assignment.elementMapId = aemToReplace.id;
                bool didReplace = aemToReplace.controllerMap.ReplaceElementMap(assignment, out aemToReplace);
                
                // If any replacement fails, the whole map should fail. Set maps to default.
                if (!didReplace)
                {
                    throw new Exception($"Keybind for {aem.actionDescriptiveName} failed, setting map to default!");
                }
                
                Dev_Logger.Debug($"didReplace {didReplace} {aem.actionDescriptiveName} aemToReplace: {aemToReplace}");
                return;
            }
        }
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
        SetupRewiredDefaults();

        bool didChange = UpdateControllerId();

        Dev_Logger.Debug($"ControllerConnectedEvent: JoystickConnected didChange {didChange} new ControllerId {ControllerId}");

        // Update rebinding UI and stop input mapper if it was listening
        Script_SettingsController.Instance.OnControllerChanged(args);
    }

    public void SetupRewiredDefaults()
    {
        SetJoystickDeadZone(DefaultJoystickDeadZone);
        
        /// <summary>
        /// Based on Rewired/Examples ControlRemapping1
        /// </summary>
        void SetJoystickDeadZone(float newDeadZone)
        {
            foreach (Joystick joystick in RewiredInput.controllers.Joysticks)
            {
                if (joystick == null)
                    continue;
                
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
    }

    // Keep controllerId up to date with first joystick only
    public bool UpdateControllerId()
    {
        int originalId = ControllerId;
        
        if (RewiredInput.controllers.joystickCount > 0)
            ControllerId = RewiredInput.controllers.Joysticks[0].id;
        else
            ControllerId = -1;

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
        SetupRewiredDefaults();   
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
                t.SetupRewiredDefaults();
            }

            if (GUILayout.Button("Set Controller Dead Zone 0f"))
            {
                t.SetupRewiredDefaults();
            }
        }
    }
#endif
}
