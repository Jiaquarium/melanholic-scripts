using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Script_ResetTrigger : Script_Trigger
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private Script_LevelBehavior myLevelBehavior;
    [SerializeField] private string completionProperty;

    [SerializeField] private bool isUseUnityEventOnly;
    [SerializeField] private UnityEvent onResetTriggerEnter;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Const_Tags.Player)
        {
            ResetSFX();
            
            if (
                myLevelBehavior != null
                && myLevelBehavior.HasProp(completionProperty)
                && myLevelBehavior.GetProp<bool>(completionProperty)
            )
            {
                return;
            }

            Dev_Logger.Debug("Reset trigger firing event.");
            
            if (isUseUnityEventOnly)
                onResetTriggerEnter.SafeInvoke();
            else
                Script_PuzzlesEventsManager.PuzzleReset();
        }
    }

    void ResetSFX()
    {
        AudioClip clip = _clip == null ? clip = Script_SFXManager.SFX.resetSFX : clip = _clip;
        Dev_Logger.Debug($"ResetTrigger playing clip {clip.name}");

        GetComponent<AudioSource>().PlayOneShot(clip, Script_SFXManager.SFX.resetSFXVol);
    }
}
