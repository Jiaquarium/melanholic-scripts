using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To Store Dynamically Updated names
/// </summary>
public class Script_DynamicStringBuilder : MonoBehaviour
{
    public static Dictionary<string, string> Params = new Dictionary<string, string>();
    
    /// <summary>
    /// Need to be in Start so we have reference to Singletons
    /// </summary>
    void Awake()
    {
        BuildParams();
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
        
        if (Script_PlayerInputManager.Instance != null)
            Params.Add("@@InventoryKey", $"<b>{Script_PlayerInputManager.Instance.GetHumanReadableBindingPath(Const_KeyCodes.Inventory)}</b>");
        else
            Params.Add("@@InventoryKey", $"<b>I</b>");
        
        if (Script_PlayerInputManager.Instance != null)
            Params.Add("@@SpeedKey", $"<b>{Script_PlayerInputManager.Instance.GetHumanReadableBindingPath(Const_KeyCodes.Speed)}</b>");
        else
            Params.Add("@@SpeedKey", $"<b>LEFT SHIFT</b>");
    }
}
