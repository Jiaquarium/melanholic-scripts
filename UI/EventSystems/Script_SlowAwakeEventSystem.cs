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
    private static float startUpTime = 0.5f;
    private float timer;
    
    void OnEnable()
    {
        GetComponent<EventSystem>().sendNavigationEvents = false;
        
        // to ensure we set with Script_EventSystemLastSelected.SetFirstSelected
        GetComponent<Script_EventSystemLastSelected>().enabled = false;
        timer = startUpTime;
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
            }
        }
    }
}
