using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

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


    [SerializeField] private Script_Puppet Latte;
    [SerializeField] private Script_Puppet Kaffe;
    [SerializeField] private Script_Marker LatteSpawn;
    [SerializeField] private Script_Marker KaffeSpawn;

    [SerializeField] private Script_VCamera puppeteerVCam;
    
    [SerializeField] private Script_Game game;

    private Script_TimelineController timelineController;
    
    public bool IsDone
    {
        get => isDone;
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();

        InitialState();

        Script_PlayerEventsManager.OnPuppeteerActivate      += OnPuppeteerActivate;
        Script_PlayerEventsManager.OnPuppeteerDeactivate    += OnPuppeteerDeactivate;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Script_PlayerEventsManager.OnPuppeteerActivate      -= OnPuppeteerActivate;
        Script_PlayerEventsManager.OnPuppeteerDeactivate    -= OnPuppeteerDeactivate;
    }

    void Awake()
    {
        timelineController = GetComponent<Script_TimelineController>();
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
        Script_PuzzlesEventsManager.PuzzleSuccess(PuzzleId);
        
        isDone = true;
    }

    // Timeline to call OnPuppeteerActivateTimelineDone
    private void OnPuppeteerActivate()
    {
        if (!isDone)
        {
            game.ChangeStateCutScene();
            timelineController.PlayableDirectorPlayFromTimelines(0, 4);
            Script_VCamManager.VCamMain.SetNewVCam(puppeteerVCam);
        }
    }

    private void OnPuppeteerDeactivate()
    {
        if (!isDone)    Script_VCamManager.VCamMain.SwitchToMainVCam(puppeteerVCam);
    }

    // ------------------------------------------------------------------
    // Trigger Unity Events
    public void FloorSwitchDown()
    {
        // If puzzle is done, the Labyrinth should be gone.
        if (IsDone)     return;

        game.ChangeStateCutScene();
        // Send event to clear doorways in case a Player/Puppet is blocking a door.
        Script_PuzzlesEventsManager.ClearDoorways();
        outerState = PuzzleOuterStates.Open;

        StartCoroutine(WaitToPuzzleTransformTimeline(0));
    }

    public void FloorSwitch2Down()
    {
        if (IsDone)     return;
        
        game.ChangeStateCutScene();
        Script_PuzzlesEventsManager.ClearDoorways();
        courtyardState = PuzzleCourtyardStates.Open;

        StartCoroutine(WaitToPuzzleTransformTimeline(2));
    }
    
    public void FloorSwitchUp(bool isInitialize = false)
    {
        if (IsDone)     return;
        
        game.ChangeStateCutScene();
        Script_PuzzlesEventsManager.ClearDoorways();
        outerState = PuzzleOuterStates.Closed;

        StartCoroutine(WaitToPuzzleTransformTimeline(1, isInitialize));
    }

    public void FloorSwitch2Up(bool isInitialize = false)
    {
        if (IsDone)     return;
        
        game.ChangeStateCutScene();
        Script_PuzzlesEventsManager.ClearDoorways();
        courtyardState = PuzzleCourtyardStates.Closed;

        StartCoroutine(WaitToPuzzleTransformTimeline(3, isInitialize));
    }

    private IEnumerator WaitToPuzzleTransformTimeline(int timelineIdx, bool isInitialize = false)
    {
        yield return new WaitForSeconds(WaitToPuzzleTransformTime);
        
        var director = timelineController.PlayableDirectorPlayFromTimelines(0, timelineIdx);

        if (isInitialize)
        {
            var playableAsset = (PlayableAsset)timelineController.timelines[timelineIdx];
            director.time = playableAsset.duration;
            director.Evaluate();
            director.Stop();

            // Timeline ending signal is not called
            OnPuzzleTransformDone();
        }
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void OnPuzzleTransformDone()
    {
        Debug.Log("ON PUZZLE TRANSFORM DONE CALLED ON TIMELINE END!!!!!!!");

        game.ChangeStateInteract();
    }

    public void PuppeteerActivateTimelinePlayerBuff()
    {
        Script_Game.Game.GetPlayer().SetBuffEffectActive(true);
    }
    
    public void OnPuppeteerActivateTimelineDone()
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
        FloorSwitchUp(true);
        FloorSwitch2Up(true);
        
        if (!isDone)
        {
            Kaffe.Teleport(KaffeSpawn.transform.position);
            Latte.Teleport(LatteSpawn.transform.position);
        }
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