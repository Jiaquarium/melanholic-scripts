using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(Script_CanvasGroupController))]
public class Script_MapNotification : MonoBehaviour
{
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;

    [SerializeField] private Script_CanvasGroupController textCanvasGroup;
    [SerializeField] private Script_TimelineTeletypeReveal teletypeReveal;
    [SerializeField] private float textCanvasGroupFadeOutTime;
    [SerializeField] private TextMeshProUGUI TMPtext;

    private Script_CanvasGroupController controller;

    public string Text
    {
        get => TMPtext.text;
        set => TMPtext.text = value;
    }
    
    public void Open(
        string text,
        bool isSFXOn,
        AudioClip sfx
    )
    {
        Text = text.AddBrackets(withSpace: true);
        
        teletypeReveal.IsSFXOn = isSFXOn;
        teletypeReveal.SfxOverride = sfx;
        
        textCanvasGroup.Close();

        controller.FadeIn(fadeInTime, () => {
            textCanvasGroup.Open();
        });
    }

    /// <summary>
    /// Check to ensure they are active to avoid coroutine error.
    /// Always make sure the callback is called though.
    /// </summary>
    public void Close(Action cb)
    {
        if (textCanvasGroup.gameObject.activeInHierarchy)
        {
            textCanvasGroup.FadeOut(textCanvasGroupFadeOutTime, () => {
                controller.FadeOut(fadeOutTime, cb);
            });
        }
        else if (controller.gameObject.activeInHierarchy)
            controller.FadeOut(fadeOutTime, cb);
        else
        {
            if (cb != null)
                cb();
        }
    }
    
    public void Setup()
    {
        controller = GetComponent<Script_CanvasGroupController>();
        controller.InitialState();
    }
}