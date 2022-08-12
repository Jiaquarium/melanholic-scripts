using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Script_UIChoice : MonoBehaviour
{
    public Image cursor;
    public bool isSelected;
    public int Id;

    [SerializeField] private Script_CanvasGroupController canvasGroupController;

    void Start()
    {
        InitializeState();
    }

    /// to prevent FOUC on showing if using same menu and changing active button while inactive
    void OnDisable()
    {
        InitializeState();
    }
    
    void Update()
    {
        if (EventSystem.current == null)    return;
        
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            isSelected = true;
            
            /// Give Buttonhighlighter precedence to hide cursor
            /// if the button is not in an active state
            Script_ButtonHighlighter myHighlighter = GetComponent<Script_ButtonHighlighter>();
            if (myHighlighter != null)
            {
                if (myHighlighter.isLoading)    cursor.enabled = false;
            }
            else            
            {
                cursor.enabled = true;
            }
        }
        else
        {
            cursor.enabled = false;
            isSelected = false;
        }
    }

    public virtual void HandleSelect() {}

    public void InitializeState()
    {
        cursor.enabled = false;
    }

    /// <summary>
    /// Call from OnClick handler to fade out (used when transitioning from UI).
    /// </summary>
    public void HandleFadeOut(float t = Script_CanvasGroupController.DefaultFadeTime, Action cb = null)
    {
        canvasGroupController?.FadeOut(onFadedOut: () => {
            canvasGroupController.MyCanvasGroup.alpha = 1f;
            canvasGroupController.MyCanvasGroup.gameObject.SetActive(false);

            if (cb != null)
                cb();            
        });
    }

    virtual public void TransitionSFX()
    {
        Script_SFXManager.SFX.PlaySubmitTransition();
    }

    /// <summary>
    /// - LastElevatorPromptChoices UIChoice: Yes
    /// </summary>
    virtual public void TransitionLastElevatorSFX()
    {
        Script_SFXManager.SFX.PlaySubmitTransition();
    }

    /// <summary>
    /// - LastElevatorPromptChoices UIChoice: No
    /// </summary>
    virtual public void TransitionLastElevatorSFXCancel()
    {
        Script_SFXManager.SFX.PlaySubmitTransitionCancel();
    }
}
