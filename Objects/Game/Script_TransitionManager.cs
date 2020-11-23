using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_TransitionManager : MonoBehaviour
{
    public Script_CanvasGroupFadeInOut fader;
    public readonly static float RestartPlayerFadeInTime = 0.25f;
    public readonly static float RestartPlayerFadeOutTime = 1f;
    
    public IEnumerator FadeIn(float t, Action action)
    {
        return fader.FadeInCo(t, action);
    }

    public IEnumerator FadeOut(float t, Action action)
    {
        return fader.FadeOutCo(t, action);
    }

    public void Setup()
    {
        fader.gameObject.SetActive(true);
    }
}
