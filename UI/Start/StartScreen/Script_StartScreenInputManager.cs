using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_StartScreenInputManager : MonoBehaviour
{
    [SerializeField] private Script_StartOverviewController mainController;
    public virtual void HandleEnterInput()
    {
        if (Input.GetButtonDown(Const_KeyCodes.Submit))
        {
            mainController.ToSavedGames();
        }
    }
}
