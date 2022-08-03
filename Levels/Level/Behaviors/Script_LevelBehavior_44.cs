using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_44 : Script_LevelBehavior
{
    public const string MapName = Script_Names.XXXWorld;
    
    // ==================================================================
    // State Data

    public bool didIntro;
    
    // ==================================================================

    [SerializeField] private Script_InteractablePaintingEntrance[] paintingEntrances;
    [SerializeField] private Script_InteractablePaintingEntrance ballroomPaintingEntrance;

    [SerializeField] private Script_ScarletCipherPiece[] scarletCipherPieces;

    [SerializeField] private float waitBeforeIntroTime;
    [SerializeField] private Script_DialogueNode introNode;

    private bool didMapNotification;

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete                    += OnLevelInitCompleteEvent;
        Script_ScarletCipherEventsManager.OnScarletCipherPiecePickUp    += OnScarletCipherPickUp;
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete                    -= OnLevelInitCompleteEvent;
        Script_ScarletCipherEventsManager.OnScarletCipherPiecePickUp    -= OnScarletCipherPickUp;
    }

    private void OnLevelInitCompleteEvent()
    {
        if (!didMapNotification)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName, HandleIntroDialogue);
            didMapNotification = true;
        }
        else
        {
            HandleIntroDialogue();
        }

        void HandleIntroDialogue()
        {
            if (game.faceOffCounter != 2 || didIntro)
                return;

            game.ChangeStateCutScene();

            StartCoroutine(WaitToIntroDialogue());

            didIntro = true;

            IEnumerator WaitToIntroDialogue()
            {
                yield return new WaitForSeconds(waitBeforeIntroTime);

                Script_DialogueManager.DialogueManager.StartDialogueNode(introNode);            
            }        
        }        
    }

    // Hide all Scarlet Cipher pieces when any is picked up on a World Tile.
    private void OnScarletCipherPickUp(int scarletCipherId)
    {
        if (scarletCipherId == scarletCipherPieces[0].ScarletCipherId)
        {
            foreach (var scarletCipherPiece in scarletCipherPieces)
            {
                scarletCipherPiece.UpdateActiveState();
            }
        }
    }
    
    // ------------------------------------------------------------------
    // Timeline Signals
    
    public void FinishQuestPaintings()
    {
        foreach (var painting in paintingEntrances)
        {
            painting.DonePainting();
        }

        ballroomPaintingEntrance.DonePainting();
    }
    
    // ------------------------------------------------------------------
    // Next Node Actions

    public void OnIntroDialogueDone()
    {
        game.ChangeStateInteract();
    }

    // ------------------------------------------------------------------
    
    public override void Setup()
    {
        base.Setup();
    }
}