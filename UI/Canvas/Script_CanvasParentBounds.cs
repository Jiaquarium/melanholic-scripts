using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_CanvasParentBounds : MonoBehaviour
{
    [SerializeField] private Script_ScalingBounds bounds;
    [SerializeField] private bool isCustomScaling;

    [SerializeField] private Script_CanvasConstantPixelScaler[] canvasScalers;
    
    [Header("Editor Only")]
    [SerializeField] private float scaleFactor;
    
    void OnValidate()
    {
        SetBounds();
    }

    private void SetBounds()
    {
        canvasScalers = GetComponentsInChildren<Script_CanvasConstantPixelScaler>(true);
        foreach (var canvasScaler in canvasScalers)
        {
            canvasScaler.Bounds = bounds;
            canvasScaler.IsCustomScaling = isCustomScaling;
        }
    }

    private void SetScaleFactor()
    {
        if (!Application.isEditor || Application.isPlaying)
            return;

        foreach (var canvasScaler in canvasScalers)
        {
            canvasScaler.ScaleFactor = scaleFactor;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_CanvasParentBounds))]
    public class Script_CanvasParentBoundsTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_CanvasParentBounds t = (Script_CanvasParentBounds)target;
            
            if (GUILayout.Button("Set Canvas Scale"))
            {
                t.SetScaleFactor();
            }
        }
    }
#endif
}
