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

    /// <summary>
    /// - New, Continue Game
    /// - Demo End Note to Main Menu
    /// </summary>
    virtual public void TransitionSFX()
    {
        Script_SFXManager.SFX.PlaySubmitTransition();
    }

    /// <summary>
    /// - Demo End Note Quit to Desktop
    /// </summary>
    virtual public void TransitionCancelSFX()
    {
        Script_SFXManager.SFX.PlaySubmitTransitionCancel();
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

    /// <summary>
    /// - Title: SavedGamesSubmenu_Continue
    /// - Title: SavedGamesSubmenu_New
    /// - Title: SavedGamesSubmenu_Copy
    /// - Title: SavedGamesSubmenu_Delete
    /// </summary>
    public void PencilExitSubmenuSFX()
    {
        Script_SFXManager.SFX.PlayExitSubmenuPencil();
    }

    /// <summary>
    /// - Title: Copy Yes
    /// </summary>
    public void PencilEditSFX()
    {
        Script_SFXManager.SFX.PlayTakeNote();
    }

    /// <summary>
    /// - Title: Delete Yes
    /// </summary>
    public void ChainWrappingCloseMenuSFX()
    {
        Script_SFXManager.SFX.PlayChainWrappingCloseMenuSFX();
    }
}
