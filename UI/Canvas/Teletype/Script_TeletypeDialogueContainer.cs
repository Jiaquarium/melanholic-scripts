using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Script_TeletypeDialogueContainer : MonoBehaviour
{
    [Tooltip("Set True if only want behavior to show after key input")]
    [SerializeField] private bool isOnlyTMProBehaviorOnClose;
    [SerializeField] private bool isOnlyFadeOutBehavior;
    
    [SerializeField] private Script_TeletypeTextContainer[] texts;
    
    [SerializeField] private List<Script_TMProBehavior> TMProBehaviors;
    private Script_CanvasGroupController canvasGroupController;

    public bool IsOnlyTMProBehaviorOnClose => isOnlyTMProBehaviorOnClose;
    public bool IsOnlyFadeOutBehavior => isOnlyFadeOutBehavior;
    
    // Start is called before the first frame update
    void OnValidate()
    {
        FindTexts();
        PopulateTMPBehaviors();
    }

    void OnEnable()
    {
        if (IsOnlyFadeOutBehavior || IsOnlyTMProBehaviorOnClose)
            EnableTMProBehaviors(false);
    }
    
    void Awake()
    {
        canvasGroupController = GetComponent<Script_CanvasGroupController>();
    }
    
    public void EnableTMProBehaviors(bool isEnabled)
    {
        if (TMProBehaviors != null)
            TMProBehaviors.ForEach(tmp => tmp.enabled = isEnabled);
    }

    public void FadeOut(float fadeTime, Action cb)
    {
        canvasGroupController.FadeOut(fadeTime, cb, isForceCanvasRemainOpen: true);
    }
    
    void FindTexts()
    {
        texts = GetComponentsInChildren<Script_TeletypeTextContainer>(true);
    }

    void PopulateTMPBehaviors()
    {
        TMProBehaviors = GetComponentsInChildren<Script_TMProBehavior>(true).ToList();
    }

    public void InitialState()
    {
        texts = GetComponentsInChildren<Script_TeletypeTextContainer>(true);
        PopulateTMPBehaviors();

        foreach (var t in texts)
            t.Close();
    }
}
