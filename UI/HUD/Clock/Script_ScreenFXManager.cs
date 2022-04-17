using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_ScreenFXManager : MonoBehaviour
{
    const float AwareDuration           = 1f;
    const float AwareAmplitude          = 1f;
    const float AwareFrequency          = 1f;
    const float AwareInterval           = 60f;

    const float WarningDuration         = 1.75f;
    const float WarningAmplitude        = 1f;
    const float WarningFrequency        = 1f;
    const float WarningInterval         = 20f;

    const float DangerDuration          = 2f;
    const float DangerAmplitude         = 1f;
    const float DangerFrequency         = 1f;
    const float DangerInterval          = 5f;

    [SerializeField] private Script_Game game;
    [SerializeField] private Script_ClockManager clockManager;
    [SerializeField] private Script_HUDManager HUDManager;
    
    [SerializeField] private float currentDuration;
    [SerializeField] private float currentAmplitude;
    [SerializeField] private float currentFrequency;
    [SerializeField] private float currentInterval;
    [SerializeField] private float timer;

    [SerializeField] private Script_AudioSourceFader rumbleAudio;
    [SerializeField] private float rumbleFadeOutTime = 0.25f;
    
    private Script_Clock.TimeStates lastState;

    private bool isScreenShaking;

    void Update()
    {
        UpdateScreenFX();
        
        if (game.IsScreenFXDisabled())
            return;
        
        if (
            clockManager.ClockTimeState == Script_Clock.TimeStates.None
            || clockManager.ClockState == Script_Clock.States.Done
            || !HUDManager.IsClockShowing()
            || Const_Dev.IsTrailerMode
        )
            return;

        timer -= Time.unscaledDeltaTime;

        if (timer <= 0)
        {
            if (!isScreenShaking)
                ShakeScreen();
            
            timer = currentInterval;
        }
    }

    public void ShakeScreen()
    {
        Script_VCamManager.VCamMain.Shake(
            currentDuration,
            currentAmplitude,
            currentFrequency
        );

        var sfx = Script_SFXManager.SFX;
        rumbleAudio.Source.volume = sfx.ScreenShakeRumbleVol;
        rumbleAudio.Source.clip = sfx.ScreenShakeRumble;
        rumbleAudio.Source.Play();

        isScreenShaking = true;

        StartCoroutine(WaitToFadeOutAudio());

        IEnumerator WaitToFadeOutAudio()
        {
            yield return new WaitForSeconds(currentDuration);

            rumbleAudio.FadeOut(rumbleFadeOutTime);
            
            isScreenShaking = false;

            // In the case Player exits level during shake, Script_CameraShake will not end it.
            Script_VCamManager.VCamMain.StopShake();
        }
    }

    private void UpdateScreenFX()
    {
        Script_Clock.TimeStates timeState;
        
        // Use different const values based on clock time state 
        switch (clockManager.ClockTimeState)
        {
            case (Script_Clock.TimeStates.Aware):
                currentDuration     = AwareDuration;
                currentAmplitude    = AwareAmplitude;
                currentFrequency    = AwareFrequency;
                currentInterval     = AwareInterval;
                break;
            case (Script_Clock.TimeStates.Warning):
                currentDuration     = WarningDuration;
                currentAmplitude    = WarningAmplitude;
                currentFrequency    = WarningFrequency;
                currentInterval     = WarningInterval;
                break;
            case (Script_Clock.TimeStates.Danger):
                currentDuration     = DangerDuration;
                currentAmplitude    = DangerAmplitude;
                currentFrequency    = DangerFrequency;
                currentInterval     = DangerInterval;
                break;
        }

        timeState = clockManager.ClockTimeState;
        
        if (lastState != timeState)
        {
            EndTimer();
            lastState = timeState;
        }

        void EndTimer()
        {
            timer = 0f;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_ScreenFXManager))]
public class Script_ScreenFXManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_ScreenFXManager t = (Script_ScreenFXManager)target;
        if (GUILayout.Button("Shake Screen"))
        {
            t.ShakeScreen();
        }
    }
}
#endif