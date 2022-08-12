using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

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

    [SerializeField] private bool isAlphaInput;

    [SerializeField] private Script_InputManager inputManager;

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
        if (!EventSystem.current.sendNavigationEvents)
            return;
        
        PlayerInput playerInput = Script_PlayerInputManager.Instance.MyPlayerInput;
        
        if (
            // NOTE: CPU-ONLY
            Input.GetKeyDown(KeyCode.DownArrow)
            || (
                playerInput.actions[Const_KeyCodes.UISubmit].WasPressedThisFrame()
                // NOTE: CPU-ONLY
                // Space should just give a null SFX, not navigate to Submit
                && !Input.GetKeyDown(KeyCode.Space)
            )
            || playerInput.actions[Const_KeyCodes.UICancel].WasPressedThisFrame()
        )
        {
            // Ignore letters for entry inputs requiring typing alphabet characters.
            if (isAlphaInput && inputManager.GetInputIsAlpha())
            {
                return;
            }
            
            TMPGUI.GetComponent<TMP_InputField>().DeactivateInputField();
            // set Submit as selected
            EventSystem.current.SetSelectedGameObject(submitButton);
        }
    }
    
    public void OnSelect(BaseEventData e)
    {
        // If not highlighting whole selection, manually set the caret position.
        if (!isSelectAllOnFocus)
        {
            // Set cursor to end when we initialize with existing entry (this already happens on deselect).
            TMPInputField.caretPosition = caretPositionZeroAlways ? 0 : TMPInputField.text.Length;
            
            Debug.Log($@"Setting caret position to: {TMPInputField?.caretPosition}
                with blinkrate {TMPInputField?.caretBlinkRate}");
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
