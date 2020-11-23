using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Trackable : MonoBehaviour
{
    public Transform origin;
    [SerializeField] private Script_Tracker[] trackers;
    public Vector3 fromStartingPos { get; private set; }
    public Vector3 startingPosFromOrigin { get; private set; }
    private Vector3 spawnLocation;

    private void Update() {
        fromStartingPos = transform.position - spawnLocation;

        foreach(Script_Tracker tracker in trackers)     tracker.UpdatePos(this);
    }

    /// <summary>
    /// Matches the position of my first tracker (use for completion state)
    /// </summary>
    public void MatchMyTrackerPosition()
    {
        if (trackers.Length > 0)
            transform.position = origin.position + trackers[0].GetTrackablePositionFromOrigin();
    }

    public void InitialState()
    {
        transform.position = spawnLocation;
        startingPosFromOrigin = spawnLocation - origin.position;

        foreach(Script_Tracker tracker in trackers)     tracker.Setup(this);
    }

    public void Setup()
    {
        spawnLocation = transform.position;
        startingPosFromOrigin = spawnLocation - origin.position;

        foreach(Script_Tracker tracker in trackers)     tracker.Setup(this);
    }
}
