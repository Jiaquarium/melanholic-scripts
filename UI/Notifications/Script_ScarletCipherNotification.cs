using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Script_ScarletCipherNotification : MonoBehaviour
{
    private const int RandomizerTryLimit = 5;
    
    [SerializeField] private TextMeshProUGUI TMPtext;

    public string Text
    {
        get => TMPtext.text;
        set => TMPtext.text = value;
    }

    public int FinalRevealDigit { get; set; }

    private Script_CanvasGroupController controller;
    private int lastDigit = -1;

    // ------------------------------------------------------------------
    // Timeline Signals

    /// <summary>
    /// Get random digit that differs vs. last revealed guaranteeing a visual change.
    /// </summary>
    public void RandomizeDigit()
    {
        int randomDigit = lastDigit;
        int tryCount = 0;

        while (
            randomDigit == lastDigit
            && tryCount < RandomizerTryLimit)
        {
            randomDigit = UnityEngine.Random.Range(0, 10);
            tryCount++;
        }

        Text = randomDigit.ToString();
        lastDigit = randomDigit;
    }

    /// <summary>
    /// Get random digit that also differs from the final reveal guaranteeing a visual change.
    /// </summary>
    public void RandomizeDigitNotFinal()
    {
        int randomDigit = lastDigit;
        int tryCount = 0;

        while (
            randomDigit == lastDigit
            || randomDigit == FinalRevealDigit
            && tryCount < RandomizerTryLimit
        )
        {
            randomDigit = UnityEngine.Random.Range(0, 10);
            tryCount++;
        }

        Text = randomDigit.ToString();
        lastDigit = randomDigit;
    }

    public void RevealFinalDigit()
    {
        Text = FinalRevealDigit.ToString();
    }

    // ------------------------------------------------------------------
    
    public void InitialState()
    {
        lastDigit = -1;
    }
    
    public void Setup()
    {
        controller = GetComponent<Script_CanvasGroupController>();
        controller.Close();

        InitialState();
    }
}
