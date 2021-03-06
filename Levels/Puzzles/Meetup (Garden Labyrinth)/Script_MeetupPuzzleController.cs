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
    private enum PuzzleOuterStates
    {
        Closed          = 0,
        Open            = 1,
    }

    private enum PuzzleCourtyardStates
    {
        Closed          = 0,
        Open            = 1,
    }


    [SerializeField] private float WaitToPuzzleTransformTime;
    
    [SerializeField] private PuzzleOuterStates outerState;
    [SerializeField] private PuzzleCourtyardStates courtyardState;
    public List<Script_Player> playersOnTrigger;
    
    [SerializeField] private List<Script_Player> targetPlayersOnTrigger;
    [SerializeField] private bool isDone;

    [SerializeField] private Script_Game game;
    
    protected override void OnEnable()
    {
        base.OnEnable();

        InitialState();
    }
    
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

    // ------------------------------------------------------------------
    // Trigger Unity Events
    public void FloorSwitchDown()
    {
        game.ChangeStateCutScene();
        // Send event to clear doorways in case a Player/Puppet is blocking a door.
        Script_PuzzlesEventsManager.ClearDoorways();
        outerState = PuzzleOuterStates.Open;

        StartCoroutine(WaitToPuzzleTransformTimeline(0));
    }

    public void FloorSwitchUp()
    {
        game.ChangeStateCutScene();
        Script_PuzzlesEventsManager.ClearDoorways();
        outerState = PuzzleOuterStates.Closed;

        StartCoroutine(WaitToPuzzleTransformTimeline(1));
    }

    public void FloorSwitch2Down()
    {
        game.ChangeStateCutScene();
        Script_PuzzlesEventsManager.ClearDoorways();
        courtyardState = PuzzleCourtyardStates.Open;

        StartCoroutine(WaitToPuzzleTransformTimeline(2));
    }

    public void FloorSwitch2Up()
    {
        game.ChangeStateCutScene();
        Script_PuzzlesEventsManager.ClearDoorways();
        courtyardState = PuzzleCourtyardStates.Closed;

        StartCoroutine(WaitToPuzzleTransformTimeline(3));
    }

    private IEnumerator WaitToPuzzleTransformTimeline(int timelineIdx)
    {
        yield return new WaitForSeconds(WaitToPuzzleTransformTime);
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, timelineIdx);
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
        FloorSwitchUp();
        FloorSwitch2Up();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_MeetupPuzzleController))]
public class Script_MeetupPuzzleControllerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_MeetupPuzzleController t = (Script_MeetupPuzzleController)target;
        if (GUILayout.Button("FloorSwitchDown"))
        {
            t.FloorSwitchDown();
        }

        if (GUILayout.Button("FloorSwitchUp"))
        {
            t.FloorSwitchUp();
        }

        if (GUILayout.Button("FloorSwitch2Down"))
        {
            t.FloorSwitch2Down();
        }

        if (GUILayout.Button("FloorSwitch2Up"))
        {
            t.FloorSwitch2Up();
        }

        if (GUILayout.Button("Initial State"))
        {
            t.InitialState();
        }
    }
}
#endif