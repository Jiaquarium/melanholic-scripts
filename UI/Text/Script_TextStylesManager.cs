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
    Note = 27,
    NoteCursive = 28,
    CreditsText = 29,
    SavedGameLabel = 30,
    SavedGameDay = 31,
    SavedGameEmpty = 32,
    TrueEndingMessage = 33,
    TrueEndingMessageSubtitle = 34,
    SettingsAction = 35,
    SettingsActionItalic = 36,
    SettingsMessage = 37,
    SettingsKey = 38,
    SettingsKeyUpper = 39,
    SettingsWarning = 40,
    SettingsActionUnknownController = 41,
    SettingsDisplayCurrent = 42,
    SettingsHelpText = 43,
    SettingsSystemChoice = 44,
    SettingsKeyControllerSelect = 45,
    DayNotification = 46,
    DayNotificationTime = 47,
    DayNotificationSubtext = 48,
    DayNotificationSubtextSun = 49,
    HUDDay = 50,
    ContractInput = 51,
    NoteLobbyHeading = 52,
    NoteLobbyBody = 53,
    LastWellMapHeading = 54,
    LastWellMap = 55,
    SavedGameSBookName = 56,
    SaveProgress = 57,
    DDRComment = 58,
    EileensMindDoubtTitle0 = 59,
    EileensMindDoubtTitle1 = 60,
    EileensMindDoubtAsterisk = 61,
    SettingsKeyItalic = 62,
}

public class Script_TextStylesManager : MonoBehaviour
{
    public static Script_TextStylesManager Instance;
    
    [SerializeField] private Script_TextStyle DialogueDefaultStyle;
    [SerializeField] private Script_TextStyle DialogueDefaultStyle_SteamDeck;
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
    [SerializeField] private Script_TextStyle MenuItemNameStyle_SteamDeck;
    [SerializeField] private Script_TextStyle CN_MenuItemNameStyle;
    [SerializeField] private Script_TextStyle JP_MenuItemNameStyle;
    [Space]
    [SerializeField] private Script_TextStyle MenuItemDescriptionStyle;
    [SerializeField] private Script_TextStyle MenuItemDescriptionStyle_SteamDeck;
    [SerializeField] private Script_TextStyle CN_MenuItemDescriptionStyle;
    [SerializeField] private Script_TextStyle JP_MenuItemDescriptionStyle;
    [Space]
    [SerializeField] private Script_TextStyle DayNotificationStyle;
    [SerializeField] private Script_TextStyle CN_DayNotificationStyle;
    [SerializeField] private Script_TextStyle JP_DayNotificationStyle;
    [Space]
    [SerializeField] private Script_TextStyle DayNotificationTimeStyle;
    [SerializeField] private Script_TextStyle CN_DayNotificationTimeStyle;
    [SerializeField] private Script_TextStyle JP_DayNotificationTimeStyle;
    [Space]
    [SerializeField] private Script_TextStyle DayNotificationSubtextStyle;
    [SerializeField] private Script_TextStyle CN_DayNotificationSubtextStyle;
    [SerializeField] private Script_TextStyle JP_DayNotificationSubtextStyle;
    [Space]
    [SerializeField] private Script_TextStyle DayNotificationSubtextSunStyle;
    [SerializeField] private Script_TextStyle CN_DayNotificationSubtextSunStyle;
    [SerializeField] private Script_TextStyle JP_DayNotificationSubtextSunStyle;
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
    [SerializeField] private Script_TextStyle TrueEndingMessageStyle;
    [SerializeField] private Script_TextStyle CN_TrueEndingMessageStyle;
    [SerializeField] private Script_TextStyle JP_TrueEndingMessageStyle;
    [Space]
    [SerializeField] private Script_TextStyle TrueEndingMessageSubtitleStyle;
    [SerializeField] private Script_TextStyle CN_TrueEndingMessageSubtitleStyle;
    [SerializeField] private Script_TextStyle JP_TrueEndingMessageSubtitleStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsHeadingStyle;
    [SerializeField] private Script_TextStyle CN_SettingsHeadingStyle;
    [SerializeField] private Script_TextStyle JP_SettingsHeadingStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsActionStyle;
    [SerializeField] private Script_TextStyle CN_SettingsActionStyle;
    [SerializeField] private Script_TextStyle JP_SettingsActionStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsActionItalicStyle;
    [SerializeField] private Script_TextStyle CN_SettingsActionItalicStyle;
    [SerializeField] private Script_TextStyle JP_SettingsActionItalicStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsMessageStyle;
    [SerializeField] private Script_TextStyle CN_SettingsMessageStyle;
    [SerializeField] private Script_TextStyle JP_SettingsMessageStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsKeyStyle;
    [SerializeField] private Script_TextStyle CN_SettingsKeyStyle;
    [SerializeField] private Script_TextStyle JP_SettingsKeyStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsKeyUpperStyle;
    [SerializeField] private Script_TextStyle CN_SettingsKeyUpperStyle;
    [SerializeField] private Script_TextStyle JP_SettingsKeyUpperStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsKeyItalicStyle;
    [SerializeField] private Script_TextStyle CN_SettingsKeyItalicStyle;
    [SerializeField] private Script_TextStyle JP_SettingsKeyItalicStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsWarningStyle;
    [SerializeField] private Script_TextStyle CN_SettingsWarningStyle;
    [SerializeField] private Script_TextStyle JP_SettingsWarningStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsActionUnknownControllerStyle;
    [SerializeField] private Script_TextStyle CN_SettingsActionUnknownControllerStyle;
    [SerializeField] private Script_TextStyle JP_SettingsActionUnknownControllerStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsDisplayCurrentStyle;
    [SerializeField] private Script_TextStyle CN_SettingsDisplayCurrentStyle;
    [SerializeField] private Script_TextStyle JP_SettingsDisplayCurrentStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsHelpTextStyle;
    [SerializeField] private Script_TextStyle CN_SettingsHelpTextStyle;
    [SerializeField] private Script_TextStyle JP_SettingsHelpTextStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsSystemChoiceStyle;
    [SerializeField] private Script_TextStyle CN_SettingsSystemChoiceStyle;
    [SerializeField] private Script_TextStyle JP_SettingsSystemChoiceStyle;
    [Space]
    [SerializeField] private Script_TextStyle SettingsKeyControllerSelectStyle;
    [SerializeField] private Script_TextStyle CN_SettingsKeyControllerSelectStyle;
    [SerializeField] private Script_TextStyle JP_SettingsKeyControllerSelectStyle;
    [Space]
    [SerializeField] private Script_TextStyle CtaStyle;
    [SerializeField] private Script_TextStyle CN_CtaStyle;
    [SerializeField] private Script_TextStyle JP_CtaStyle;
    [Space]
    [SerializeField] private Script_TextStyle FileActionBannerStyle;
    [SerializeField] private Script_TextStyle CN_FileActionBannerStyle;
    [SerializeField] private Script_TextStyle JP_FileActionBannerStyle;
    [Space]
    [SerializeField] private Script_TextStyle SavedGameLabelStyle;
    [SerializeField] private Script_TextStyle CN_SavedGameLabelStyle;
    [SerializeField] private Script_TextStyle JP_SavedGameLabelStyle;
    [Space]
    [SerializeField] private Script_TextStyle SavedGameDayStyle;
    [SerializeField] private Script_TextStyle CN_SavedGameDayStyle;
    [SerializeField] private Script_TextStyle JP_SavedGameDayStyle;
    [Space]
    [SerializeField] private Script_TextStyle SavedGameEmptyStyle;
    [SerializeField] private Script_TextStyle CN_SavedGameEmptyStyle;
    [SerializeField] private Script_TextStyle JP_SavedGameEmptyStyle;
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
    [SerializeField] private Script_TextStyle CreditsTextStyle;
    [SerializeField] private Script_TextStyle CN_CreditsTextStyle;
    [SerializeField] private Script_TextStyle JP_CreditsTextStyle;
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
    [Space]
    [SerializeField] private Script_TextStyle NoteStyle;
    [SerializeField] private Script_TextStyle CN_NoteStyle;
    [SerializeField] private Script_TextStyle JP_NoteStyle;
    [Space]
    [SerializeField] private Script_TextStyle NoteCursiveStyle;
    [SerializeField] private Script_TextStyle CN_NoteCursiveStyle;
    [SerializeField] private Script_TextStyle JP_NoteCursiveStyle;
    [Space]
    [SerializeField] private Script_TextStyle NoteLobbyHeadingStyle;
    [SerializeField] private Script_TextStyle CN_NoteLobbyHeadingStyle;
    [SerializeField] private Script_TextStyle JP_NoteLobbyHeadingStyle;
    [Space]
    [SerializeField] private Script_TextStyle NoteLobbyBodyStyle;
    [SerializeField] private Script_TextStyle CN_NoteLobbyBodyStyle;
    [SerializeField] private Script_TextStyle JP_NoteLobbyBodyStyle;
    [Space]
    [SerializeField] private Script_TextStyle HUDDayStyle;
    [SerializeField] private Script_TextStyle CN_HUDDayStyle;
    [SerializeField] private Script_TextStyle JP_HUDDayStyle;
    [Space]
    [SerializeField] private Script_TextStyle ContractInputStyle;
    [SerializeField] private Script_TextStyle CN_ContractInputStyle;
    [SerializeField] private Script_TextStyle JP_ContractInputStyle;
    [Space]
    [SerializeField] private Script_TextStyle LastWellMapHeadingStyle;
    [SerializeField] private Script_TextStyle CN_LastWellMapHeadingStyle;
    [SerializeField] private Script_TextStyle JP_LastWellMapHeadingStyle;
    [Space]
    [SerializeField] private Script_TextStyle LastWellMapStyle;
    [SerializeField] private Script_TextStyle CN_LastWellMapStyle;
    [SerializeField] private Script_TextStyle JP_LastWellMapStyle;
    [Space]
    [SerializeField] private Script_TextStyle SavedGameSBookNameStyle;
    [SerializeField] private Script_TextStyle CN_SavedGameSBookNameStyle;
    [SerializeField] private Script_TextStyle JP_SavedGameSBookNameStyle;
    [Space]
    [SerializeField] private Script_TextStyle SaveProgressStyle;
    [SerializeField] private Script_TextStyle CN_SaveProgressStyle;
    [SerializeField] private Script_TextStyle JP_SaveProgressStyle;
    [Space]
    [SerializeField] private Script_TextStyle DDRCommentStyle;
    [SerializeField] private Script_TextStyle CN_DDRCommentStyle;
    [SerializeField] private Script_TextStyle JP_DDRCommentStyle;
    [Space]
    [SerializeField] private Script_TextStyle EileensMindDoubtTitle0Style;
    [SerializeField] private Script_TextStyle CN_EileensMindDoubtTitle0Style;
    [SerializeField] private Script_TextStyle JP_EileensMindDoubtTitle0Style;
    [Space]
    [SerializeField] private Script_TextStyle EileensMindDoubtTitle1Style;
    [SerializeField] private Script_TextStyle CN_EileensMindDoubtTitle1Style;
    [SerializeField] private Script_TextStyle JP_EileensMindDoubtTitle1Style;
    [Space]
    [SerializeField] private Script_TextStyle EileensMindDoubtAsteriskStyle;
    [SerializeField] private Script_TextStyle CN_EileensMindDoubtAsteriskStyle;
    [SerializeField] private Script_TextStyle JP_EileensMindDoubtAsteriskStyle;

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
            Script_Game.IsSteamRunningOnSteamDeck
                ? MenuItemNameStyle_SteamDeck
                : MenuItemNameStyle,
            CN_MenuItemNameStyle,
            JP_MenuItemNameStyle
        ),
        TextStyle.MenuItemDescription => GetLocalized(
            Script_Game.IsSteamRunningOnSteamDeck
                ? MenuItemDescriptionStyle_SteamDeck
                : MenuItemDescriptionStyle,
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
        TextStyle.Note => GetLocalized(
            NoteStyle,
            CN_NoteStyle,
            JP_NoteStyle
        ),
        TextStyle.NoteCursive => GetLocalized(
            NoteCursiveStyle,
            CN_NoteCursiveStyle,
            JP_NoteCursiveStyle
        ),
        TextStyle.CreditsText => GetLocalized(
            CreditsTextStyle,
            CN_CreditsTextStyle,
            JP_CreditsTextStyle
        ),
        TextStyle.SavedGameLabel => GetLocalized(
            SavedGameLabelStyle,
            CN_SavedGameLabelStyle,
            JP_SavedGameLabelStyle
        ),
        TextStyle.SavedGameDay => GetLocalized(
            SavedGameDayStyle,
            CN_SavedGameDayStyle,
            JP_SavedGameDayStyle
        ),
        TextStyle.SavedGameEmpty => GetLocalized(
            SavedGameEmptyStyle,
            CN_SavedGameEmptyStyle,
            JP_SavedGameEmptyStyle
        ),
        TextStyle.TrueEndingMessage => GetLocalized(
            TrueEndingMessageStyle,
            CN_TrueEndingMessageStyle,
            JP_TrueEndingMessageStyle
        ),
        TextStyle.TrueEndingMessageSubtitle => GetLocalized(
            TrueEndingMessageSubtitleStyle,
            CN_TrueEndingMessageSubtitleStyle,
            JP_TrueEndingMessageSubtitleStyle
        ),
        TextStyle.SettingsAction => GetLocalized(
            SettingsActionStyle,
            CN_SettingsActionStyle,
            JP_SettingsActionStyle
        ),
        TextStyle.SettingsActionItalic => GetLocalized(
            SettingsActionItalicStyle,
            CN_SettingsActionItalicStyle,
            JP_SettingsActionItalicStyle
        ),
        TextStyle.SettingsMessage => GetLocalized(
            SettingsMessageStyle,
            CN_SettingsMessageStyle,
            JP_SettingsMessageStyle
        ),
        TextStyle.SettingsKey => GetLocalized(
            SettingsKeyStyle,
            CN_SettingsKeyStyle,
            JP_SettingsKeyStyle
        ),
        TextStyle.SettingsKeyUpper => GetLocalized(
            SettingsKeyUpperStyle,
            CN_SettingsKeyUpperStyle,
            JP_SettingsKeyUpperStyle
        ),
        TextStyle.SettingsWarning => GetLocalized(
            SettingsWarningStyle,
            CN_SettingsWarningStyle,
            JP_SettingsWarningStyle
        ),
        TextStyle.SettingsActionUnknownController => GetLocalized(
            SettingsActionUnknownControllerStyle,
            CN_SettingsActionUnknownControllerStyle,
            JP_SettingsActionUnknownControllerStyle
        ),
        TextStyle.SettingsDisplayCurrent => GetLocalized(
            SettingsDisplayCurrentStyle,
            CN_SettingsDisplayCurrentStyle,
            JP_SettingsDisplayCurrentStyle
        ),
        TextStyle.SettingsHelpText => GetLocalized(
            SettingsHelpTextStyle,
            CN_SettingsHelpTextStyle,
            JP_SettingsHelpTextStyle
        ),
        TextStyle.SettingsSystemChoice => GetLocalized(
            SettingsSystemChoiceStyle,
            CN_SettingsSystemChoiceStyle,
            JP_SettingsSystemChoiceStyle
        ),
        TextStyle.SettingsKeyControllerSelect => GetLocalized(
            SettingsKeyControllerSelectStyle,
            CN_SettingsKeyControllerSelectStyle,
            JP_SettingsKeyControllerSelectStyle
        ),
        TextStyle.DayNotification => GetLocalized(
            DayNotificationStyle,
            CN_DayNotificationStyle,
            JP_DayNotificationStyle
        ),
        TextStyle.DayNotificationTime => GetLocalized(
            DayNotificationTimeStyle,
            CN_DayNotificationTimeStyle,
            JP_DayNotificationTimeStyle
        ),
        TextStyle.DayNotificationSubtext => GetLocalized(
            DayNotificationSubtextStyle,
            CN_DayNotificationSubtextStyle,
            JP_DayNotificationSubtextStyle
        ),
        TextStyle.DayNotificationSubtextSun => GetLocalized(
            DayNotificationSubtextSunStyle,
            CN_DayNotificationSubtextSunStyle,
            JP_DayNotificationSubtextSunStyle
        ),
        TextStyle.HUDDay => GetLocalized(
            HUDDayStyle,
            CN_HUDDayStyle,
            JP_HUDDayStyle
        ),
        TextStyle.ContractInput => GetLocalized(
            ContractInputStyle,
            CN_ContractInputStyle,
            JP_ContractInputStyle
        ),
        TextStyle.NoteLobbyHeading => GetLocalized(
            NoteLobbyHeadingStyle,
            CN_NoteLobbyHeadingStyle,
            JP_NoteLobbyHeadingStyle
        ),
        TextStyle.NoteLobbyBody => GetLocalized(
            NoteLobbyBodyStyle,
            CN_NoteLobbyBodyStyle,
            JP_NoteLobbyBodyStyle
        ),
        TextStyle.LastWellMapHeading => GetLocalized(
            LastWellMapHeadingStyle,
            CN_LastWellMapHeadingStyle,
            JP_LastWellMapHeadingStyle
        ),
        TextStyle.LastWellMap => GetLocalized(
            LastWellMapStyle,
            CN_LastWellMapStyle,
            JP_LastWellMapStyle
        ),
        TextStyle.SavedGameSBookName => GetLocalized(
            SavedGameSBookNameStyle,
            CN_SavedGameSBookNameStyle,
            JP_SavedGameSBookNameStyle
        ),
        TextStyle.SaveProgress => GetLocalized(
            SaveProgressStyle,
            CN_SaveProgressStyle,
            JP_SaveProgressStyle
        ),
        TextStyle.DDRComment => GetLocalized(
            DDRCommentStyle,
            CN_DDRCommentStyle,
            JP_DDRCommentStyle
        ),
        TextStyle.EileensMindDoubtTitle0 => GetLocalized(
            EileensMindDoubtTitle0Style,
            CN_EileensMindDoubtTitle0Style,
            JP_EileensMindDoubtTitle0Style
        ),
        TextStyle.EileensMindDoubtTitle1 => GetLocalized(
            EileensMindDoubtTitle1Style,
            CN_EileensMindDoubtTitle1Style,
            JP_EileensMindDoubtTitle1Style
        ),
        TextStyle.EileensMindDoubtAsterisk => GetLocalized(
            EileensMindDoubtAsteriskStyle,
            CN_EileensMindDoubtAsteriskStyle,
            JP_EileensMindDoubtAsteriskStyle
        ),
        TextStyle.SettingsKeyItalic => GetLocalized(
            SettingsKeyItalicStyle,
            CN_SettingsKeyItalicStyle,
            JP_SettingsKeyItalicStyle
        ),
        _ => GetLocalized(
            Script_Game.IsSteamRunningOnSteamDeck
                ? DialogueDefaultStyle_SteamDeck
                : DialogueDefaultStyle,
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
