public class Script_SaveChoice : Script_UIChoice
{
    public Script_SaveViewManager saveManager;
    
    public override void HandleSelect()
    {
        saveManager.InputChoice(Id);
    }
}