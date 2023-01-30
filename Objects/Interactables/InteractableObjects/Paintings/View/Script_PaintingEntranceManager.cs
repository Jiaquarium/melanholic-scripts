using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PaintingEntranceManager : MonoBehaviour
{
    public Script_DialogueChoice[] choices;
    public CanvasGroup paintingEntranceChoiceCanvas;
    public Script_DialogueNode disabledPaintingEntranceReactionNode;
    
    [SerializeField] private float beforeDisabledReactionWaitTime;
    [SerializeField] private Script_Game game;
    
    public bool DidTryDisabledEntrance { get; set; }
    public float BeforeDisabledReactionWaitTime => beforeDisabledReactionWaitTime;
    
    public void StartPaintingEntrancePromptMode()
    {
        // to get rid of flash at beginning
        foreach(Script_DialogueChoice choice in choices)
        {
            choice.cursor.enabled = false;
        }

        paintingEntranceChoiceCanvas.gameObject.SetActive(true);
    }

    public void InputChoice(int Id)
    {
        /// Use NextNodeAction() on "yes" node to handle PaintingEntrance
        EndPrompt();

        if (Id == 0)
        {
            Script_DialogueNode currentNode = Script_DialogueManager.DialogueManager.currentNode;
            Script_DialogueNode_PaintingEntrance paintingNode = (Script_DialogueNode_PaintingEntrance)currentNode;
            
            paintingNode.paintingEntrance.HandleExit();
            
            Script_DialogueManager.DialogueManager.HandleEndDialogue();
        }
        else
        {
            Script_DialogueManager.DialogueManager.NextDialogueNode(Id);
        }
    }

    // ------------------------------------------------------------------
    // Next Node Actions

    public void OnDisabledPaintingEntranceReactionDone()
    {
        Script_SFXManager.SFX.PlayTVChannelChangeStatic(game.ChangeStateInteract);
    }

    // ------------------------------------------------------------------
    // Unity Events

    // On Choice Cancel (Id: 1)
    public void PlayPaintingEntranceCancel()
    {
        Script_SFXManager.SFX.PlayPaintingEntranceCancel();
    }
    
    
    // ------------------------------------------------------------------

    private void EndPrompt()
    {
        paintingEntranceChoiceCanvas.gameObject.SetActive(false);
    }

    public void Setup()
    {
        paintingEntranceChoiceCanvas.gameObject.SetActive(false);
    }
}
