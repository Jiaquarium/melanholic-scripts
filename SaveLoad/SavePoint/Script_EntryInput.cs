using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Rewired;
using UnityEngine.UI;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// NOTE: Set before Default time Script Execution Order
/// 
/// For entry inputs handled by inputManager, use SetValidation to fill in vaidator
/// Because separate canvas for savePoint, manually assign that
/// 
/// Note: Ensure the Game Object is set inactive at game start or initial input field focus will not work.
/// 
/// For behavior to work with Letter Select:
///     1. set the attached Button Highlighter isIgnoreEventSystemFirstSelectedLetterSelect: true to not highlight
///        this entry input when the Letter Select Grid should be first highlighted (on initialization we actually
///        select this first to make the caret appear then wait a frame to select Letter Select Grid)
///     2. set the desired first selected Letter Select element's Button Highlighter isForceHighlightOnSlowAwake: true
/// </summary>
public class Script_EntryInput : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private const float WaitTimeAfterAutoNavigate = 0.2f;
    
    [SerializeField] private Const_InputValidation.Validators entryValidator;
    
    public GameObject submitButton;
    

    [TextArea]
    [SerializeField] private string defaultValue;
    [SerializeField] private bool isResetDefault = true;
    [SerializeField] private bool isSelectAllOnFocus;
    [Tooltip("Set true to not nav to submit on Down input")]
    [SerializeField] private bool disableDelete;

    [Space][Header("Caret Properties")][Space]
    [SerializeField] private bool caretPositionZeroAlways;
    [SerializeField] private bool isCustomCaretColor;
    [SerializeField] private Color customCaretColor;
    [SerializeField] private bool isCustomCaretWidth;
    [SerializeField] private int customCaretWidth;
    // If we're treating the caret as a highlighter, then disable it going past the last character.
    // Also disables blinking.
    [SerializeField] private bool isHighlighterCaret;
    // Ignores OnDeselect hiding of Caret for LetterSelect set up which deselects this (EntryInput)
    private bool isCaretInitForLetterSelect;

    [Space][Header("Letter Select")][Space]
    [SerializeField] private Script_LetterSelectGrid letterSelectGrid;
    [SerializeField] private Image elementToMove;
    [SerializeField] private bool isMoveOnLetterSelect;
    [SerializeField] private Vector3 onLetterSelectPosition;
    [SerializeField] private Vector3 defaultPosition;

    [Space][Header("Code Select")][Space]
    [SerializeField] private List<Script_FocusArrowsSet> focusArrowSets;
    private bool isFocusArrowsDisabled;
    private ControllerType lastControllerType = ControllerType.Keyboard;
    
    [Space][Header("Other")][Space]
    [SerializeField] private Script_InputManager inputManager;

    private TMP_InputField _TMPInputField;  
    private TextMeshProUGUI TMPGUI;
    private float caretBlinkRate;
    private Color caretColor;
    
    private bool isLetterSelectGridActive;
    private string cachedText;
    private Coroutine cachedTextCoroutine;

    private bool isIgnoreSubmitOnDownJoystick;
    private bool disableMoveStartEndOfLineCode;

    public bool IsLetterSelectState => IsJoystickConnected && letterSelectGrid != null;
    private bool IsJoystickConnected => Script_PlayerInputManager.Instance.IsJoystickConnected;
    
    private TMP_InputField TMPInputField
    {
        get
        {
            if (_TMPInputField == null)
                _TMPInputField = GetComponent<TMP_InputField>();
            
            return _TMPInputField;
        }
        set => _TMPInputField = value;
    }

    void OnValidate()
    {
        SetDefault();
    }

    void OnEnable()
    {
        if (isResetDefault)
            SetDefault();
        
        HandleFocusArrows(isHideAll: true);
        
        // Handle Letter Select screen state
        if (!IsLetterSelectState)
        {
            if (isMoveOnLetterSelect)
                elementToMove.rectTransform.anchoredPosition = defaultPosition;
            
            if (letterSelectGrid != null)
                letterSelectGrid.Close();
        }
        else
        {
            if (isMoveOnLetterSelect)
                elementToMove.rectTransform.anchoredPosition = onLetterSelectPosition;

            if (letterSelectGrid != null)
                letterSelectGrid.Open();
            
            isCaretInitForLetterSelect = true;
        }
    }
    
    void OnDisable()
    {
        if (cachedTextCoroutine != null)
        {
            StopCoroutine(cachedTextCoroutine);
            cachedTextCoroutine = null;
        }
    }

    void Awake()
    {
        TMPGUI = GetComponent<TextMeshProUGUI>();

        switch (entryValidator)
        {
            case (Const_InputValidation.Validators.Answer):
            case (Const_InputValidation.Validators.Name):
                isIgnoreSubmitOnDownJoystick = false;
                break;
            case (Const_InputValidation.Validators.Code):
                isIgnoreSubmitOnDownJoystick = true;
                disableMoveStartEndOfLineCode = true;
                break;
            default:
                break;
        }
        
        // Prevent Input Field being select all'ed when Letter Select is active. It's too messy handling
        // the select all state when doing typing via Letter Select.
        if (!IsLetterSelectState)
            TMPInputField.onFocusSelectAll = isSelectAllOnFocus;
        else
            TMPInputField.onFocusSelectAll = false;

        SetCaretAttributes();

        SetDefault();
    }
    
    void OnGUI()
    {
        if (disableMoveStartEndOfLineCode && entryValidator == Const_InputValidation.Validators.Code)
            HandleCodeDisableMoveStartEndOfLine();
        
        if (disableDelete)
            HandlePreventDelete();
    }

    // Note: EntryInput is set before Default time in Script Execution Order
    void Update()
    {
        SetLastController();
        HandleCaretLetterSelectState();
        HandleInputsWhenSelected();
        PreventCaretOverflow();

        // Wait until the end of frame to save the text length so we can check if backspace action was successful
        // to play the Backspace SFX, since the UI text properties seem to update before we can test for text length
        cachedTextCoroutine = StartCoroutine(CacheTextLengthEndOfFrame());
        IEnumerator CacheTextLengthEndOfFrame()
        {
            yield return new WaitForEndOfFrame();

            cachedText = TMPInputField.text;
        }
    }

    void LateUpdate()
    {
        isFocusArrowsDisabled = false;
    }

    // Disable Up/Down events on CCTV Code to not interfere with focus arrows (which currently only appear with Joystick)
    private void HandleCodeDisableMoveStartEndOfLine()
    {
        if (
            (
                Event.current.keyCode == Const_KeyCodes.KeycodeDownArrow
                || Event.current.keyCode == Const_KeyCodes.KeycodeUpArrow
            )
            && (Event.current.type == EventType.KeyUp || Event.current.type == EventType.KeyDown)
        )
        {
            Event.current.Use();
        }
    }

    private void HandlePreventDelete()
    {
        if (
            (
                Event.current.keyCode == Const_KeyCodes.KeycodeBackspace
                || Event.current.keyCode == Const_KeyCodes.KeycodeDelete
            )
            && (Event.current.type == EventType.KeyUp || Event.current.type == EventType.KeyDown)
        )
        {
            Event.current.Use();
        }
    }

    private void SetLastController()
    {
        var _lastController = Script_PlayerInputManager.Instance.GetLastActiveController;
        if (_lastController != null)
            lastControllerType = _lastController.type;
    }

    private void HandleInputsWhenSelected()
    {
        // Script_SlowAwakeEventSystem will disable navigation events on prompt start up.
        // If we're still in this start up period, disable auto submit selection.
        if (
            EventSystem.current == null
            || !EventSystem.current.sendNavigationEvents
            || EventSystem.current.currentSelectedGameObject != TMPInputField.gameObject
        )
        {
            HandleFocusArrows(isHideAll: true);
            return;
        }

        Rewired.Player rewiredInput = Script_PlayerInputManager.Instance.RewiredInput;
        
        HandleNavigateToSubmit(lastControllerType);
        
        if (entryValidator == Const_InputValidation.Validators.Code)
            HandleFocusArrows(lastControllerType, rewiredInput: rewiredInput);
        
        // Keyboard handles backspace and moving through text natively via Input Field
        if (lastControllerType == ControllerType.Joystick)
        {
            // Don't handle backspace for Code Input or it will begin deleting the code.
            // Backspace (B Button) for Joystick should just nav to Submit (since it is also UICancel action).
            if (entryValidator == Const_InputValidation.Validators.Code)
                HandleCodeJoystickInputMove();
            else
                HandleJoystickBackspace();
        }
        // For keyboard, backspace should always work
        else
        {
            if (entryValidator == Const_InputValidation.Validators.Code)
                HandleBackspaceDisabled();
            else
                HandleKeyboardBackspaceSFX();
        }

        void HandleNavigateToSubmit(ControllerType controllerType)
        {
            // If Joystick, can test with ButtonDown Vert
            if (controllerType == ControllerType.Joystick)
            {
                if (
                    (!isIgnoreSubmitOnDownJoystick && rewiredInput.GetNegativeButtonDown(Const_KeyCodes.RWVertical))
                    || rewiredInput.GetButtonDown(Const_KeyCodes.RWUISubmit)
                    || (
                        rewiredInput.GetButtonDown(Const_KeyCodes.RWUICancel)
                        && entryValidator == Const_InputValidation.Validators.Code
                    )
                )
                {
                    NavToSubmitShortcut();
                }
            }
            // For keyboard, only allow down arrow (otherwise typing "S" will cause move down)
            else
            {
                if (
                    // NOTE: KEYBOARD-ONLY
                    // Focus arrows will be hidden so can always allow for Down Arrow shortcut 
                    rewiredInput.GetButtonDown(Const_KeyCodes.RWKeyboardDownArrow)
                    || (
                        rewiredInput.GetButtonDown(Const_KeyCodes.RWUISubmit)
                        // NOTE: KEYBOARD-ONLY
                        // Space should just give a null SFX, not navigate to Submit
                        && !rewiredInput.GetButtonDown(Const_KeyCodes.RWKeyboardSpace)
                    )
                )
                {
                    NavToSubmitShortcut();
                }
                else if (rewiredInput.GetButtonDown(Const_KeyCodes.RWUICancel))
                {
                    // On keyboard, when pressing esc and nav'ing to Submit with all text highlighted
                    // it will delete everything. Prevent this by refilling the text (at this point the text
                    // will be seen as empty).
                    TMPInputField.text = cachedText;
                    NavToSubmitShortcut();
                }
            }
        }
        
        // Backspace on Joystick should delete letter
        void HandleJoystickBackspace()
        {
            if (rewiredInput.GetButtonDown(Const_KeyCodes.RWBackspace))
            {
                // This will handle error SFX
                DeleteLetter();
            }
        }

        void HandleBackspaceDisabled()
        {
            if (rewiredInput.GetButtonDown(Const_KeyCodes.RWBackspace))
                inputManager.ErrorSFX();
        }

        // Move right/left with joystick
        void HandleCodeJoystickInputMove()
        {
            if (rewiredInput.GetButtonDown(Const_KeyCodes.RWHorizontal))
                MoveCursor(true);
            else if (rewiredInput.GetNegativeButtonDown(Const_KeyCodes.RWHorizontal))
                MoveCursor(false);
        }

        // Compare current length (which will be updated for the deletion) with the end of last frame's.
        // This is to mimic DeleteLetter's SFX for when using Keyboard.
        void HandleKeyboardBackspaceSFX()
        {
            if (rewiredInput.GetButtonDown(Const_KeyCodes.RWBackspace))
            {
                Dev_Logger.Debug($"HandleKeyboardBackspaceSFX() TMPInputField.text.Length {TMPInputField.text.Length} cachedText.Length {cachedText.Length}");
                
                if (TMPInputField.text.Length < cachedText.Length)
                    inputManager.BackspaceSFX();
                else if (cachedText.Length == 0)
                    inputManager.ErrorSFX();
            }
        }
    }

    public void NavToSubmitShortcut()
    {
        // Note: on text input and submit shortcut button same frame, the letter will still be
        // inputted along with nav'ing to submit
        EventSystem.current.sendNavigationEvents = false;
        TMPGUI.GetComponent<TMP_InputField>().DeactivateInputField();
        StartCoroutine(NavigateToSubmitAndWait());

        // Wait to prevent consuming the current Down press event
        IEnumerator NavigateToSubmitAndWait()
        {
            EventSystem.current.SetSelectedGameObject(submitButton);
            
            yield return new WaitForSecondsRealtime(WaitTimeAfterAutoNavigate);
            EventSystem.current.sendNavigationEvents = true;
        }
    }

    // Set active focus arrow set that is same index as current caret position
    private void HandleFocusArrows(
        ControllerType controllerType = ControllerType.Keyboard,
        bool isHideAll = false,
        Player rewiredInput = null
    )
    {
        // Don't use arrows when Keyboard was the last used controller
        if (isHideAll || controllerType == ControllerType.Keyboard)
        {
            focusArrowSets.ForEach(arrowSet => arrowSet.gameObject.SetActive(false));
            return;
        }
        
        // Show the arrow set on the caret position, hide the rest
        int caretPosition = Mathf.Clamp(TMPInputField.caretPosition, 0, TMPInputField.text.Length - 1);
        for (var i = 0; i < focusArrowSets.Count; i++)
            focusArrowSets[i].gameObject.SetActive(i == caretPosition);
        
        // Only take input on frame after OnSelect (isFocusArrowsDisabled is unset in LateUpdate), otherwise, when
        // navigating up to Entry Input, it will count as an Up Button Down event (and increment the code).
        if (!isFocusArrowsDisabled)
            HandleFocusArrowsInput();
        
        void HandleFocusArrowsInput()
        {
            if (rewiredInput.GetButtonDown(Const_KeyCodes.RWVertical))
            {
                ModifyHighlightedCodeValue(true);
                focusArrowSets[caretPosition].OnClickUpArrow();
            }
            else if (rewiredInput.GetNegativeButtonDown(Const_KeyCodes.RWVertical))
            {
                ModifyHighlightedCodeValue(false);
                focusArrowSets[caretPosition].OnClickDownArrow();
            }
        }
        
        void ModifyHighlightedCodeValue(bool isIncrement)
        {
            if (int.TryParse(TMPInputField.text[caretPosition].ToString(), out int highlightedValue))
            {
                if (isIncrement)
                    highlightedValue = (highlightedValue + 1) % 10;
                else
                {
                    if (highlightedValue <= 0)
                        highlightedValue = 10;

                    highlightedValue--;
                }
                
                string newCodeValue = highlightedValue.ToString();
                string oldCodeText = TMPInputField.text;
                TMPInputField.text = oldCodeText.Remove(caretPosition, 1).Insert(caretPosition, newCodeValue);

                inputManager.InsertCodeSFX();
            }
        }
    }

    private void MoveCursor(bool isRight)
    {
        int newCaretPositionRaw = isRight
            ? TMPInputField.caretPosition + 1
            : TMPInputField.caretPosition - 1;
        
        TMPInputField.caretPosition = Mathf.Clamp(newCaretPositionRaw, 0, TMPInputField.text.Length - 1);
        
        TMPInputField.ForceLabelUpdate();
    }
    
    public void OnSelect(BaseEventData e)
    {
        // If not highlighting whole selection, manually set the caret position.
        if (!isSelectAllOnFocus)
        {
            // Set cursor to end when we initialize with existing entry (this already happens on deselect).
            TMPInputField.caretPosition = caretPositionZeroAlways ? 0 : TMPInputField.text.Length;
            
            Dev_Logger.Debug($@"Setting caret position to: {TMPInputField?.caretPosition}
                with blinkrate {TMPInputField?.caretBlinkRate}");
        }

        SetCaretVisible(true);

        // If Code, ensure the frame we get selected we can't take input; otherwise, will lead to incrementing
        // when nav'ing Up from Submit
        if (entryValidator == Const_InputValidation.Validators.Code)
        {
            isFocusArrowsDisabled = true;
            Dev_Logger.Debug($"Entry Input (Code) On Select: isFocusArrowsDisabled {isFocusArrowsDisabled}");
        }
    }

    public void OnDeselect(BaseEventData e)
    {
        // Set for Name & Answer to avoid flashing caret on init
        if (!isCaretInitForLetterSelect)
            SetCaretVisible(false);
    }

    // Adds specified letter to the entry input. Caret will be moved to the end.
    public void AddLetter(string letter)
    {
        if (TMPInputField.text.Length >= Const_InputValidation.GetMaxCharCount(entryValidator))
        {
            inputManager.ErrorSFX();
            return;
        }
        
        TMPInputField.text = TMPInputField.text + letter;
        SetCaretDefaultPosition();
        inputManager.InsertSFX();
    }

    // Remove 1 letter from the entry input. Caret will be moved to the end.
    public void DeleteLetter()
    {
        if (TMPInputField.text.Length <= 0)
        {
            inputManager.ErrorSFX();
            return;
        }
        
        TMPInputField.text = TMPInputField.text.Remove(TMPInputField.text.Length - 1);
        SetCaretDefaultPosition();
        inputManager.BackspaceSFX();
    }

    // Handle setting caret visibility state when entering and leaving the Letter Select Grid.
    // E.g. when nav'ing to Submit, the caret should be off. On Letter Select Grid it should be visible.
    // On EntryInput, it should also be visible but TMP_InputField will handle it there.
    private void HandleCaretLetterSelectState()
    {
        // Ignore if not using Letter Select.
        // Also ignore if selection is EntryInput, since TMP_InputField will handle caret there.
        if (
            letterSelectGrid == null
            || (
                EventSystem.current != null
                && EventSystem.current.currentSelectedGameObject == TMPInputField.gameObject
            )
        )
        {
            isLetterSelectGridActive = false;
            return;
        }

        bool wasLetterSelectGridActive = isLetterSelectGridActive;
        isLetterSelectGridActive = letterSelectGrid.GetIsActive();
        
        if (isLetterSelectGridActive != wasLetterSelectGridActive)
            SetCaretVisible(isLetterSelectGridActive);
    }

    private void SetCaretVisible(bool isOn)
    {
        // As a precaution, if we're selecting the input field
        // ensure it's active. Sometimes the caret doesn't show.
        if (!TMPInputField.gameObject.activeInHierarchy && isOn)
        {
            Dev_Logger.Debug("Setting TMPInputField active before setting Caret properties");
            TMPInputField.gameObject.SetActive(true);
        }

        if (isOn)
        {
            TMPInputField.caretBlinkRate    = caretBlinkRate;
            TMPInputField.caretColor        = caretColor;

            Dev_Logger.Debug($"{name} Set caret on");
        }
        else
        {
            if (TMPInputField != null)
            {
                Color newCaretColor             = caretColor;
                newCaretColor.a                 = 0f;
                TMPInputField.caretColor        = newCaretColor;

                Dev_Logger.Debug($"{name} Set caret off");
            }
        }

        TMPInputField.ForceLabelUpdate();
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

    public void ActivateTMPInputField()
    {
        // Will be called on Slow Awake. We first set TMPInputField active to enable caret. Then, next frame
        // set desired Letter Select element active (e.g. the letter "A").
        if (IsLetterSelectState)
        {
            TMPInputField.enabled = true;
            TMPInputField.interactable = true;
            TMPInputField.ActivateInputField();
            SetCaretDefaultPosition();
            TMPInputField.ForceLabelUpdate();
            
            StartCoroutine(WaitToSetLetterSelectGrid());
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(TMPInputField.gameObject);
            TMPInputField.enabled = true;
            TMPInputField.interactable = true;
            TMPInputField.ActivateInputField();
        }

        // No Select SFX as long as time frame is before Slow Awake Event System wakes up
        IEnumerator WaitToSetLetterSelectGrid()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(letterSelectGrid.FirstSelected.gameObject);
            
            // Reenable hiding caret OnDeselect
            isCaretInitForLetterSelect = false;
        }
    }

    private void SetCaretDefaultPosition() => TMPInputField.caretPosition = caretPositionZeroAlways ? 0 : TMPInputField.text.Length;

    public void InitializeSlowAwakeTMPInputField()
    {
        TMPInputField.interactable = false;
        TMPInputField.enabled = false;
    }

    public void InitializeState(string text)
    {
        TMPInputField = GetComponent<TMP_InputField>();
        TMPInputField.text = text;
    }

    public void Setup()
    {
        
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_EntryInput))]
    public class Script_EntryInputTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_EntryInput t = (Script_EntryInput)target;
            
            if (GUILayout.Button("Activate TMP Input Field"))
            {
                t.ActivateTMPInputField();
            }

            if (GUILayout.Button("Add Letter i"))
            {
                t.AddLetter("i");
            }

            if (GUILayout.Button("Delete Letter"))
            {
                t.DeleteLetter();
            }

            if (GUILayout.Button("Set Caret Active"))
            {
                t.SetCaretVisible(true);
            }

            if (GUILayout.Button("Set Caret Inactive"))
            {
                t.SetCaretVisible(false);
            }

            if (GUILayout.Button("Move Cursor Right"))
            {
                t.MoveCursor(true);
            }

            if (GUILayout.Button("Move Cursor Left"))
            {
                t.MoveCursor(false);
            }
        }
    }
#endif
}