using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// For entry inputs handled by inputManager, use SetValidation to fill in vaidator
/// Because separate canvas for savePoint, manually assign that
/// </summary>
public class Script_EntryInput : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject submitButton;
    private TMP_InputField TMPInputField;
    private TextMeshProUGUI TMPGUI;
    private float caretBlinkRate;
    private Color caretColor;

    void Update()
    {
        HandleKeyInput();
    }

    void HandleKeyInput()
    {
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
        // set cursor to end when we initialize with existing entry (this already happens on deselect)
        TMPInputField.caretPosition = TMPInputField.text.Length;
        Debug.Log($"Setting caret position to: {TMPInputField?.caretPosition}");
        Debug.Log($"Caret blinkrate is: {TMPInputField?.caretBlinkRate}");
        SetCaretVisible(true);
    }

    public void OnDeselect(BaseEventData e)
    {
        SetCaretVisible(false);
    }

    void SetCaretVisible(bool isOn)
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
                TMPInputField.caretBlinkRate    = 0f;
                Color newCaretColor             = caretColor;
                newCaretColor.a                 = 0f;
                TMPInputField.caretColor        = newCaretColor;
            }
        }
    }

    public void InitializeState(string text)
    {
        TMPInputField = GetComponent<TMP_InputField>();
        TMPInputField.text = text;
    }

    public void Awake()
    {
        TMPGUI = GetComponent<TextMeshProUGUI>();
        TMPInputField = GetComponent<TMP_InputField>();
        caretBlinkRate = TMPInputField.caretBlinkRate;
        caretColor = TMPInputField.caretColor;
    }

    public void Setup()
    {
        
    }
}
