using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// World Tiles References:
/// - Game
/// - World Tiles Controller
/// - LB46 Painting Entrances
/// </summary>

public class Script_LevelBehavior_43 : Script_LevelBehavior
{
    public const string MapName = Script_Names.CelestialGardensWorld;
    
    // ==================================================================
    // State Data

    public bool didIntro;
    
    // ==================================================================
    
    [SerializeField] private float waitBeforeIntroTime;
    [SerializeField] private Script_DialogueNode introNode;
    
    private bool didMapNotification;

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete    += OnLevelInitCompleteEvent;
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
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
            if (game.faceOffCounter != 1 || didIntro)
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

    // ----------------------------------------------------------------------
    // Next Node Action

    public void OnIntroDialogueDone()
    {
        game.ChangeStateInteract();
    }    
}