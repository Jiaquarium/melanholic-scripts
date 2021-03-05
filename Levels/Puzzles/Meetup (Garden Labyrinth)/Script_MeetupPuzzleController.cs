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
    public void SwitchDown()
    {
        game.ChangeStateCutScene();
        
        outerState = PuzzleOuterStates.Open;

        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
    }

    public void SwitchUp()
    {
        game.ChangeStateCutScene();
        
        outerState = PuzzleOuterStates.Closed;

        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
    }

    public void Switch2Down()
    {
        game.ChangeStateCutScene();

        courtyardState = PuzzleCourtyardStates.Open;

        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 2);
    }

    public void Switch2Up()
    {
        game.ChangeStateCutScene();

        courtyardState = PuzzleCourtyardStates.Closed;

        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 3);
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
        SwitchUp();
        Switch2Up();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_MeetupPuzzleController))]
public class Script_MeetupPuzzleControllerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_MeetupPuzzleController t = (Script_MeetupPuzzleController)target;
        if (GUILayout.Button("SwitchDown"))
        {
            t.SwitchDown();
        }

        if (GUILayout.Button("SwitchUp"))
        {
            t.SwitchUp();
        }

        if (GUILayout.Button("Switch2Down"))
        {
            t.Switch2Down();
        }

        if (GUILayout.Button("Switch2Up"))
        {
            t.Switch2Up();
        }
    }
}
#endif