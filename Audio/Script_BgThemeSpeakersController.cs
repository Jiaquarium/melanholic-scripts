using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_BgThemeSpeakersController : MonoBehaviour
{
    public void PauseSpeakers()
    {
        Script_Speaker[] speakers = GetComponentsInChildren<Script_Speaker>();

        foreach (var speaker in speakers)
        {
            speaker.Pause();
        }
    }

    public void UnPauseSpeakers()
    {
        Script_Speaker[] speakers = GetComponentsInChildren<Script_Speaker>();

        foreach (var speaker in speakers)
        {
            speaker.UnPause();
        }   
    }
}
