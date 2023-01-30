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
    
    [SerializeField] private Script_ElevatorManager elevatorManager;
    [SerializeField] private Script_Elevator elevator; /// Ref'ed by ElevatorManager

    private bool isInit = true;

    protected override void OnEnable()
    {
        PauseBgmForElevator();

        Script_Game.IsRunningDisabled = true;

        void PauseBgmForElevator()
        {
            Dev_Logger.Debug($"PauseBgmForElevator elevatorManager.IsBgmOn {elevatorManager.IsBgmOn}");

            // Only stop Bgm if the elevator manager hasn't already restarted it.
            // This happens on same frame but after Bgm Start on InitLevel.
            if (!elevatorManager.IsBgmOn)
            {
                Script_BackgroundMusicManager.Control.Stop();
            }
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Script_Game.IsRunningDisabled = false;
    }
    
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
            Dev_Logger.Debug($"Has field {TestField}: {lb.HasField(TestField)}");
        }
    }
}
#endif