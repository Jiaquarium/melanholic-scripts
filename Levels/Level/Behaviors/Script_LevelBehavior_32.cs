using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_TimelineController))]
[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_32 : Script_LevelBehavior
{
    private static string BGMParam = Const_AudioMixerParams.ExposedBGVolume;
    
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool didStartThought;
    /* ======================================================================= */
    
    [SerializeField] private Script_DialogueNode startNode;
    
    [SerializeField] private float beforeInternalThoughtWaitTime;
    
    [SerializeField] private Script_DialogueNode newGameNode;
    [SerializeField] private Script_DialogueNode onDisabledSurveillanceNode;
    
    [SerializeField] private float cameraPanDoneWaitTime;
    [SerializeField] private float faceDirectionDoneWaitTime;

    [SerializeField] private Script_DialogueNode tueR1Node;
    [SerializeField] private Script_DialogueNode wedR1Node;
    [SerializeField] private Script_DialogueNode monR2Node;
    
    [SerializeField] private Script_DialogueNode[] frontDoorNodes;

    [SerializeField] private Script_InteractableObjectInput CCTVAdminComputer;

    [SerializeField] private Script_InteractableObject hotelFrontDoor;
    [SerializeField] private Script_InteractableObject CCTVCamera;
    [SerializeField] private Script_Interactable invisibleBarrier;

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
    // Day Notifications
    [SerializeField] private Script_GlitchFXManager glitchManager;

    // ------------------------------------------------------------------
    // Note Tally Tracker
    [SerializeField] private Script_NotesTallyTracker notesTallyTracker;

    private int frontDoorDialogueIndex;
    private bool isFirstLoad = true;

    
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
                _isInteractAfter: false
            );
        }

        // Tue R1 Rin speaks about her reasons
        else if (
            game.CycleCount == 0
            && game.Run.dayId == Script_Run.DayId.tue
            && isFirstLoad
        )
        {
            Script_DayNotificationManager.Control.PlayDayNotification(() =>
                {
                    Script_BackgroundMusicManager.Control.UnPause();
                    Script_BackgroundMusicManager.Control.FadeInSlow(
                        () => StartOpeningDialogue(tueR1Node),
                        BGMParam
                    );
                },
                _isInteractAfter: false
            );

            StartCoroutine(CloseUnderDialogueBlackScreenNextFrame());
        }
        
        // Wed R1 Rin speaks about "Hardening"
        else if (
            game.CycleCount == 0
            && game.Run.dayId == Script_Run.DayId.wed
            && isFirstLoad
        )
        {
            Script_DayNotificationManager.Control.PlayDayNotification(() =>
                {
                    Script_BackgroundMusicManager.Control.UnPause();
                    Script_BackgroundMusicManager.Control.FadeInSlow(
                        () => StartOpeningDialogue(wedR1Node),
                        BGMParam
                    );
                },
                _isInteractAfter: false
            );

            StartCoroutine(CloseUnderDialogueBlackScreenNextFrame());
        }
        
        // Mon R2 Rin directs attention towards repeating cycle
        else if (
            game.CycleCount == 1
            && game.Run.dayId == Script_Run.DayId.mon
            && isFirstLoad
        )
        {
            Script_DayNotificationManager.Control.PlayDayNotification(() =>
                {
                    Script_BackgroundMusicManager.Control.UnPause();
                    Script_BackgroundMusicManager.Control.FadeInSlow(
                        () => StartOpeningDialogue(monR2Node),
                        BGMParam
                    );
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
                    Script_BackgroundMusicManager.Control.UnPause();
                    Script_BackgroundMusicManager.Control.FadeInSlow(() => {
                        game.ChangeStateInteract();
                    }, BGMParam);
                },
                _isInteractAfter: false
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
    }

    public override int OnSubmit(string CCTVcodeInput) {
        // Check Cipher
        bool isSuccessfulSubmit = CheckCCTVCode(CCTVcodeInput);
        
        Debug.Log($"------------ Result: {isSuccessfulSubmit} ------------");

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
        Script_BackgroundMusicManager.Control.UnPause();
        Script_BackgroundMusicManager.Control.FadeInSlow(null, BGMParam);
        
        /// Fade out black canvas
        Script_TransitionManager.Control.UnderDialogueFadeOut(
            Script_TransitionManager.UnderDialogueFadeTime, () => {
            
            // Wait a moment to start Node: hotel-lobby_player-internal_time-stopped
            StartCoroutine(WaitForPlayerThought());
        });

        IEnumerator WaitForPlayerThought()
        {
            yield return new WaitForSeconds(beforeInternalThoughtWaitTime);

            Script_DialogueManager.DialogueManager.StartDialogueNode(newGameNode);
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

    // ------------------------------------------------------------------
    // InteractableObject UnityEvents
    public void OnTryToExitFrontDoor()
    {
        Debug.Log("Move camera to hotel camera cut scene!!!");

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
                Debug.Log("@@@@@@@@@ GOOD ENDING, remove door @@@@@@@@@");

                game.EndingCutScene(Script_TransitionManager.Endings.Good);

                break;
            
            case (Script_TransitionManager.Endings.True):
                Debug.Log("@@@@@@@@@ TRUE ENDING, remove door @@@@@@@@@");

                game.EndingCutScene(Script_TransitionManager.Endings.True);

                break;

            case (Script_TransitionManager.Endings.Dream):
                Debug.Log("@@@@@@@@@ DREAM ENDING, remove door @@@@@@@@@");

                game.EndingCutScene(Script_TransitionManager.Endings.Dream);

                break;

            default:
                break;
        }
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
    }

    public void OnDisableSurveillanceDone()
    {
        Script_DialogueManager.DialogueManager.StartDialogueNode(onDisabledSurveillanceNode);
    }

    // ------------------------------------------------------------------

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
                Debug.Log("------ SETTING UP TRUE ENDING, remove door ------");
                HandleEndingExitState(true);
                break;

            case (Script_TransitionManager.Endings.Dream):
                Debug.Log("------ SETTING UP DREAM ENDING, remove door ------");
                HandleEndingExitState(true);
                break;

            default:
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

                Debug.Log($"------------ Result: {result} ------------");
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
