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
/// 
/// For Behaviors:
/// The "Controller" Script_TimelineTeletypeReveal should be the last line revealed in cut, and must be marked 
/// isFadeOutParentsBehaviors to tell the parent container to fade out / start any fade out behaviors.
/// For TMP behaviors text to work, need to have TMProBehavior on all TMPs under the Script_TeletypeDialogueContainer
/// </summary>

[RequireComponent(typeof(TextMeshProUGUI))]
public class Script_TimelineTeletypeReveal : MonoBehaviour
{
    private static float waitToFadeOutTime = 0.5f;
    private static float fadeOutTime = 0.25f;

    [SerializeField] private Script_TimelineSequenceController sequenceController;
    
    // Give option to wait for Submit input to move Timeline forward, instead of automatically.
    [SerializeField] private bool isOnInputContinue;

    [SerializeField] private UnityEvent pauseAction;
    [SerializeField] private UnityEvent resumeAction;
    [SerializeField] private UnityEvent onTypingDoneAction;

    [SerializeField] private bool isGlitchText;
    [SerializeField] private bool isSFXOn;
    [SerializeField] private AudioClip sfxOverride;

    [SerializeField] private bool isFadeOutParentsBehaviors;
    [SerializeField] private Script_TeletypeDialogueContainer teletypeDialogueContainer;

    private bool isListening;

    private UnityEvent PauseAction
    {
        get => pauseAction;
    }

    private UnityEvent ResumeAction
    {
        get => resumeAction;
    }

    public bool IsSFXOn
    {
        get => isSFXOn;
        set => isSFXOn = value;
    }

    public AudioClip SfxOverride
    {
        get => sfxOverride;
        set => sfxOverride = value;
    }

    void OnEnable()
    {
        Dev_Logger.Debug($"{name} on enable called");
        
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
                silenceOverride: IsSFXOn, 
                isGlitchText: isGlitchText,
                sfxOverride: SfxOverride
            );
            StartCoroutine(coroutine);
        }
    }

    void Update()
    {
        if (
            isListening
            && Script_PlayerInputManager.Instance.RewiredInput.GetButtonDown(Const_KeyCodes.RWUISubmit)
        )
        {
            Dev_Logger.Debug("Playing timeline on input");
            
            ResumeAction.SafeInvoke();
            isListening = false;
            
            if (isFadeOutParentsBehaviors)
            {
                FadeOutSetBehavior();
                
                return;
            }
            
            sequenceController.Play();
        }
    }

    private void OnTypingDone()
    {
        Dev_Logger.Debug("Notify timeline to start next action");
        
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

    private void FadeOutSetBehavior()
    {
        Dev_Logger.Debug("Fade Out Set Behavior");
        
        if (teletypeDialogueContainer.IsOnlyTMProBehaviorOnClose)
            teletypeDialogueContainer.EnableTMProBehaviors(true);

        Dev_Logger.Debug("Starting coroutine wait to fade out");
        
        StartCoroutine(WaitToFadeOut());

        IEnumerator WaitToFadeOut()
        {
            yield return new WaitForSeconds(waitToFadeOutTime);

            teletypeDialogueContainer.FadeOut(
                fadeOutTime,
                () => {
                    Dev_Logger.Debug("Play sequence controller");
                    sequenceController.Play();
                    isListening = false;
                }
            );
        }
    }
}
