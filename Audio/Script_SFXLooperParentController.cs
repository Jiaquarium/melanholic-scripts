using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SFXLooperParentController : MonoBehaviour
{
    [SerializeField] private bool isSetOffsetStartTime;
    [SerializeField] private float offsetStartTime;
    [SerializeField] private bool isSetInterval;
    [SerializeField] private float interval;
    [SerializeField] private bool isSetMaxDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private bool isSetMaxVol;
    [SerializeField] private float maxVol;
    
    [SerializeField] private Script_SFXLoopSpeaker[] speakers;

    void OnValidate()
    {
        speakers = GetComponentsInChildren<Script_SFXLoopSpeaker>(true);

        if (isSetOffsetStartTime)
        {
            foreach (var speaker in speakers)
                speaker.OffsetStartTime = offsetStartTime;
        }

        if (isSetInterval)
        {
            foreach (var speaker in speakers)
                speaker.Interval = interval;
        }

        if (isSetMaxDistance)
        {
            foreach (var speaker in speakers)
                speaker.MaxDistance = maxDistance;
        }

        if (isSetMaxVol)
        {
            foreach (var speaker in speakers)
                speaker.MaxVol = maxVol;
        }
    }
}
