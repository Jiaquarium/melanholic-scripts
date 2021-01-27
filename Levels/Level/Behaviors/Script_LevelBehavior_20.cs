using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Add Cinemachine track to Timeline to control VCams via Timeline
/// Timeline: Use a "controller" PlayableDirector if controlling multiple things
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_20 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public Seasons season;
    public bool isPuzzleComplete;
    public bool entranceCutSceneDone;
    public bool didPickUpMasterKey;
    public bool didUnlockMasterLock;
    /* ======================================================================= */
    
    public Script_Pushable rock;
    [SerializeField] Script_SeasonsTree[] ManTrees;
    [SerializeField] Script_SeasonsTree[] WomanTrees;
    [SerializeField] private Script_VCamera vCamMid;
    [SerializeField] private Script_VCamera vCamEntrance;
    [SerializeField] private float waitToChangeCameraTime; // 1s
    [SerializeField] private float waitToChangeSeasonTime; // 1.5s camera + .5s pause
    [SerializeField] private float seasonChangeWaitTime; //  1.25s slow season fade + .75s pause
    [SerializeField] private float revertCameraWaitTime; //  1.5s
    [SerializeField] private float successPostChangeSeasonWaitTime;
    [SerializeField] private GameObject SuccessExplosion;
    [SerializeField] private Script_SeasonStonesPuzzleController puzzleController;
    [SerializeField] private PlayableDirector SuccessDirector;
    [SerializeField] private Script_UsableObject masterKey;
    [SerializeField] private Script_UsableTarget masterLock;
    [SerializeField] private Script_TileMapExitEntrance lastExit;
    [SerializeField] private Transform transmutationCircle;
    [SerializeField] private Script_SavePoint savePoint;
    [SerializeField] private Script_MovingNPC Kaffe;
    [SerializeField] private Script_MovingNPC Latte;
    [SerializeField] private Script_MovingNPC Melz;
    [SerializeField] private Script_MovingNPC Ids;
    [SerializeField] private PlayableDirector IntroDirector;
    [SerializeField] private PlayableDirector IntroExitDirector;
    [SerializeField] private PlayableDirector AftermathDirector;
    [SerializeField] private Script_DialogueNode MelzNode;
    [SerializeField] private Script_DialogueNode PlayerReactionNode;
    [SerializeField] private Script_DialogueNode onUrselksEmbraceNode;
    [SerializeField] private Script_DialogueNode IdsNode;
    [SerializeField] private Script_DialogueNode KaffeLatteNode;
    [SerializeField] private Script_PRCSPlayer climaxPlayer;
    [SerializeField] private Script_PRCS embracePRCS;
    [SerializeField] private Transform playerMoveDestination;
    [SerializeField] private float EmbraceDialogueWaitTime;
    [SerializeField] private float IdsDialogueWaitTime;
    [SerializeField] private float KaffeLatteDialogueWaitTime;
    [SerializeField] private Script_BgThemePlayer bellsBgPlayer;
    [SerializeField] private float shakeDuration;

    /// =======================================================================
    /// Melz Intro START
    /// =======================================================================
    public Transform MelzIntroMelzParent;
    /// Melz Intro END
    /// =======================================================================

    private bool isInitialCutScene;
    private bool isInit = true;
    
    /// DEV ONLY
    public Transform rockDestination;
    
    public List<Script_DestroyTriggerCollectibles> destroyTriggerCollectibles;
    public List<GameObject> buffs;
    private float fadeTime;


    private Dictionary<string, Seasons> SeasonStonesEnums = new Dictionary<string, Seasons>{
        {"collectible_winter-stone",    Seasons.Winter},
        {"collectible_spring-stone",    Seasons.Spring},
        {"collectible_summer-stone",    Seasons.Summer},
        {"collectible_autumn-stone",    Seasons.Autumn},
    };

    private Dictionary<Seasons, int> SeasonInt = new Dictionary<Seasons, int>{
        {Seasons.Winter,            0},
        {Seasons.Spring,            1},
        {Seasons.Summer,            2},
        {Seasons.Autumn,            3},
    };

    private void Awake()
    {
        foreach(Script_SeasonsTree tree in ManTrees)    tree.Setup(season);
        foreach(Script_SeasonsTree tree in WomanTrees)  tree.Setup(season);   
        foreach(Script_DestroyTriggerCollectibles trig in destroyTriggerCollectibles)
                                                        trig.gameObject.SetActive(false);
        foreach(GameObject buff in buffs)               buff.SetActive(false);

        /// Melz Intro Run
        if (game.Run.dayId == Script_Run.DayId.none)
        {
            if (!entranceCutSceneDone)
            {
                EntranceCutScene();
            }
        }        

        void EntranceCutScene()
        {
            Kaffe.gameObject.SetActive(false);
            Latte.gameObject.SetActive(false);
            Melz.gameObject.SetActive(false);
            
            game.ChangeStateCutScene();
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(1, 1);
            /// Set VCam through code as when Timeline ends we need control to remain
            /// on VCam focusing on Melz and the Lovers 
            Script_VCamManager.VCamMain.SetNewVCam(vCamEntrance);
            
            entranceCutSceneDone = true;
            isInitialCutScene = true; // Set this to change to cut scene
        }
    }
    
    protected override void OnEnable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess     += OnPuzzleSuccess;
        Script_PuzzlesEventsManager.OnPuzzleProgress    += OnPuzzleProgress;
        Script_ItemsEventsManager.OnItemPickUp          += OnItemPickUp;
        Script_ItemsEventsManager.OnUnlock              += OnUnlockMasterLock;
        Script_PRCSEventsManager.OnPRCSDone             += PRCSClimaxDoneReaction;
        SuccessDirector.stopped                         += OnSuccessPlayableDone;
        AftermathDirector.stopped                       += OnAftermathPlayableDone; 
        
        /// Melz Intro Run
        if (game.Run.dayId == Script_Run.DayId.none)
        {
            IntroDirector.stopped                           += OnIntroPlayableDone;
            IntroExitDirector.stopped                       += OnIntroExitPlayableDone;
        }
    }

    protected override void OnDisable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess     -= OnPuzzleSuccess;
        Script_PuzzlesEventsManager.OnPuzzleProgress    -= OnPuzzleProgress;
        Script_ItemsEventsManager.OnItemPickUp          -= OnItemPickUp;
        Script_ItemsEventsManager.OnUnlock              -= OnUnlockMasterLock;
        Script_PRCSEventsManager.OnPRCSDone             -= PRCSClimaxDoneReaction;
        SuccessDirector.stopped                         -= OnSuccessPlayableDone;
        AftermathDirector.stopped                       -= OnAftermathPlayableDone; 
        
        /// Melz Intro Run
        if (game.Run.dayId == Script_Run.DayId.none)
        {
            IntroDirector.stopped                           -= OnIntroPlayableDone;
            IntroExitDirector.stopped                       -= OnIntroExitPlayableDone;
        }
    }

    protected override void Update()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if(audioSource.isPlaying && audioSource.time > fadeTime)
        {
            audioSource.volume = 0f;
        }

        base.Update();
    }

    public override void OnLevelInitComplete()
    {
        OnMelzIntroCutScene();
    }

    protected override void HandleAction()
    {
        base.HandleDialogueAction();
    }

    private void OnUnlockMasterLock(Script_Usable key, string targetId)
    {
        if (targetId == masterLock.Id)
        {
            Debug.Log($"OnUnlock Event caught with key, targetId: {key}, {targetId}");
            didUnlockMasterLock = true;
        }
    }

    private void OnItemPickUp(string itemId)
    {
        if (itemId == masterKey.Item.id)
        {
            didPickUpMasterKey = true;
        }
    }
    
    private void OnPuzzleProgress()
    {
        game.ChangeStateCutScene();
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.SeasonsPuzzleProgress,
            Script_SFXManager.SFX.SeasonsPuzzleProgressVol
        );
        Script_VCamManager.VCamMain.GetComponent<Script_CameraShake>().Shake(
            shakeDuration,
            Const_Camera.Shake.AmplitudeMed,
            Const_Camera.Shake.FrequencyMed,
            () => game.ChangeStateInteract()
        );           
    }

    private void OnPuzzleSuccess(string Id)
    {
        if (!isPuzzleComplete)
        {
            game.PauseBgMusic();
            
            Debug.Log("trigger Id was: " + Id);

            // if pushing the rock did not result in success case
            // then it's from a season stone drop, so play ChangeSeason anim
            if (Id != puzzleController.rockTrigger.Id)
            {
                Debug.Log("Season stone puzzle completion was from STONE DROP.");
                ChangeSeason(Id, () => StartCoroutine(WaitAlchemistCircle()));
                return;
            }

            Debug.Log("Season stone puzzle completion was from ROCK MOVE.");
            AlchemistCircleAnim();
        }

        // need to wait in the case of changing season to avoid camera cutting
        // since timeline is starting before changeSeason anim is finishing
        IEnumerator WaitAlchemistCircle()
        {
            game.ChangeStateCutScene();
            yield return new WaitForSeconds(successPostChangeSeasonWaitTime);
            AlchemistCircleAnim();
        }

        void AlchemistCircleAnim()
        {
            isPuzzleComplete = true;
            game.ChangeStateCutScene();
            print("AlchemistCircleAnim(): game changed to cutScene");
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
        }
    }

    /// After Spotlight entrance timeline finishes, start Melz dialogue nodes, refusing to S-bind the two
    public void OnIntroPlayableDone(PlayableDirector aDirector)
    {
        print("OnIntroPlayableDone(): Entrance scene done");
        Script_DialogueManager.DialogueManager.StartDialogueNode(MelzNode, SFXOn: false);
    }

    public void OnIntroExitPlayableDone(PlayableDirector aDirector)
    {
        print("OnIntroPlayableDone(): Entrance scene done");
        Script_DialogueManager.DialogueManager.StartDialogueNode(
            PlayerReactionNode,
            SFXOn: false,
            Const_DialogueTypes.Type.Read
        );
    }

    public void OnSuccessPlayableDone(PlayableDirector aDirector)
    {
        if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[0])
        {
            print("Success timeline done (stones exploding, Urselk sprites floating, show Embrace PRCS)!");
            
            // Start Dialogue Node with action to go to PRCS
            game.ChangeStateCutScene(); // For Timeline dev'ing since playing timeline will remain in interact mode
            StartCoroutine(WaitForEmbraceDialogue());

            // Remove Urselk Lovers while in PRCS view
            Kaffe.gameObject.SetActive(false);
            Latte.gameObject.SetActive(false);
        }

        IEnumerator WaitForEmbraceDialogue()
        {
            yield return new WaitForSeconds(EmbraceDialogueWaitTime);
            Script_DialogueManager.DialogueManager.StartDialogueNode(onUrselksEmbraceNode, SFXOn: false);
        }
    }

    public void OnAftermathPlayableDone(PlayableDirector aDirector)
    {
        if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[3])
        {
            StartCoroutine(WaitForKaffeLatteDialogue());
            
            IEnumerator WaitForKaffeLatteDialogue()
            {
                yield return new WaitForSeconds(KaffeLatteDialogueWaitTime);

                Script_DialogueManager.DialogueManager.StartDialogueNode(KaffeLatteNode);
            }
        }
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[4])
        {
            Debug.Log("FINISHED MASTER LOCK ANIMATION!!!!!!!!!!!!");
            game.ChangeStateInteract();
        }
    }

    public void PRCSClimaxDoneReaction(Script_PRCSPlayer PRCSPlayer)
    {
        if (PRCSPlayer == climaxPlayer)
        {
            Debug.Log("CLIMAX SCENE DONE, stopping player now... MOVE ONTO Ids Dialogue");
            // need to remove Embrace PRCS that is still on screen
            embracePRCS.gameObject.SetActive(false);
            
            // Move player to designated position to be next to Ids
            Script_Game.Game.GetPlayer().Teleport(playerMoveDestination.position);
            Script_Game.Game.GetPlayer().FaceDirection(Directions.Up);
            Ids.gameObject.SetActive(true);
            
            // Remove Climax PRCS
            climaxPlayer.Stop(IdsDialogue);

            // Fade Out Bells Audio
            bellsBgPlayer.FadeOutStop(() => AfterglowBgMusic());

            void IdsDialogue()
            {
                StartCoroutine(WaitForIdsDialogue());
                
                IEnumerator WaitForIdsDialogue()
                {
                    yield return new WaitForSeconds(IdsDialogueWaitTime);

                    Script_DialogueManager.DialogueManager.StartDialogueNode(IdsNode);
                }
            }
        }
    }
    
    // ----------------------------------------------------------------------
    // Unity Events START
    // Switch_Cycles
    public void SetNextCycleWeekday()
    {
        game.SetNextCycle(Script_RunsManager.Cycle.Weekday);

        // TBD Remove After Demo
        game.DieEffects(Script_GameOverController.DeathTypes.DemoOver);   
    }

    public void SetNextCycleWeekend()
    {
        game.SetNextCycle(Script_RunsManager.Cycle.Weekend);
    }

    // Unity Events END
    // ----------------------------------------------------------------------

    /// <summary> ==============================================================================
    /// NextNodeAction(s) Start 
    /// </summary> =============================================================================
    public void MelzExit()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(2, 2);
        Script_VCamManager.VCamMain.SwitchToMainVCam(vCamEntrance);
    }
    
    public void OnPlayerReactionNodeDone()
    {
        game.ChangeStateInteract();
    }
    
    public void PRCSClimax()
    {
        Debug.Log("Play climax PRCS!!!");
        
        climaxPlayer.Play();
    }

    public void IdsExit()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(3, 3);
    }

    public void KaffeLatteSendKeyDown()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(3, 4);
    }
    /// NextNodeAction(s) End
    /// =============================================================================

    public void ChangeSeason(string seasonStoneId, Action cb = null)
    {   
        if (isPuzzleComplete)   return;

        Seasons lastSeason = season;

        if (!SeasonStonesEnums.TryGetValue(seasonStoneId, out season))
            Debug.LogError($"Script_LevelBehavior_20: There is no season key {seasonStoneId}");
        
        /// CHANGE SEASON ANIMATION & SFX
        StartCoroutine(ChangeSeasonCutScene());

        IEnumerator ChangeSeasonCutScene()
        {
            game.ChangeStateCutScene();
            
            // TODO: could show some animation on drop 
            yield return new WaitForSeconds(waitToChangeCameraTime);
            Script_VCamManager.VCamMain.SetNewVCam(vCamMid);
            yield return new WaitForSeconds(waitToChangeSeasonTime);

            // SFX
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = Script_SFXManager.SFX.Rumble254ms;
            audioSource.volume = Script_SFXManager.SFX.Rumble254msVol;
            audioSource.Play();

            FadeInOutSeason();
            yield return new WaitForSeconds(seasonChangeWaitTime);

            Script_VCamManager.VCamMain.SwitchToMainVCam(vCamMid);
            yield return new WaitForSeconds(revertCameraWaitTime);

            game.ChangeStateInteract();
            if (cb != null)     cb();
        }

        void FadeInOutSeason()
        {
            // if the season is the same, don't change the Sprite
            if (season == lastSeason)   return;
            
            // fade out old season sprite if was one
            if (lastSeason != Seasons.None)
            {
                // get SeasonTree corresponding to it
                int seasonInt;
                SeasonInt.TryGetValue(lastSeason, out seasonInt);
                ManTrees[seasonInt].FadeOut(null);
                WomanTrees[seasonInt].FadeOut(null);
            }

            // fade in new season sprite
            if (season != Seasons.None)
            {
                // get SeasonTree corresponding to it
                int seasonInt;
                SeasonInt.TryGetValue(season, out seasonInt);
                ManTrees[seasonInt].FadeIn(null);
                WomanTrees[seasonInt].FadeIn(null);

                fadeTime = Script_GraphicsManager.GetFadeTime(
                    WomanTrees[seasonInt].fadeSpeed
                );
            }
        }
    }

    private void OnMelzIntroCutScene()
    {
        /// Melz Intro Run
        if (game.Run.dayId == Script_Run.DayId.none)
        {
            /// Set in Awake() to signal we're doing Melz intro cut scene
            if (isInitialCutScene)
            {
                game.ChangeStateCutScene();
                isInitialCutScene = false;
            }
        }
    }

    private void AfterglowBgMusic()
    {
        game.SwitchBgMusic(13);
    }

    public override void Setup()
    {
        game.SetupSavePoint(savePoint, isInit);
        game.SetupMovingNPC(Kaffe, isInit);
        game.SetupMovingNPC(Latte, isInit);
        game.SetupMovingNPC(Ids, isInit);
        Ids.gameObject.SetActive(false);

        SuccessExplosion.SetActive(false);

        /// No Master Lock on default runs
        // if (didUnlockMasterLock)
        // {
        //     masterLock.gameObject.SetActive(false);
        //     lastExit.IsDisabled = false;
        // }
        masterLock.gameObject.SetActive(false);
        lastExit.IsDisabled = false;
        
        if (isPuzzleComplete)
        {
            // Afterglow music
            AfterglowBgMusic();
            
            // Remove Urselk Lovers
            Kaffe.gameObject.SetActive(false);
            Latte.gameObject.SetActive(false);

            // Show Magic Circle
            transmutationCircle.gameObject.SetActive(true);
            
            // Remove Season Trees
            foreach (var t in ManTrees) t.gameObject.SetActive(false);
        }
        else
        {
            transmutationCircle.gameObject.SetActive(false);
        }
        
        if (isPuzzleComplete && !didPickUpMasterKey)
        {
            masterKey.gameObject.SetActive(true);
            rock.SetActive(false);
        }
        else if (isPuzzleComplete && didPickUpMasterKey)
        {
            rock.SetActive(false);
            masterKey.gameObject.SetActive(false);
        }
        else
        {
            rock.SetActive(true);
            masterKey.gameObject.SetActive(false);
        }

        /// Melz Intro Run
        if (game.Run.dayId == Script_Run.DayId.none)
        {
            game.SetupMovingNPC(Melz, isInit);
            MelzIntroMelzParent.gameObject.SetActive(true);   
        }
        else
        {
            MelzIntroMelzParent.gameObject.SetActive(false);   
        }

        isInit = false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_20))]
public class Script_LevelBehavior_20Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_20 lb = (Script_LevelBehavior_20)target;
        if (GUILayout.Button("PlaceRock()"))
        {
            lb.rock.transform.position = lb.rockDestination.transform.position;
        }
        if (GUILayout.Button("SetNextCycleWeekend()"))
        {
            lb.SetNextCycleWeekend();
        }
        if (GUILayout.Button("SetNextCycleWeekday()"))
        {
            lb.SetNextCycleWeekday();
        }
    }
}
#endif