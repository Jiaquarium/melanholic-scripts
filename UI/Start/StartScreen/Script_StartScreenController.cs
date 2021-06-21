using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_StartScreenController : MonoBehaviour
{
    [SerializeField] private Script_StartOverviewController mainController;

    // Timer to Restart Intro if Player is inactive for specified time.
    [SerializeField] private float inactivityResetTime;
    [SerializeField] private float timer;

    [SerializeField] private Script_EventSystemLastSelected eventSystem;
    
    void OnEnable()
    {
        Initialize();
    }

    void OnDisable()
    {
        // Prevent sound effect on re-initialization.
        eventSystem.InitializeState();
    }
    
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            mainController.InitializeIntro();
        }
        
        GetComponent<Script_StartScreenInputManager>().HandleEnterInput();
    }

    public void Initialize()
    {
        timer = inactivityResetTime;
    }
}
