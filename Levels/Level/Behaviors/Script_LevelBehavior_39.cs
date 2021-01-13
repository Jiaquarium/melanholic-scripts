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
        Debug.Log("Triggering Flan convo");
        game.ChangeStateCutScene();
        Flan.TriggerDialogue();

        /// NOTE Flan's last dialogue node needs to call EndGuardDialogue()
    }

    public void OnEndGuardDialogue()
    {
        game.ChangeStateInteract();
    }

    // Unity Events END
    // ----------------------------------------------------------------------

    public override void Setup()
    {
    }        
}

#if UNITY_EDITOR
#endif