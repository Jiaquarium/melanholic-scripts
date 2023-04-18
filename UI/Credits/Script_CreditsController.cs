using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_CreditsController : MonoBehaviour
{
    private const float FadeToTitleTime = 3f;
    private Script_CreditsInputManager creditsInputManager;
    private bool isExitInputDetected;
    
    void Awake()
    {
        creditsInputManager = GetComponent<Script_CreditsInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        creditsInputManager.HandleInput();
    }

    public void ToTitle()
    {
        if (isExitInputDetected)
            return;
        
        Script_TransitionManager.Control.ToTitleFadeOut(FadeToTitleTime);
        isExitInputDetected = true;
    }

    public void Setup()
    {
        gameObject.SetActive(false);
    }
}
