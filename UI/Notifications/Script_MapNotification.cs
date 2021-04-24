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

    [SerializeField] private TextMeshProUGUI TMPtext;

    private Script_CanvasGroupController controller;
    
    public void Open(string text)
    {
        TMPtext.text = text.AddBrackets(withSpace: true);
        
        controller.FadeIn(fadeInTime);
    }

    public void Close(Action cb)
    {
        controller.FadeOut(fadeOutTime, cb);
    }
    
    public void Setup()
    {
        controller = GetComponent<Script_CanvasGroupController>();
        controller.InitialState();
    }
}