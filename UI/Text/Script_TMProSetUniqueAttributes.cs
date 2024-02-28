using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_TMProSetUniqueAttributes : MonoBehaviour
{
    [SerializeField] private bool isUniquePositions;
    [SerializeField] private Vector2 EN_position;
    [SerializeField] private Vector2 CN_position;
    [Space]
    [SerializeField] private bool isUniqueAlignment;
    [SerializeField] private TextAlignmentOptions EN_alignment;
    [SerializeField] private TextAlignmentOptions CN_alignment;

    // Set attributes based on the specific needs of that object
    public void SetUniqueAttributes(TextMeshProUGUI text)
    {
        Script_LocalizationUtils.SwitchActionOnLang(
            EN_action: EN_SetUniqueAttributes,
            CN_action: CN_SetUniqueAttributes
        );

        void EN_SetUniqueAttributes()
        {
            SetPosition(EN_position);
            SetAlignment(EN_alignment);
        }

        void CN_SetUniqueAttributes()
        {
            SetPosition(CN_position);
            SetAlignment(CN_alignment);
        }

        void SetPosition(Vector2 newPosition)
        {
            if (isUniquePositions)
                text.rectTransform.anchoredPosition = newPosition;
        }

        void SetAlignment(TextAlignmentOptions textAlignmentOption)
        {
            if (isUniqueAlignment)
                text.alignment = textAlignmentOption;
        }
    }

#if UNITY_EDITOR
    // For dev use only
    private void SetUniqueAttributesToCurrentInspector()
    {
        var TMProSetFontUniqueStyle = GetComponent<Script_TMProSetFontUniqueStyle>();
        
        if (TMProSetFontUniqueStyle != null)
        {
            var text = TMProSetFontUniqueStyle.GetComponent<TextMeshProUGUI>();

            if (text != null)
            {
                Script_LocalizationUtils.ActionForEachLang(
                    EN_action: () => EN_attributesToInspector(text),
                    CN_action: () => CN_attributesToInspector(text)
                );

                return;
            }
        }

        Debug.LogWarning("Make sure there is both Script_TMProSetFontUniqueStyle and TextMeshProUGUI components on this object.");

        void EN_attributesToInspector(TextMeshProUGUI text)
        {
            EN_position = text.rectTransform.anchoredPosition;
            EN_alignment = text.alignment;
        }

        void CN_attributesToInspector(TextMeshProUGUI text)
        {
            CN_position = text.rectTransform.anchoredPosition;
            CN_alignment = text.alignment;
        }
    }

    [CustomEditor(typeof(Script_TMProSetUniqueAttributes))]
    public class Script_TMProSetUniqueAttributesTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_TMProSetUniqueAttributes t = (Script_TMProSetUniqueAttributes)target;

            if (GUILayout.Button("Set Unique Attributes to Inspector's"))
            {
                Undo.RecordObject(t, "Set unique attribute values to current values in the inspector");
                t.SetUniqueAttributesToCurrentInspector();
                PrefabUtility.RecordPrefabInstancePropertyModifications(t);
            }
        }
    }
#endif
}