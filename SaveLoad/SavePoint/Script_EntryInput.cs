using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// For entry inputs handled by inputManager, use SetValidation to fill in vaidator
/// Because separate canvas for savePoint, manually assign that
/// 
/// Note: Ensure the Game Object is set inactive at game start or initial input field focus will not work.
/// </summary>
public class Script_EntryInput : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject submitButton;
    
    [SerializeField] private bool caretPositionZeroAlways;
    [SerializeField] private bool disableDelete;
    [SerializeField] private bool isCustomCaretColor;
    [SerializeField] private Color customCaretColor;
    [SerializeField] private bool isCustomCaretWidth;
    [SerializeField] private int customCaretWidth;
    // If we're treating the caret as a highlighter, then disable it going past the last character.
    // Also disables blinking.
    [SerializeField] private bool isHighlighterCaret;

    [TextArea]
    [SerializeField] private string defaultValue;
    [SerializeField] private bool isResetDefault = true;
    [SerializeField] private bool isSelectAllOnFocus;

    private TMP_InputField TMPInputField;
    private TextMeshProUGUI TMPGUI;
    private float caretBlinkRate;
    private Color caretColor;

    void OnValidate()
    {
        SetDefault();
    }
    
    void Awake()
    {
        TMPGUI = GetComponent<TextMeshProUGUI>();
        TMPInputField = GetComponent<TMP_InputField>();
        
        TMPInputField.onFocusSelectAll = isSelectAllOnFocus;

        SetCaretAttributes();

        SetDefault();
    }
    
    void OnEnable()
    {
        SetCaretAttributes();
        
        if (isResetDefault)
            SetDefault();
    }
    
    void OnGUI()
    {
        if (
            disableDelete
            && (Event.current.keyCode == KeyCode.Backspace || Event.current.keyCode == KeyCode.Delete)
            && (Event.current.type == EventType.KeyUp || Event.current.type == EventType.KeyDown)
        )
        {
            Event.current.Use();
        }
    }
    
    void Update()
    {
        HandleNavigateToSubmit();
        PreventCaretOverflow();
    }

    private void HandleNavigateToSubmit()
    {
        // Script_SlowAwakeEventSystem will disable navigation events on prompt start up.
        // If we're still in this start up period, disable auto submit selection.
        if (!EventSystem.current.sendNavigationEvents)  return;
        
        if (
            Input.GetKeyDown(KeyCode.DownArrow) /// NOTE: CPU-ONLY
            || Input.GetButtonDown(Const_KeyCodes.Submit)
            || Input.GetButtonDown(Const_KeyCodes.Cancel)
        )
        {
            TMPGUI.GetComponent<TMP_InputField>().DeactivateInputField();
            // set Submit as selected
            EventSystem.current.SetSelectedGameObject(submitButton);
        }
    }
    
    public void OnSelect(BaseEventData e)
    {
        if (!isSelectAllOnFocus)
        {
            // set cursor to end when we initialize with existing entry (this already happens on deselect)
            TMPInputField.caretPosition = caretPositionZeroAlways ? 0 : TMPInputField.text.Length;
            Debug.Log($"Setting caret position to: {TMPInputField?.caretPosition}");
            Debug.Log($"Caret blinkrate is: {TMPInputField?.caretBlinkRate}");
        }

        SetCaretVisible(true);
    }

    public void OnDeselect(BaseEventData e)
    {
        SetCaretVisible(false);
    }

    private void SetCaretVisible(bool isOn)
    {
        // As a precaution, if we're selecting the input field
        // ensure it's active. Sometimes the caret doesn't show.
        if (!TMPInputField.gameObject.activeInHierarchy && isOn)
        {
            Debug.Log("Setting TMPInputField active before setting Caret properties");
            TMPInputField.gameObject.SetActive(true);
        }

        if (isOn)
        {
            TMPInputField.caretBlinkRate    = caretBlinkRate;
            TMPInputField.caretColor        = caretColor;
        }
        else
        {
            if (TMPInputField != null)
            {
                // TMPInputField.caretBlinkRate    = 0f;
                Color newCaretColor             = caretColor;
                newCaretColor.a                 = 0f;
                TMPInputField.caretColor        = newCaretColor;
            }
        }
    }

    private void SetCaretAttributes()
    {
        caretBlinkRate = isHighlighterCaret ? 0 : TMPInputField.caretBlinkRate;
        caretColor = isCustomCaretColor ? customCaretColor : TMPInputField.caretColor;
        TMPInputField.caretWidth = isCustomCaretWidth ? customCaretWidth : TMPInputField.caretWidth;
    }

    private void PreventCaretOverflow()
    {
        if (isHighlighterCaret && TMPInputField.caretPosition >= TMPInputField.text.Length)
        {
            TMPInputField.caretPosition = TMPInputField.text.Length - 1;
            TMPInputField.ForceLabelUpdate();
        }
    }

    private void SetDefault()
    {
        var input = TMPInputField ?? GetComponent<TMP_InputField>();
        
        input.text = string.IsNullOrEmpty(defaultValue)
            ? string.Empty
            : defaultValue;
    }

    public void InitializeState(string text)
    {
        TMPInputField = GetComponent<TMP_InputField>();
        TMPInputField.text = text;
    }

    public void Setup()
    {
        
    }
}
