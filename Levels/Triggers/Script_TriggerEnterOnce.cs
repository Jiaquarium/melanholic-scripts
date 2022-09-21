using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Either hook up to Game which calls LB or use UnityEvent
/// </summary>
public class Script_TriggerEnterOnce : Script_Trigger
{
    public Script_Game game;
    [SerializeField] private UnityEvent action;
    [SerializeField] private UnityEvent<Transform> refAction;
    [SerializeField] protected List<Const_Tags.Tags> specifiedUniqueTags;

    [SerializeField] private bool isForceHide;

    void OnTriggerEnter(Collider other)
    {
        // If no tags are specified default to detecting Player.
        if (
            (specifiedUniqueTags.Count > 0 && specifiedUniqueTags.CheckInTags(other.tag))
            || other.tag == Const_Tags.Player
        )
        {
            Dev_Logger.Debug($"{name} detected other <{other.gameObject.name}> with tag <{other.tag}>");
            OnEnter(other);
        }
    }

    public void Reactivate()
    {
        this.gameObject.SetActive(true);
    }

    private void OnEnter(Collider other)
    {
        bool isUnityAction      = action.CheckUnityEventAction();
        bool isCustomUnityEvent = refAction.CheckUnityEvent();
        
        // For actions, handle setting this to inactive in the invoked object in order to do conditionally.
        if (isUnityAction || isCustomUnityEvent)
        {
            if (isUnityAction)          action.Invoke();
            if (isCustomUnityEvent)     refAction.Invoke(other.transform.GetParentRecursive<Transform>());
        }
        // For entering once.
        else if (game.ActivateTrigger(Id))
        {
            Dev_Logger.Debug($"{name} Entered: <{other.gameObject.name}> Setting inactive");
            this.gameObject.SetActive(false);
        }
        
        if (isForceHide)
        {
            this.gameObject.SetActive(false);
        }
    }
}
