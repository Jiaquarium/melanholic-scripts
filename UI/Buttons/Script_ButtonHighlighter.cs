using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles button highlighting and disabled state
/// </summary>
public class Script_ButtonHighlighter : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool isActive = true;
    public Image[] outlines;
    [SerializeField] protected bool isHighlighted;
    [SerializeField] protected Image deactivateOverlay;
    
    /// Ensure initializing comes before OnSelect
    void OnEnable()
    {
        InitializeState();
    }

    /// Prevent FOUC on showing if using same menu and changing active button while inactive
    void OnDisable() {
        InitializeState();
    }

    void Update()
    {
        /// Don't show highlight for Slow Awake Event Systems (Choices) 
        if (EventSystem.current != null && !EventSystem.current.sendNavigationEvents)
        {
            foreach (Image img in outlines)     img.enabled = false;
        }
        else
        {
            if (isHighlighted)                  foreach (Image img in outlines) img.enabled = true;
        }
    }
    
    /// <summary>
    /// For public use
    /// </summary>
    public void Select()
    {
        HighlightOutline(true);
    }
    
    public virtual void OnSelect(BaseEventData e)
    {
        HighlightOutline(true);
    }

    public virtual void OnDeselect(BaseEventData e)
    {
        HighlightOutline(false);
    }

    public void InitializeState()
    {
        isActive = true;
        // isHighlighted = true;
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
            return;
        
        HighlightOutline(false);
    }

    public void Activate(bool _isActive)
    {
        GetComponent<Button>().enabled = _isActive;
        isActive = _isActive;

        if (deactivateOverlay != null)
        {
            deactivateOverlay.gameObject.SetActive(!_isActive);
        }
    }

    protected virtual void HighlightOutline(bool isOn)
    {
        // if (isHighlighted == isOn)  return;)

        foreach (Image img in outlines)
        {
            img.enabled = isOn;
        }
        
        isHighlighted = isOn;
    }
}
