﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_38 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */

    /* ======================================================================= */
    [SerializeField] private bool isTriggerActivated;
       
    public void Start()
    {
    
    }

    public void OnTriggerWallTransition()
    {
        Debug.Log("Change wall sprites");
        isTriggerActivated = true;
    }
    
    public override void InitialState()
    {
        isTriggerActivated = false;
    }
    
    public override void Setup()
    {
    }        
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_38))]
public class Script_LevelBehavior_38Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_38 t = (Script_LevelBehavior_38)target;
        if (GUILayout.Button("InitalState()"))
        {
            t.InitialState();
        }
    }
}
#endif