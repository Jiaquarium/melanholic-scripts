using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Use for when want to show dialogue purely via Timeline. Useful if the format is extremely different
/// than the default provided by Dialogue Manager.
/// 
/// Can also be used without timeline to purely reveal Text On Enable.
/// 
/// Note: For On Input Continue, ensure to give at least 4 frames of buffer (@ 30 fps) to give timeline
/// some buffer since it will not stop on the exact frame.
/// </summary>

[RequireComponent(typeof(TextMeshProUGUI))]
public class Script_TimelineTeletypeReveal : MonoBehaviour
{
    [SerializeField] private Script_TimelineSequenceController sequenceController;
    
    // Give option to wait for Submit input to move Timeline forward, instead of automatically.
    [SerializeField] private bool isOnInputContinue;

    [SerializeField] private UnityEvent pauseAction;
    [SerializeField] private UnityEvent resumeAction;
    [SerializeField] private UnityEvent onTypingDoneAction;

    [SerializeField] private bool isGlitchText;

    private bool isListening;

    private UnityEvent PauseAction
    {
        get => pauseAction;
    }

    private UnityEvent ResumeAction
    {
        get => resumeAction;
    }

    void OnEnable()
    {
        var textUI = GetComponent<TextMeshProUGUI>();
        
        // Pause Timeline
        if (sequenceController != null)
        {
            PauseAction.SafeInvoke();
            sequenceController.Pause(sequenceController.Time);
        }

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
                OnTypingDone,
                isGlitchText: isGlitchText
            );
            StartCoroutine(coroutine);
        }
    }

    void Update()
    {
        if (
            isListening
            && Script_PlayerInputManager.Instance.MyPlayerInput.actions[Const_KeyCodes.UISubmit].WasPressedThisFrame()
        )
        {
            Debug.Log("Playing timeline on input");
            
            ResumeAction.SafeInvoke();
            sequenceController.Play();

            isListening = false;
        }
    }

    private void OnTypingDone()
    {
        Debug.Log("Notify timeline to start next action");
        
        onTypingDoneAction.SafeInvoke();
        
        if (isOnInputContinue)
        {
            isListening = true;
            
            return;
        }

        if (sequenceController != null)
        {
            ResumeAction.SafeInvoke();
            sequenceController.Play();
        }
    }
}
