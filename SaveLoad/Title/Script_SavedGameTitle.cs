using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Script_HideGameObjects))]
[RequireComponent(typeof(Script_Slot))]
public class Script_SavedGameTitle : MonoBehaviour
{
    [SerializeField] private Script_StartOverviewController mainController;
    [SerializeField] private GameObject savedState;
    [SerializeField] private GameObject emptyState;
    [SerializeField] private Transform heartsParent;
    [SerializeField] private GameObject[] heartObjs;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI headlineText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private Transform continueSubmenu;
    [SerializeField] private Transform newGameSubmenu;
    public bool isRendered { get; private set; }
    
    void OnValidate()
    {
        Image[] heartImgs = heartsParent.GetChildren<Image>();
        heartObjs = new GameObject[heartImgs.Length];
        for (int i = 0; i < heartImgs.Length; i++)
        {
            heartObjs[i] = heartImgs[i].gameObject;
        }
    }

    public void OnClick()
    {
        switch (mainController.State)
        {
            case SavedGameState.Start:
                mainController.EnterFileChoices(this);
                break;
            case SavedGameState.Delete:
                mainController.EnterDeleteFileChoices(this);
                break; 
            case SavedGameState.Copy:
                mainController.HandleEnterPasteView(this);
                break; 
            case SavedGameState.Paste:
                mainController.EnterPasteFileChoices(this);
                break; 
            default:
                mainController.EnterFileChoices(this);
                break;
        }
    }
    
    private void EmptyState()
    {
        emptyState.gameObject.SetActive(true);
        savedState.gameObject.SetActive(false);

        isRendered = false;
    }

    private void Render(Model_SavedGameTitleData savedGame)
    {
        int maxHp               = savedGame.health;
        string name             = savedGame.name;
        string headline         = savedGame.headline;
        DateTime dateTime       = DateTime.FromBinary(savedGame.date);
        float playTime          = savedGame.playTime;

        GetComponent<Script_HideGameObjects>().HideAfterNum(maxHp, heartObjs);
        nameText.text           = name;
        headlineText.text       = headline;
        dateText.text           = dateTime.FormatDateTime();
        playTimeText.text       = playTime.FormatSecondsHHMMSS();
        
        savedState.gameObject.SetActive(true);
        emptyState.gameObject.SetActive(false);

        isRendered = true;
    }

    public void InitializeState()
    {
        int Id = GetComponent<Script_Slot>().Id;

        // ask Script_SavedGameTitleControl for file
        Model_SavedGameTitleData savedGame = Script_SavedGameTitleControl.Control.Load(Id);
        continueSubmenu.gameObject.SetActive(false);
        newGameSubmenu.gameObject.SetActive(false);

        if (mainController == null) Debug.LogError("SavedGameTitle: You need to make ref to mainController");

        if (savedGame == null)
        {
            EmptyState();
        }
        else
        {
            Render(savedGame);
        }
    }

    public void Setup()
    {
        InitializeState();
    }
}
