using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_CanvasZalgofyHelper : MonoBehaviour
{
    [SerializeField] private List<Script_TMProZalgofy> zalgoTexts;
    
    void OnValidate()
    {
        zalgoTexts = new List<Script_TMProZalgofy>(GetComponentsInChildren<Script_TMProZalgofy>(true));
    }

    // Note: for showing a Zalgo canvas with different zalgofied text each time, set the Zalgo text to update.
    // Then on Disable, manually call this to force another zalgofy, since TMProZalgofy's Update tracks
    // if the TMPro has been zalgofied.
    public void ZalgofyAll()
    {
        zalgoTexts.ForEach(zalgoText => zalgoText.Zalgofy());
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_CanvasZalgofyHelper))]
public class Script_CanvasZalgofyHelperTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_CanvasZalgofyHelper t = (Script_CanvasZalgofyHelper)target;
        if (GUILayout.Button("ZalgofyAll"))
            t.ZalgofyAll();
    }
}
#endif