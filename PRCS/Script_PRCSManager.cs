using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///  Pre-Rendered Cut Scenes Manager (Singleton)
/// </summary>
public class Script_PRCSManager : MonoBehaviour
{
    public static Script_PRCSManager Control;
    [SerializeField] private CanvasGroup PRCSCanvasGroup;
    [SerializeField] private Canvas PRCSCanvas;
    
    public void Awake()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }
    }
    
    public void Start()
    {
        Initialize();
    }

    public void ShowPRCS(
        Script_PRCS PRCS,
        FadeSpeeds fadeInSpeed,
        Action cb
    )
    {
        PRCSCanvasGroup.alpha = 1f;
        PRCSCanvasGroup.gameObject.SetActive(true);

        PRCS.Setup();
        PRCS.gameObject.SetActive(true);
        PRCS.FadeIn(fadeInSpeed, cb);
    }

    public void HidePRCS(
        Script_PRCS PRCS,
        FadeSpeeds fadeOutSpeed,
        Action cb 
    )
    {
        PRCS.FadeOut(fadeOutSpeed, () =>
        {
            Initialize();
            if (Script_Game.Game.GetPlayer().GetIsViewing())
                Script_Game.Game.GetPlayer().SetIsInteract();

            if (cb != null)     cb();
        });
    }

    public void OpenPRCSNoFade(Script_PRCS PRCS)
    {
        PRCSCanvasGroup.alpha = 1f;
        PRCSCanvasGroup.gameObject.SetActive(true);

        PRCS.Setup();
        PRCS.Open();   
    }

    public void ClosePRCSNoFade(Script_PRCS PRCS)
    {
        PRCS.Close();
        
        PRCSCanvasGroup.alpha = 0f;
        PRCSCanvasGroup.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        /// Hide CanvasGroup but ensure the PRCS canvas and ready to use
        PRCSCanvasGroup.gameObject.SetActive(false);
        PRCSCanvasGroup.alpha = 0f;
        PRCSCanvas.gameObject.SetActive(true);

        Script_PRCS []allPRCS = PRCSCanvasGroup.GetComponentsInChildren<Script_PRCS>(true);
        foreach (Script_PRCS prcs in allPRCS)
        {
            prcs.gameObject.SetActive(false);
        }
    }
}
