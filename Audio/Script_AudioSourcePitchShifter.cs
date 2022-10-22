using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_AudioSourcePitchShifter : MonoBehaviour
{
    [SerializeField] private float startingPitch;
    
    [SerializeField] [Range(-3f, 3f)] private float lowerBound;
    [SerializeField] [Range(-3f, 3f)] private float upperBound;
    
    [SerializeField] private float maxTimer;
    [SerializeField] private float delayStartTime;

    [SerializeField] private Vector4[] alternateSettings;

    [Tooltip("True to ignore randomizer and force a pitch")]
    [SerializeField] private bool isConstantPitch;
    
    [Tooltip("Set true to start at startingVolume and allow for volume changes")]
    [SerializeField] private bool isAdjustVolume;
    [SerializeField] private float startingVolume;

    private float timer;
    private float delayTimer;
    private AudioSource source;

    public bool IsAdjustVolume
    {
        get => isAdjustVolume;
        set => isAdjustVolume = value;
    }

    public float StartingVolume
    {
        get => startingVolume;
        set => startingVolume = value;
    }

    private AudioSource Source
    {
        get
        {
            if (source == null)
                source = GetComponent<AudioSource>();
            
            return source;
        }
        set => source = value;
    }
    
    void Start()
    {
        timer = maxTimer;
        delayTimer = delayStartTime;

        if (startingPitch != 0f)
            Source.pitch = startingPitch;
        
        if (isAdjustVolume)
            Source.volume = startingVolume;
    }

    void Update()
    {
        delayTimer -= Time.unscaledDeltaTime;
        
        if (delayTimer <= 0f)
        {
            timer -= Time.unscaledDeltaTime;
            delayTimer = 0f;

            if (isConstantPitch)
            {
                Source.pitch = lowerBound;
                return;
            }
        }

        if (timer <= 0f && !isConstantPitch)
        {
            Source.pitch = UnityEngine.Random.Range(lowerBound, upperBound);
            
            timer = maxTimer;
        }
    }

    public void SwitchMySettings(int i)
    {
        var settings = alternateSettings[i];
        
        lowerBound = settings.x;
        upperBound = settings.y;
        maxTimer = settings.z;

        if (isConstantPitch)
            Source.pitch = lowerBound;
        
        if (isAdjustVolume)
            Source.volume = settings.w;
    }
}
