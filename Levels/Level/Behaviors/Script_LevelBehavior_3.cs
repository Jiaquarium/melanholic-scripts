using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_3 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isDone = false;
    public bool isActivated = false;
    public int activeTriggerIndex = 0;
    public bool[] demonSpawns;
    /* ======================================================================= */
    
    
    public Transform demonsParent;
    public Script_MovingNPC Ero;
    public Script_DialogueNode[] triggerNodes;
    [SerializeField] private Script_DialogueNode onPRCSTimelineDoneNode;
    public Script_DialogueManager dm;
    [SerializeField] private PlayableDirector ErasDirector;
    [SerializeField] private Script_VCamera VCamLB3;
    [SerializeField] private Script_PRCSPlayer IntroPlayerPart1;
    [SerializeField] private Script_PRCSPlayer IntroPlayerPart2;

    /// =======================================================================
    /// Ero Intro START
    /// =======================================================================
    public Transform EroIntroParent;
    public Transform EroIntroTriggersParent;
    /// Ero Intro END
    /// =======================================================================

    public Script_BgThemePlayer EroBgThemePlayerPrefab;
    
    protected override void OnEnable()
    {
        if (game.Run.dayId == Script_Run.DayId.none)
        {
            ErasDirector.stopped += OnErasMovesDone;
            Script_PRCSEventsManager.OnPRCSDone += PRCSDoneReaction;
        }
    }

    protected override void OnDisable()
    {
        if (game.Run.dayId == Script_Run.DayId.none)
        {
            ErasDirector.stopped -= OnErasMovesDone;
            Script_PRCSEventsManager.OnPRCSDone -= PRCSDoneReaction;
        }
    }
    
    public override void Cleanup() {
        if (game.Run.dayId == Script_Run.DayId.none)
        {
            if (isDone)     game.DestroyNPCs();
        }
    }
    
    public override bool ActivateTrigger(string Id)
    {
        if (game.Run.dayId == Script_Run.DayId.none)
        {
            if (
                (
                    (Id == "room_1" && activeTriggerIndex == 0 )
                    || (Id == "room_2" && activeTriggerIndex == 1)
                ) && !isDone
            )
            {
                game.ChangeStateCutScene();
                
                if (activeTriggerIndex == 0)
                {
                    game.PauseBgMusic();
                    game.PlayNPCBgTheme(EroBgThemePlayerPrefab);
                    game.PlayerFaceDirection(Directions.Down);
                    Ero.GetComponent<Script_MovingNPCFaceDirectionController>().FacePlayer();
                }
                else if (activeTriggerIndex == 1)
                {
                    game.PlayerFaceDirection(Directions.Right);
                    Ero.GetComponent<Script_MovingNPCFaceDirectionController>().FacePlayer();
                    Script_VCamManager.VCamMain.SetNewVCam(VCamLB3);
                }
                
                dm.StartDialogueNode(triggerNodes[activeTriggerIndex]);
                
                activeTriggerIndex++;
                if (activeTriggerIndex > 1)     isDone = true;

                return true;
            }
        }

        return false;
    }    
    

    /// <summary>
    /// Set Demon states
    /// </summary>
    public override void EatDemon(int Id) {
        demonSpawns[Id] = false;
    }

    /// <summary> =============================================================
    /// NextNodeAction(s) START
    /// </summary> ============================================================
    /// Called from NextNodeAction after moving cut scene finishes
    public void ErasMoveToGuardExitCutScene()
    {
        game.ChangeStateCutScene();
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
    }
    /// <summary>
    /// (1) Eras finishes farewell dialogue 
    /// </summary>
    public void DefaultCamera()
    {
        Script_VCamManager.VCamMain.SwitchToMainVCam(VCamLB3);
        game.ChangeStateInteract();
    }
    public void PRCSKelsingorIntro()
    {
        game.ChangeStateCutScene();
        IntroPlayerPart1.Play();
    }
    // Called from onPRCSTimelineDoneNode NextNodeAction
    // after part2 timeline is done (scrolling to Magic Circle) 
    public void OnPart2NodeDone()
    {
        // Remove PRCS
        IntroPlayerPart2.Stop();
    }
    /// NextNodeAction(s) END
    /// ===========================================================================
    
    private void PRCSDoneReaction(Script_PRCSPlayer PRCSPlayer)
    {
        Debug.Log("PRCS is Done");
        // GetComponent<Script_PRCSPlayer>().Stop(() => OnDone());
        if (PRCSPlayer == IntroPlayerPart1)
        {
            Debug.Log("PART 1 PRCS DONE!!!");

            // Play part 2
            IntroPlayerPart2.Play();
        }
        else if (PRCSPlayer == IntroPlayerPart2)
        {
            Debug.Log("PART 2 PRCS DONE!!!");

            dm.StartDialogueNode(onPRCSTimelineDoneNode, SFXOn: false);
        }
    }

    private void OnErasMovesDone(PlayableDirector aDirector)
    {
        Ero.FaceDirection(Directions.Down);
        DefaultCamera();
        game.ChangeStateInteract();
    }

    public override void Setup()
    {
        /// <summary> 
        /// Demon Handlers: initialize state
        /// </summary> 
        if (!isActivated)
        {
            demonSpawns = new bool[demonsParent.childCount];
            for (int i = 0; i < demonSpawns.Length; i++)
                demonSpawns[i] = true;
        }
        
        game.SetupDemons(demonsParent, demonSpawns);
        
        if (game.Run.dayId == Script_Run.DayId.none)
        {
            game.SetupMovingNPC(Ero, !isActivated);
            
            EroIntroParent.gameObject.SetActive(true);
            EroIntroTriggersParent.gameObject.SetActive(true);    
        }
        else
        {
            EroIntroParent.gameObject.SetActive(false);
            EroIntroTriggersParent.gameObject.SetActive(false);    
        }
        
        isActivated = true;
    }
}
