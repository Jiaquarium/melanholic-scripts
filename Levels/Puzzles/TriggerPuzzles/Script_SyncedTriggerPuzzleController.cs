using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// controls triggers where pushabled need to be pushed at the same time
/// NOTE: the trigger Ids must be named DIFFERENTLY
/// NOTE: currently a success case only when pushable nameIds match!!!
/// 
/// once a pushable hits a trigger, begins a coroutine that sets the time limit
/// to fill an array with every subsequent pushable entering a trigger
/// once time runs out, state is initialized
/// 
/// Pushable Ids are stored in a List to detect if new matches previously added
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_SyncedTriggerPuzzleController : Script_TriggerPuzzleController
{   
    [SerializeField] private int successCount; /// set equal to number of syncs needed for success
    [SerializeField] private float timeBuffer; // give some slack to emulate pushing at "same time"
    [SerializeField] private Script_PushablesParent pushablesParent;
    [SerializeField] Script_LevelBehavior myLevelBehavior;
    [SerializeField] string initializeFireMethodName;
    
    [Space]

    public bool isComplete;
    [SerializeField] private string[] activatedTriggersIds = new string[0];
    public List<string> activatedPushableIds; // public for debugging
    [SerializeField] private float time;
    [SerializeField] private AudioClip audioClip;
    private Coroutine trackingCoroutine;
    public int currentSuccessCount {get; private set;}
    private bool isTracking;

    
    /// Using fixed update to keep in sync with the Pushable / allow to work on lower fps
    private void FixedUpdate()
    {
        /// do timer things here, since we want to calc time consistently with physics frame rate
        /// NOTE: NEVER DO IMPORTANT TIMERS ON A DIFFERENT THREAD
        if (!isTracking)    return;

        time -= Time.fixedDeltaTime;
        if (time < 0f)
        {
            time = 0f;
            print($"Time.deltaTime: {Time.deltaTime}");
            print("RAN OUT OF TIME BUFFER ------------------------------; resetting");
            StopTracking();
        }
    }
    
    public override void TriggerActivated(string triggerId, Collider col)
    {
        if (isComplete)     return;
        
        string pushableId = col.transform.parent.GetComponent<Script_Pushable>().nameId;
        if (string.IsNullOrEmpty(pushableId))   Debug.LogError($"Script_Pushable is missing nameId: {col}");
        
        if (!isTracking)
        {
            InitializeTracking(triggerId, pushableId);
            return;
        }
        
        // if triggerId already in triggers (pushing into same trigger)
        // OR if pushableId doesn't match previously pushed in pushable
        // then restart tracking again fresh
        bool isPushableIdMatching = activatedPushableIds.Count > 0 && pushableId == activatedPushableIds[activatedPushableIds.Count - 1];
        if (Array.IndexOf(activatedTriggersIds, triggerId) > 0 || !isPushableIdMatching)
        {
            print($"reinitializing tracking; pushableId: {pushableId}, last pushableId: {activatedPushableIds[activatedPushableIds.Count - 1]}");
            InitializeTracking(triggerId, pushableId);
        }
        else
        {
            // fill the tracking arrays
            TrackTriggerActivation(triggerId, pushableId);
        }
    }

    public override void InitialState()
    {
        if (isComplete)     return;
        
        // tell pushables to go back to initialState
        Script_Pushable[] pushables = pushablesParent.transform.GetChildren<Script_Pushable>();
        foreach (Script_Pushable p in pushables)    p.InitialState();

        // currentSuccessCount back to original
        currentSuccessCount = successCount;

        // return fire back to normal
        Dev_Logger.Debug($"Checking if method name exists: {initializeFireMethodName}");
        if (
            myLevelBehavior != null
            && myLevelBehavior.HasMethod(initializeFireMethodName)
        )
        {
            Dev_Logger.Debug($"Invoking: {initializeFireMethodName}");
            myLevelBehavior.InvokeMethod(initializeFireMethodName);
        }
        
        StopTracking();
    }

    private void TrackTriggerActivation(string triggerId, string pushableId)
    {
        for (int i = 0; i < activatedTriggersIds.Length; i++)
        {
            // fill in the next available trigger slot
            // also add pushableId to List
            if (activatedTriggersIds[i] == null)
            {
                activatedTriggersIds[i] = triggerId;
                activatedPushableIds.Add(pushableId);
                
                // we're filling in last trigger
                if (i == activatedTriggersIds.Length - 1)
                {
                    print($"Delay between syncing was: {timeBuffer - time}");
                    StopTracking();
                    currentSuccessCount--;

                    ProgressSFX();
                    
                    // notify subscribers of success or progress 
                    if (currentSuccessCount == 0)
                    {
                        print("NOTIFY SUCCESS PUZZLE COMPLETE!!!!!!!!!!!!!!!!!!!!!");
                        isComplete = true;
                        Script_PuzzlesEventsManager.PuzzleSuccess(null);
                    }
                    else
                    {
                        print("NOTIFY PUZZLE PROGRESS!");
                        Script_PuzzlesEventsManager.PuzzleProgress();
                    }
                }
                // it's not the last trigger so it's not a progress case
                else
                {
                    NonprogressSFX();
                }
            }
        }
    }
    
    private void InitializeTracking(string triggerId, string pushableId)
    {
        StopTracking();
        
        activatedTriggersIds[0] = triggerId;
        activatedPushableIds.Add(pushableId);
        isTracking = true;
        time = timeBuffer;

        NonprogressSFX();
    }
    private void StopTracking()
    {
        isTracking = false;
        activatedTriggersIds = new string[triggers.Length];
        activatedPushableIds = new List<string>();

        if (trackingCoroutine != null) StopCoroutine(trackingCoroutine);
        trackingCoroutine = null;
    }

    private void NonprogressSFX()
    {
        Dev_Logger.Debug("Nonprogress SFX()");
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.fireLightUp,
            Script_SFXManager.SFX.fireLightUpVol
        );
    }

    private void ProgressSFX()
    {
        Dev_Logger.Debug("ProgressSFX()");
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.fireLightUp2,
            Script_SFXManager.SFX.fireLightUp2Vol
        );
    }

    public override void Setup()
    {
        Script_Pushable[] pushables = pushablesParent.transform.GetChildren<Script_Pushable>();

        foreach (Script_Pushable p in pushables)
        {
             p.Setup();
             if (isComplete)    p.SetActive(false);
        }

        InitialState();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_SyncedTriggerPuzzleController))]
public class Script_SyncedTriggerPuzzleControllerTester : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_SyncedTriggerPuzzleController ctrl = (Script_SyncedTriggerPuzzleController)target;
        if (GUILayout.Button("InitialState()"))
        {
            ctrl.InitialState();
        }
    }
}
#endif