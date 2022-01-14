using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// This GameObject should remain active.
/// </summary>
public class Script_SettingsController : MonoBehaviour
{
    private enum States
    {
        Overview = 0,
        Controls = 1,
        Graphics = 2,
    }

    [SerializeField] private States state;
    [SerializeField] private bool isThrottledInGame;
    [SerializeField] private float throttleTime;
    
    [SerializeField] private List<Button> initialButtons;
    
    [SerializeField] private UnityEvent OnBackFromOverviewAction;
    
    [SerializeField] private Script_CanvasGroupController overviewCanvasGroup;
    [SerializeField] private Script_CanvasGroupController controlsCanvasGroup;
    [SerializeField] private Script_CanvasGroupController bgCanvasGroup;
    [SerializeField] private FadeSpeeds bgFadeSpeed;
    
    [SerializeField] private Script_SettingsInputManager settingsInputManager;
    [SerializeField] private EventSystem settingsEventSystem;

    [SerializeField] private AudioSource audioSource;

    private float timer;

    public bool IsThrottledInGame { get => isThrottledInGame; }
    
    void OnEnable()
    {
        Script_MenuEventsManager.OnExitMenu += StartThrottlingInGame;
    }

    void OnDisable()
    {
        Script_MenuEventsManager.OnExitMenu -= StartThrottlingInGame;
    }
    
    void Update()
    {
        if (settingsInputManager.gameObject.activeInHierarchy)
            settingsInputManager.HandleExitInput();
        
        if (isThrottledInGame)
        {
            timer -= Time.unscaledDeltaTime;
            
            if (timer < 0f)
                timer = 0f;
            
            if (timer == 0f)
                isThrottledInGame = false;
        }   
    }
    
    public void OpenOverview(int firstSelectedIdx)
    {
        bgCanvasGroup.FadeIn(bgFadeSpeed.GetFadeTime(), isUnscaledTime: true);
        overviewCanvasGroup.Open();
        controlsCanvasGroup.Close();

        settingsEventSystem.gameObject.SetActive(true);
        settingsInputManager.gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(initialButtons[firstSelectedIdx].gameObject);

        state = States.Overview;
    }

    public void Close(bool isFade = false, Action cb = null)
    {
        settingsEventSystem.gameObject.SetActive(false);
        settingsInputManager.gameObject.SetActive(false);
        
        overviewCanvasGroup.Close();
        controlsCanvasGroup.Close();
        
        if (isFade)
            bgCanvasGroup.FadeOut(bgFadeSpeed.GetFadeTime(), cb, isUnscaledTime: true);
        else
            bgCanvasGroup.Close();
    }

    public void ToControls()
    {
        overviewCanvasGroup.Close();
        controlsCanvasGroup.Open();

        // Set Controls First Selected active.
        EventSystem.current.SetSelectedGameObject(controlsCanvasGroup.firstToSelect.gameObject);

        EnterMenuSFX();

        state = States.Controls;
    }

    // From UI Back Buttons
    public void Back()
    {
        switch (state)
        {
            case (States.Overview):
                Close(
                    isFade: true,
                    cb: () => {
                        OnBackFromOverviewAction.SafeInvoke();
                    }
                );
                ExitMenuSFX();
                
                // This will only affect in game to prevent
                // player from accidentally spamming Esc.
                StartThrottlingInGame();
                
                break;
            case (States.Controls):
                OpenOverview(0);
                ExitMenuSFX();
                break;
            case (States.Graphics):
                OpenOverview(1);
                ExitMenuSFX();
                break;
        }
    }

    private void EnterMenuSFX()
    {
        audioSource.PlayOneShot(
            Script_SFXManager.SFX.OpenCloseBook,
            Script_SFXManager.SFX.OpenCloseBookVol
        );
    }

    private void ExitMenuSFX()
    {
        audioSource.PlayOneShot(
            Script_SFXManager.SFX.OpenCloseBookReverse,
            Script_SFXManager.SFX.OpenCloseBookReverseVol
        );
    }

    private void EnterSubmenuSFX()
    {
        audioSource.PlayOneShot(
            Script_SFXManager.SFX.OpenCloseBookHeavy,
            Script_SFXManager.SFX.OpenCloseBookHeavyVol
        );
    }

    /// <summary>
    /// Disable settings after exitting Menu so Player doesn't accidentally
    /// activate settings.
    /// </summary>
    private void StartThrottlingInGame()
    {
        timer = throttleTime;
        isThrottledInGame = true;
    }

    public void Setup()
    {
        gameObject.SetActive(true);
        audioSource.gameObject.SetActive(true);
        Close();
    }
}
