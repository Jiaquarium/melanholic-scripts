using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Simple Intro Input
/// </summary>
public class Script_IntroSimpleInputManager : MonoBehaviour
{
    [SerializeField] private Script_StartOverviewController startOverviewController;
    [SerializeField] private Script_IntroControllerSimple introControllerSimple;
    private Script_PlayerInputManager playerInputManager;

    void Start()
    {
        playerInputManager = Script_PlayerInputManager.Instance;
    }
    
    void Update()
    {
        if (
            // Prevent duplicate calls if Intro Simple timeline also ends here
            !introControllerSimple.isDonePlaying
            && (
                playerInputManager.RewiredInput.GetButtonDown(Const_KeyCodes.RWUISubmit)
                // UICancel allows Known Controller "Start" button to work too
                || playerInputManager.RewiredInput.GetButtonDown(Const_KeyCodes.RWUICancel)
                || playerInputManager.RewiredInput.GetButtonDown(Const_KeyCodes.RWUnknownControllerSettings)
            )
        )
        {
            Dev_Logger.Debug("Skip simple intro, first load");
            introControllerSimple.HandleSkip();
        }
    }
}
