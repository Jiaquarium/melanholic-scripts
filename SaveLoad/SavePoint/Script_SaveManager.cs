using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

/// <summary>
/// Save Point manager, similar to InputManager
/// </summary>
public class Script_SaveManager : MonoBehaviour
{
    public Script_Game game;
    public CanvasGroup saveChoiceCanvas;
    public CanvasGroup saveEntryCanvas;
    public GameObject saveProgressCanvasGroup;
    public GameObject saveProgressCanvas;
    public GameObject saveCompleteCanvas;
    public Script_DialogueChoice[] choices;
    public Script_DialogueManager dm;
    public Script_EntryInput entryInput;
    public Script_EntryManager entryManager;
    [SerializeField] private TextMeshProUGUI timestampDisplayText;
    [SerializeField] private TMP_InputValidator TMPInputEntryValidator;
    
    [SerializeField] private float showSavingMinTime;
    [SerializeField] private float showSavingCompleteTime;
    private bool isShowingSaving;

    void Update()
    {
        string timestampNow = DateTime.Now.FormatDateTime();
        if (timestampNow != timestampDisplayText.text)
            timestampDisplayText.text = timestampNow;
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
    public void InputChoice(int Id)
    {
        // yes, save
        if (Id == 0)
        {
            string savePointNameId = game.GetSavePointData().nameId;
            // check to see if editting or new entry
            Script_Entry existingEntry = entryManager.GetExistingEntry(savePointNameId);
            if (existingEntry != null)
                entryInput.InitializeState(existingEntry.text);
            else
                entryInput.InitializeState(string.Empty);
            
            StartEntryMode();
            dm.HideDialogue();
        }
        // no, don't save
        else
        {
            dm.NextDialogueNode(1);
        }
        EndSavePrompt();
    }

    /// <summary>
    /// called on save entry INPUT view by Choice
    /// </summary>
    public void InputSaveEntryChoice(int Id, string playerInputText)
    {
        // Submit
        if (Id == 0)
        {
            Model_SavePointData spData = game.GetSavePointData();
            
            // create Entry
            entryManager.AddEntry(
                spData.nameId,
                playerInputText,
                DateTime.Now,
                spData.headline
            );
            
            saveProgressCanvas.SetActive(true);
            Script_SaveGameControl.control.Save();

            isShowingSaving = true;
            Script_AwaitFile.AwaitFile(Script_SaveGameControl.saveFilePath);
            isShowingSaving = false;
            
            StartCoroutine(AwaitSaveComplete());
        }
        // Cancel
        else if (Id == 1)
        {
            Debug.Log("Cancelling save entry input!");
            dm.NextDialogueNode(1);
        }
        EndEntryMode();
    }

    /// <summary>
    ///     to mock saving; our saves are near-instant but i still want the feel of a "real" save
    /// </summary>
    IEnumerator AwaitSaveComplete()
    {
        yield return new WaitForSeconds(showSavingMinTime);
        // wait for AwaitFile if needed (it will turn isShowingSaving false)
        while (isShowingSaving)    { }

        saveCompleteCanvas.SetActive(true);
        saveProgressCanvas.SetActive(false);

        Script_Game.Game.PlayerFullHeal();

        yield return new WaitForSeconds(showSavingCompleteTime);

        saveCompleteCanvas.SetActive(false);
        dm.NextDialogueNode(0);
    }

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
        saveChoiceCanvas.gameObject.SetActive(false);
        saveEntryCanvas.gameObject.SetActive(false);
        saveProgressCanvasGroup.SetActive(true);
        saveProgressCanvas.SetActive(false);
        saveCompleteCanvas.SetActive(false);
        entryInput.Setup();
    }
}
