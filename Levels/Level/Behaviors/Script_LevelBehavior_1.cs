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
    
    public Script_DemonNPC Ero;

    public Script_DialogueNode[] IdsSickEroNodes;
    public Script_DialogueNode EroNode;
    
    public Script_DialogueManager dialogueManager;
    public Script_VCamera VCamLB1;
    
    [SerializeField] private PlayableDirector ErosDirector;

    private bool didIdsMusicCutScene;
    

    public Script_BgThemePlayer EroBgThemePlayerPrefab;
    
    protected override void OnEnable() {
        ErosDirector.stopped                            += OnEroExitDone;
        Script_GameEventsManager.OnLevelInitComplete    += OnLevelInit;
    }

    protected override void OnDisable() {
        ErosDirector.stopped                            -= OnEroExitDone;
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInit;
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
        Script_BackgroundMusicManager.Control.FadeOutFast(() => {
            Script_BackgroundMusicManager.Control.Pause();
        });
    }

    public void FadeInMusic()
    {
        Script_BackgroundMusicManager.Control.FadeInFast(null);
    }

    public void OnIdsMusicTimelineDone()
    {
        Script_BackgroundMusicManager.Control.UnPause();
        Script_BackgroundMusicManager.Control.FadeInFast(null);
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

    private void OnEroExitDone(PlayableDirector aDirector)
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
        dialogueManager.StartDialogueNode(EroNode);
    }

    public override void Setup()
    {
        // Weekend Cycle Conditions
        if (game.RunCycle == Script_RunsManager.Cycle.Weekend)
        {
            if (Script_EventCycleManager.Control.IsIdsDead())
            {
                Ero.gameObject.SetActive(false);
            }
            else if (Script_EventCycleManager.Control.IsIdsInSanctuary())
            {
                Ero.SwitchPsychicNodes(IdsSickEroNodes);
            }    
        }
        
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