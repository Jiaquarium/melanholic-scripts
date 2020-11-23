using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
/// <summary>
/// AudioSource must have PlayOnAwake checked in inspector
/// this will prevent the AudioSource from playing when
/// reentering the level
/// </summary>
public class Script_PlayOnAwakeOnce : MonoBehaviour
{
    void Start()
    {
        GetComponent<AudioSource>().playOnAwake = false;        
    }
}
