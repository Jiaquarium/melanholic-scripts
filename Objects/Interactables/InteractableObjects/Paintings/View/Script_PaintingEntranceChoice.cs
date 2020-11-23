public class Script_PaintingEntranceChoice : Script_UIChoice
{
    public override void HandleSelect()
    {
        Script_DialogueManager.DialogueManager
            .paintingEntranceManager.InputChoice(Id);
    }
}
