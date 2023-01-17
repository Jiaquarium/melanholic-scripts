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
    [Tooltip("Set this true if you are planning to call Zalgofy from elsewhere manually")]
    [SerializeField] private bool isNoUpdate;

    private TextMeshProUGUI textUI;
    private bool didZalgofy;
    
    void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        
        if (isNoUpdate)
            return;
        
        // Hide TMP until it is Zalgofied
        // Note: even if this component is disabled this will still be called.
        textUI.maxVisibleCharacters = 0;

        Dev_Logger.Debug($"{name} Setting max visible characters to 0");
    }

    void Update()
    {
        if (!didZalgofy && !isNoUpdate)
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
            // TBD Note: DialogueManager's zalgofy may be 1 off too short
            if (!isNoUpdate)
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