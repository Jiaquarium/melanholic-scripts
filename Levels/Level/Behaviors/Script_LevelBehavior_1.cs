using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_1 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    /* ======================================================================= */
    
    public bool isInit = true;
    public bool isDone = false;
    public Transform pianoTextParent;
    public Transform paintingTextParent;
    public Script_MovingNPC Ero;
    public Script_DialogueNode EroNode;
    public Script_DialogueManager dialogueManager;
    public Script_VCamera VCamLB1;
    [SerializeField] private PlayableDirector ErasDirector;
    

    public Script_BgThemePlayer EroBgThemePlayerPrefab;
    
    protected override void OnEnable() {
        ErasDirector.stopped += OnErasExitDone;
    }

    protected override void OnDisable() {
        ErasDirector.stopped -= OnErasExitDone;
    }
    
    public override bool ActivateTrigger(string Id){
        if (Id == "piano" && !isDone)
        {
            game.PauseBgMusic();
            game.PlayNPCBgTheme(EroBgThemePlayerPrefab);
            game.ChangeStateCutScene();
            StartCoroutine(TriggerAction());
            return true;
        }

        return false;
    }

    /// <summary>
    /// OnNextNodeAction handler START ================================================================================
    /// </summary>
    public void ExitCutScene()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
        
        isDone = true;
        game.DisableExits(false, 0);
    }

    public void SaveGame()
    {
        // Script_SaveGameControl.control.Save();
    }

    /// <summary>
    /// OnNextNodeAction handler END ================================================================================
    /// </summary>

    private void OnErasExitDone(PlayableDirector aDirector)
    {
        game.ChangeStateInteract();
        Script_VCamManager.VCamMain.SwitchToMainVCam(VCamLB1);
    }

    IEnumerator TriggerAction()
    {
        yield return new WaitForSeconds(Const_WaitTimes.OnTrigger);

        Script_VCamManager.VCamMain.SetNewVCam(VCamLB1);
        game.PlayerFaceDirection(Directions.Down);
        game.NPCFaceDirection(0, Directions.Up);
        game.ChangeCameraTargetToNPC(0);
        dialogueManager.StartDialogueNode(EroNode);
    }

    protected override void HandleAction()
    {
        base.HandleDialogueAction();
    }

    public override void Setup()
    {
        game.SetupInteractableObjectsText(pianoTextParent, isInit);
        game.SetupInteractableObjectsText(paintingTextParent, isInit);
        game.SetupMovingNPC(Ero, isInit);
        
        if (isInit && (!Debug.isDebugBuild || !Const_Dev.IsDevMode))
        {
            /// Save to skip the whole woods part in case Player has died
            // Script_SaveGameControl.control.Save();
        }

        if (isDone)
        {
            game.DisableExits(false, 0);
            Ero.gameObject.SetActive(false);
        }
        // else
        // {
        //     game.DisableExits(true, 0);
        // }
        
        isInit = false;
    }
}
