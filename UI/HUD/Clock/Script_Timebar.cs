using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_Timebar : MonoBehaviour
{
    [SerializeField] private Image imageFill;
    [SerializeField] private Slider slider;
    
    public float TimeElapsed
    {
        get => imageFill.fillAmount;
        set 
        {
            slider.value = value;
            imageFill.fillAmount = value;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_Timebar))]
public class Script_TimebarTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_Timebar t = (Script_Timebar)target;
        if (GUILayout.Button("+ 1f"))
        {
            t.TimeElapsed += 1f / (float)Script_ClockManager.TimebarIncrements;
        }

        if (GUILayout.Button("- 1f"))
        {
            t.TimeElapsed -= 1f / (float)Script_ClockManager.TimebarIncrements;
        }
    }
}
#endif