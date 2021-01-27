using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_PlayerStepsSFX : MonoBehaviour
{
    [SerializeField] private List<AudioClip> stepSFXs;
    [SerializeField][Range(0f, 1f)] private float stepSFXVol;
    private AudioSource audioSource;
    private int stepIdx;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Step()
    {
        if (stepSFXs?.Count == 0)   return;
        
        AudioClip clip = stepSFXs[stepIdx];
        audioSource.PlayOneShot(clip, stepSFXVol);
        
        stepIdx++;
        if (stepIdx >= stepSFXs.Count)  stepIdx = 0;
    }
}
