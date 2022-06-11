using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        Const_KeyCodes.Interact,
        Const_KeyCodes.Inventory,
        Const_KeyCodes.MaskEffect,
        Const_KeyCodes.Speed,
    };

    public PlayerInput MyPlayerInput { get => playerInput; }

    public void Awake()
    {
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

    public void Save()
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
            
            Debug.Log($"GetKeyboardControl: {GetKeyboardControl(overridePath)}, isValidControl: {isValidControl}");

            if (!String.IsNullOrEmpty(overridePath) && isValidControl)
            {
                overrides[i] = overridePath;
                MatchUIActions(actionName, overridePath);
            }
        }
        
        var keyBinds = new Model_KeyBindsData(
            _Interact: overrides[0],
            _Inventory: overrides[1],
            _MaskEffect: overrides[2],
            _Speed: overrides[3]
        );

        string json = JsonUtility.ToJson(keyBinds);
        File.WriteAllText(FilePath, json);
    }

    /// <summary>
    /// JsonUtility will take care of building the model with the proper fields (e.g. if a player
    /// changes the Interact field to Interactz, it will skip the value).
    /// We use GetKeyboardControl to then validate the given path.
    /// </summary>
    public void Load()
    {
        if(!File.Exists(FilePath))
            return;
        
        Model_KeyBindsData keyBinds = JsonUtility.FromJson<Model_KeyBindsData>(File.ReadAllText(FilePath));
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
            
            Debug.Log($"GetKeyboardControl: {GetKeyboardControl(keyBindsPaths[0])}, isValidControl: {isValidControl}");

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
            case Const_KeyCodes.Interact:
                InputAction inputAction = MyPlayerInput.actions.FindActionMap(Const_KeyCodes.UIMap)
                    .FindAction(Const_KeyCodes.UISubmit);
                
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
    
    private string FilePath => $"{Script_SaveGameControl.path}/{Script_Utils.KeyRebindsFile()}";

    /// <summary>
    /// Note: Can only be called after SettingsController is SetUp.
    /// </summary>
    public void Setup()
    {
        MyPlayerInput.actions.FindActionMap(Const_KeyCodes.PlayerMap).Enable();
        MyPlayerInput.actions.FindActionMap(Const_KeyCodes.UIMap).Enable();
        
        Script_PlayerInputManager.Instance.Load();
        UpdateKeyBindingUIs();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_PlayerInputManager))]
    public class Script_PlayerInputManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_PlayerInputManager t = (Script_PlayerInputManager)target;
            
            if (GUILayout.Button("Save Key Rebinds"))
            {
                t.Save();
            }

            if (GUILayout.Button("Load Key Rebinds"))
            {
                t.Load();
            }

            if (GUILayout.Button("Set Default"))
            {
                t.SetDefault();
            }
        }
    }
#endif
}
