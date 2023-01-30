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
    [SerializeField] protected bool isSilentNonInteractState;
    
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

    public Vector3 SpeakerLocation => speakerLocationOverride == null
        ? transform.position
        : speakerLocationOverride.position;

    public bool IsDisabled { get; set; }
    public bool IsForceOnNonInteractState { get; set; }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        // SFX speakers only need volume to be adjusted.
        if (!isSFXSpeaker)
            audioSource.Stop();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
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
        var game = Script_Game.Game;
        
        if (
            game == null
            || !game.GetPlayerIsSpawned()
            || IsDisabled
            || (
                isSilentNonInteractState
                && !IsForceOnNonInteractState
                && (
                    game.state != Const_States_Game.Interact
                    || game.GetPlayer().State != Const_States_Player.Interact
                )
            )
        )
        {
            audioSource.volume = 0f;
            return;
        }
        
        currentDistance = Vector3.Distance(Script_Game.Game.GetPlayerLocation(), SpeakerLocation);
        
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
        var position = SpeakerLocation;
        Handles.color = Color.yellow;
        
        Handles.DrawWireDisc(position, new Vector3(0, 1, 0), maxDistance);
    }
#endif
}
