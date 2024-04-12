using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_TMProSetFontUniqueStyle : Script_TMProSetFontUnique
{
    [SerializeField] private Script_TextStylesManager textStylesManager;
    [SerializeField] private TextStyle textStyle;
    private Script_TextStyle myTextStyle;
    [SerializeField] private Script_TMProSetUniqueAttributes uniqueAttributes;
    [Space]
    [SerializeField] private bool isForceTextOverride;
    [SerializeField] private Script_TextStyle ENOverride;
    [SerializeField] private Script_TextStyle CNOverride;
    [SerializeField] private Script_TextStyle JPOverride;

    public Script_TextStylesManager TextStylesManager { get => textStylesManager; }
    
    public override void SetFontAttributes()
    {
        // To prevent editor prefab errors
        if (textStylesManager == null && !isForceTextOverride)
            return;
        
        text = GetComponent<TextMeshProUGUI>();

        myTextStyle = isForceTextOverride
            ? Script_LocalizationUtils.SwitchTextStyleOnLang(
                ENOverride,
                CNOverride,
                JPOverride
            )
            : textStylesManager.GetTextStyle(textStyle);

        font = myTextStyle.fontAsset;
        text.font = font;
        
        text.fontSize = myTextStyle.fontSize;
        text.characterSpacing = myTextStyle.characterSpacing;
        text.lineSpacing = myTextStyle.lineSpacing;
        text.wordSpacing = myTextStyle.wordSpacing;

        if (myTextStyle.isForceFontStyle)
            text.fontStyle = myTextStyle.fontStyle;

        if (uniqueAttributes != null)
            uniqueAttributes.SetUniqueAttributes(text);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_TMProSetFontUniqueStyle))]
    [CanEditMultipleObjects]
    public class Script_TMProSetFontUniqueStyleTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            if (GUILayout.Button("Text Style Manager: Refresh Font Attributes"))
            {
                Script_TextStylesManager.RefreshFontAttributes();
                EditorApplication.QueuePlayerLoopUpdate();
                SceneView.RepaintAll();
            }
        }
    }
#endif
}
