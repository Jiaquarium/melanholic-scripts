using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Text;
#endif

public class Script_LightFXCurve : MonoBehaviour
{
    // Note: to calculate the end percent to reach specific end intensity, use the formula
    // (8f - StartingIntensity) * x + StartingIntensity = NewEndingIntensity
    // where 8f is the default endingIntensity and x is the ending curve y value.
    [SerializeField] private AnimationCurve lightCurve;

    public AnimationCurve LightCurve => lightCurve;

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_LightFXCurve))]
    public class Script_LightFXCurveTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LightFXCurve t = (Script_LightFXCurve)target;
            if (GUILayout.Button("Check Light Curve Always Ascending"))
            {
                var last = 0f;
                bool isAscending = true;
                
                for (float i = 0f; i <= 1f; i = i + 0.0001f)
                {
                    float current = t.lightCurve.Evaluate(i);

                    if (current < last)
                    {
                        Debug.Log($"<color=red>Error at time {i} with diff {current - last}</color>");
                        isAscending = false;
                    }

                    last = current;
                }

                if (isAscending)
                    Debug.Log($"<color=green>Always ascending</color>");
                else
                    Debug.Log($"<color=red>Errors, descends at above points</color>");
            }

            if (GUILayout.Button("Print Light Chart"))
            {
                int MaxTime = 60;
                const float StartingIntensity = 0.15f;
                const float EndingIntensity = 8f;
                var stringBuilder = new StringBuilder();

                for (int i = 0; i <= MaxTime; i = i + 1)
                {
                    float percentElapsed = t.lightCurve.Evaluate(i / 60f);
                    var lightIntensity = StartingIntensity + (percentElapsed * (EndingIntensity - StartingIntensity));

                    stringBuilder.AppendLine($"{lightIntensity}");
                }
                
                Debug.Log($"{stringBuilder}");
            }
        }
    }
#endif
}
