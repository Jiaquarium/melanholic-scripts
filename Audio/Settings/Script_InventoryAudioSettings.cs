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
    public AudioSource clickEnterSubmenuAudioSource;
    public AudioSource selectAudioSource;
    public AudioSource inventoryAudioSource;
}
