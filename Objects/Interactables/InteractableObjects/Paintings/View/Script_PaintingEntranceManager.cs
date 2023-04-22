using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PaintingEntranceManager : MonoBehaviour
{
    public Script_DialogueChoice[] choices;
    public CanvasGroup paintingEntranceChoiceCanvas;
    public Script_DialogueNode disabledPaintingEntranceReactionNode;
    
    [SerializeField] private float beforeDisabledReactionWaitTime;
    
    [SerializeField] private Script_PaintingEntranceController paintingEntranceController;
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
        
        // Set controller ready, but it should still be inactive here / not detecting input
        paintingEntranceController.IsReady = true;
    }

    public void InputChoice(int Id)
    {
        // Prevent multiple calls if esc and UI select press are on same frame
        if (!paintingEntranceController.IsReady)
            return;
        
        paintingEntranceController.IsReady = false;
        paintingEntranceController.gameObject.SetActive(false);
        
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
            PlayPaintingEntranceCancelSFX();
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
    
    public void SetPaintingEntranceInputManagerActive()
    {
        paintingEntranceController.gameObject.SetActive(true);
    }
    
    // ------------------------------------------------------------------

    private void PlayPaintingEntranceCancelSFX()
    {
        Script_SFXManager.SFX.PlayPaintingEntranceCancel();
    }
    
    private void EndPrompt()
    {
        paintingEntranceChoiceCanvas.gameObject.SetActive(false);
    }

    public void Setup()
    {
        paintingEntranceChoiceCanvas.gameObject.SetActive(false);
        paintingEntranceController.gameObject.SetActive(false);
        paintingEntranceController.IsReady = false;
    }
}
