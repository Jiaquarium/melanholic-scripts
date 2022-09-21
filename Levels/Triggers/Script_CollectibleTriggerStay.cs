using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_CollectibleTriggerStay : Script_Trigger
{
    public bool isOn;
    public bool isDisabled;
    public List<Script_CollectibleObject> collectibles;
    [SerializeField] private Script_TriggerPuzzleController triggerPuzzleController;
    
    void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete += EnableAfterLevelInit;
        Script_GameEventsManager.OnLevelBeforeDestroy += DisableBeforeLevelDestroy;
    }

    void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete -= EnableAfterLevelInit;
        Script_GameEventsManager.OnLevelBeforeDestroy -= DisableBeforeLevelDestroy;
    }
    
    private void EnableAfterLevelInit()
    {
        isDisabled = false;
    }
    
    private void DisableBeforeLevelDestroy()
    {
        isDisabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Script_ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        if (
            other.tag == Const_Tags.ItemObject
            // check for if its a collectible
            && other.transform.parent.GetComponent<Script_CollectibleObject>() != null
            && !isOn
        )
        {
            isOn = true;
            collectibles.Add(other.transform.parent.GetComponent<Script_CollectibleObject>());
            foreach(Script_CollectibleObject obj in collectibles)   Dev_Logger.Debug($"{obj}");

            if (!isInitializing && !isDisabled)
            {
                Dev_Logger.Debug("activating trigger: " + Id);
                triggerPuzzleController.TriggerActivated(Id, other);
            }
            else
            {
                Dev_Logger.Debug($"reactivating trigger: {Id} on initialization");
                triggerPuzzleController.TriggerReactivated(Id, other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Script_ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
        Dev_Logger.Debug("onTriggerExit other.tag" + other.tag);
        Dev_Logger.Debug("onTriggerExit gameObject" + gameObject);
        if (
            other.tag == Const_Tags.ItemObject
            && other.transform.parent.GetComponent<Script_CollectibleObject>() != null
        )
        {
            Dev_Logger.Debug("onTriggerExit other.transform.parent.GetComponent<Script_CollectibleObject>(): "
            + other.transform.parent.GetComponent<Script_CollectibleObject>());
            isOn = false;
            collectibles.Remove(other.transform.parent.GetComponent<Script_CollectibleObject>());
            
            /// Don't notify if the deactivation is a result of tearing down level
            if (!isDisabled)    triggerPuzzleController.TriggerDeactivated(Id, other);

            foreach(Script_CollectibleObject obj in collectibles)   Dev_Logger.Debug(obj.name);
        }
    }
}
