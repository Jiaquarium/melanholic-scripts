using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    
    // ------------------------------------------------------------------
    // Dev Only TBD DELETE
    public string DEVELOPMENT_CCTVCodeInput;

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
        bool isSuccessfulSubmit = CheckCCTVCode(CCTVcodeInput);
        
        Debug.Log($"------------ Result: {isSuccessfulSubmit} ------------");

        // Call Interactable Object
        if (isSuccessfulSubmit)
        {
            CCTVAdminComputer.OnSubmitSuccess();
        }
        else
        {
            CCTVAdminComputer.OnSubmitFailure();
        }

        return -1;
    }

    public bool CheckCCTVCode(string CCTVcodeInput)
    {
        return Script_ScarletCipherManager.Control.CheckCCTVCode(CCTVcodeInput);
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

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_32))]
public class Script_LevelBehavior_32Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_32 t = (Script_LevelBehavior_32)target;
        if (GUILayout.Button("Check CCTV Code"))
        {
            bool result = t.CheckCCTVCode(t.DEVELOPMENT_CCTVCodeInput);

            Debug.Log($"------------ Result: {result} ------------");
        }
    }
}
#endif