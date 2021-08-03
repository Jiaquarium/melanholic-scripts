using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Frame Canvas Group should always be active while the Canvas Scaler object will
/// dictate visibility.
/// </summary>
public class Script_ArtFrameManager : MonoBehaviour
{
    public static Script_ArtFrameManager Control;
    
    [SerializeField] private Script_CanvasGroupController frameController;

    [SerializeField] private Script_CanvasConstantPixelScaler frameScaler;
    
    public void Open(Action cb)
    {
        frameScaler.AnimateOpen(cb);
    }

    public void Close(Action cb)
    {
        frameScaler.AnimateClose(cb);
    }
    
    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }

        frameController.gameObject.SetActive(true);
        frameScaler.gameObject.SetActive(false);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_ArtFrameManager))]
public class Script_ArtFrameManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_ArtFrameManager t = (Script_ArtFrameManager)target;
        if (GUILayout.Button("Open"))
        {
            t.Open(null);
        }

        if (GUILayout.Button("Close"))
        {
            t.Close(null);
        }
    }
}
#endif