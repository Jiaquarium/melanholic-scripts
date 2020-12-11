using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_32 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool didStartThought;
    /* ======================================================================= */
    
    [SerializeField] private Script_DialogueNode startNode;

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

    protected override void HandleAction()
    {
        // for cutScene dialogue
        base.HandleDialogueAction();
    }

    public override void Setup()
    {
        
    }        
}