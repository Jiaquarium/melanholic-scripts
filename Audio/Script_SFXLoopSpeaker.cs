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

    protected override void OnEnable()
    {
        base.OnEnable();

        offsetTimer = offsetStartTime;
        timer = 0f;
    }

    protected override void Update()
    {
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
}
