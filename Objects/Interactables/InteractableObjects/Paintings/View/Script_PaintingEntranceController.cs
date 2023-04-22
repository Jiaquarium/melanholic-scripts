using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PaintingEntranceController : MonoBehaviour
{
    const int cancelIdx = 1;

    [SerializeField] private Script_PaintingEntranceManager paintingEntranceManager;

    public bool IsReady { get; set; }

    void Update()
    {
        HandleExitInput();
    }

    public void HandleExitInput()
    {
        var rewiredInput = Script_PlayerInputManager.Instance.RewiredInput;
        
        if (rewiredInput.GetButtonDown(Const_KeyCodes.RWUICancel))
        {
            if (IsReady)
                paintingEntranceManager.InputChoice(cancelIdx);
        }
    }
}
