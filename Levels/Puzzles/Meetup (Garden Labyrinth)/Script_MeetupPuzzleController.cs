using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// TriggerStay should send this the objects inside the Trigger, and
/// this controller will check to ensure the required objects are present.
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
public class Script_MeetupPuzzleController : Script_PuppetPuzzleController
{
    [SerializeField] private float WaitToPuzzleTransformTime;
    [SerializeField] private float ResetTriggerWaitTime;
    
    // MeetupTriggerReliableStay will add players to this List.
    public List<Script_Player> playersOnTrigger;
    
    [SerializeField] private List<Script_Player> targetPlayersOnTrigger;

    [SerializeField] private Script_Puppet Latte;
    [SerializeField] private Script_Puppet Kaffe;
    [SerializeField] private Script_Marker LatteSpawn;
    [SerializeField] private Script_Marker KaffeSpawn;

    [Tooltip("Specify floor switches on map")]
    [SerializeField] private List<Script_TriggerReliableStayFloorSwitch> floorSwitches;
    [SerializeField] private Script_VCamera puppeteerVCam;
    
    [SerializeField] private Script_LevelBehavior_46 LB46;

    private bool isWallsMoving;
    private bool isIgnorePhysicsOnReset;
    
    protected override void OnEnable()
    {
        base.OnEnable();

        InitialState();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    void Update()
    {
        if (CheckMatchingPlayersOnTrigger())
            CompleteState();
    }
    
    public override void CompleteState()
    {
        if (IsDone)     return;

        Dev_Logger.Debug("PUZZLE IS DONE!!!!!!!!! BOTH TARGETS ON PLATFORM");
        Script_PuzzlesEventsManager.PuzzleSuccess(PuzzleId);
        
        IsDone = true;
    }

    protected override void OnPuppeteerActivate()
    {
        if (!IsDone)
        {
            game.ChangeStateCutScene();

            Script_SFXManager.SFX.PlayPuppeteerEffectOn();
            
            // This Timeline is also shared with Urselks Saloon Hallway Puppet Puzzle.
            timelineController.PlayableDirectorPlayFromTimelines(0, 4);
        }
    }

    protected override void OnPuppeteerDeactivate()
    {
        if (!IsDone)
        {
            game.ChangeStateCutScene();

            Script_SFXManager.SFX.PlayPuppeteerEffectOff();
            
            // This Timeline is also shared with Urselks Saloon Hallway Puppet Puzzle.
            timelineController.PlayableDirectorPlayFromTimelines(0, 5);
        }   
    }

    // ------------------------------------------------------------------
    // Trigger Unity Events
    public void FloorSwitchDown()
    {
        // If puzzle is done, the Labyrinth should be gone.
        if (IsDone || isWallsMoving)
            return;
        
        isWallsMoving = true;

        game.ChangeStateCutScene();
        // Send event to clear doorways in case a Player/Puppet is blocking a door.
        Script_PuzzlesEventsManager.ClearDoorways();

        StartCoroutine(WaitToPuzzleTransformTimeline(0));
    }

    public void FloorSwitch2Down()
    {
        if (IsDone || isWallsMoving)
            return;
        
        isWallsMoving = true;
        
        game.ChangeStateCutScene();
        Script_PuzzlesEventsManager.ClearDoorways();

        StartCoroutine(WaitToPuzzleTransformTimeline(2));
    }
    
    public void FloorSwitchUp(bool isInitialize = false)
    {
        // Avoid calling this On Disable for ReliableStayTriggers.
        if (!gameObject.activeInHierarchy || IsDone || isWallsMoving || isIgnorePhysicsOnReset)
            return;
        
        isWallsMoving = true;
        
        if (!isInitialize)
            game.ChangeStateCutScene();
        
        Script_PuzzlesEventsManager.ClearDoorways();

        if (isInitialize)
            FastForwardTransform(1);
        else
            StartCoroutine(WaitToPuzzleTransformTimeline(1));
    }

    public void FloorSwitch2Up(bool isInitialize = false)
    {
        // Avoid calling this On Disable for ReliableStayTriggers.
        if (!gameObject.activeInHierarchy || IsDone || isWallsMoving || isIgnorePhysicsOnReset)
            return;
        
        isWallsMoving = true;
        
        if (!isInitialize)
            game.ChangeStateCutScene();
        
        Script_PuzzlesEventsManager.ClearDoorways();

        if (isInitialize)
            FastForwardTransform(3);
        else
            StartCoroutine(WaitToPuzzleTransformTimeline(3));
    }

    private IEnumerator WaitToPuzzleTransformTimeline(int timelineIdx)
    {
        yield return new WaitForSeconds(WaitToPuzzleTransformTime);
        
        timelineController.PlayableDirectorPlayFromTimelines(0, timelineIdx);
    }

    private void FastForwardTransform(int timelineIdx)
    {
        var director = timelineController.PlayableDirectorPlayFromTimelines(0, timelineIdx);

        // On Initialization, don't actually play the timeline animation. Only need the ending
        // state, so fast forward to it.
        var playableAsset = (PlayableAsset)timelineController.timelines[timelineIdx];
        director.time = playableAsset.duration;
        director.Evaluate();
        director.Stop();

        // Timeline ending signal function (OnPuzzleTransformDone) is not called on manual Evaluate.
        // https://forum.unity.com/threads/timeline-notifications-arent-sent-in-playabledirector-manual-mode.711494/
        OnPuzzleTransformDone(isInitialize: true);
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void OnPuzzleTransformDone(bool isInitialize = false)
    {
        isWallsMoving = false;
        
        // If it is the unique blocking cut scene, need to remain in cut scene state.
        // Allow LB46 to handle changing state back to interact.
        // This should only happen if there's a bug and Kaffe moves 2 spaces out of
        // the FloorSwitch2 onto his Unique Blocking Trigger.
        if (
            !LB46.IsUniqueBlockedCutScene
            // Initialize Moving Walls Timeline should not change state to cut scene,
            // so ignore trying to reset state to Interact.
            && !isInitialize
        )
        {
            game.ChangeStateInteract();
        }
    }

    public void SetPuppeteerVCam()
    {
        Script_VCamManager.VCamMain.SetNewVCam(puppeteerVCam);
    }

    public override void PuppeteerDeactivateTimelinePuppetBuffs()
    {
        Latte.SetBuffEffectActive(true);
        Kaffe.SetBuffEffectActive(true);
    }

    public void SetMainVCam()
    {
        Script_VCamManager.VCamMain.SwitchToMainVCam(puppeteerVCam);
    }

    public override void OnPuppeteerDeactivateTimelineDone()
    {
        game.ChangeStateInteract();
        Latte.SetBuffEffectActive(false);
        Kaffe.SetBuffEffectActive(false);
    }

    // From reset trigger
    // Note: reset trigger must opt out of using events and will call this directly
    public void InitialStateResetTrigger()
    {
        FloorSwitchUp(true);
        FloorSwitch2Up(true);
        
        game.ChangeStateCutScene();

        // Disable SwitchUp events & the floorSwitch SFXs (to allow Puppets to move off switches without triggering them)
        isIgnorePhysicsOnReset = true;
        floorSwitches.ForEach(floorSwitch => floorSwitch.IsSFXDisabled = true);
        
        InitialStatePuppets();

        StartCoroutine(WaitToInteract());

        IEnumerator WaitToInteract()
        {
            yield return new WaitForSecondsRealtime(ResetTriggerWaitTime);

            isIgnorePhysicsOnReset = false;
            floorSwitches.ForEach(floorSwitch => floorSwitch.IsSFXDisabled = false);
            game.ChangeStateInteract();
        }
    }
    
    // ------------------------------------------------------------------

    private bool CheckMatchingPlayersOnTrigger()
    {
        if (playersOnTrigger.Count == 0)
            return false;
        
        foreach (Script_Player player in targetPlayersOnTrigger)
        {
            if (playersOnTrigger.Find(target => player == target) == null)
                return false;
        }

        return true;
    }

    // Reset puzzle on enable. Moving the puppets in OnEnable prevents unwanted trigger effects (SFX) from occurring
    // since OnEnable happens before physics.
    public override void InitialState()
    {
        FloorSwitchUp(true);
        FloorSwitch2Up(true);
        
        InitialStatePuppets();
    }

    private void InitialStatePuppets()
    {
        if (!IsDone)
        {
            if (!LB46.IsInitialized)
            {
                LB46.InitializePuppets();
            }
            
            Kaffe.Teleport(KaffeSpawn.transform.position);
            Kaffe.FaceDirection(Directions.Right);
            
            Latte.Teleport(LatteSpawn.transform.position);
            Latte.FaceDirection(Directions.Down);
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