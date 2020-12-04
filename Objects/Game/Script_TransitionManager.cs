using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_TransitionManager : MonoBehaviour
{
    public Script_CanvasGroupFadeInOut fader;
    [SerializeField] private Script_CanvasGroupController underDialogueController;
    public readonly static float RestartPlayerFadeInTime = 0.25f;
    public readonly static float RestartPlayerFadeOutTime = 1f;
    [SerializeField] private float _underDialogueFadeTime;
    public float UnderDialogueFadeTime {
        get
        {
            return _underDialogueFadeTime;
        }
    }
    
    public IEnumerator FadeIn(float t, Action action)
    {
        return fader.FadeInCo(t, action);
    }

    public IEnumerator FadeOut(float t, Action action)
    {
        return fader.FadeOutCo(t, action);
    }

    public void UnderDialogueBlackScreen()
    {
        underDialogueController.Open();
    }

    public void UnderDialogueFadeIn(float t, Action action)
    {
        underDialogueController.FadeIn(t, action);
    }
    
    public void UnderDialogueFadeOut(float t, Action action)
    {
        underDialogueController.FadeOut(t, action);
    }

    public void Setup()
    {
        fader.gameObject.SetActive(true);
    }
}
