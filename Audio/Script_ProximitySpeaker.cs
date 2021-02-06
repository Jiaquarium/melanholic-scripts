using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Should exist in world, not instanstiated.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_ProximitySpeaker : Script_Speaker
{
    public AudioSource audioSource;
    public float maxDistance;
    public float maxVol;
    [SerializeField] private float currentDistance; // for Dev

    protected virtual void OnDisable()
    {
        audioSource.Stop();
    }

    protected virtual void OnEnable()
    {
        AdjustVolume();
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
        
        currentDistance = Vector3.Distance(Script_Game.Game.GetPlayerLocation(), transform.position);
        
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
}
