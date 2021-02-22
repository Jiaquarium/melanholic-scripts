using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_32 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool didStartThought;
    /* ======================================================================= */
    
    [SerializeField] private Script_DialogueNode startNode;
    [SerializeField] private Transform interactableObjectsParent;
    [SerializeField] private Script_BgThemePlayer dreamBgmPlayer;
    [SerializeField] private Script_InteractableObjectInput CCTVAdminComputer;
    private bool isInit = true;

    private void Start()
    {
        Debug.Log($"{name} didStartThought: {didStartThought}");
        
        if (!didStartThought)
        {
            Debug.Log($"**** {name} starting openeing cut scene ****");
            game.UnderDialogueBlackScreen();            
        }
    }

    public override void OnLevelInitComplete()
    {
        if (!didStartThought)
        {
            game.ChangeStateCutScene();
            Script_DialogueManager.DialogueManager.StartDialogueNode(startNode);
            didStartThought = true;
        }
    }

    public override int OnSubmit(string CCTVcodeInput) {
        // Check Cipher
        Debug.Log("Checking CIPHER!!!!!!!!");

        // Call Interactable Object
        
        return -1;
    }

    // ------------------------------------------------------------------
    // Next Node Action START
    public void OnEndStartDialogue()
    {
        dreamBgmPlayer.gameObject.SetActive(false);
        game.UnPauseBgMusic();
        
        /// Fade out black canvas
        game.UnderDialogueTransitionFadeOut(game.GetUnderDialogueFadeTime(), () => {
            /// Initial Save
            // game.SaveDefault();
            
            game.ChangeStateInteract();
        });
    }
    // Next Node Action END
    // ------------------------------------------------------------------

    // ------------------------------------------------------------------
    // InteractableObject UnityEvents START
    public void OnTryToExitFrontDoor()
    {
        Debug.Log("Move camera to hotel camera cut scene!!!");

        game.ChangeStateCutScene();
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
    }
    // InteractableObject UnityEvents END
    // ------------------------------------------------------------------
    
    // ------------------------------------------------------------------
    // Timeline Signals START
    public void OnHotelCameraPan()
    {
        game.GetPlayer().FaceDirection(Directions.Right);
    }

    public void OnHotelCameraPanDone()
    {
        game.ChangeStateInteract();
    }
    // Timeline Signals END
    // ------------------------------------------------------------------

    protected override void HandleAction()
    {
        // for cutScene dialogue
        base.HandleDialogueAction();
    }

    public override void Setup()
    {
        game.SetupInteractableObjectsText(interactableObjectsParent, isInit);

        if (!didStartThought)
        {
            game.PauseBgMusic();
            dreamBgmPlayer.gameObject.SetActive(true);
        }
        else
        {
            dreamBgmPlayer.gameObject.SetActive(false);
        }

        isInit = false;
    }        
}