using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_StartScreenInputManager))]
public class Script_StartScreenController : MonoBehaviour
{
    public enum Type
    {
        Intro,
        FromBack
    }
    
    [SerializeField] private Script_StartOverviewController mainController;

    // Timer to Restart Intro if Player is inactive for specified time.
    [SerializeField] private float inactivityResetTime;
    [SerializeField] private float timer;

    [SerializeField] private Script_CanvasGroupController titleCanvasGroup;
    [SerializeField] private Script_TitleLogo titleLogo;
    [SerializeField] private Script_CanvasGroupController startOptionsCanvasGroup;
    
    [Space][Header("Intro / Default")][Space]

    [SerializeField] private float artFadeInTimeDefault = 2f;
    [SerializeField] private float logoWaitToFadeInTimeDefault;
    [SerializeField] private float logoFadeInTimeDefault;

    [Space][Header("From Back")][Space]

    [SerializeField] private float logoWaitToFadeInTimeFromBack;
    [SerializeField] private float logoFadeInTimeFromBack;

    [SerializeField] private Script_EventSystemLastSelected eventSystem;

    [SerializeField] private Transform startScreen;

    public float TitleFadeInTime
    {
        get => artFadeInTimeDefault;
    }
    
    void OnDisable()
    {
        // Prevent sound effect on re-initialization.
        eventSystem.InitializeState();
    }
    
    void Update()
    {
        GetComponent<Script_StartScreenInputManager>().HandleEnterInput();
    }

    public void FadeInTitle(Type type)
    {
        float artFadeInTime;
        float logoWaitToFadeInTime;
        float logoFadeInTime;
        
        switch (type)
        {
            case (Type.FromBack):
                logoWaitToFadeInTime = logoWaitToFadeInTimeFromBack;
                logoFadeInTime = logoFadeInTimeFromBack;
                artFadeInTime = artFadeInTimeDefault;
                break;
            default:
                logoWaitToFadeInTime = logoWaitToFadeInTimeDefault;
                logoFadeInTime = logoFadeInTimeDefault;
                artFadeInTime = artFadeInTimeDefault;
                break;
        }
        
        titleCanvasGroup.InitialState();
        titleLogo.InitialState();
        titleCanvasGroup.FadeIn(artFadeInTime);
        titleLogo.WaitToFadeIn(logoWaitToFadeInTime, logoFadeInTime);
    }

    public void Setup()
    {
        startScreen.gameObject.SetActive(false);
        startOptionsCanvasGroup.Close();
        
        titleCanvasGroup.Setup();
        titleLogo.Setup();

        eventSystem.gameObject.SetActive(false);
    }
}
