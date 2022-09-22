using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Test Cases for Day Notifications:
/// 1. Day 1
/// 2. Default R1 with Dialogue
/// 3. Default R1
/// 4. R2 Day 1 with Dialogue
/// 5. Default R2 (no dialogue follow ups)
/// 6. Sunday
/// </summary>

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_TimelineController))]
[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_32 : Script_LevelBehavior
{
    public const string MapName = Script_Names.HotelLobby;
    private static string BGMParam = Const_AudioMixerParams.ExposedBGVolume;
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool didStartThought;
    /* ======================================================================= */
    
    [SerializeField] private Script_DialogueNode startNode;
    
    [SerializeField] private float afterSigningWaitTime;
    [SerializeField] private float beforeInternalThoughtWaitTime;
    
    [SerializeField] private Script_DialogueNode newGameNode;
    [SerializeField] private Script_DialogueNode onDisabledSurveillanceNode;
    
    [SerializeField] private float cameraPanDoneWaitTime;
    [SerializeField] private float faceDirectionDoneWaitTime;

    [SerializeField] private Script_DialogueNode tueR1Node;
    [SerializeField] private Script_DialogueNode wedR1Node;
    [SerializeField] private Script_DialogueNode monR2Node;
    [SerializeField] private Script_DialogueNode satWeekendStartNode;
    
    [SerializeField] private Script_DialogueNode[] frontDoorNodes;

    [SerializeField] private Script_InteractableObjectInput CCTVAdminComputer;

    [SerializeField] private Script_InteractableObject hotelFrontDoor;
    [SerializeField] private Script_InteractableObject CCTVCamera;

    // ------------------------------------------------------------------
    // Dynamic Environment
    [SerializeField] private Transform MonBooks;
    [SerializeField] private Transform TueBooks;
    [SerializeField] private Transform WedBooks;
    [SerializeField] private Transform WeekendBooks;
    [SerializeField] private Script_DialogueNode weekendNewBookTriggerNode;

    [SerializeField] private Transform WeekdayWalls;
    [SerializeField] private Transform WeekendWalls;

    // ------------------------------------------------------------------
    // Endings
    [SerializeField] private Script_Interactable invisibleBarrier;
    [SerializeField] private Script_DoorExit goodEndingExitPrompter;
    [SerializeField] private Script_DialogueNode goodEndingPrompt;
    
    // ------------------------------------------------------------------
    // Day Notifications
    [SerializeField] private Script_GlitchFXManager glitchManager;

    // ------------------------------------------------------------------
    // Note Tally Tracker
    [SerializeField] private Script_NotesTallyTracker notesTallyTracker;

    private int frontDoorDialogueIndex;
    private bool isFirstLoad = true;

    // ------------------------------------------------------------------
    // Map Notification

    private bool didMapNotification;

    // ------------------------------------------------------------------
    // Dev Only TBD DELETE
    public string DEVELOPMENT_CCTVCodeInput;

    protected override void OnEnable()
    {
        base.OnEnable();

        glitchManager.InitialState();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        glitchManager.InitialState();
    }

    public override void OnLevelInitComplete()
    {
        // On New Game, the starting sequence will follow the Day Notification.
        if (!didStartThought && !Const_Dev.IsDevMode)
        {
            Script_DayNotificationManager.Control.PlayDayNotification(
                StartNewGameSequence,
                _isInteractAfter: false,
                isFirstDay: true,
                
                // Standalone FadeOut Timeline will call OnBeforeFadeOut to
                // allow Player to interact. 
                beforeFadeOutCb: () => {
                    StartNewGameDialogue();
                }
            );
        }

        // Tue R1 Rin speaks about her reasons
        else if (
            game.CycleCount == 0
            && game.Run.dayId == Script_Run.DayId.tue
            && isFirstLoad
        )
        {
            HandleOpeningDialogueSetup(tueR1Node);
        }
        
        // Wed R1 Rin speaks about "Hardening"
        else if (
            game.CycleCount == 0
            && game.Run.dayId == Script_Run.DayId.wed
            && isFirstLoad
        )
        {
            HandleOpeningDialogueSetup(wedR1Node);
        }
        
        // Mon R2 Rin directs attention towards repeating cycle
        else if (
            game.CycleCount == 1
            && game.Run.dayId == Script_Run.DayId.mon
            && isFirstLoad
        )
        {
            HandleOpeningDialogueSetup(monR2Node);
        }

        // Sat Weekend Start suggests Rin isn't fully aware of cycle but is becoming more cognizant of it.
        else if (
            game.CycleCount == 0
            && game.Run.dayId == Script_Run.DayId.thu
            && isFirstLoad
        )
        {
            HandleOpeningDialogueSetup(satWeekendStartNode);
        }

        // Sunday, don't do typewriting for Map Name
        else if (
            game.Run.dayId == Script_Run.DayId.sun
            && isFirstLoad
        )
        {
            // On First Load of subsequent days, play the Day Notification only once.
            Script_DayNotificationManager.Control.PlayDayNotification(() =>
                {
                    game.StartBgMusicNoFade();
                    var bgmManager = Script_BackgroundMusicManager.Control;
                    bgmManager.SetVolume(0f, BGMParam);
                    bgmManager.FadeInSlow(game.ChangeStateInteract, BGMParam);
                },
                _isInteractAfter: false
            );

            StartCoroutine(CloseUnderDialogueBlackScreenNextFrame());
        }

        // Default Day Notification.
        else if (isFirstLoad)
        {
            // On First Load of subsequent days, play the Day Notification only once.
            Script_DayNotificationManager.Control.PlayDayNotification(() =>
                {
                    game.StartBgMusicNoFade();
                    var bgmManager = Script_BackgroundMusicManager.Control;
                    bgmManager.SetVolume(0f, BGMParam);
                    bgmManager.FadeInSlow(null, BGMParam);
                },
                _isInteractAfter: false,
                beforeFadeOutCb: () => {
                    HandleMapNotification(() => {
                        game.ChangeStateInteract();
                    });
                }
            );

            StartCoroutine(CloseUnderDialogueBlackScreenNextFrame());
            
            // If not first load, we do not need to play the Day Notification and
            // will not have take down the UnderDialogue Canvas bc did not set it up.
        }

        isFirstLoad = false;
        
        IEnumerator CloseUnderDialogueBlackScreenNextFrame()
        {
            yield return null;

            Script_TransitionManager.Control.UnderDialogueBlackScreen(false);
        }

        void StartNewGameSequence()
        {
            game.ChangeStateCutScene();
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(startNode);
            didStartThought = true;
        }

        void StartOpeningDialogue(Script_DialogueNode dialogueNode)
        {
            game.ChangeStateCutScene();

            StartCoroutine(WaitForPlayerThought());

            IEnumerator WaitForPlayerThought()
            {
                yield return new WaitForSeconds(beforeInternalThoughtWaitTime);

                Script_DialogueManager.DialogueManager.StartDialogueNode(dialogueNode);
            }
        }

        void HandleOpeningDialogueSetup(Script_DialogueNode dialogueNode)
        {
            Script_DayNotificationManager.Control.PlayDayNotification(() =>
                {
                    game.StartBgMusicNoFade();
                    var bgmManager = Script_BackgroundMusicManager.Control;
                    bgmManager.SetVolume(0f, BGMParam);
                    bgmManager.FadeInSlow(null, BGMParam);
                },
                _isInteractAfter: false,
                beforeFadeOutCb: () => {
                    HandleMapNotification(() => {
                        StartOpeningDialogue(dialogueNode);
                    });                    
                }
            );

            StartCoroutine(CloseUnderDialogueBlackScreenNextFrame());            
        }
    }

    public override int OnSubmit(string CCTVcodeInput) {
        // Check Cipher
        bool isSuccessfulSubmit = CheckCCTVCode(CCTVcodeInput);
        
        Dev_Logger.Debug($"------------ Result: {isSuccessfulSubmit} ------------");

        // Call Interactable Object
        if (isSuccessfulSubmit)
        {
            CCTVAdminComputer.OnSubmitSuccess();

            game.ActiveEnding = Script_TransitionManager.Endings.Good;
            
            DisableSurveillanceSequence();
        }
        else
        {
            CCTVAdminComputer.OnSubmitFailure();
            WrongPasswordSFX();            
        }

        return -1;
    }

    public bool CheckCCTVCode(string CCTVcodeInput)
    {
        return Script_ScarletCipherManager.Control.CheckCCTVCode(CCTVcodeInput);
    }

    // ------------------------------------------------------------------
    // Next Node Action START
    
    // Called from StartNode on New Game.
    public void OnEndStartDialogue()
    {
        game.StartBgMusicNoFade();
        var bgmManager = Script_BackgroundMusicManager.Control;
        bgmManager.SetVolume(0f, BGMParam);
        bgmManager.FadeInSlow(null, BGMParam);
        
        // Wait to ZoomOut
        StartCoroutine(WaitToFadeOutDay1());

        IEnumerator WaitToFadeOutDay1()
        {
            yield return new WaitForSeconds(afterSigningWaitTime);
            
            // Start FadeOutDay1 Timeline
            Script_DayNotificationManager.Control.PlayFadeOutDay1();
            
            // Hide Underdialogue fade out
            Script_TransitionManager.Control.UnderDialogueBlackScreen(false);
        }
    }

    // newGameNode
    // onDisabledSurveillanceNode
    public void GameInteract()
    {
        game.ChangeStateInteract();
    }

    public void PanToCCTVCamera()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);        
    }

    public void OnGoodEndingPromptConfirm()
    {
        game.GetPlayer().TimelineMoveUp();
    }

    public void OnGoodEndingPromptCancel()
    {
        game.ChangeStateInteract();
        game.GetPlayer().FaceDirection(Directions.Down);
    }

    // ------------------------------------------------------------------
    // InteractableObject UnityEvents
    public void OnTryToExitFrontDoor()
    {
        Dev_Logger.Debug("Move camera to hotel camera cut scene!!!");

        game.ChangeStateCutScene();
        
        Script_DialogueManager.DialogueManager.StartDialogueNodeNextFrame(
            frontDoorNodes[frontDoorDialogueIndex],
            SFXOn: false
        );

        IncrementFrontDoorDialogueIndex();
    }

    public void EndingCutScene()
    {
        switch (game.ActiveEnding)
        {
            case (Script_TransitionManager.Endings.Good):
                Dev_Logger.Debug("@@@@@@@@@ GOOD ENDING, remove door @@@@@@@@@");

                game.EndingCutScene(Script_TransitionManager.Endings.Good);

                break;
            
            case (Script_TransitionManager.Endings.True):
                Dev_Logger.Debug("@@@@@@@@@ TRUE ENDING, remove door @@@@@@@@@");

                game.EndingCutScene(Script_TransitionManager.Endings.True);

                break;

            case (Script_TransitionManager.Endings.Dream):
                Dev_Logger.Debug("@@@@@@@@@ DREAM ENDING, remove door @@@@@@@@@");

                game.EndingCutScene(Script_TransitionManager.Endings.Dream);

                break;

            default:
                break;
        }
    }

    public void GoodEndingExitPrompt()
    {
        game.ChangeStateCutScene();
        game.GetPlayer().FaceDirection(Directions.Up);
        Script_DialogueManager.DialogueManager.StartDialogueNode(goodEndingPrompt);
    }

    // ------------------------------------------------------------------
    // Trigger UnityEvents
    
    // New Book Trigger: hint player to interact with Coffee Table book.
    // This should only happen once, upon entering the Weekend Cycle.
    public void NoticeNewBook()
    {
        if (game.CycleCount != 0 || game.Run.dayId != Script_Run.DayId.thu)
            return;

        game.ChangeStateCutScene();
        Script_DialogueManager.DialogueManager.StartDialogueNode(weekendNewBookTriggerNode);
    }

    // ------------------------------------------------------------------
    // Next Node Actions
    
    // weekendNewBookTriggerNode
    public void OnNoticeNewBookDone()
    {
        game.ChangeStateInteract();
    }
    
    // ------------------------------------------------------------------
    // Timeline Signals START
    
    public void OnHotelCameraPan()
    {
        // game.GetPlayer().FaceDirection(Directions.Right);
    }

    public void OnHotelCameraPanDone()
    {
        StartCoroutine(WaitToInteract());

        IEnumerator WaitToInteract()
        {
            yield return new WaitForSeconds(cameraPanDoneWaitTime);

            game.GetPlayer().FaceDirection(Directions.Down);
            
            yield return new WaitForSeconds(faceDirectionDoneWaitTime);

            game.ChangeStateInteract();
        }
    }

    public void OnDisableSurveillanceWhiteScreen()
    {
        HandleEndingExitState(true);
        goodEndingExitPrompter.gameObject.SetActive(true);
    }

    public void OnDisableSurveillanceDone()
    {
        Script_DialogueManager.DialogueManager.StartDialogueNode(onDisabledSurveillanceNode);
    }

    // ------------------------------------------------------------------

    // Standalone FadeOut Day 1 Timeline
    private void StartNewGameDialogue()
    {
        HandleMapNotification(() => StartCoroutine(WaitToStartNewGameDialogue()));

        IEnumerator WaitToStartNewGameDialogue()
        {
            yield return new WaitForSeconds(beforeInternalThoughtWaitTime);

            Script_DialogueManager.DialogueManager.StartDialogueNode(newGameNode);
        }
    }
    
    private void DisableSurveillanceSequence()
    {
        game.ChangeStateCutScene();
        
        // Quest Complete SFX
        Script_SFXManager.SFX.PlayQuestComplete(() => {
            // Camera Pan to Surveillance Cam
            // Electricity Effect
            // Explosion
            // Fade to White
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(1, 1);
        });
    }
    
    private void HandleEndingExitState(bool isOpen)
    {
        hotelFrontDoor.gameObject.SetActive(!isOpen);
        CCTVCamera.gameObject.SetActive(!isOpen);
        
        // Remove barrier to allow player to step on Ending Cut Scene Trigger.
        invisibleBarrier.gameObject.SetActive(!isOpen);
    }

    private void IncrementFrontDoorDialogueIndex()
    {
        frontDoorDialogueIndex++;

        if (frontDoorDialogueIndex >= frontDoorNodes.Length)
            frontDoorDialogueIndex = 0;
    }

    /// <summary>
    /// Map Notification should play while the Day Notification is still fading out,
    /// the typewriter effect will start exactly when Day Notification is fully faded out.
    /// </summary>
    private void HandleMapNotification(Action cb)
    {
        Script_MapNotificationsManager.Control.PlayMapNotification(MapName, () => {
                if (cb != null)
                    cb();
            },
            isInteractAfter: false,
            isSFXOn: true,
            sfx: Script_SFXManager.SFX.TypewriterTypingSFX
        );
        
        didMapNotification = true;
    }

    private void HandleDayEnvironment()
    {
        bool isWeekend = game.RunCycle == Script_RunsManager.Cycle.Weekend;
        
        MonBooks.gameObject.SetActive(game.IsRunDay(Script_Run.DayId.mon));
        TueBooks.gameObject.SetActive(game.IsRunDay(Script_Run.DayId.tue));
        WedBooks.gameObject.SetActive(game.IsRunDay(Script_Run.DayId.wed));
        WeekendBooks.gameObject.SetActive(isWeekend);

        WeekdayWalls.gameObject.SetActive(!isWeekend);
        WeekendWalls.gameObject.SetActive(isWeekend);
    }

    private void WrongPasswordSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(Script_SFXManager.SFX.UIWrongSFX, Script_SFXManager.SFX.UIWrongSFXVol);
    }

    public override void InitialState()
    {
        frontDoorDialogueIndex = 0;
    }

    public override void Setup()
    {
        HandleDayEnvironment();
        notesTallyTracker.UpdateNotesTallyUI();
        
        // Cover screen with Black to prevent flash of Lobby on reloads.
        if (isFirstLoad)
        {
            Script_TransitionManager.Control.UnderDialogueBlackScreen(true);            

            // Must Pause BGM until after Day Notification.
            Script_BackgroundMusicManager.Control.SetVolume(0f, BGMParam);
            Script_BackgroundMusicManager.Control.Pause();
        }
        
        InitialState();
        
        // Active Ending will be set when leaving from Last Elevator in Game.SaveWaitRestartAtLobby().
        // Good Ending is handled via interaction with the CCTV Interactable.
        switch (game.ActiveEnding)
        {
            case (Script_TransitionManager.Endings.True):
                Dev_Logger.Debug("------ SETTING UP TRUE ENDING, remove door ------");
                HandleEndingExitState(true);
                goodEndingExitPrompter.gameObject.SetActive(false);
                break;

            // State changes will happen via Timeline.
            case (Script_TransitionManager.Endings.Good):
                HandleEndingExitState(true);
                goodEndingExitPrompter.gameObject.SetActive(true);
                break;

            default:
                goodEndingExitPrompter.gameObject.SetActive(false);
                break;
        }

        // TBD TODO: Remove Only for Dev
        if (Const_Dev.IsTrueEnding)
        {
            game.ActiveEnding = Script_TransitionManager.Endings.True;
            HandleEndingExitState(true);
        }
        else if (Const_Dev.IsGoodEnding)
        {
            game.ActiveEnding = Script_TransitionManager.Endings.Good;
            HandleEndingExitState(true);
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

                Dev_Logger.Debug($"------------ Result: {result} ------------");
            }

            GUILayout.Space(8);
            EditorGUILayout.LabelField("Endings", EditorStyles.miniLabel);

            if (GUILayout.Button("Disable Surveillance"))
            {
                Script_Game.Game.ActiveEnding = Script_TransitionManager.Endings.Good;
                t.DisableSurveillanceSequence();
            }
            
            if (GUILayout.Button("Good Ending"))
            {
                Script_Game.Game.ActiveEnding = Script_TransitionManager.Endings.Good;
                t.EndingCutScene();
            }

            if (GUILayout.Button("True Ending"))
            {
                Script_Game.Game.ActiveEnding = Script_TransitionManager.Endings.True;
                t.EndingCutScene();
            }
        }
    }
    #endif
}
