using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_CanvasGroupController))]
public class Script_TitleLogo : MonoBehaviour
{
    [SerializeField] private Script_CanvasGroupController titleLogoCanvasGroup;

    private Coroutine fadeInCoroutine;


    void OnDisable()
    {
        StopMyCoroutines();
    }
    
    public void WaitToFadeIn(float waitTime, float fadeInTime)
    {
        gameObject.SetActive(true);

        fadeInCoroutine = StartCoroutine(WaitToFadeInLogo());

        IEnumerator WaitToFadeInLogo()
        {
            yield return new WaitForSeconds(waitTime);
            
            titleLogoCanvasGroup.FadeIn(fadeInTime);
        }
    }

    private void StopMyCoroutines()
    {
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }
    }

    public void InitialState()
    {
        titleLogoCanvasGroup.InitialState();
    }

    public void Setup()
    {
        titleLogoCanvasGroup.Setup();
    }
}
