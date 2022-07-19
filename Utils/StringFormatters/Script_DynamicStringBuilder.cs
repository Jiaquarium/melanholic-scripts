using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To Store Dynamically Updated names
/// </summary>
public class Script_DynamicStringBuilder : MonoBehaviour
{
    public static Dictionary<string, string> Params = new Dictionary<string, string>();

    public static string InventoryKey = "@@InventoryKey";
    public static string SpeedKey = "@@SpeedKey";
    public static string DefaultInventoryBinding = "<b>INVENTORY</b>";
    public static string DefaultSpeedBinding = "<b>???</b>";
    
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
    
    public static void BuildParams()
    {
        Params = new Dictionary<string, string>();
        
        Params.Add("@@Run", $"<b>{Script_Game.Game?.Run.dayName.FormatRun() ?? "?"}</b>");
        Params.Add("@@CycleCount", $"<b>{Script_Game.Game?.CycleCount.ToString() ?? "?"}</b>");
        
        Params.Add("@@DDRCurrentTry", $"<b>{Script_Game.Game?.IdsRoomBehavior.CurrentTry.ToString() ?? "?"}</b>");
    }

    public static void BuildInventoryParam()
    {
        if (Script_PlayerInputManager.Instance != null)
            Params.Add(InventoryKey, $"<b>{Script_PlayerInputManager.Instance.GetHumanReadableBindingPath(Const_KeyCodes.Inventory)}</b>");
        else
            BuildDefaultDynamicKeys();
    }

    public static void BuildSpeedParam()
    {
        if (Script_PlayerInputManager.Instance != null)
            Params.Add(SpeedKey, $"<b>{Script_PlayerInputManager.Instance.GetHumanReadableBindingPath(Const_KeyCodes.Speed)}</b>");
        else
            BuildDefaultDynamicKeys();
    }

    private static void BuildDefaultDynamicKeys()
    {
        string inventoryOutput;
        string speedOutput;

        if (!Params.TryGetValue(InventoryKey, out inventoryOutput))
            Params.Add(InventoryKey, DefaultInventoryBinding);
        
        if (!Params.TryGetValue(SpeedKey, out speedOutput))
            Params.Add(SpeedKey, DefaultSpeedBinding);
    }
}
