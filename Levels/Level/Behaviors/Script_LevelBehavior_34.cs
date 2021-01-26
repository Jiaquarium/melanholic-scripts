using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_34 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    
    /* ======================================================================= */

    [SerializeField] private Transform exitParent;
    [SerializeField] private Script_Elevator elevator; /// Ref'ed by ElevatorManager

    private bool isInit = true;

    public override void Setup()
    {
        game.SetupInteractableObjectsExit(exitParent, isInit);

        isInit = false;    
    }        
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_34))]
public class Script_LevelBehavior_34Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_34 lb = (Script_LevelBehavior_34)target;
        if (GUILayout.Button("Test for elevator Field"))
        {
            string TestField = "elevator";
            Debug.Log($"Has field {TestField}: {lb.HasField(TestField)}");
        }
    }
}
#endif