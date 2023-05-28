using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

/// <summary>
/// To Store Dynamically Updated names
/// </summary>
public class Script_DynamicStringBuilder : MonoBehaviour
{
    public static Dictionary<string, string> Params = new Dictionary<string, string>();

    // Rebindable
    public static string InventoryKey = "@@InventoryKey";
    public static string SpeedKey = "@@SpeedKey";
    public static string MaskCommandKey = "@@MaskCommandKey";
    public static string DefaultInventoryBinding = "<b>INVENTORY</b>";
    public static string DefaultSpeedBinding = "<b>???</b>";
    public static string DefaultMaskCommandBinding = "<b>MASK COMMAND</b>";

    // Dynamic strings
    public static string RunKey = "@@Run";
    public static string CycleCountKey = "@@CycleCount";
    public static string DDRCurrentTryKey = "@@DDRCurrentTry";
    public static string WearMaskKey = "@@WearMask";
    public static string WearMaskValKeyboard = "{82}";
    public static string WearMaskValJoystick = "{15}";
    
    /// <summary>
    /// Need to be in Start so we have reference to Singletons
    /// </summary>
    void Awake()
    {
        BuildParams();
        BuildDefaultDynamicKeys();
    }
    
    void OnValidate()
    {
        BuildParams();
    }
    
    // Builds a new dict on each format call.
    public static void BuildParams()
    {
        Params = new Dictionary<string, string>();
        
        Params.Add(RunKey, $"<b>{Script_Game.Game?.Run.dayName.FormatRun() ?? "?"}</b>");
        Params.Add(CycleCountKey, $"<b>{Script_Game.Game?.CycleCount.ToString() ?? "?"}</b>");
        
        Params.Add(DDRCurrentTryKey, $"<b>{((Script_Game.Game?.IdsRoomBehavior.CurrentTry ?? 1) + 1).ToString() ?? "?"}</b>");

        var playerInputManager = Script_PlayerInputManager.Instance;
        if (playerInputManager != null && playerInputManager.RewiredInput != null)
        {
            Controller lastController = playerInputManager.RewiredInput.controllers.GetLastActiveController();
            var lastControllerType = lastController != null ? lastController.type : ControllerType.Keyboard;
            SetControllerBasedParams(lastControllerType);
        }
        else
            SetControllerBasedParams(ControllerType.Keyboard);

        // Params that depend on the controller type
        void SetControllerBasedParams(ControllerType t)
        {
            Params.Add(WearMaskKey, t == ControllerType.Joystick ? WearMaskValJoystick : WearMaskValKeyboard);
        }
    }

    // Must call BuildParams before calling this to build new Params dict and set last controller
    private static void BuildDynamicParam(string paramKey, string actionName)
    {
        var playerInputManager = Script_PlayerInputManager.Instance;
        
        if (playerInputManager != null)
        {
            Player rewiredInput = playerInputManager.RewiredInput;
            Controller controller = rewiredInput.controllers.GetLastActiveController();
            
            string firstBoundKey = null;
            
            // If last controller is null, default to Keyboard
            if (controller != null && controller.type == ControllerType.Joystick)
                firstBoundKey = Script_Utils.GetFirstMappingJoystickByActionName(rewiredInput, controller, actionName);
            else
                firstBoundKey = Script_Utils.GetFirstMappingKeyboardByActionName(rewiredInput, actionName);

            if (!string.IsNullOrEmpty(firstBoundKey))
            {
                Dev_Logger.Debug($"Added paramKey {paramKey} firstBoundKey {firstBoundKey} to Params Dict");

                Params.Add(paramKey, $"<b>{firstBoundKey.ToUpper()}</b>");
                return;
            }
        }
        
        BuildDefaultDynamicKeys();
    }
    
    public static void BuildInventoryParam() => BuildDynamicParam(InventoryKey, Const_KeyCodes.RWInventory);

    public static void BuildSpeedParam() => BuildDynamicParam(SpeedKey, Const_KeyCodes.RWSpeed);

    public static void BuildMaskCommandParam() => BuildDynamicParam(MaskCommandKey, Const_KeyCodes.RWMaskCommand);

    private static void BuildDefaultDynamicKeys()
    {
        string inventoryOutput;
        string speedOutput;
        string maskOutput;

        if (!Params.TryGetValue(InventoryKey, out inventoryOutput))
            Params.Add(InventoryKey, DefaultInventoryBinding);
        
        if (!Params.TryGetValue(SpeedKey, out speedOutput))
            Params.Add(SpeedKey, DefaultSpeedBinding);
        
        if (!Params.TryGetValue(MaskCommandKey, out maskOutput))
            Params.Add(MaskCommandKey, DefaultMaskCommandBinding);
    }
}
