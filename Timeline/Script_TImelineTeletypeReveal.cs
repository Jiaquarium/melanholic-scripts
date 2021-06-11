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
        // Pause Timeline
        introController.Pause();
        
        var textUI = GetComponent<TextMeshProUGUI>();
        
        IEnumerator coroutine = Script_DialogueManager.TeletypeRevealLine(
            textUI.text,
            textUI,
            Script_DialogueManager.charPauseDefault,
            OnTypingDone
        );
        StartCoroutine(coroutine);
    }

    private void OnTypingDone()
    {
        Debug.Log("Notify timeline to start next action");
        introController.Play();
    }
}
