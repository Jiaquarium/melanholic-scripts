using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_StartScreenInputManager : MonoBehaviour
{
    [SerializeField] private Script_StartOverviewController mainController;
    [SerializeField] private Script_StartScreenController startController;
    
    void Update()
    {
        if (Input.anyKey)
        {
            startController.Initialize();
        }
    }

    public virtual void HandleEnterInput()
    {
        if (Input.GetButtonDown(Const_KeyCodes.Submit))
        {
            mainController.StartOptionsOpen();

            var sfx = Script_SFXManager.SFX;
            sfx.Play(sfx.OpenCloseBook, sfx.OpenCloseBookVol);
        }
    }
}
