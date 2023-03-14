using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_GameOverInputManager : MonoBehaviour
{
    [SerializeField] private Script_StartOverviewController mainController;
    public virtual void HandleEnterInput()
    {
        if (Script_PlayerInputManager.Instance.RewiredInput.GetButtonDown(Const_KeyCodes.RWUISubmit))
        {
            mainController.ToStartScreenNonIntro();
        }
    }
}
