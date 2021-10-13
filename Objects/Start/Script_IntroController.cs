using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

/// <summary>
/// Controls the Intro Sequence.
/// 
/// On Key Press will skip to the end of the end and activate Start Menu.
/// </summary>
public class Script_IntroController : Script_TimelineSequenceController
{
    [SerializeField] private float startScreenFrame;

    [SerializeField] private Script_IntroInputManager inputManager;
    [SerializeField] private Script_StartOverviewController mainController;

    void OnEnable()
    {
        inputManager.IsDisabled = false;
    }
    
    void Update()
    {
        inputManager.HandleEnterInput();
    }

    // Skip to frame where Start Screen starts. Timeline will then initialize Start Screen via Signals.
    public void SkipToStartScreen()
    {
        director.time = startScreenFrame / ((TimelineAsset)director.playableAsset).editorSettings.fps;
        director.Evaluate();
        
        if (!director.playableGraph.IsPlaying())
        {
            Debug.Log($"{name} Playable is currently Paused");
            Play();
        }

        // Must manually call StartScreenStart, cannot use Signal because Evaluate ignores.
        // https://forum.unity.com/threads/timeline-notifications-arent-sent-in-playabledirector-manual-mode.711494/
        mainController.StartScreenStart(false);
    }

    public void DisableInput()
    {
        inputManager.IsDisabled = true;
    }
}
