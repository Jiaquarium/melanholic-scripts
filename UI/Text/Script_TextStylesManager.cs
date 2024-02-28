using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum TextStyle
{
    Default = 0,
    Name = 1,
    UI = 2,
    UIEmphasis = 6,
    UILetterSelect = 18,
    BadEndingLeadIn = 3,
    BadEndingDescription = 4,
    BadEndingHeading = 5,
    MenuTopNav = 7,
    MenuItemName = 8,
    MenuItemDescription = 9,
    MapNotification = 10,
    TeletypeMyne = 11,
    FaceOff = 12,
    Awakening = 13,
    GoodTrueEnding = 14,
    SettingsHeading = 15,
    Cta = 16,
    FileActionBanner = 17,
    NotesHint = 19,
    CreditsRole = 20,
    CreditsName = 21,
    CreditsHeader = 22,
    CreditsHeaderLarge = 23,
    SymbolsGlitch = 24,
    CreditsEndingHeader = 25,
    CreditsEndingHeader2 = 26,
}

public class Script_TextStylesManager : MonoBehaviour
{
    [SerializeField] private Script_TextStyle DialogueDefaultStyle;
    [SerializeField] private Script_TextStyle CN_DialogueDefaultStyle;
    [SerializeField] private Script_TextStyle JP_DialogueDefaultStyle;
    [Space]
    [SerializeField] private Script_TextStyle DialogueNameStyle;
    [SerializeField] private Script_TextStyle CN_DialogueNameStyle;
    [SerializeField] private Script_TextStyle JP_DialogueNameStyle;
    [Space]
    [SerializeField] private Script_TextStyle UIStyle;
    [SerializeField] private Script_TextStyle CN_UIStyle;
    [SerializeField] private Script_TextStyle JP_UIStyle;
    [Space]
    [SerializeField] private Script_TextStyle UIEmphasisStyle;
    [SerializeField] private Script_TextStyle CN_UIEmphasisStyle;
    [SerializeField] private Script_TextStyle JP_UIEmphasisStyle;
    [Space]
    [SerializeField] private Script_TextStyle UILetterSelectStyle;
    [SerializeField] private Script_TextStyle CN_UILetterSelectStyle;
    [SerializeField] private Script_TextStyle JP_UILetterSelectStyle;
    [Space]
    [SerializeField] private Script_TextStyle BadEndingLeadInStyle;
    [SerializeField] private Script_TextStyle CN_BadEndingLeadInStyle;
    [SerializeField] private Script_TextStyle JP_BadEndingLeadInStyle;
    [Space]
    [SerializeField] private Script_TextStyle BadEndingDescriptionStyle;
    [SerializeField] private Script_TextStyle CN_BadEndingDescriptionStyle;
    [SerializeField] private Script_TextStyle JP_BadEndingDescriptionStyle;
    [Space]
    [SerializeField] private Script_TextStyle BadEndingHeadingStyle;
    [SerializeField] private Script_TextStyle CN_BadEndingHeadingStyle;
    [SerializeField] private Script_TextStyle JP_BadEndingHeadingStyle;
    [Space]
    [SerializeField] private Script_TextStyle MenuTopNavStyle;
    [SerializeField] private Script_TextStyle CN_MenuTopNavStyle;
    [SerializeField] private Script_TextStyle JP_MenuTopNavStyle;
    [Space]
    [SerializeField] private Script_TextStyle MenuItemNameStyle;
    [SerializeField] private Script_TextStyle CN_MenuItemNameStyle;
    [SerializeField] private Script_TextStyle JP_MenuItemNameStyle;
    [Space]
    [SerializeField] private Script_TextStyle MenuItemDescriptionStyle;
    [SerializeField] private Script_TextStyle CN_MenuItemDescriptionStyle;
    [SerializeField] private Script_TextStyle JP_MenuItemDescriptionStyle;
    [Space]
    [SerializeField] private Script_TextStyle MapNotificationStyle;
    [SerializeField] private Script_TextStyle CN_MapNotificationStyle;
    [SerializeField] private Script_TextStyle JP_MapNotificationStyle;
    [Space]
    [SerializeField] private Script_TextStyle TeletypeMyneStyle;
    [SerializeField] private Script_TextStyle CN_TeletypeMyneStyle;
    [SerializeField] private Script_TextStyle JP_TeletypeMyneStyle;
    [Space]
    [SerializeField] private Script_TextStyle FaceOffStyle;
    [SerializeField] private Script_TextStyle CN_FaceOffStyle;
    [SerializeField] private Script_TextStyle JP_FaceOffStyle;
    [Space]
    [SerializeField] private Script_TextStyle AwakeningStyle;
    [SerializeField] private Script_TextStyle CN_AwakeningStyle;
    [SerializeField] private Script_TextStyle JP_AwakeningStyle;
    [Space]
    [SerializeField] private Script_TextStyle GoodTrueEndingStyle;
    [SerializeField] private Script_TextStyle CN_GoodTrueEndingStyle;
    [SerializeField] private Script_TextStyle JP_GoodTrueEndingStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsHeadingStyle;
    [SerializeField] private Script_TextStyle CN_SettingsHeadingStyle;
    [SerializeField] private Script_TextStyle JP_SettingsHeadingStyle;
    [Space]
    [SerializeField] private Script_TextStyle CtaStyle;
    [SerializeField] private Script_TextStyle CN_CtaStyle;
    [SerializeField] private Script_TextStyle JP_CtaStyle;
    [Space]
    [SerializeField] private Script_TextStyle FileActionBannerStyle;
    [SerializeField] private Script_TextStyle CN_FileActionBannerStyle;
    [SerializeField] private Script_TextStyle JP_FileActionBannerStyle;
    [Space]
    [SerializeField] private Script_TextStyle NotesHintStyle;
    [SerializeField] private Script_TextStyle CN_NotesHintStyle;
    [SerializeField] private Script_TextStyle JP_NotesHintStyle;
    [Space]
    [SerializeField] private Script_TextStyle CreditsRoleStyle;
    [SerializeField] private Script_TextStyle CN_CreditsRoleStyle;
    [SerializeField] private Script_TextStyle JP_CreditsRoleStyle;
    [Space]
    [SerializeField] private Script_TextStyle CreditsNameStyle;
    [SerializeField] private Script_TextStyle CN_CreditsNameStyle;
    [SerializeField] private Script_TextStyle JP_CreditsNameStyle;
    [Space]
    [SerializeField] private Script_TextStyle CreditsHeaderStyle;
    [SerializeField] private Script_TextStyle CN_CreditsHeaderStyle;
    [SerializeField] private Script_TextStyle JP_CreditsHeaderStyle;
    [Space]
    [SerializeField] private Script_TextStyle CreditsHeaderLargeStyle;
    [SerializeField] private Script_TextStyle CN_CreditsHeaderLargeStyle;
    [SerializeField] private Script_TextStyle JP_CreditsHeaderLargeStyle;
    [Space]
    [SerializeField] private Script_TextStyle SymbolsGlitchStyle;
    [SerializeField] private Script_TextStyle CN_SymbolsGlitchStyle;
    [SerializeField] private Script_TextStyle JP_SymbolsGlitchStyle;
    [Space]
    [SerializeField] private Script_TextStyle CreditsEndingHeaderStyle;
    [SerializeField] private Script_TextStyle CN_CreditsEndingHeaderStyle;
    [SerializeField] private Script_TextStyle JP_CreditsEndingHeaderStyle;
    [Space]
    [SerializeField] private Script_TextStyle CreditsEndingHeader2Style;
    [SerializeField] private Script_TextStyle CN_CreditsEndingHeader2Style;
    [SerializeField] private Script_TextStyle JP_CreditsEndingHeader2Style;

    public Script_TextStyle GetTextStyle(TextStyle style) => style switch
    {
        TextStyle.Name => GetLocalized(
            DialogueNameStyle,
            CN_DialogueNameStyle,
            JP_DialogueNameStyle
        ),
        TextStyle.UI => GetLocalized(
            UIStyle,
            CN_UIStyle,
            JP_UIStyle
        ),
        TextStyle.UILetterSelect => GetLocalized(
            UILetterSelectStyle,
            CN_UILetterSelectStyle,
            JP_UILetterSelectStyle
        ),
        TextStyle.UIEmphasis => GetLocalized(
            UIEmphasisStyle,
            CN_UIEmphasisStyle,
            JP_UIEmphasisStyle
        ),
        TextStyle.BadEndingLeadIn => GetLocalized(
            BadEndingLeadInStyle,
            CN_BadEndingLeadInStyle,
            JP_BadEndingLeadInStyle
        ),
        TextStyle.BadEndingDescription => GetLocalized(
            BadEndingDescriptionStyle,
            CN_BadEndingDescriptionStyle,
            JP_BadEndingDescriptionStyle
        ),
        TextStyle.BadEndingHeading => GetLocalized(
            BadEndingHeadingStyle,
            CN_BadEndingHeadingStyle,
            JP_BadEndingHeadingStyle
        ),
        TextStyle.MenuTopNav => GetLocalized(
            MenuTopNavStyle,
            CN_MenuTopNavStyle,
            JP_MenuTopNavStyle
        ),
        TextStyle.MenuItemName => GetLocalized(
            MenuItemNameStyle,
            CN_MenuItemNameStyle,
            JP_MenuItemNameStyle
        ),
        TextStyle.MenuItemDescription => GetLocalized(
            MenuItemDescriptionStyle,
            CN_MenuItemDescriptionStyle,
            JP_MenuItemDescriptionStyle
        ),
        TextStyle.MapNotification => GetLocalized(
            MapNotificationStyle,
            CN_MapNotificationStyle,
            JP_MapNotificationStyle
        ),
        TextStyle.TeletypeMyne => GetLocalized(
            TeletypeMyneStyle,
            CN_TeletypeMyneStyle,
            JP_TeletypeMyneStyle
        ),
        TextStyle.FaceOff => GetLocalized(
            FaceOffStyle,
            CN_FaceOffStyle,
            JP_FaceOffStyle
        ),
        TextStyle.Awakening => GetLocalized(
            AwakeningStyle,
            CN_AwakeningStyle,
            JP_AwakeningStyle
        ),
        TextStyle.GoodTrueEnding => GetLocalized(
            GoodTrueEndingStyle,
            CN_GoodTrueEndingStyle,
            JP_GoodTrueEndingStyle
        ),
        TextStyle.SettingsHeading => GetLocalized(
            SettingsHeadingStyle,
            CN_SettingsHeadingStyle,
            JP_SettingsHeadingStyle
        ),
        TextStyle.Cta => GetLocalized(
            CtaStyle,
            CN_CtaStyle,
            JP_CtaStyle
        ),
        TextStyle.FileActionBanner => GetLocalized(
            FileActionBannerStyle,
            CN_FileActionBannerStyle,
            JP_FileActionBannerStyle
        ),
        TextStyle.NotesHint => GetLocalized(
            NotesHintStyle,
            CN_NotesHintStyle,
            JP_NotesHintStyle
        ),
        TextStyle.CreditsRole => GetLocalized(
            CreditsRoleStyle,
            CN_CreditsRoleStyle,
            JP_CreditsRoleStyle
        ),
        TextStyle.CreditsName => GetLocalized(
            CreditsNameStyle,
            CN_CreditsNameStyle,
            JP_CreditsNameStyle
        ),
        TextStyle.CreditsHeader => GetLocalized(
            CreditsHeaderStyle,
            CN_CreditsHeaderStyle,
            JP_CreditsHeaderStyle
        ),
        TextStyle.CreditsHeaderLarge => GetLocalized(
            CreditsHeaderLargeStyle,
            CN_CreditsHeaderLargeStyle,
            JP_CreditsHeaderLargeStyle
        ),
        TextStyle.SymbolsGlitch => GetLocalized(
            SymbolsGlitchStyle,
            CN_SymbolsGlitchStyle,
            JP_SymbolsGlitchStyle
        ),
        TextStyle.CreditsEndingHeader => GetLocalized(
            CreditsEndingHeaderStyle,
            CN_CreditsEndingHeaderStyle,
            JP_CreditsEndingHeaderStyle
        ),
        TextStyle.CreditsEndingHeader2 => GetLocalized(
            CreditsEndingHeader2Style,
            CN_CreditsEndingHeader2Style,
            JP_CreditsEndingHeader2Style
        ),
        _ => GetLocalized(
            DialogueDefaultStyle,
            CN_DialogueDefaultStyle,
            JP_DialogueDefaultStyle
        )
    };

    private Script_TextStyle GetLocalized(
        Script_TextStyle styleEN,
        Script_TextStyle styleCN,
        Script_TextStyle styleJP
    ) => Script_LocalizationUtils.SwitchTextStyleOnLang(
        styleEN,
        styleCN,
        styleJP
    );

    // For use in dev only
    public static void RefreshFontAttributes()
    {
        var textStyles = Resources.FindObjectsOfTypeAll(typeof(Script_TMProSetFontUniqueStyle))
            as Script_TMProSetFontUniqueStyle[];
        
        foreach (var style in textStyles)
        {
            Debug.Log(style.name);
            style.SetFontAttributes();
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_TextStylesManager))]
    public class Script_TextStylesManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_TextStylesManager t = (Script_TextStylesManager)target;
            if (GUILayout.Button("Refresh Font Attributes"))
            {
                RefreshFontAttributes();
                EditorApplication.QueuePlayerLoopUpdate();
                SceneView.RepaintAll();
            }
        }
    }
    #endif
}
