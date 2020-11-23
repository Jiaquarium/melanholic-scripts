using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[RequireComponent(typeof(AudioSource))]
public class Script_ResetTrigger : Script_Trigger
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private Script_LevelBehavior myLevelBehavior;
    [SerializeField] private string completionProperty;
    
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

            Debug.Log("Reset trigger firing event.");
            Script_PuzzlesEventsManager.PuzzleReset();
        }
    }

    void ResetSFX()
    {
        AudioClip clip = _clip == null ? clip = Script_SFXManager.SFX.resetSFX : clip = _clip;
        Debug.Log($"ResetTrigger playing clip {clip.name}");

        GetComponent<AudioSource>().PlayOneShot(clip, Script_SFXManager.SFX.resetSFXVol);
    }
}
