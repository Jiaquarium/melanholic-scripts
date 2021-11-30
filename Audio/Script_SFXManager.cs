using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// World SFX here
/// For UI use Script_InventoryAudioSettings
/// </summary>
public class Script_SFXManager : MonoBehaviour
{
    public static Script_SFXManager SFX;

    public AudioSource SFXSource;
    
    [Header("---- World SFX ----")]
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

    public AudioClip ScarletCipherPiecePickup;
    [Range(0f, 1f)] public float ScarletCipherPiecePickupVol;
    public float ScarletCipherPiecePickupDuration = 3.0f;
    
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

    public AudioClip WellSFX;
    [Range(0f, 1f)] public float WellSFXVol;
    
    public AudioClip FloorSwitchDown;
    [Range(0f, 1f)] public float FloorSwitchDownVol;
    
    public AudioClip FloorSwitchUp;
    [Range(0f, 1f)] public float FloorSwitchUpVol;
    
    public AudioClip Secret;
    [Range(0f, 1f)] public float SecretVol;
    public float SecretDuration = 3.0f;

    public AudioClip CorrectPartialProgress;
    [Range(0f, 1f)] public float CorrectPartialProgressVol;
    public float CorrectPartialProgressDuration = 2.0f;
    
    public AudioClip MainQuestDone;
    [Range(0f, 1f)] public float MainQuestDoneVol;
    public float MainQuestDuration = 5.0f;

    public AudioClip Freeze;
    [Range(0f, 1f)] public float FreezeVol;

    public AudioClip Dawn;
    [Range(0f, 1f)] public float DawnVol;

    public AudioClip DawnWeekend;
    [Range(0f, 1f)] public float DawnWeekendVol;

    public AudioClip IceShatter;
    [Range(0f, 1f)] public float IceShatterVol;

    public AudioClip WindPushBack;
    [Range(0f, 1f)] public float WindPushBackVol;

    [Header("---- UI SFX ----")]

    public AudioClip UIErrorSFX;
    [Range(0f, 1f)] public float UIErrorSFXVol;

    public AudioClip ErrorBlip;
    [Range(0f, 1f)] public float ErrorBlipVol;

    public AudioClip UIWrongSFX;
    [Range(0f, 1f)] public float UIWrongSFXVol;
    
    public AudioClip UITypingSFX;
    [Range(0f, 1f)] public float UITypingSFXVol;
    
    public AudioClip UICodeTypingSFX;
    [Range(0f, 1f)] public float UICodeTypingSFXVol;

    public AudioClip OpenCloseBook;
    [Range(0f, 1f)] public float OpenCloseBookVol;

    public AudioClip OpenCloseBookHeavy;
    [Range(0f, 1f)] public float OpenCloseBookHeavyVol;

    public AudioClip Select;
    [Range(0f, 1f)] public float SelectVol;

    public AudioClip StickerOn;
    [Range(0f, 1f)] public float StickerOnVol;

    public AudioClip StickerOff;
    [Range(0f, 1f)] public float StickerOffVol;

    public AudioClip CrunchDown;
    [Range(0f, 1f)] public float CrunchDownVol;

    public AudioClip CrunchUp;
    [Range(0f, 1f)] public float CrunchUpVol;
    
    public AudioClip TheDrop;
    [Range(0f, 1f)] public float TheDropVol;
    
    public void PlayQuestProgress(Action cb = null)
    {
        float SFXduration = CorrectPartialProgressDuration;
        
        SFXSource.PlayOneShot(CorrectPartialProgress, CorrectPartialProgressVol);

        if (cb != null)
            StartCoroutine(OnSFXDone());
        
        IEnumerator OnSFXDone()
        {
            yield return new WaitForSeconds(SFXduration);
            cb();
        }
    }
    
    public void PlayQuestComplete(Action cb = null)
    {
        float SFXduration = SecretDuration;
        
        SFXSource.PlayOneShot(Secret, SecretVol);

        if (cb != null)
            StartCoroutine(OnSFXDone());
        
        IEnumerator OnSFXDone()
        {
            yield return new WaitForSeconds(SFXduration);
            cb();
        }
    }
    
    public void PlayMainQuestDone(Action cb = null)
    {
        float SFXduration = MainQuestDuration;
        
        SFXSource.PlayOneShot(MainQuestDone, MainQuestDoneVol);

        if (cb != null) StartCoroutine(OnSFXDone());
        
        IEnumerator OnSFXDone()
        {
            yield return new WaitForSeconds(SFXduration);
            cb();
        }
    }

    public void PlayScarletCipherPickupSFX(Action cb = null)
    {
        float SFXduration = ScarletCipherPiecePickupDuration;
        
        SFXSource.PlayOneShot(ScarletCipherPiecePickup, ScarletCipherPiecePickupVol);

        if (cb != null) StartCoroutine(OnSFXDone());
        
        IEnumerator OnSFXDone()
        {
            yield return new WaitForSeconds(SFXduration);
            cb();
        }   
    }

    public void PlayDawn()
    {
        SFXSource.PlayOneShot(Dawn, DawnVol);    
    }

    public void PlayDawnWeekend()
    {
        SFXSource.PlayOneShot(DawnWeekend, DawnWeekendVol);    
    }

    public void PlayIceShatter()
    {
        SFXSource.PlayOneShot(IceShatter, IceShatterVol);    
    }

    public void Play(AudioClip sfx, float vol = 1f)
    {
        SFXSource.PlayOneShot(sfx, vol);
    }
    
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
