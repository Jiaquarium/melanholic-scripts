using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Put on event system to disable on Awake()
/// Useful for choices when going through dialogue
/// </summary>
[RequireComponent(typeof(EventSystem))]
public class Script_SlowAwakeEventSystem : MonoBehaviour
{
    private static float startUpTime = 0.6f;
    private float timer;
    
    void OnEnable()
    {
        GetComponent<EventSystem>().sendNavigationEvents = false;
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
            }
        }
    }
}
