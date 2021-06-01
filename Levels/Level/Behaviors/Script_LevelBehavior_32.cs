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

    [SerializeField] private Transform WeekdayWalls;
    [SerializeField] private Transform WeekendWalls;

    private int frontDoorDialogueIndex;
    private bool isFirstLoad = true;

    
    // ------------------------------------------------------------------
    // Dev Only TBD DELETE
    public string DEVELOPMENT_CCTVCodeInput;

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
        else
        {
            // On First Load of subsequent days, play the Day Notification only once.
            if (isFirstLoad)
            {
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
            }
            
            // If not first load, we do not need to play the Day Notification and
            // will not have take down the UnderDialogue Canvas bc did not set it up.

            IEnumerator CloseUnderDialogueBlackScreenNextFrame()
            {
                yield return null;

                Script_TransitionManager.Control.UnderDialogueBlackScreen(false);
            }
        }

        isFirstLoad = false;

        void StartNewGameSequence()
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

            game.ActiveEnding = Script_TransitionManager.Endings.Good;
            HandleEndingExitState(true);
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
    public void OnEndStartDialogue()
    {
        Script_BackgroundMusicManager.Control.UnPause();
        Script_BackgroundMusicManager.Control.FadeInSlow(null, BGMParam);
        
        /// Fade out black canvas
        Script_TransitionManager.Control.UnderDialogueFadeOut(
            Script_TransitionManager.UnderDialogueFadeTime, () => {
            game.ChangeStateInteract();
        });
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
    // Timeline Signals START
    public void OnHotelCameraPan()
    {
        game.GetPlayer().FaceDirection(Directions.Right);
    }

    public void OnHotelCameraPanDone()
    {
        game.ChangeStateInteract();
    }
    // ------------------------------------------------------------------

    protected override void HandleAction()
    {
        // for cutScene dialogue
        base.HandleDialogueAction();
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