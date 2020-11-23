using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_InputManager : MonoBehaviour
{
    [SerializeField] private Script_DialogueManager dialogueManager;
    [SerializeField] private Script_InputNameHandler nameHandler;
    [SerializeField] private Script_InputAnswerHandler answerHandler;
    [SerializeField] public TMP_InputField TMPInputField;
    private Script_InputHandler activeInputHandler;
    public CanvasGroup inputCanvas;
    public Script_EntryInput entryInput;
    public Script_InputAudioSettings settings;
    public AudioSource audioSource;
    public float errorVol;
    public float typingVol;
    public float deleteVol; // TODO


    private AudioClip typingSFX;
    private AudioClip errorSFX;
    private AudioClip deleteSFX;
    
    private void OnEnable() {
        // initialize state of entry input
        entryInput.InitializeState(string.Empty);
        // set active
    }

    /// <summary>
    /// called on save entry INPUT view by Choice
    /// </summary>
    public void InputSaveEntryChoice(int Id, string playerInputText)
    {
        int nextChildNodeIdx = 0;
        // Submit
        if (Id == 0)
            nextChildNodeIdx = activeInputHandler.HandleSubmit(playerInputText);

        OnSubmit(nextChildNodeIdx);
        entryInput.InitializeState(string.Empty);
    }

    private void OnSubmit(int nextChildNodeIdx)
    {
        if (nextChildNodeIdx > -1)
        {
            audioSource.PlayOneShot(typingSFX, typingVol);
            dialogueManager.EndInputMode(nextChildNodeIdx);
        }
        else
            audioSource.PlayOneShot(errorSFX, errorVol);
    }

    /// <summary>
    /// mode will dictate the max char count, custom validation etc
    /// </summary>
    /// <param name="inputMode">the enum of which Mode we want to enter</param>
    public void Initialize(InputMode inputMode)
    {
        print($"input mode {inputMode}");
        switch (inputMode)
        {
            case InputMode.Name:
                activeInputHandler = nameHandler;
                activeInputHandler.SetValidation(TMPInputField);
                break;
            case InputMode.Answer:
                activeInputHandler = answerHandler;
                activeInputHandler.SetValidation(TMPInputField);
                break;
        }
    }
    
    public void Setup()
    {
        entryInput.Setup();
        inputCanvas.gameObject.SetActive(false);
        errorSFX = settings.errorSFX;
        typingSFX = settings.typingSFX;
        deleteSFX = settings.deleteSFX;
    }
}
