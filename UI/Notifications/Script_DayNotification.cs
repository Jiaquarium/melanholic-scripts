using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(Script_CanvasGroupController))]
public class Script_DayNotification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI daySubtext;

    private Script_CanvasGroupController controller;

    public string DayText
    {
        get => dayText.text;
        set => dayText.text = value;
    }

    public string DaySubtext
    {
        get => daySubtext.text;
        set => daySubtext.text = value;
    }

    public void Setup()
    {
        controller = GetComponent<Script_CanvasGroupController>();
        controller.InitialState();
    }
}
