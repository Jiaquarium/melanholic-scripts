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
    public float PlayerStashItemVol;
    public AudioClip PlayerDropItem;
    public float PlayerDropItemVol;
    public AudioClip Rumble254ms;
    public float Rumble254msVol;
    public AudioClip PillarPuzzleProgress;
    public float PillarPuzzleProgressVol;
    public AudioClip PillarPuzzleProgress2;
    public float PillarPuzzleProgress2Vol;
    public AudioClip SeasonsPuzzleProgress;
    public float SeasonsPuzzleProgressVol;
    public AudioClip EnergySpikeAttack;
    public float EnergySpikeAttackVol;
    public AudioClip EnergySpikeHurt;
    public float EnergySpikeHurtVol;
    public AudioClip SwitchOff;
    public float SwitchOffVol;
    public AudioClip SwitchOn;
    public float SwitchOnVol;
    public AudioClip ThoughtsDone;
    public float ThoughtsDoneVol;
    public AudioClip ItemPickUp;
    public float ItemPickUpVol;
    public AudioClip ItemPickUpError;
    public float ItemPickUpErrorVol;
    public AudioClip exitSFX;
    public float exitSFXVol;
    public AudioClip resetSFX;
    public float resetSFXVol;
    public AudioClip playerDropFinishSFX;
    public float playerDropFinishSFXVol;
    public AudioClip doorLock;
    public float doorLockVol;
    public AudioClip useKey;
    public float useKeyVol;
    public AudioClip heartBeat;
    public float heartBeatVol;
    public AudioClip fireLightUp;
    public float fireLightUpVol;
    public AudioClip fireLightUp2;
    public float fireLightUp2Vol;
    public AudioClip shellShock;
    public float shellShockVol;
    
    private void Awake()
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
