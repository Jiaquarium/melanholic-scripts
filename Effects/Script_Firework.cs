using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_Firework : MonoBehaviour
{
    void OnEnable()
    {
        var sfx = Script_SFXManager.SFX;
        GetComponent<AudioSource>().PlayOneShot(sfx.Fireworks, sfx.FireworksVol);
    }
}
