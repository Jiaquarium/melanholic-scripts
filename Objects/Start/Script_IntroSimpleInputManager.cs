using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Simple Intro Input
/// </summary>
public class Script_IntroSimpleInputManager : MonoBehaviour
{
    [SerializeField] private Script_StartOverviewController startOverviewController;
    private Script_PlayerInputManager playerInputManager;

    void Start()
    {
        playerInputManager = Script_PlayerInputManager.Instance;
    }
    
    void Update()
    {
        if (
            playerInputManager.RewiredInput.GetButtonDown(Const_KeyCodes.RWUISubmit)
            || playerInputManager.RewiredInput.GetButtonDown(Const_KeyCodes.RWUnknownControllerSettings)
        )
        {
            Dev_Logger.Debug("Skip simple intro, first load");
            
            startOverviewController.StopIntroSimple();
            startOverviewController.FadeInTitleScreen(withCTA: true);
        }
    }
}
