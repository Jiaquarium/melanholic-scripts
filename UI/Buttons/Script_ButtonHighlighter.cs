using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles button highlighting and disabled state
/// 
/// initializes as isLoading
/// until Slow Awake Event System activated nav events this stays in isLoading state
/// 
/// InitializeState() -> HandleSlowAwake()
/// </summary>
public class Script_ButtonHighlighter : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool isActive = true;
    public bool isLoading = false;
    public Image[] outlines;
    [SerializeField] protected bool isHighlighted;
    [SerializeField] protected Image deactivateOverlay;

    [SerializeField] private Script_ItemHighlight itemHighlighter;
    
    /// Ensure initializing comes before OnSelect
    void OnEnable()
    {
        InitializeState();
        HandleSlowAwakeOnEnable();
    }

    /// Prevent FOUC on showing if using same menu and changing active button while inactive
    void OnDisable()
    {
        InitializeState();
    }

    void Update()
    {
        HandleSlowAwake();
    }
    
    public virtual void OnSelect(BaseEventData e)
    {
        Debug.Log($"OnSelect {name}");
        HighlightOutline(true);

        if (itemHighlighter != null)
            itemHighlighter.HighlightAndShowDescription(true);
    }

    public virtual void OnDeselect(BaseEventData e)
    {
        Debug.Log($"OnDeselect {name}");
        HighlightOutline(false);

        if (itemHighlighter != null)
            itemHighlighter.HighlightAndShowDescription(false);
    }

    public void Select()
    {
        Debug.Log($"Select {name}");
        HighlightOutline(true);
    }

    public void InitializeState()
    {
        isActive = true;
        isLoading = true;
        
        HighlightOutline(false);
        
        /// If this is slow awakening UI store if this should be highlighted 
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
            isHighlighted = true;
    }

    // HandleSlowAwake will handle in Update. This handles before rendering.
    private void HandleSlowAwakeOnEnable()
    {
        if (
            EventSystem.current != null
            && EventSystem.current.GetComponent<Script_SlowAwakeEventSystem>() != null
        )
            HandleHighlight(false);
    }

    /// <summary>
    /// SlowAwakeEventSystem will disable sendNavigationEvents until it is ready to be interacted with
    /// </summary>
    private void HandleSlowAwake()
    {
        /// Don't show highlight for Slow Awake Event Systems (Choices) 
        if (
            EventSystem.current != null
            && EventSystem.current.GetComponent<Script_SlowAwakeEventSystem>() != null
            && !EventSystem.current.sendNavigationEvents
            && isLoading
        )
        {
            HandleHighlight(false);
        }
        else
        {
            isLoading = false;
            if (isHighlighted)
                HandleHighlight(true);
        }
    }

    protected virtual void HighlightOutline(bool isOn)
    {
        if (isOn)   Debug.Log($"{name}: HighlightOutline {isOn}");

        HandleHighlight(isOn);
        
        isHighlighted = isOn;
    }

    private void HandleHighlight(bool isOn)
    {
        foreach (Image img in outlines)
            img.enabled = isOn;
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
}
