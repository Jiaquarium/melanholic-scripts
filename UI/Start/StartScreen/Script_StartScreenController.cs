using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_StartScreenInputManager))]
public class Script_StartScreenController : MonoBehaviour
{
    [SerializeField] private Script_StartOverviewController mainController;

    // Timer to Restart Intro if Player is inactive for specified time.
    [SerializeField] private float inactivityResetTime;
    [SerializeField] private float timer;

    [SerializeField] private Script_CanvasGroupController titleCanvasGroup;
    [SerializeField] private Script_CanvasGroupController startOptionsCanvasGroup;
    
    // Seconds
    [SerializeField] private float titleFadeInTime = 2f;

    [SerializeField] private Script_EventSystemLastSelected eventSystem;

    [SerializeField] private Transform startScreen;

    public float TitleFadeInTime
    {
        get => titleFadeInTime;
    }
    
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

    public void FadeInTitle()
    {
        titleCanvasGroup.InitialState();
        titleCanvasGroup.FadeIn(titleFadeInTime);
    }
    
    public void Initialize()
    {
        timer = inactivityResetTime;
    }

    public void Setup()
    {
        startScreen.gameObject.SetActive(false);
        startOptionsCanvasGroup.Close();
        
        titleCanvasGroup.Setup();

        eventSystem.gameObject.SetActive(false);
    }
}
