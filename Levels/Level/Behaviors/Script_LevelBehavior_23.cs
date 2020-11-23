using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// NOTE: when dev'ing ALWAYS start in lobby for triggers to be fully functional
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_23 : Script_LevelBehavior
{
    [SerializeField] private Script_LevelBehavior_24 LB24;
    [SerializeField] private Transform triggerParent;
    [SerializeField] private Transform pillarParent;
    [SerializeField] private Transform pushablesParent;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeDurationLong;
    [SerializeField] private Script_PushableTriggerStay[] triggers;
    [SerializeField] private Script_Tracker[] pillars;
    [SerializeField] private Script_Pushable[] pushables;
    [SerializeField] private Script_FullArtParent fullArtParent;

    private bool isInit = true;
    
    protected override void OnEnable()
    {
        // OnPuzzleSuccess is handled in LB24
        if (!LB24.isPuzzleComplete)
        {
            Script_PuzzlesEventsManager.OnPuzzleProgress += OnPuzzleProgress;
            Script_PuzzlesEventsManager.OnPuzzleProgress2 += OnPillarsOnCorrectTriggers;
        }
        
        SetPillarsVisibility(false);
        ActivateTriggersAndPillars(true);
    }

    protected override void OnDisable() {
        if (!LB24.isPuzzleComplete)
        {
            Script_PuzzlesEventsManager.OnPuzzleProgress -= OnPuzzleProgress;
            Script_PuzzlesEventsManager.OnPuzzleProgress2 -= OnPillarsOnCorrectTriggers;
        }

        ActivateTriggersAndPillars(false);
        SetPillarsVisibility(true);
    }   

    private void OnPuzzleProgress()
    {
        print("puzzle progress");

        game.ChangeStateCutScene();
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.PillarPuzzleProgress,
            Script_SFXManager.SFX.PillarPuzzleProgressVol
        );
        Script_VCamManager.VCamMain.GetComponent<Script_CameraShake>().Shake(
            shakeDuration,
            Const_Camera.Shake.AmplitudeMed,
            Const_Camera.Shake.FrequencyMed, 
            () => game.ChangeStateInteract()
        );
    }
    /// <summary>
    /// show reaction to completing the puzzle
    /// not actually finished though until the player goes into KTX Room2
    /// subscribed to PuzzleProgress2 event
    /// </summary>
    private void OnPillarsOnCorrectTriggers()
    {
        print("puzzle progress 2!!! all triggers have pillars on top now!");
        
        game.ChangeStateCutScene();
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.PillarPuzzleProgress2,
            Script_SFXManager.SFX.PillarPuzzleProgress2Vol
        );
        Script_VCamManager.VCamMain.GetComponent<Script_CameraShake>().Shake(
            shakeDurationLong,
            Const_Camera.Shake.AmplitudeMed,
            Const_Camera.Shake.FrequencyMed, 
            () => game.ChangeStateInteract()
        );
    }

    private void ActivateTriggersAndPillars(bool isActive)
    {
        // set triggers and pillars to active
        triggerParent.gameObject.SetActive(isActive);
        pillarParent.gameObject.SetActive(isActive);
    }

    private void SetPillarsVisibility(bool isOn)
    {
        foreach (Script_Tracker pillar in pillars)  pillar.SetVisibility(isOn);
    }

    /// <summary>
    /// Match the trackers since they are immovable.
    /// Deactivate pushables to be pushed after.
    /// Should reload like this on loads.
    /// ** LB24 will call CompletedState here **
    /// </summary>
    public void CompletedState()
    {
        foreach (Script_Pushable pushable in pushables)
        {
            pushable.GetComponent<Script_Trackable>().MatchMyTrackerPosition();
            pushable.IsDisabled = true;
        }
    }

    private void Awake()
    {
        triggers    = triggerParent.GetChildren<Script_PushableTriggerStay>();
        pillars     = pillarParent.GetChildren<Script_Tracker>();
        pushables   = pushablesParent.GetChildren<Script_Pushable>();

        /// LB24 will call CompletedState as well
        /// call LB24's PuzzleFinishedState to set the pillars and in turn the trackables
        /// will set position based on them
        /// player enters on a reload
        if (LB24.isPuzzleComplete)  LB24.PuzzleFinishedState();
    }

    public override void Setup()
    {
        game.SetupInteractableFullArt(fullArtParent.transform, isInit);
        
        isInit = false;
    }

    #if UNITY_EDITOR
    public void Test_SetSuccessCase()
    {
        pushables[0].transform.position = new Vector3(-58, 0, 238);
        pushables[1].transform.position = new Vector3(-60, 0, 234);
        pushables[2].transform.position = new Vector3(-54, 0, 233);
    }
    #endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_23))]
public class Script_LevelBehavior_23Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_23 lb = (Script_LevelBehavior_23)target;
        if (GUILayout.Button("Test_SetSuccessCase()"))
        {
            lb.Test_SetSuccessCase();
        }
    }
}
#endif
