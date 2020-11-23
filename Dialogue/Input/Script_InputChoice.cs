using TMPro;

public class Script_InputChoice : Script_UIChoice
{
    public Script_InputManager inputManager;
    public TMP_InputField inputField;

    /// <summary>
    /// called from OnClick
    /// </summary>
    public override void HandleSelect()
    {
        inputManager.InputSaveEntryChoice(Id, inputField.text);
    }
}
