using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI / Menu SFX here
/// DO NOT MAKE SINGLETON bc this may have different settings vs. Start Menu 
/// </summary>
public class Script_InventoryAudioSettings : MonoBehaviour
{
    public AudioSource clickAudioSource;
    public AudioClip clickSFX;
    public float clickVolume;
    
    
    public AudioSource clickEnterSubmenuAudioSource;
    public AudioClip clickEnterSubmenuSFX;
    public float clickEnterSubemenuSFXVolume;

    public AudioSource selectAudioSource;
    public AudioClip selectSFX;
    public float selectVolume;

    public AudioSource inventoryAudioSource;
    public AudioClip errorSFX;
    public float errorVolume;
    public AudioClip stickerOnSFX;
    public float stickerOnVol;
    public AudioClip stickerOffSFX;
    public float stickerOffVol;
    public AudioClip UsableTargetNotFound;
    public float UsableTargetNotFoundVol;
    
    
    /// <summary>
    /// Used in Title scene
    /// </summary>
    public AudioClip crunchDown;
    public float crunchDownVol;
    public AudioClip crunchUp;
    public float crunchUpVol;    
}
