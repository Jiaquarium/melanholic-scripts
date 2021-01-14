using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_39 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */

    /* ======================================================================= */
    
    [SerializeField] private Script_DemonNPC Flan;
    [SerializeField] private Script_Interactable barrier;
    [SerializeField] private Script_Trigger trigger;

    private bool didGuardConfirm;


    // Need for Guard OnTrigger Cut Scene
    protected override void HandleAction()
    {
        base.HandleDialogueAction();
    }

    // ----------------------------------------------------------------------
    // Unity Events START
    
    // called from Trigger in front of Guard
    public void StartGuardDialogue()
    {
        bool isPsychicDuckActive = Script_ActiveStickerManager.Control.GetSticker()?.id == Const_Items.PsychicDuckId;
        
        /// If already spoken with guard by either interacting with or activating trigger
        /// then don't activate trigger anymore and let Vx pass automatically 
        if (isPsychicDuckActive && didGuardConfirm)
        {
            return;
        }
        
        Debug.Log("Triggering Flan convo");
        game.ChangeStateCutScene();
        Flan.TriggerDialogue();

        /// NOTE Flan's last dialogue node needs to call EndGuardDialogue()
    }

    public void OnEndGuardDialogue()
    {
        /// Reinstate barrier
        barrier.gameObject.SetActive(true);
        
        game.ChangeStateInteract();

        // must set back to nonconfirm state to force Guard to speak up next time
        // you walk past trigger with Psychic Duck (applicable if get confirmed 
        // once but come back without the sticker attached)
        didGuardConfirm = false;
    }

    public void OnEndGuardPsychicDialogue()
    {
        /// Remove barrier
        barrier.gameObject.SetActive(false);
        
        game.ChangeStateInteract();

        didGuardConfirm = true;
    }

    // Unity Events END
    // ----------------------------------------------------------------------

    public override void Setup()
    {
    }        
}

#if UNITY_EDITOR
#endif