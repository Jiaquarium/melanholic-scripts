using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(TextMeshProUGUI))]
public class Script_TMProZalgofy : MonoBehaviour
{
    private TextMeshProUGUI textUI;
    private bool didZalgofy;
    
    void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        
        // Hide TMP until it is Zalgofied
        textUI.maxVisibleCharacters = 0;
    }

    void Update()
    {
        if (!didZalgofy)
            Zalgofy();
    }

    public void Zalgofy()
    {
        // Wait until textInfo characterCount is available.
        if (
            textUI != null
            && textUI.textInfo != null
            && textUI.textInfo.characterCount > 0
        )
        {
            string zalgofied = Script_DialogueManager.ZalgofyString(textUI.text, textUI);
            textUI.text = zalgofied;

            // Make TMP fully visible.
            textUI.maxVisibleCharacters = textUI.textInfo.characterCount + 1;

            didZalgofy = true;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_TMProZalgofy))]
public class Script_TMProZalgofyTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_TMProZalgofy t = (Script_TMProZalgofy)target;
        if (GUILayout.Button("Zalgofy"))
            t.Zalgofy();
    }
}
#endif