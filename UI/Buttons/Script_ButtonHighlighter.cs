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
    public bool isLoading = false;
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
        HandleSlowAwake();
    }
    
    /// <summary>
    /// For public use
    /// </summary>
    public void Select()
    {
        Debug.Log($"Select {name}");
        HighlightOutline(true);
    }
    
    public virtual void OnSelect(BaseEventData e)
    {
        Debug.Log($"OnSelect {name}");
        HighlightOutline(true);
    }

    public virtual void OnDeselect(BaseEventData e)
    {
        Debug.Log($"OnDeselect {name}");
        HighlightOutline(false);
    }

    public void InitializeState()
    {
        isActive = true;
        isLoading = true;
        // isHighlighted = true;
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
            return;
        
        HighlightOutline(false);
    }

    /// <summary>
    /// For use when needing to fade out deactivated buttons (start menu)
    /// </summary>
    /// <param name="_isActive"></param>
    public void Activate(bool _isActive)
    {
        GetComponent<Button>().enabled = _isActive;
        isActive = _isActive;

        if (deactivateOverlay != null)
        {
            deactivateOverlay.gameObject.SetActive(!_isActive);
        }
    }

    /// <summary>
    /// SlowAwakeEventSystem will disable sendNavigationEvents until it is ready to be interacted with
    /// </summary>
    void HandleSlowAwake()
    {
        /// Don't show highlight for Slow Awake Event Systems (Choices) 
        if (EventSystem.current != null && !EventSystem.current.sendNavigationEvents)
        {
            isLoading = true;
            foreach (Image img in outlines) img.enabled = false;
        }
        else
        {
            isLoading = false;
            if (isHighlighted)  foreach (Image img in outlines) img.enabled = true;
        }
    }

    protected virtual void HighlightOutline(bool isOn)
    {
        if (isOn)   Debug.Log($"{name}: HighlightOutline {isOn}");
        // if (isHighlighted == isOn)  return;)

        foreach (Image img in outlines)
        {
            img.enabled = isOn;
        }
        
        isHighlighted = isOn;
    }
}
