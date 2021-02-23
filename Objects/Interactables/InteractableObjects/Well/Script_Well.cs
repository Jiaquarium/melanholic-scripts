using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_Well : Script_InteractableObject
{
    public override void ActionDefault()
    {
        if (CheckDisabled())  return;

        WellSFX();

        // Fire interaction event that Puzzle Controller can react to.
        Script_InteractableObjectEventsManager.WellInteract(this);
    }

    private void WellSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(Script_SFXManager.SFX.WellSFX, Script_SFXManager.SFX.WellSFXVol);
    }
}
