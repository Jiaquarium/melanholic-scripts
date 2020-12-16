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
    private bool isInit = true;

    private void Awake()
    {
        if (!didStartThought)
        {
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

    /// NextNodeAction START ===============================================================
    public void OnEndStartDialogue()
    {
        /// Fade out black canvas
        game.UnderDialogueTransitionFadeOut(game.GetUnderDialogueFadeTime(), () => {
            /// Initial Save
            
            game.ChangeStateInteract();
        });
    }
    /// NextNodeAction END =================================================================
    /// InteractableObject UnityEvents START ===============================================
    
    public void OnTryToExitFrontDoor()
    {
        Debug.Log("Move camera to hotel camera cut scene!!!");

        game.ChangeStateCutScene();
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
    }
    /// InteractableObject UnityEvents END =================================================
    /// Timeline Signals START =============================================================
    public void OnHotelCameraPan()
    {
        game.GetPlayer().FaceDirection(Directions.Right);
    }

    public void OnHotelCameraPanDone()
    {
        game.ChangeStateInteract();
    }
    /// Timeline Signals END ===============================================================

    protected override void HandleAction()
    {
        // for cutScene dialogue
        base.HandleDialogueAction();
    }

    public override void Setup()
    {
        game.SetupInteractableObjectsText(interactableObjectsParent, isInit);

        isInit = false;      
    }        
}