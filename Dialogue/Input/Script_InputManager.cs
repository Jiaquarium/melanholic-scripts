﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class Script_InputManager : MonoBehaviour
{
    public static Script_InputManager Control;
    
    [SerializeField] private Script_DialogueManager dialogueManager;
    [SerializeField] private Script_InputNameHandler nameHandler;
    [SerializeField] private Script_InputAnswerHandler answerHandler;
    [SerializeField] private Script_InputCodeHandler codeHandler;
    [SerializeField] public TMP_InputField TMPInputField;
    [SerializeField] public TMP_InputField TMPNameInputField;
    
    // For Dev Only
    [SerializeField] public TMP_InputField TMPCodeInputField;
    
    
    [SerializeField] private CanvasGroup inputCanvasGroup;
    [SerializeField] private CanvasGroup _CCTVInputCanvasGroup;
    [SerializeField] private CanvasGroup nameInputCanvasGroup;
    
    public Script_EntryInput entryInput;
    
    [SerializeField] private Script_SFXManager SFXManager;
    
    [SerializeField] private Script_AlphaInputUtils alphaInputUtils;

    private AudioSource audioSource;
    
    private Script_InputHandler activeInputHandler;
    private TMP_InputField activeInputField;
    private CanvasGroup activeInputCanvasGroup;

    public CanvasGroup CCTVInputCanvasGroup
    {
        get => _CCTVInputCanvasGroup;
    }
    
    private void OnEnable() {
        // initialize state of entry input
        entryInput.InitializeState(string.Empty);
        // set active
    }

    // Singleton instantion in Awake because Input Manager is set up on every level.
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
        
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }
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

    public void InteractableObjectInputSubmit(int Id, string text)
    {
        activeInputHandler.HandleSubmit(text);
    }

    public void InsertSFX()
    {
        audioSource.PlayOneShot(SFXManager.UITypingSFX, SFXManager.UITypingSFXVol);
    }

    // Since letter grid move makes InsertSFX, need to differentiate inserting from here
    public void InsertLetterGridSFX()
    {
        audioSource.PlayOneShot(SFXManager.PencilEditShort, SFXManager.PencilEditShortVol);
    }

    public void InsertCodeSFX()
    {
        audioSource.PlayOneShot(SFXManager.UICodeTypingSFX, SFXManager.UICodeTypingSFXVol);
    }
    
    public void ErrorSFX()
    {
        audioSource.PlayOneShot(SFXManager.UIErrorSFX, SFXManager.UIErrorSFXVol);
    }

    public void BackspaceSFX()
    {
        SFXManager.PlayExitSubmenuPencil();
    }

    private void OnSubmit(int nextChildNodeIdx)
    {
        if (nextChildNodeIdx > -1)
        {
            InsertSFX();
            dialogueManager.EndInputMode(nextChildNodeIdx);
        }
        else
        {
            ErrorSFX();
        }   
    }

    public void End()
    {
        inputCanvasGroup.gameObject.SetActive(false);
        nameInputCanvasGroup.gameObject.SetActive(false);
        CCTVInputCanvasGroup.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public bool GetInputIsAlpha()
    {
        return alphaInputUtils.IsKeyDownAlpha();
    }
    
    /// <summary>
    /// mode will dictate the max char count, custom validation etc
    /// </summary>
    /// <param name="inputMode">the enum of which Mode we want to enter</param>
    public void Initialize(
        InputMode inputMode,
        TMP_InputField inputField,
        CanvasGroup canvasGroup
    )
    {
        Dev_Logger.Debug($"input mode {inputMode}");
        
        switch (inputMode)
        {
            // Via Dialogue
            case InputMode.Name:
                activeInputField = TMPNameInputField;
                activeInputCanvasGroup = nameInputCanvasGroup;
                
                activeInputHandler = nameHandler;
                activeInputHandler.SetValidation(activeInputField);
                activeInputCanvasGroup.gameObject.SetActive(true);
                break;
            
            // Via Dialogue
            case InputMode.Answer:
                activeInputField = inputField ?? TMPInputField;
                activeInputCanvasGroup = canvasGroup ?? inputCanvasGroup;
                
                activeInputHandler = answerHandler;
                activeInputHandler.SetValidation(activeInputField);
                activeInputCanvasGroup.gameObject.SetActive(true);
                break;
            
            // Via Interactable Object
            case InputMode.Code:
                activeInputField = inputField ?? TMPInputField;
                activeInputCanvasGroup = canvasGroup ?? inputCanvasGroup;
                
                activeInputHandler = codeHandler;
                activeInputHandler.SetValidation(activeInputField);
                activeInputCanvasGroup.gameObject.SetActive(true);
                break;
        }
    }
    
    public void Setup()
    {
        entryInput.Setup();
        inputCanvasGroup.gameObject.SetActive(false);
        nameInputCanvasGroup.gameObject.SetActive(false);
        CCTVInputCanvasGroup.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_InputManager))]
    public class Script_InputManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_InputManager t = (Script_InputManager)target;
            if (GUILayout.Button("Populate CCTV Code"))
            {
                string scarletCipherText = "";
                int[] scarletCipher = Script_ScarletCipherManager.Control.ScarletCipher;

                foreach (int codeNum in scarletCipher)
                {
                    scarletCipherText += codeNum.ToString();
                }
                
                t.TMPCodeInputField.text = scarletCipherText;
            }
        }
    }
#endif
}
