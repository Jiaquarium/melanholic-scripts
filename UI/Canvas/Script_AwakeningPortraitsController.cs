using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AwakeningPortraitsController : MonoBehaviour
{
    [SerializeField] private Script_CanvasGroupController myController;
    [SerializeField] private Script_CanvasGroupController awakeningPortraitsCanvasGroup;
    
    public void OpenAwakeningPortraits()
    {
        awakeningPortraitsCanvasGroup.Open();
        myController.Open();
    }

    public void CloseAwakeningPortraits()
    {
        myController.Close();
        awakeningPortraitsCanvasGroup.Close();
    }
    
    public void Setup()
    {
        CloseAwakeningPortraits();
    }
}
