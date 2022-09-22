using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Put on event system to disable on Awake()
/// Useful for choices when going through dialogue
/// </summary>
[RequireComponent(typeof(EventSystem))]
[RequireComponent(typeof(Script_EventSystemLastSelected))]
public class Script_SlowAwakeEventSystem : MonoBehaviour
{
    private static float startUpTime = 0.75f;
    private float timer;

    public bool IsTimerDone { get; set; }
    
    void OnEnable()
    {
        Dev_Logger.Debug($"OnEnable SlowAwakeEventSystem {name}");
        
        GetComponent<EventSystem>().sendNavigationEvents = false;
        
        // to ensure we set with Script_EventSystemLastSelected.SetFirstSelected
        GetComponent<Script_EventSystemLastSelected>().enabled = false;
        InitialState();
    }

    void OnDisable()
    {
        InitialState();      
    }

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            
            if (timer <= 0f)
            {
                timer = 0f;
                GetComponent<EventSystem>().sendNavigationEvents = true;

                // to ensure we set with Script_EventSystemLastSelected.SetFirstSelected
                GetComponent<Script_EventSystemLastSelected>().enabled = true;

                IsTimerDone = true;
            }
        }
    }

    private void InitialState()
    {
        timer = startUpTime;
        IsTimerDone = false;
    }
}
