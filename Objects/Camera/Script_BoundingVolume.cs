using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_BoundingVolume : MonoBehaviour
{
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_BoundingVolume))]
public class Script_BoundingVolumeTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_BoundingVolume t = (Script_BoundingVolume)target;
        if (GUILayout.Button("Invalidate Confiner Cache"))
        {
            Script_VCamManager.ActiveVCamera.InvalidateConfinerCache();
        }
    }
}
#endif
