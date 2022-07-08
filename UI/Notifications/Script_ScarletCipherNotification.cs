using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Script_ScarletCipherNotification : MonoBehaviour
{
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;

    [Tooltip("Time how long the randomizer animation should last. Should stop on last beat of SFX.")]
    [SerializeField] private float randomDuration = 1.74f;
    [SerializeField] private float switchDigitTime;
    [SerializeField] private float pauseAfterNotifyingTime;

    [SerializeField] private TextMeshProUGUI TMPtext;
    
    [SerializeField] private bool isRandomAnimation;

    private Script_CanvasGroupController controller;
    private float timer;

    public string Text
    {
        get => TMPtext.text;
        set => TMPtext.text = value;
    }

    public float PauseAfterNotifyingTime
    {
        get => pauseAfterNotifyingTime;
    }

    void Update()
    {
        if (isRandomAnimation)
        {
            timer -= Time.smoothDeltaTime;

            if (timer <= 0f)
            {
                int randomDigit = UnityEngine.Random.Range(0, 10);
                Text = randomDigit.ToString();
                
                timer = switchDigitTime;
            }
        }
    }
    
    public void Open(string text)
    {
        isRandomAnimation = true;
        timer = switchDigitTime;
        
        controller.FadeIn(fadeInTime);

        StartCoroutine(WaitToRevealNumber());

        IEnumerator WaitToRevealNumber()
        {
            yield return new WaitForSeconds(randomDuration);

            isRandomAnimation = false;
            timer = 0f;
            
            Text = text;
        }
    }

    public void Close(Action cb)
    {
        controller.FadeOut(fadeOutTime, cb);
    }
    
    public void Setup()
    {
        controller = GetComponent<Script_CanvasGroupController>();
        controller.Close();
    }
}
