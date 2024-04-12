using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Central point for all localization switches to run through. This way, when updating a new
/// language, we will be alerted everywhere a switch occurs.
/// </summary>
public static class Script_LocalizationUtils
{
    public enum LangEnum
    {
        Default = 0,
        CN = 1,
        JP = 2,
        EN = 9
    }

    // TBD Remove for v1.2.0; only used for v1.1.0 SteamDeck Verification because localization is WIP
    public static bool IsDisabled = false;
    public static bool IsFormatPeriodsCN = false;
    const char PeriodSymbol = '.';
    // Font must be under TextMesh Pro/Resources/Fonts & Materials
    const string SpecialPeriodFontCN = "TT_SHUSONG_UNDERLAY";

    public static string LangEnumToLang(this LangEnum langEnum) => langEnum switch
    {
        LangEnum.Default => Const_Languages.EN,
        LangEnum.CN => Const_Languages.CN,
        LangEnum.JP => Const_Languages.JP,
        LangEnum.EN => Const_Languages.EN,
        _ => Const_Languages.EN
    };

    public static void SwitchGameLangByLang(string lang)
    {
        switch (lang)
        {
            case Const_Languages.EN:
                Script_Game.ChangeLangToEN();
                break;
            case Const_Languages.CN:
                Script_Game.ChangeLangToCN();
                break;
            case Const_Languages.JP:
                Script_Game.ChangeLangToJP();
                break;
            default:
                Script_Game.ChangeLangToEN();
                break;
        }
    }

    // When switching text
    public static string SwitchTextOnLang(
        string EN_text,
        string CN_text,
        string JP_text
     ) => Script_Game.Lang switch
    {
        Const_Languages.EN => EN_text,
        Const_Languages.CN => CN_text,
        Const_Languages.JP => JP_text,
        _ => EN_text
    };

    // When switching function call
    public static void SwitchActionOnLang(
        Action EN_action,
        Action CN_action,
        Action JP_action
    )
    {
        switch (Script_Game.Lang)
        {
            case Const_Languages.EN:
                EN_action();
                break;
            case Const_Languages.CN:
                CN_action();
                break;
            case Const_Languages.JP:
                JP_action();
                break;
            default:
                EN_action();
                break;
        }
    }

    public static void ActionForEachLang(
        Action EN_action,
        Action CN_action,
        Action JP_action
    )
    {
        EN_action();
        CN_action();
        JP_action();
    }

    public static Script_TextStyle SwitchTextStyleOnLang(
        Script_TextStyle styleEN,
        Script_TextStyle styleCN,
        Script_TextStyle styleJP
    ) => Script_Game.Lang switch
    {
        Const_Languages.EN => styleEN,
        Const_Languages.CN => styleCN,
        Const_Languages.JP => styleJP,
        _ => styleEN
    };

    public static bool CheckValidLang(string lang) => lang switch
    {
        Const_Languages.EN => true,
        Const_Languages.CN => true,
        Const_Languages.JP => true,
        _ => false
    };

    public static string FormatPeriodsCN(this string sentence)
    {
        return sentence.Replace(
            PeriodSymbol.ToString(),
            $"<font=\"{SpecialPeriodFontCN}\">{PeriodSymbol.ToString()}</font>"
        );
    }

    public static void SaveLangPref(Model_LanguagePreference langPref)
    {
        langPref.lang = Script_Game.Lang;
    }
}
