using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AudioConfiguration : MonoBehaviour
{
    public static Script_AudioConfiguration Instance;
    
    [SerializeField] private Script_BackgroundMusicManager bgm;
    [SerializeField] private List<Script_Speaker> speakers;

    void OnEnable()
    {
        AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;
    }

    void OnDisable()
    {
        AudioSettings.OnAudioConfigurationChanged -= OnAudioConfigurationChanged;
    }

    public void AddSpeaker(Script_Speaker speaker)
    {
        speakers.Add(speaker);
    }

    public void RemoveSpeaker(Script_Speaker speaker)
    {
        speakers.Remove(speaker);     
    }

    public void PauseAll()
    {
        bgm.Pause();
        
        foreach (var speaker in speakers)
        {
            if (speaker != null)
                speaker.Pause();
        }
    }

    /// <summary>
    /// Event handler to rebuild speaker states after AudioSetting.Reset is called automatically by
    /// Unity when Device output changes, will call with deviceWasChanged = false.
    /// </summary>
    private void OnAudioConfigurationChanged(bool deviceWasChanged)
    {
        Dev_Logger.Debug($"Device was changed {deviceWasChanged}");

        if (deviceWasChanged)
        {
            AudioConfiguration config = AudioSettings.GetConfiguration();
            AudioSettings.Reset(config);
        }

        RestartBgmPlayState();
    }

    private void RestartBgmPlayState()
    {
        Script_SaveSettingsControl.Instance.LoadMasterVolume();
        
        if (bgm.IsPlayingThisFrame)
            bgm.Source?.Play();
        
        for (var i = 0; i < speakers.Count; i++)
        {
            if (speakers[i].IsPlayingThisFrame)
                speakers[i].Source?.Play();
        }
    }

    public void Setup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}