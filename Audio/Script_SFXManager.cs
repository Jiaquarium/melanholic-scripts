using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// World SFX here
/// For UI use Script_InventoryAudioSettings
/// </summary>
public class Script_SFXManager : MonoBehaviour
{
    public static Script_SFXManager SFX;
    public AudioClip PlayerStashItem;
    [Range(0f, 1f)] public float PlayerStashItemVol;
    public AudioClip PlayerDropItem;
    [Range(0f, 1f)] public float PlayerDropItemVol;
    public AudioClip Rumble254ms;
    [Range(0f, 1f)] public float Rumble254msVol;
    public AudioClip PillarPuzzleProgress;
    [Range(0f, 1f)] public float PillarPuzzleProgressVol;
    public AudioClip PillarPuzzleProgress2;
    [Range(0f, 1f)] public float PillarPuzzleProgress2Vol;
    public AudioClip SeasonsPuzzleProgress;
    [Range(0f, 1f)] public float SeasonsPuzzleProgressVol;
    public AudioClip EnergySpikeAttack;
    [Range(0f, 1f)] public float EnergySpikeAttackVol;
    public AudioClip EnergySpikeHurt;
    [Range(0f, 1f)] public float EnergySpikeHurtVol;
    public AudioClip SwitchOff;
    [Range(0f, 1f)] public float SwitchOffVol;
    public AudioClip SwitchOn;
    [Range(0f, 1f)] public float SwitchOnVol;
    public AudioClip ThoughtsDone;
    [Range(0f, 1f)] public float ThoughtsDoneVol;
    public AudioClip ItemPickUp;
    [Range(0f, 1f)] public float ItemPickUpVol;
    public AudioClip ItemPickUpError;
    [Range(0f, 1f)] public float ItemPickUpErrorVol;
    public AudioClip exitSFX;
    [Range(0f, 1f)] public float exitSFXVol;
    public AudioClip resetSFX;
    [Range(0f, 1f)] public float resetSFXVol;
    public AudioClip playerDropFinishSFX;
    [Range(0f, 1f)] public float playerDropFinishSFXVol;
    public AudioClip doorLock;
    [Range(0f, 1f)] public float doorLockVol;
    public AudioClip useKey;
    [Range(0f, 1f)] public float useKeyVol;
    public AudioClip heartBeat;
    [Range(0f, 1f)] public float heartBeatVol;
    public AudioClip fireLightUp;
    [Range(0f, 1f)] public float fireLightUpVol;
    public AudioClip fireLightUp2;
    [Range(0f, 1f)] public float fireLightUp2Vol;
    public AudioClip shellShock;
    [Range(0f, 1f)] public float shellShockVol;
    public AudioClip psychicDuckQuack;
    [Range(0f, 1f)] public float psychicDuckQuackVol;
    public AudioClip empty;
    [Range(0f, 1f)] public float emptyVol;
    public AudioClip piano;
    [Range(0f, 1f)] public float pianoVol;
    
    public void Setup()
    {
        if (SFX == null)
        {
            SFX = this;
        }
        else if (SFX != this)
        {
            Destroy(this.gameObject);
        }
    }
}
