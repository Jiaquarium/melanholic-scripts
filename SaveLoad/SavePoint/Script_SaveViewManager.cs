using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

/// <summary>
/// Save Point manager, similar to InputManager
/// </summary>
public class Script_SaveViewManager : MonoBehaviour
{
    public static Script_SaveViewManager Control;
    
    [SerializeField] private Script_Game game;
    
    [SerializeField] private Script_CanvasGroupController saveAndRestartCanvasGroup;
    [SerializeField] private Script_CanvasGroupController saveAndStartWeekendCanvasGroup;
    [SerializeField] private List<Script_SaveMessage> saveMessages;
    
    [SerializeField] private CanvasGroup saveChoiceCanvas;
    [SerializeField] private CanvasGroup saveEntryCanvas;
    
    [SerializeField] private Script_DialogueChoice[] choices;
    
    [SerializeField] private Script_DialogueManager dm;
    
    [SerializeField] private Script_EntryInput entryInput;
    [SerializeField] private Script_EntryManager entryManager;
    
    // [SerializeField] private TextMeshProUGUI timestampDisplayText;
    [SerializeField] private TMP_InputValidator TMPInputEntryValidator;
    
    [SerializeField] private float showSavingMinTime;
    [SerializeField] private float showSavingCompleteTime;

    private bool isShowingSaving;
    
    public void ShowSaveAndRestarMessage()
    {
        saveAndRestartCanvasGroup.FadeIn();
    }

    public void ShowSaveAndStartWeekendMessage()
    {
        saveAndStartWeekendCanvasGroup.FadeIn();
    }

    public void SetSaveMessagesDoneState()
    {
        saveMessages.ForEach(saveMessage => saveMessage.Done());
    }
    
    public void StartSavePromptMode()
    {
        // to get rid of flash at beginning
        foreach(Script_DialogueChoice choice in choices)
        {
            choice.cursor.enabled = false;
        }

        saveChoiceCanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// called on save prompt (yes, no)
    /// </summary>
    public void InputChoice(int Id) {}

    /// <summary>
    /// called on save entry INPUT view by Choice
    /// </summary>
    public void InputSaveEntryChoice(int Id, string playerInputText) {}

    void EndSavePrompt()
    {
        saveChoiceCanvas.gameObject.SetActive(false);
    }

    void StartEntryMode()
    {
        saveEntryCanvas.gameObject.SetActive(true);
    }

    void EndEntryMode()
    {
        saveEntryCanvas.gameObject.SetActive(false);
    }

    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }
        
        saveChoiceCanvas.gameObject.SetActive(false);
        saveEntryCanvas.gameObject.SetActive(false);
        entryInput.Setup();
        
        saveAndRestartCanvasGroup.Close();
        saveAndStartWeekendCanvasGroup.Close();
    }
}
