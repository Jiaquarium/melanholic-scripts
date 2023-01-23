using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(Script_CanvasGroupController))]
public class Script_MapNotification : MonoBehaviour
{
    [SerializeField] private Script_MapNotificationsManager mapNotificationsManager;
    [SerializeField] private Script_CanvasGroupController textCanvasGroup;
    [SerializeField] private Script_TimelineTeletypeReveal teletypeReveal;
    [SerializeField] private TextMeshProUGUI TMPtext;

    private Script_CanvasGroupController controller;
    private Coroutine delayTeletypeCoroutine;

    public string Text
    {
        get => TMPtext.text;
        set => TMPtext.text = value;
    }
    
    public void Open(
        string text,
        bool isSFXOn,
        AudioClip sfx,
        float fadeInTime,
        float delayTeletypeTime
    )
    {
        Text = text.AddBrackets(withSpace: true);
        
        teletypeReveal.IsSFXOn = isSFXOn;
        teletypeReveal.SfxOverride = sfx;
        
        textCanvasGroup.Close();

        controller.FadeIn(fadeInTime, () => {
            delayTeletypeCoroutine = StartCoroutine(WaitToTeletype());
        });

        IEnumerator WaitToTeletype()
        {
            yield return new WaitForSeconds(delayTeletypeTime);

            textCanvasGroup.Open();
        }
    }

    /// <summary>
    /// Check to ensure they are active to avoid coroutine error.
    /// Always make sure the callback is called though.
    /// </summary>
    public void Close(Action cb, float fadeOutTime)
    {
        if (textCanvasGroup.gameObject.activeInHierarchy)
        {
            textCanvasGroup.FadeOut(mapNotificationsManager.TextCanvasGroupFadeOutTime, () => {
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

    /// <summary>
    /// Close instantly for when something interrupts the Map Notification like exiting back from
    /// where you came from.
    /// </summary>
    public void InitialState()
    {
        StopCoroutines();
        
        // Close() will stop these canvas group ctrl's coroutines
        controller.Close();
        textCanvasGroup.Close();

        void StopCoroutines()
        {
            if (delayTeletypeCoroutine != null)
            {
                StopCoroutine(delayTeletypeCoroutine);
                delayTeletypeCoroutine = null;
            }
        }
    }
    
    public void Setup()
    {
        controller = GetComponent<Script_CanvasGroupController>();
        InitialState();
    }
}