using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SFXLoopSpeaker : Script_ProximitySpeaker
{
    [SerializeField] private float offsetStartTime;
    [SerializeField] private float interval;

    [SerializeField] private AudioClip clip;

    private float offsetTimer;
    private float timer;

    public float OffsetStartTime
    {
        get => offsetStartTime;
        set => offsetStartTime = value;
    }

    public float Interval
    {
        get => interval;
        set => interval = value;
    }

    public bool IsPaused { get; set; }

    protected override void OnEnable()
    {
        base.OnEnable();

        InitialState();
    }

    protected override void Update()
    {
        if (IsPaused)
            return;
        
        base.Update();

        if (offsetTimer > 0f)
        {
            offsetTimer = Mathf.Max(0, offsetTimer - Time.deltaTime);
            return;
        }

        timer = Mathf.Max(0, timer - Time.deltaTime);

        if (timer <= 0f)
        {
            PlaySFX(clip);
            timer = interval;
        }
    }

    public void StopLoop()
    {
        IsPaused = true;
        InitialState();
    }

    public void ForcePlay()
    {
        IsPaused = false;
    }

    private void InitialState()
    {
        offsetTimer = offsetStartTime;
        timer = 0f;
    }
}
