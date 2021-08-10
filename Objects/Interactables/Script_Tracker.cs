using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// trackable refs this and sets startingPos based on origin
/// updated through the trackable, diff * my multiplier
/// </summary>
public class Script_Tracker : Script_Interactable
{
    [SerializeField] private int trackingMultipler;
    [SerializeField] private Transform origin;
    [SerializeField] private Renderer graphics;
    [SerializeField] private bool isTracking = true;
    
    private Vector3 startPos;
    private Script_StopAnimation stopAnimationHelper;
    private Script_InteractableObjectText[] interactableObjectTexts;

    protected override void Awake()
    {
        base.Awake();
        
        stopAnimationHelper = GetComponent<Script_StopAnimation>();
        interactableObjectTexts = GetComponentsInChildren<Script_InteractableObjectText>(true);
    }
    
    // Called from Script_Trackable after it updates position
    public void UpdatePos(Script_Trackable trackable)
    {
        if (!isTracking) return;

        Vector3 myDiff = trackable.fromStartingPos * trackingMultipler;
        transform.position = startPos + myDiff;
    }

    public void SetVisibility(bool isOn)
    {
        graphics.enabled = isOn;
    }

    public void Done()
    {
        if (stopAnimationHelper != null)
            stopAnimationHelper.StopAnimation();
        
        foreach (var text in interactableObjectTexts)
            text.State = Script_InteractableObject.States.Disabled;
        
        isTracking = false;
    }

    public Vector3 GetTrackablePositionFromOrigin()
    {
        return (transform.position - origin.position) / trackingMultipler;
    }

    // called from Trackable
    public void Setup(Script_Trackable trackable)
    {
        transform.position = origin.position + (trackable.startingPosFromOrigin * trackingMultipler);
        startPos = transform.position;
        Debug.Log($"Tracker pos: {startPos}");
    }
}
