﻿using System.Collections;
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

    public bool IsPaused { get; set; }
    
    /// Ensure initializing comes before OnSelect
    void OnEnable()
    {
        Dev_Logger.Debug($"{name} onEnable");
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
        Dev_Logger.Debug($"OnSelect {name}");
        
        Script_SlowAwakeEventSystem slowAwakeEventSystem = null;
        if (EventSystem.current != null)
            slowAwakeEventSystem = EventSystem.current.GetComponent<Script_SlowAwakeEventSystem>();
        
        if (slowAwakeEventSystem != null && !slowAwakeEventSystem.IsTimerDone)
            return;
        
        if (IsPaused)
            return;
        
        HighlightOutline(true);

        if (itemHighlighter != null)
            itemHighlighter.HighlightAndShowDescription(isOn: true);
    }

    public virtual void OnDeselect(BaseEventData e)
    {
        if (IsPaused)
            return;
        
        Dev_Logger.Debug($"OnDeselect {name}");
        HighlightOutline(false);

        if (itemHighlighter != null)
            itemHighlighter.HighlightAndShowDescription(isOn: false);
    }

    public void Select()
    {
        Dev_Logger.Debug($"Select {name}");
        HighlightOutline(true);
    }

    public void InitializeState()
    {
        isActive = true;
        isLoading = true;
        
        HighlightOutline(false);
        
        // If this is slow awakening UI store if this should be highlighted.
        // Basically, we want to call HighlightOutline in two parts with Slow Awake Event System;
        // HandleSlowAwake will handle the call to the other part of Highlight Outline.
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
        {
            Dev_Logger.Debug($"{name} HandleSlowAwakeOnEnable");
            HandleHighlight(false);
        }
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
            {
                HandleHighlight(true);
            }
        }
    }

    protected virtual void HighlightOutline(bool isOn)
    {
        if (isOn)
            Dev_Logger.Debug($"{name}: HighlightOutline {isOn}");

        // Slow Awake: Handled by HandleSlowAwake
        HandleHighlight(isOn);
        
        // Slow Awake: Handled by InitializeState
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
