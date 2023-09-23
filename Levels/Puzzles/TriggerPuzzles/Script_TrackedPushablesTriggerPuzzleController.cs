using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controls the tracking pillars-like puzzle
/// </summary>
public class Script_TrackedPushablesTriggerPuzzleController : Script_TriggerPuzzleController
{
    [SerializeField] private Transform trackablesParent;
    [SerializeField] private Script_Trackable[] trackables;
    [SerializeField] private Transform trackersParent;
    [SerializeField] private Script_Tracker[] trackers;
    
    protected override void OnValidate()
    {
        base.OnValidate();
        trackables      = trackablesParent.GetChildren<Script_Trackable>();
        trackers        = trackersParent.GetChildren<Script_Tracker>();
    }

    /// <summary>
    /// called by trigger, if was the last trigger to be activated, give a more
    /// dramatic indication of progress (that player is done, just need to verify)
    /// </summary>
    /// <param name="Id">trigger Id</param>
    /// <param name="other">trigger collider obj</param>
    public override void TriggerActivated(string Id, Collider other)
    {
        if (CheckTriggersAllOccupied())
        {
            Script_PuzzlesEventsManager.PuzzleProgress2();
        }
        else
        {
            Script_PuzzlesEventsManager.PuzzleProgress();
        }
    }
    
    public void CheckSuccessCase()
    {
        if (CheckTriggersAllOccupied())
            Script_PuzzlesEventsManager.PuzzleSuccess(null);
    }
    
    protected override bool CheckTriggersAllOccupied()
    {
        foreach (
            Script_PushableTriggerStay trigger in
            transform.GetChildren<Script_PushableTriggerStay>()
        )
        {
            if (!trigger.isOn)  return false;
        }

        return true;
    }

    public override void InitialState()
    {
        foreach (Script_Trackable t in trackables)  t.InitialState();
    }

    public override void CompleteState()
    {
        for (var i = 0; i < trackers.Length; i++)
        {
            trackers[i].Done();
            trackers[i].transform.position = triggers[i].transform.position;

            /// Disable trackers to solve quest
            trackers[i].gameObject.SetActive(false);
        }
    }

    public override void Setup()
    {
        foreach (Script_Trackable t in trackables)  t.Setup();
    }
    
    // ------------------------------------------------------------------
    // DEV ONLY
    public void DevPlaceTrackersOnTriggers()
    {
        for (var i = 0; i < trackers.Length; i++)
        {
            trackers[i].Done();
            trackers[i].transform.position = triggers[i].transform.position;
        }
    }
    // ------------------------------------------------------------------

}
