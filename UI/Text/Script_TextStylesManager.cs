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
}

public class Script_TextStylesManager : MonoBehaviour
{
    [SerializeField] private Script_TextStyle DialogueDefaultStyle;
    [SerializeField] private Script_TextStyle DialogueNameStyle;
    [SerializeField] private Script_TextStyle UIStyle;
    [SerializeField] private Script_TextStyle UIEmphasisStyle;
    [SerializeField] private Script_TextStyle UILetterSelectStyle;
    [SerializeField] private Script_TextStyle BadEndingLeadInStyle;
    [SerializeField] private Script_TextStyle BadEndingDescriptionStyle;
    [SerializeField] private Script_TextStyle BadEndingHeadingStyle;
    [SerializeField] private Script_TextStyle MenuTopNavStyle;
    [SerializeField] private Script_TextStyle MenuItemNameStyle;
    [SerializeField] private Script_TextStyle MenuItemDescriptionStyle;
    [SerializeField] private Script_TextStyle MapNotificationStyle;
    [SerializeField] private Script_TextStyle TeletypeMyneStyle;
    [SerializeField] private Script_TextStyle FaceOffStyle;
    [SerializeField] private Script_TextStyle AwakeningStyle;
    [SerializeField] private Script_TextStyle GoodTrueEndingStyle;
    [SerializeField] private Script_TextStyle SettingsHeadingStyle;
    [SerializeField] private Script_TextStyle CtaStyle;
    [SerializeField] private Script_TextStyle FileActionBannerStyle;
    [SerializeField] private Script_TextStyle NotesHintStyle;

    public Script_TextStyle GetTextStyle(TextStyle style) => style switch
    {
        TextStyle.Name => DialogueNameStyle,
        TextStyle.UI => UIStyle,
        TextStyle.UILetterSelect => UILetterSelectStyle,
        TextStyle.UIEmphasis => UIEmphasisStyle,
        TextStyle.BadEndingLeadIn => BadEndingLeadInStyle,
        TextStyle.BadEndingDescription => BadEndingDescriptionStyle,
        TextStyle.BadEndingHeading => BadEndingHeadingStyle,
        TextStyle.MenuTopNav => MenuTopNavStyle,
        TextStyle.MenuItemName => MenuItemNameStyle,
        TextStyle.MenuItemDescription => MenuItemDescriptionStyle,
        TextStyle.MapNotification => MapNotificationStyle,
        TextStyle.TeletypeMyne => TeletypeMyneStyle,
        TextStyle.FaceOff => FaceOffStyle,
        TextStyle.Awakening => AwakeningStyle,
        TextStyle.GoodTrueEnding => GoodTrueEndingStyle,
        TextStyle.SettingsHeading => SettingsHeadingStyle,
        TextStyle.Cta => CtaStyle,
        TextStyle.FileActionBanner => FileActionBannerStyle,
        TextStyle.NotesHint => NotesHintStyle,
        _ => DialogueDefaultStyle,
    };

    // For use in dev only
    private void RefreshFontAttributes()
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
                t.RefreshFontAttributes();
                EditorApplication.QueuePlayerLoopUpdate();
                SceneView.RepaintAll();
            }
        }
    }
    #endif
}
