using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_CanvasGroupController))]
[RequireComponent(typeof(Animator))]
public class Script_PulseImage : MonoBehaviour
{
    public static readonly int PulseImage = Animator.StringToHash("Pulse Image");
    [SerializeField] private Animator animator;
    [SerializeField] private Script_CanvasGroupController canvasGroupController;
    
    // ------------------------------------------------------------
    // Unity Events
    
    public void OpenPulse()
    {
        canvasGroupController.Open();
        Pulse();
    }

    public void Close()
    {
        canvasGroupController.Close();
    }

    // ------------------------------------------------------------

    private void Pulse()
    {
        animator.Play(PulseImage);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_PulseImage))]
public class Script_PulseImageTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_PulseImage t = (Script_PulseImage)target;
        if (GUILayout.Button("OpenPulse"))
            t.OpenPulse();
    }
}
#endif