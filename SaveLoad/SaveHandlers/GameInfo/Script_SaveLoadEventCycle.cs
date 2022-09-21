using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadEventCycle : MonoBehaviour
{
    [SerializeField] private Script_EventCycleManager eventCycleManager;
    
    public void SaveEventCycle(Model_SaveData data)
    {
        Model_EventCycleData eventCycle = new Model_EventCycleData(
            _didInteractPositivelyWithIds: eventCycleManager.DidInteractPositivelyWithIds,
            _idsPositiveInteractionCount: eventCycleManager.IdsPositiveInteractionCount,
            _didTalkToElleniaCountdown: eventCycleManager.DidTalkToEllenia
        );

        data.eventCycleData = eventCycle;
    }

    public void LoadEventCycle(Model_SaveData data)
    {
        if (data.eventCycleData == null)
        {
            Dev_Logger.Debug($"There is no {this} state data to load.");
            return;
        }

        Model_EventCycleData eventCycle = new Model_EventCycleData(
            _didInteractPositivelyWithIds: data.eventCycleData.didInteractPositivelyWithIds,
            _idsPositiveInteractionCount: data.eventCycleData.idsPositiveInteractionCount,
            _didTalkToElleniaCountdown: data.eventCycleData.didTalkToElleniaCountdown
        );

        eventCycleManager.DidInteractPositivelyWithIds = eventCycle.didInteractPositivelyWithIds;
        eventCycleManager.IdsPositiveInteractionCount = eventCycle.idsPositiveInteractionCount;
        eventCycleManager.DidTalkToEllenia = eventCycle.didTalkToElleniaCountdown;
        
        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(eventCycle);
    }
}
