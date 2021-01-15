using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to allow replacement of @@NAME in text; use for
/// Items
/// </summary>
public class Script_ItemStringBuilder : MonoBehaviour
{
    public static Dictionary<string, string> Params = new Dictionary<string, string>();
    
    void Awake()
    {
        BuildParams();
    }
    
    void OnValidate()
    {
        BuildParams();
    }
    
    void BuildParams()
    {
        Params = new Dictionary<string, string>();
        
        Params.Add("@@PsychicDuck",         "<b>Psychic Duck</b>");
        Params.Add("@@BoarNeedle",          "<b>Boar Needle</b>");
        Params.Add("@@AnimalWithin",        "<b>Animal Within</b>");
        
        Params.Add("@@WinterStone",         "<b>Winter Stone</b>");
        Params.Add("@@AutumnStone",         "<b>Autumn Stone</b>");
        Params.Add("@@SummerStone",         "<b>Summer Stone</b>");
        Params.Add("@@SpringStone",         "<b>Spring Stone</b>");

        Params.Add("@@MasterKey",           "<b>Master Key</b>");
    }
}
