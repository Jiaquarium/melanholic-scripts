using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class Script_EnergySpike : MonoBehaviour
{
    public Script_HitBox hitBox;

    public void Play()
    {
        GetComponent<PlayableDirector>().Play();
    }
}
