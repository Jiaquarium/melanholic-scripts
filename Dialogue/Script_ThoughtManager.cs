using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_ThoughtManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    [SerializeField] private Script_Canvas canvas;
    public AudioClip thoughtStartSoundFX;
    public AudioSource audioSource;
    public TextMeshProUGUI thoughtText;


    public float timePerChar;


    private IEnumerator coroutine;
    private bool isShowingThought;


    public float waitTimeBuffer;

    public void ShowThought(Model_Thought thought)
    {
        // stop any coroutine here
        if (isShowingThought)
        {
            StopCoroutine(coroutine);
            thoughtText.text = "";   
        }

        isShowingThought = true;
        
        thoughtText.text = Script_Utils.FormatString(thought.thought);

        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        audioSource.PlayOneShot(thoughtStartSoundFX, 1.0f);
    }

    public void CloseThought(Model_Thought thoughtObj)
    {
        float waitTime = 0f;

        foreach(char letter in thoughtObj.thought.ToCharArray())
        {
            waitTime += timePerChar;
        }

        waitTime += waitTimeBuffer;

        coroutine = WaitToCloseThought(waitTime);

        StartCoroutine(coroutine);
    }

    IEnumerator WaitToCloseThought(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        HideThought();
        
        isShowingThought = false;
        thoughtText.text = string.Empty;
    }

    public void HideThought()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void Setup()
    {
        if (canvas.ContinuationIcon != null) canvas.ContinuationIcon.Setup();
        HideThought();
    }
}
