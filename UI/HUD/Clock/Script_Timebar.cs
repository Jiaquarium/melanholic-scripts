using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_Timebar : MonoBehaviour
{
    public Slider slider;
    
    public float TimeElapsed
    {
        get => slider.value;
        set => slider.value = value;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_Timebar))]
public class Script_TimebarTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_Timebar t = (Script_Timebar)target;
        if (GUILayout.Button("+ 0.1f"))
        {
            t.TimeElapsed += 0.1f;
        }

        if (GUILayout.Button("- 0.1f"))
        {
            t.TimeElapsed -= 0.1f;
        }
    }
}
#endif