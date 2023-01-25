using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class Script_Well : Script_InteractableObject
{
    private static float waitBeforeSFXTime = 0.5f;
    
    protected override void ActionDefault()
    {
        if (CheckDisabled())
            return;

        Script_InteractableObjectEventsManager.WellInteract(this);
    }

    // To be called by reaction to WellInteract event
    public void WellTalk(Action cb = null)
    {
        // If Wells Puzzle is done, force play the success well SFX
        bool isCorrectWellOrDone = game.WellsWorldBehavior.WellsPuzzleController.IsDone
            || game.WellsWorldBehavior.WellsPuzzleController.IsCorrectWell(this);

        AudioSource SFXSource = GetComponent<AudioSource>();
        Script_SFXManager sfx = Script_SFXManager.SFX;
        float duration = sfx.WellSFXDuration;
        var wellSfx = isCorrectWellOrDone ? sfx.WellCorrectSFX : sfx.WellSFX;
        var wellSfxVol = isCorrectWellOrDone ? sfx.WellCorrectSFXVol : sfx.WellSFXVol;
        
        game.ChangeStateCutScene();
        
        StartCoroutine(WaitToPlaySFX());
        
        IEnumerator WaitToPlaySFX()
        {
            yield return new WaitForSeconds(waitBeforeSFXTime);

            Dev_Logger.Debug($"{name} playing: {wellSfx.name}");
            
            SFXSource.PlayOneShot(wellSfx, wellSfxVol);

            StartCoroutine(OnSFXDone());
        }
        
        IEnumerator OnSFXDone()
        {
            yield return new WaitForSeconds(duration);
            
            game.ChangeStateInteract();

            if (cb != null)
                cb();
        }
    }
}
