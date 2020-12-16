using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    drop this gameObject into the World
*/
public class Script_ProximitySpeaker : MonoBehaviour
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
