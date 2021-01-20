using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    private bool didIdsMusicCutScene;
    

    public Script_BgThemePlayer EroBgThemePlayerPrefab;
    
    protected override void OnEnable() {
        ErasDirector.stopped += OnErasExitDone;
        Script_GameEventsManager.OnLevelInitComplete += OnLevelInit;
    }

    protected override void OnDisable() {
        ErasDirector.stopped -= OnErasExitDone;
        Script_GameEventsManager.OnLevelInitComplete -= OnLevelInit;
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

    protected override void HandleAction()
    {
        base.HandleDialogueAction();
    }

    private void OnLevelInit()
    {
        if (game.IsRunDay(Script_Run.DayId.sun) && !didIdsMusicCutScene)
        {
            IdsMusicCutScene();
        }
    }

    /// OnNextNodeAction handler START ================================================================================
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

    /// OnNextNodeAction handler END ================================================================================
    // ------------------------------------------------------------------------------------
    // Timeline Signals Start
    public void FadeOutMusic()
    {
        Script_BackgroundMusicManager.Control.FadeOutMasterFast(() => {
            Script_BackgroundMusicManager.Control.Pause();
        });
    }

    public void FadeInMusic()
    {
        Script_BackgroundMusicManager.Control.FadeInMasterFast(null);
    }

    public void OnIdsMusicTimelineDone()
    {
        Script_BackgroundMusicManager.Control.UnPause();
        Script_BackgroundMusicManager.Control.FadeInMasterFast(null);
        game.ChangeStateInteract();
    }
    // Timeline Signals End
    // ------------------------------------------------------------------------------------

    public void IdsMusicCutScene()
    {
        game.ChangeStateCutScene();
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(1, 1);

        didIdsMusicCutScene = true;
    }

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

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_1))]
public class Script_LevelBehavior_1Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_1 t = (Script_LevelBehavior_1)target;
        if (GUILayout.Button("IdsMusicCutScene()"))
        {
            t.IdsMusicCutScene();
        }
        if (GUILayout.Button("FadeOutMusic()"))
        {
            t.FadeOutMusic();
        }
        if (GUILayout.Button("FadeInMusic()"))
        {
            t.FadeInMusic();
        }
    }
}
#endif