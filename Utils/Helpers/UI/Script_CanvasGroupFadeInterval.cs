using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_CanvasGroupFadeInOut))]
[RequireComponent(typeof(CanvasGroup))]
public class Script_CanvasGroupFadeInterval : MonoBehaviour
{
    [SerializeField] private float interval;
    [SerializeField] private bool isManualStart;

    void OnEnable()
    {
        if (!isManualStart)
        {
            GetComponent<Script_CanvasGroupFadeInOut>().Initialize();
            FadeIn();
        }
    }
    
    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void FadeIn()
    {
        StartCoroutine(
            GetComponent<Script_CanvasGroupFadeInOut>().FadeInCo(interval, () => 
            {
                FadeOut();
            })
        );
    }

    public void FadeOut()
    {
        StartCoroutine(
            GetComponent<Script_CanvasGroupFadeInOut>().FadeOutCo(interval, () => 
            {
                FadeIn();
            })
        );
    }
}
