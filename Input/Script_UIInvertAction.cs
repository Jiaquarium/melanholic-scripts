using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class Script_UIInvertAction : MonoBehaviour
{
    private enum States
    {
        None = 0,
        Edit = 1,
    }

    private const string TMPIdYes = "controls_invert_option_0";
    private const string TMPIdNo = "controls_invert_option_1";
    private const string TMPIdNoText = "controls_no-text";
    
    [Header("Rewired Settings")]
    [SerializeField] private RWActions myRWAction;

    [Header("My Settings")]
    [SerializeField] private States state;

    [SerializeField] private List<Animator> controllerSelectArrows;
    [SerializeField] private Image editHighlight;
    [SerializeField] private Script_TMProPopulator invertTMPText;
    [SerializeField] private Button button;
    [SerializeField] private Script_SettingsController settingsController;

    public int ActionId => myRWAction.GetRWActionName().RWActionNamesToId();
    public Player MyPlayer => settingsController.MyPlayer;
    public string MyActionName => myRWAction.GetRWActionName();
    public Button MyButton => button;

    private bool isInvert;
    private bool isInputDisabled;
    
    void OnEnable()
    {
        InitialState();
    }
    
    // Call from SettingsController. Call in Update when this object is the current selected by Event System.
    public void HandleCurrentSelected()
    {
        if (!gameObject.activeInHierarchy || isInputDisabled)
            return;
        
        if (
            state == States.None
            && MyPlayer.GetButtonDown(Const_KeyCodes.RWUISubmit)
        )
        {
            // When entering Edit mode, the call to SetNavigationEnabled will prevent any Back nav from UICancel listeners
            state = States.Edit;
            settingsController.SetNavigationEnabled(false);
            
            EnterSFX();
            EditStateGraphics(true);
        }
        else if (
            state == States.Edit
            && (
                MyPlayer.GetButtonDown(Const_KeyCodes.RWUICancel)
                // Allow moving up/down to escape as well
                || MyPlayer.GetButtonDown(Const_KeyCodes.RWVertical)
                || MyPlayer.GetNegativeButtonDown(Const_KeyCodes.RWVertical)
            )
        )
        {
            isInputDisabled = true;
            ExitSFX();
            EditStateGraphics(false);
            state = States.None;

            StartCoroutine(WaitToResetState());
        }

        switch (state)
        {
            case States.Edit:
                HandleEditInput();
                break;
            default:
                break;
        }

        IEnumerator WaitToResetState()
        {
            yield return new WaitForSecondsRealtime(Script_SettingsController.WaitBeforeListeningTime);

            settingsController.SetNavigationEnabled(true);
            isInputDisabled = false;
        }
    }

    public void UpdateBehaviorUIText()
    {
        ControllerMap map = settingsController.MyControllerMap;

        if (map == null)
            return;

        ActionElementMap firstAem = map.GetFirstActionElementMapByMap(ActionId);
        
        if (firstAem == null)
        {
            invertTMPText.UpdateTextId(TMPIdNoText);
            isInvert = false;
        }
        else if (firstAem.invert)
        {
            invertTMPText.UpdateTextId(TMPIdYes);
            isInvert = true;
        }
        else
        {
            invertTMPText.UpdateTextId(TMPIdNo);
            isInvert = false;
        }
    }

    private void HandleEditInput()
    {
        if (MyPlayer.GetNegativeButtonDown(Const_KeyCodes.RWHorizontal))
        {
            controllerSelectArrows[0].SetTrigger(Script_SettingsController.ClickTrigger);
            ToggleInversion();
        }
        else if (MyPlayer.GetButtonDown(Const_KeyCodes.RWHorizontal))
        {
            controllerSelectArrows[1].SetTrigger(Script_SettingsController.ClickTrigger);
            ToggleInversion();
        }
    }

    private void ToggleInversion()
    {
        Dev_Logger.Debug($"Toggling Inversion to {!isInvert}");

        isInputDisabled = true;
        EditSFX();
        Script_PlayerInputManager.Instance.SetKeybindMappingInversion(
            ActionId, settingsController.MyController, !isInvert
        );
        Script_SaveSettingsControl.Instance.Save();
        UpdateBehaviorUIText();

        StartCoroutine(WaitToToggle());

        IEnumerator WaitToToggle()
        {
            yield return new WaitForSecondsRealtime(Script_SettingsController.WaitBeforeListeningTime);
            isInputDisabled = false;
        }
    }

    /// <summary>
    /// Handle isFocused graphics, highlights, etc.
    /// </summary>
    private void EditStateGraphics(bool isActive)
    {
        controllerSelectArrows.ForEach(arrow => arrow.gameObject.SetActive(isActive));
        editHighlight.gameObject.SetActive(isActive);
    }

    private void EditSFX() => Script_SFXManager.SFX.PlayUISuccessEdit();
    
    private void EnterSFX() => Script_SFXManager.SFX.PlayEnterSubmenu();

    private void ExitSFX() => Script_SFXManager.SFX.PlayExitSubmenuPencil();

    private void ErrorSFX() => Script_SFXManager.SFX.PlayDullError();

    private void InitialState()
    {
        state = States.None;
        EditStateGraphics(false);
    }
}
