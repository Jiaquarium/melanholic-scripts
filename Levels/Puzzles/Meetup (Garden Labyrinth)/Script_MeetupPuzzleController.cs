using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// TriggerStay should send this the objects inside the Trigger, and
/// this controller will check to ensure the required objects are present.
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
public class Script_MeetupPuzzleController : Script_PuzzleController
{
    private enum PuzzleStates
    {
        Default         = 0,
        CourtyardOpen   = 1,
    }
    [SerializeField] private PuzzleStates state;
    public List<Script_Player> playersOnTrigger;
    
    [SerializeField] private List<Script_Player> targetPlayersOnTrigger;
    [SerializeField] private bool isDone;

    [SerializeField] private Script_Game game;
    
    void Update()
    {
        if (CheckMatchingPlayersOnTrigger())
            CompleteState();
    }
    
    public override void CompleteState()
    {
        if (isDone)     return;

        Debug.Log("PUZZLE IS DONE!!!!!!!!! BOTH TARGETS ON PLATFORM");
        
        isDone = true;
    }

    public void WeightTriggerOn()
    {
        game.ChangeStateCutScene();
        
        state = PuzzleStates.CourtyardOpen;

        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
    }

    public void WeightTriggerOff()
    {
        game.ChangeStateCutScene();
        
        state = PuzzleStates.Default;

        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void OnPuzzleTransformDone()
    {
        game.ChangeStateInteract();
    }
    // ------------------------------------------------------------------

    private bool CheckMatchingPlayersOnTrigger()
    {
        if (playersOnTrigger.Count != targetPlayersOnTrigger.Count)     return false;
        
        foreach (Script_Player player in targetPlayersOnTrigger)
        {
            if (playersOnTrigger.Find(target => player == target) == null)    return false;
        }

        return true;
    }

    public override void InitialState()
    {
        WeightTriggerOff();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_MeetupPuzzleController))]
public class Script_MeetupPuzzleControllerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_MeetupPuzzleController t = (Script_MeetupPuzzleController)target;
        if (GUILayout.Button("WeightTriggerOn"))
        {
            t.WeightTriggerOn();
        }

        if (GUILayout.Button("WeightTriggerOff"))
        {
            t.WeightTriggerOff();
        }
    }
}
#endif