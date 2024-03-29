﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(Script_SaveGameControl))]
public class Script_SaveTester : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Script_SaveGameControl control = (Script_SaveGameControl)target;
        if (GUILayout.Button("Save"))
        {
            control.Save();
        }

        if (GUILayout.Button("Delete Game Data"))
        {
            Script_SaveGameControl.Delete();
        }

        if (GUILayout.Button("ChangeSaveFileToSlot1"))
        {
            Script_SaveGameControl.saveSlotId = 1;
        }

        if (GUILayout.Button("Copy Slot 0 to 1"))
        {
            Script_SaveGameControl.Copy(0, 1);
        }
    }
}
#endif
