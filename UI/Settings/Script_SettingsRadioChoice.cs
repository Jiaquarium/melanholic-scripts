using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Script_SettingsRadioChoice : MonoBehaviour, ISelectHandler
{
    [SerializeField] protected Script_SettingsSystemController settingsSystemController;
    [SerializeField] protected Script_ButtonHighlighter buttonHighlighter;
    
    /// <summary>
    /// Use with unfocusedHighlight to implement "held" highlights even when unfocusing from a button. Ensure following:
    /// (1) unfocusedHighlight is added to outlines in ButtonHighlighter
    /// (2) Call HoldHighlight to pause the buttonHighlighter's state and use Select/Deselect & HandleUnfocusedHighlight
    ///     to show the Unfocused Highlight
    /// </summary>
    [SerializeField] private Image focusedHighlight;
    [SerializeField] private Image unfocusedHighlight;
    
    public Script_ButtonHighlighter ButtonHighlighter => buttonHighlighter;
    
    public virtual void OnSelect(BaseEventData e) {}

    public void Select()
    {
        buttonHighlighter.Select();
    }

    public void Deselect()
    {
        buttonHighlighter.Deselect();
    }

    // Pause the button highlighter from updating based on its state
    public void HoldHighlight(bool isHold)
    {
        buttonHighlighter.IsPaused = isHold;
        buttonHighlighter.IsUpdatePaused = isHold;
    }

    public void HandleUnfocusedHighlight(bool isSelect)
    {
        // Hide focused highlights
        if (focusedHighlight != null)
        {
            if (buttonHighlighter.IsChangeImageActiveState)
                focusedHighlight.gameObject.SetActive(false);
            else
                focusedHighlight.enabled = false;
        }

        // Depending on if it's Selected or Deselected, show the secondary highlight
        if (unfocusedHighlight != null)
        {
            if (buttonHighlighter.IsChangeImageActiveState)
                unfocusedHighlight.gameObject.SetActive(isSelect);
            else
                unfocusedHighlight.enabled = isSelect;
        }
    }
}
