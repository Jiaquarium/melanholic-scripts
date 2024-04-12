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
        BuildParams(Script_Game.Lang);
    }
    
    void OnValidate()
    {
        BuildParams(Script_Game.Lang);
    }
    
    // Must call when changing language preference
    public static void BuildParams(string language)
    {
        Params = new Dictionary<string, string>();
        
        Script_LocalizationUtils.SwitchActionOnLang(
            BuildEN,
            BuildCN,
            BuildJP
        );
        
        void BuildEN()
        {
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

        void BuildCN()
        {
            Params.Add("@@Sticker_Bold",            "<b>Mask是</b>");
            Params.Add("@@Sticker_NoBold",          "Mask是");
            Params.Add("@@Stickers_Bold",           "<b>Masks是</b>");
            Params.Add("@@Stickers_NoBold",         "Masks是");
            
            Params.Add("@@PsychicDuck",             "<b>Psychic Duck是</b>");
            Params.Add("@@BoarNeedle",              "<b>Third Eye是</b>");
            Params.Add("@@AnimalWithin",            "<b>Animal Within是</b>");
            Params.Add("@@IceSpike",                "<b>Snow Woman是</b>");
            Params.Add("@@MelancholyPiano",         "<b>Melancholy Piano是</b>");
            Params.Add("@@LastElevator",            "<b>Last Elevator是</b>");
            Params.Add("@@LetThereBeLight",         "<b>Lantern是</b>");
            Params.Add("@@Puppeteer",               "<b>Puppeteer是</b>");
            Params.Add("@@MyMask",                  "<b>Summoner of Self是</b>");
            
            Params.Add("@@MasterKey",               "<b>Master Key是</b>");
            Params.Add("@@SuperSmallKey",           "<b>Super Small Key是</b>");

            Params.Add("@@WinterStone",             "<b>Winter Stone是</b>");
            Params.Add("@@AutumnStone",             "<b>Autumn Stone是</b>");
            Params.Add("@@SummerStone",             "<b>Summer Stone是</b>");
            Params.Add("@@SpringStone",             "<b>Spring Stone是</b>");
            Params.Add("@@LastWellMap",             "<b>Last Well Map是</b>");
            Params.Add("@@LastSpellRecipeBook",     "<b>Last Spell Recipe Book是</b>");
            Params.Add("@@SpeedSeal",               "<b>Speed Seal是</b>");
        }

        void BuildJP()
        {
            Params.Add("@@Sticker_Bold",            "<b>仮面</b>");
            Params.Add("@@Sticker_NoBold",          "仮面");
            Params.Add("@@Stickers_Bold",           "<b>仮面</b>");
            Params.Add("@@Stickers_NoBold",         "仮面");
            
            Params.Add("@@PsychicDuck",             "<b>サイコダック</b>");
            Params.Add("@@BoarNeedle",              "<b>三つ目</b>");
            Params.Add("@@AnimalWithin",            "<b>内なる獣</b>");
            Params.Add("@@IceSpike",                "<b>雪女</b>");
            Params.Add("@@MelancholyPiano",         "<b>憂鬱ピアノ</b>");
            Params.Add("@@LastElevator",            "<b>最終エレベーター</b>");
            Params.Add("@@LetThereBeLight",         "<b>ランタン</b>");
            Params.Add("@@Puppeteer",               "<b>人形使い</b>");
            Params.Add("@@MyMask",                  "<b>自己召喚</b>");
            
            Params.Add("@@MasterKey",               "<b>マスターキー</b>");
            Params.Add("@@SuperSmallKey",           "<b>小さな小さなキー</b>");

            Params.Add("@@WinterStone",             "<b>冬石</b>");
            Params.Add("@@AutumnStone",             "<b>秋石</b>");
            Params.Add("@@SummerStone",             "<b>夏石</b>");
            Params.Add("@@SpringStone",             "<b>春石</b>");
            Params.Add("@@LastWellMap",             "<b>最終井戸の地図</b>");
            Params.Add("@@LastSpellRecipeBook",     "<b>最終呪文のレシピ</b>");
            Params.Add("@@SpeedSeal",               "<b>加速札</b>");
        }
    }
}
