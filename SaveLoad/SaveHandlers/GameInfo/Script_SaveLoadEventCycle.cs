using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadEventCycle : MonoBehaviour
{
    [SerializeField] private Script_EventCycleManager eventCycleManager;
    
    public void SaveEventCycle(Model_SaveData data)
    {
        Model_EventCycleData eventCycle = new Model_EventCycleData(
            _didTalkToIds:                  eventCycleManager.DidTalkToIds,
            _didTalkToElleniaCountdown:     eventCycleManager.DidTalkToEllenia
        );

        data.eventCycleData = eventCycle;
    }

    public void LoadEventCycle(Model_SaveData data)
    {
        if (data.eventCycleData == null)
        {
            Debug.Log($"There is no {this} state data to load.");
            return;
        }

        Model_EventCycleData eventCycle = new Model_EventCycleData(
            _didTalkToIds:                  data.eventCycleData.didTalkToIds,
            _didTalkToElleniaCountdown:     data.eventCycleData.didTalkToElleniaCountdown
        );

        eventCycleManager.DidTalkToIds      = eventCycle.didTalkToIds;
        eventCycleManager.DidTalkToEllenia  = eventCycle.didTalkToElleniaCountdown;
        
        Debug.Log($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(eventCycle);
    }
}
