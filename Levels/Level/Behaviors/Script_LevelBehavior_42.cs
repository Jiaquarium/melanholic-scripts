using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_42 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    public bool didPickUpLastWellMap;
    public bool isMooseQuestDone;
    // ==================================================================
    
    [SerializeField] private bool isCurrentMooseQuestComplete;
    [SerializeField] private Script_WellsPuzzleController wellsPuzzleController;
    [SerializeField] private Script_FrozenWell frozenWell;
    [SerializeField] private Script_CollectibleObject lastWellMap;
    
    [SerializeField] private Script_Item lastSpellRecipeBookItem;
    
    [SerializeField] private Script_DialogueNode OnHasLastSpellRecipeBookNode;
    [SerializeField] private Script_DialogueNode OnMissingLastSpellRecipeBookNode;
    [SerializeField] private Script_DialogueNode OnMooseGiveItemDoneNode;

    [SerializeField] private Script_DemonNPC Moose;
    [SerializeField] private Script_DemonNPC Suzette;

    protected override void OnEnable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess += OnPuzzleSuccess;
        Script_ItemsEventsManager.OnItemPickUp      += OnItemPickUp;
    }

    protected override void OnDisable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess -= OnPuzzleSuccess;
        Script_ItemsEventsManager.OnItemPickUp      -= OnItemPickUp;
    }

    private void OnItemPickUp(string itemId)
    {
        if (itemId == lastWellMap.Item.id)
        {
            didPickUpLastWellMap = true;
        }
    }

    private void OnPuzzleSuccess(string puzzleId)
    {
        if (puzzleId == wellsPuzzleController.PuzzleId)
        {
            Debug.Log("PUZZLE COMPLETED!!! FREEZE WELL!!!");

            frozenWell.Freeze();
        }
    }

    protected override void HandleAction()
    {
        base.HandleDialogueAction();
    }

    // ----------------------------------------------------------------------
    // Next Node Action START

    public void CheckPlayerHasLastSpellRecipeBook()
    {
        game.ChangeStateCutScene();
        
        int slot = -1;
        bool hasBook = game.GetInventoryItem(lastSpellRecipeBookItem.id, out slot) != null;

        if (hasBook)
        {
            Script_DialogueManager.DialogueManager.StartDialogueNodeNextFrame(OnHasLastSpellRecipeBookNode);
        }
        else
        {
            Script_DialogueManager.DialogueManager.StartDialogueNodeNextFrame(OnMissingLastSpellRecipeBookNode);
        }
    }

    public void GiveSticker()
    {
        Debug.Log("-------- MOOSE GIVES PLAYER AESTHETIC STICKER --------");
        isMooseQuestDone                = true;
        
        // Also track nonpersistent to dictate spawns on the same day.
        isCurrentMooseQuestComplete     = true;

        Script_DialogueManager.DialogueManager.StartDialogueNode(OnMooseGiveItemDoneNode);
    }

    public void UpdateMooseName()
    {
        Debug.Log("-------- UPDATING MOOSE NAME --------");
        Script_Names.UpdateMoose();
    }

    public void MooseExit()
    {
        Moose.gameObject.SetActive(false);
        game.ChangeStateInteract();
    }

    public void SuzetteExit()
    {
        Suzette.gameObject.SetActive(false);
        game.ChangeStateInteract();
    }

    public void OnMooseCheckItemDialogueDone()
    {
        game.ChangeStateInteract();
    }
    
    // Next Node Action END
    // ----------------------------------------------------------------------

    public override void Setup()
    {
        wellsPuzzleController.InitialState();

        // Only Spawn Last Well Map if Player has not picked it up.
        if (lastWellMap != null)
        {
            if (didPickUpLastWellMap)
                lastWellMap.gameObject.SetActive(false);
            else
                lastWellMap.gameObject.SetActive(true);
        }

        if (isCurrentMooseQuestComplete)
        {
            Moose.gameObject.SetActive(false);
            Suzette.gameObject.SetActive(false);
        }
        else
        {
            Moose.gameObject.SetActive(true);
            Suzette.gameObject.SetActive(true);
        }
    }
}