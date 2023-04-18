using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_CreditsInputManager : MonoBehaviour
{
    private Script_CreditsController creditsController;
    private Script_PlayerInputManager playerInputManager;

    void Awake()
    {
        creditsController = GetComponent<Script_CreditsController>();
    }
    
    void Start()
    {
        // Player Input Manager must be set in Start since it sets up in Awake
        playerInputManager = Script_PlayerInputManager.Instance;
    }
    
    public void HandleInput()
    {
        if (
            playerInputManager.RewiredInput.GetButtonDown(Const_KeyCodes.RWUISubmit)
            || playerInputManager.RewiredInput.GetButtonDown(Const_KeyCodes.RWUnknownControllerSettings)
        )
        {
            creditsController.ToTitle();
        }
    }
}
