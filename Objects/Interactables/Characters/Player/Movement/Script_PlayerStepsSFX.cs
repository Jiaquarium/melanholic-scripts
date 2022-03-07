using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_PlayerStepsSFX : MonoBehaviour
{
    [SerializeField] private List<AudioClip> stepSFXs;
    [SerializeField][Range(0f, 1f)] private float stepSFXVol;

    [SerializeField] private Script_Player player;
    
    private AudioSource audioSource;
    private int stepIdx;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Step()
    {
        if (stepSFXs?.Count == 0)
            return;

        // Don't play Step SFX for mutation, it's impossible to play SFX on intervals
        // with animators being chosen at random.
        if (
            (
                player.IsFinalRound
                && Script_ActiveStickerManager.Control.ActiveSticker?.id != Const_Items.MyMaskId
            ) || Script_Game.Game.state == Const_States_Game.DDR
        )
        {
            return;
        }
        
        AudioClip clip = stepSFXs[stepIdx];
        audioSource.PlayOneShot(clip, stepSFXVol);
        
        stepIdx++;
        
        if (stepIdx >= stepSFXs.Count)
            stepIdx = 0;
    }
}
