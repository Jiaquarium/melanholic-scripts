using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_PlayerStepsSFX : MonoBehaviour
{
    [SerializeField] private List<AudioClip> stepSFXs;
    [SerializeField][Range(0f, 1f)] private float stepSFXVol;
    [SerializeField] private List<AudioClip> iceWomanStepSFXs;
    [SerializeField][Range(0f, 1f)] private float iceWomanStepSFXVol;
    [SerializeField] private List<AudioClip> animalWithinStepSFXs;
    [SerializeField][Range(0f, 1f)] private float animalWithinStepSFXVol;
    [SerializeField] private List<AudioClip> psychicDuckStepSFXs;
    [SerializeField][Range(0f, 1f)] private float psychicDuckStepSFXVol;
    [SerializeField] private List<AudioClip> thirdEyeStepSFXs;
    [SerializeField][Range(0f, 1f)] private float thirdEyeStepSFXVol;
    [SerializeField] private List<AudioClip> lanternStepSFXs;
    [SerializeField][Range(0f, 1f)] private float lanternStepSFXVol;
    [SerializeField] private List<AudioClip> puppeteerStepSFXs;
    [SerializeField][Range(0f, 1f)] private float puppeteerStepSFXVol;

    [SerializeField] private Script_Player player;
    
    private AudioSource audioSource;
    private int stepIdx;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // - Default Player Graphics Controller
    private void Step()
    {
        HandleStepSFX(stepSFXs, stepSFXVol);
    }

    // - Snow Woman Player Graphics Controller
    private void IceWomanStep()
    {
        HandleStepSFX(iceWomanStepSFXs, iceWomanStepSFXVol);
    }

    // - Animal Within Player Graphics Controller
    private void AnimalWithinStep()
    {
        HandleStepSFX(animalWithinStepSFXs, animalWithinStepSFXVol);
    }

    // - Psychic Duck Player Graphics Controller
    private void PsychicDuckStep()
    {
        HandleStepSFX(psychicDuckStepSFXs, psychicDuckStepSFXVol);
    }

    // - Third Eye Player Graphics Controller
    // For now make this same as Step()
    private void ThirdEyeStep()
    {
        HandleStepSFX(thirdEyeStepSFXs, thirdEyeStepSFXVol);
    }

    // - Lantern Player Graphics Controller
    private void LanternStep()
    {
        HandleStepSFX(lanternStepSFXs, lanternStepSFXVol);
    }

    // - Puppeteer Player Graphics Controller
    private void PuppeteerStep()
    {
        HandleStepSFX(puppeteerStepSFXs, puppeteerStepSFXVol);
    }

    private void HandleStepSFX(List<AudioClip> clips, float volume)
    {
        if (clips?.Count == 0)
            return;

        // Don't play Step SFX for mutation, it's impossible to play SFX on intervals
        // with animators being chosen at random.
        if (
            (
                player != null
                && player.IsFinalRound
                && Script_ActiveStickerManager.Control.ActiveSticker?.id != Const_Items.MyMaskId
            ) || Script_Game.Game.state == Const_States_Game.DDR
        )
        {
            return;
        }
        
        // Handle different length SFXs for different Masks.
        if (stepIdx >= clips.Count)
            stepIdx = 0;
        
        AudioClip clip = clips[stepIdx];
        audioSource.PlayOneShot(clip, volume);
        
        stepIdx++;
    }
}
