using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Can be attached to a Music Speaker or on a SFX player (isSFXSpeaker flag). 
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_ProximitySpeaker : Script_Speaker
{
    public AudioSource audioSource;
    [SerializeField] protected float maxDistance;
    public float maxVol;
    
    [SerializeField] private Transform speakerLocationOverride;
    [SerializeField] private bool isSFXSpeaker;

    [SerializeField] private float currentDistance; // for Dev

    public float MaxDistance
    {
        get => maxDistance;
        set => maxDistance = value;
    }

    public float MaxVol
    {
        get => maxVol;
        set => maxVol = value;
    }
    
    protected virtual void OnDisable()
    {
        // SFX speakers only need volume to be adjusted.
        if (!isSFXSpeaker)
            audioSource.Stop();
    }

    protected virtual void OnEnable()
    {
        AdjustVolume();
        
        if (!isSFXSpeaker)
            audioSource.Play();
    }

    protected virtual void Awake()
    {
        maxVol = maxVol == 0 ? 1f : maxVol;
        AdjustVolume();
    }
    
    protected virtual void Update()
    {
        AdjustVolume();
    }

    protected void AdjustVolume()
    {   
        if (Script_Game.Game == null || !Script_Game.Game.GetPlayerIsSpawned())
        {
            audioSource.volume = 0f;
            return;
        }
        
        var speakerLocation = speakerLocationOverride == null ? transform.position : speakerLocationOverride.position;
        currentDistance = Vector3.Distance(Script_Game.Game.GetPlayerLocation(), speakerLocation);
        
        if (currentDistance >= maxDistance)
        {
            audioSource.volume = 0f;
        }
        else
        {
            float v = currentDistance / maxDistance;
            audioSource.volume = maxVol - (maxVol * v);
        }
    }

    protected void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var position = transform.position;
        Handles.color = Color.yellow;
        
        Handles.DrawWireDisc(position, new Vector3(0, 1, 0), maxDistance);
    }
#endif
}
