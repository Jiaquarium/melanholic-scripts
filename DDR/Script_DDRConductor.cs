using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_DDRConductor : MonoBehaviour
{
    [SerializeField] private float songDspTimeStart;
    [SerializeField] private float currentDspTime;
    private float lastDspTime;

    public float SongPosition
    {
        get => (float)AudioSettings.dspTime - songDspTimeStart;
    }

    void Start()
    {
        currentDspTime = (float)AudioSettings.dspTime;
        lastDspTime = currentDspTime;
    }
    
    void Update()
    {
        lastDspTime = currentDspTime;
        currentDspTime = (float)AudioSettings.dspTime;
    }
    
    public float SetDspTimeStart()
    {
        songDspTimeStart = (float)AudioSettings.dspTime;

        return songDspTimeStart;
    }
}
