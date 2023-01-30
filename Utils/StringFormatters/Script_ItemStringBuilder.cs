using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to allow replacement of @@NAME in text; use for Items
/// 
/// Note: When changing item names here, also MUST change corresponding Scriptable Object's
/// item name, since that is used for Inventory Descriptions.
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
        
        Params.Add("@@Sticker_Bold",            "<b>Mask</b>");
        Params.Add("@@Sticker_NoBold",          "Mask");
        Params.Add("@@Stickers_Bold",           "<b>Masks</b>");
        Params.Add("@@Stickers_NoBold",         "Masks");
        
        Params.Add("@@PsychicDuck",             "<b>Psychic Duck</b>");
        Params.Add("@@BoarNeedle",              "<b>Third Eye</b>");
        Params.Add("@@AnimalWithin",            "<b>Animal Within</b>");
        Params.Add("@@IceSpike",                "<b>Snow Woman</b>");
        Params.Add("@@MelancholyPiano",         "<b>Melancholy Piano</b>");
        Params.Add("@@LastElevator",            "<b>Last Elevator</b>");
        Params.Add("@@LetThereBeLight",         "<b>Lantern</b>");
        Params.Add("@@Puppeteer",               "<b>Puppeteer</b>");
        Params.Add("@@MyMask",                  "<b>Summoner of Self</b>");
        
        Params.Add("@@MasterKey",               "<b>Master Key</b>");
        Params.Add("@@SuperSmallKey",           "<b>Super Small Key</b>");

        Params.Add("@@WinterStone",             "<b>Winter Stone</b>");
        Params.Add("@@AutumnStone",             "<b>Autumn Stone</b>");
        Params.Add("@@SummerStone",             "<b>Summer Stone</b>");
        Params.Add("@@SpringStone",             "<b>Spring Stone</b>");
        Params.Add("@@LastWellMap",             "<b>Last Well Map</b>");
        Params.Add("@@LastSpellRecipeBook",     "<b>Last Spell Recipe Book</b>");
        Params.Add("@@SpeedSeal",               "<b>Speed Seal</b>");
    }
}
