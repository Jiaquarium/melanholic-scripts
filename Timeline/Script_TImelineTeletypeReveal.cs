using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Script_TImelineTeletypeReveal : MonoBehaviour
{
    [SerializeField] private Script_IntroController introController;
    
    void OnEnable()
    {
        var textUI = GetComponent<TextMeshProUGUI>();
        
        // Pause Timeline
        introController.Pause();

        // Hide all text preemptively because we must wait a frame to allow for text replacement.
        textUI.maxVisibleCharacters = 0;
        StartCoroutine(NextFrameTeletype());
        
        IEnumerator NextFrameTeletype()
        {
            // Must wait a frame or the first PauseCommend char will not be replaced on TextUI reveal.
            yield return null;
        
            IEnumerator coroutine = Script_DialogueManager.TeletypeRevealLine(
                textUI.text,
                textUI,
                Script_DialogueManager.charPauseDefault,
                OnTypingDone
            );
            StartCoroutine(coroutine);
        }
    }

    private void OnTypingDone()
    {
        Debug.Log("Notify timeline to start next action");
        introController.Play();
    }
}
