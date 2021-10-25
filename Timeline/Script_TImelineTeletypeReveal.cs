using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Use for when want to show dialogue purely via Timeline. Useful if the format is extremely different
/// than the default provided by Dialogue Manager.
/// </summary>

[RequireComponent(typeof(TextMeshProUGUI))]
public class Script_TimelineTeletypeReveal : MonoBehaviour
{
    [SerializeField] private Script_TimelineSequenceController sequenceController;
    
    // Give option to wait for Submit input to move Timeline forward, instead of automatically.
    [SerializeField] private bool isOnInputContinue;

    private bool isListening;

    void OnEnable()
    {
        var textUI = GetComponent<TextMeshProUGUI>();
        
        // Pause Timeline
        if (sequenceController != null)
            sequenceController.Pause();

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

    void Update()
    {
        if (isListening && Input.GetButtonDown(Const_KeyCodes.Submit))
        {
            Debug.Log("Playing timeline on input");
            
            sequenceController.Play();

            isListening = false;
        }
    }

    private void OnTypingDone()
    {
        Debug.Log("Notify timeline to start next action");
        
        if (isOnInputContinue)
        {
            isListening = true;
            
            return;
        }

        if (sequenceController != null)
            sequenceController.Play();
    }
}
