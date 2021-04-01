using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_TimelineController))]
[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_42 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    public bool didPickUpLastWellMap;
    public bool isMooseQuestDone;
    // ==================================================================
    
    public bool isCurrentMooseQuestComplete;

    [SerializeField] private Script_WellsPuzzleController wellsPuzzleController;
    
    [SerializeField] private Script_FrozenWell[] frozenWells;
    [SerializeField] private Script_DoorExitFireplace fireplaceExit;
    
    [SerializeField] private Script_CollectibleObject lastWellMap;
    
    [SerializeField] private Script_Item lastSpellRecipeBookItem;
    
    [SerializeField] private Script_DialogueNode OnHasLastSpellRecipeBookNode;
    [SerializeField] private Script_DialogueNode OnMissingLastSpellRecipeBookNode;
    [SerializeField] private Script_DialogueNode OnMooseGiveItemDoneNode;

    [SerializeField] private Script_DemonNPC Moose;
    [SerializeField] private Script_DemonNPC Suzette;

    [SerializeField] private Script_WeatherFXManager weatherFXManager;
    [SerializeField] private Script_Snow[] heavySnows;

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
            game.ChangeStateCutScene();

            // Play Freeze Timeline
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);

            StartHeavySnow();
        }
    }

    protected override void HandleAction()
    {
        base.HandleDialogueAction();
    }

    public void SetFireplaceExitActive(bool isActive)
    {
        fireplaceExit.SetInteractionActive(isActive);   
    }

    public void StartHeavySnow()
    {
        foreach (var heavySnow in heavySnows)
            heavySnow.gameObject.SetActive(true);
    }

    public void StopHeavySnow()
    {
        foreach (var heavySnow in heavySnows)
            heavySnow.gameObject.SetActive(false);
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

            game.RemoveItemFromInventory(lastSpellRecipeBookItem);
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
    
    // ----------------------------------------------------------------------
    // Timeline Signals
    public void FreezeWells()
    {
        // Play SFX Once here and disable for the Wells so all don't play at once.
        foreach (Script_FrozenWell frozenWell in frozenWells)
            frozenWell.Freeze(isSFXOn: false);

        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.Freeze, Script_SFXManager.SFX.FreezeVol
        );
    }

    public void EndFreezeCutScene()
    {
        game.ChangeStateInteract();
    }

    // ----------------------------------------------------------------------

    public override void Setup()
    {
        wellsPuzzleController.InitialState();

        // On Snow Day, the Fireplace Exit is open.
        if (weatherFXManager.IsSnowDay)     fireplaceExit.SetInteractionActive(true);
        else                                fireplaceExit.SetInteractionActive(false);

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

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_42))]
public class Script_LevelBehavior_42Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_42 t = (Script_LevelBehavior_42)target;
        if (GUILayout.Button("Activate Fireplace Exit"))
        {
            t.SetFireplaceExitActive(true);
        }

        if (GUILayout.Button("Disable Fireplace Exit"))
        {
            t.SetFireplaceExitActive(false);
        }

        if (GUILayout.Button("Start Heavy Snow"))
        {
            t.StartHeavySnow();
        }

        if (GUILayout.Button("Stop Heavy Snow"))
        {
            t.StopHeavySnow();
        }
    }
}
#endif