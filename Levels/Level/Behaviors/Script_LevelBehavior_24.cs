﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_24 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    [SerializeField] private bool _isPuzzleComplete;
    public bool IsPuzzleComplete
    {
        get => _isPuzzleComplete;
        set => _isPuzzleComplete = value;
    }
    // alchemist circle persist
    public bool didPickUpSpringStone;
    public bool didPlayFaceOff;

    /* ======================================================================= */
    
    [SerializeField] private bool isCurrentPuzzleComplete;

    [SerializeField] private Script_TrackedPushablesTriggerPuzzleController triggersPuzzleController;
    
    [SerializeField] private Transform pillarParent;
    [SerializeField] private Script_Tracker[] pillars;
    [SerializeField] private Transform transformingRock;
    [SerializeField] private Script_ScarletCipherPiece scarletCipherPiece;
    [SerializeField] private Script_VCamera staticZoomOutVCam;
    
    [SerializeField] private float beforeSpawnWaitTime;
    [SerializeField] private float afterSpawnWaitTime;
    
    [SerializeField] private Script_BgThemePlayer heartBeatBgThemePlayerPrefab;
    
    // After success, BGM should change to sound like hotel.
    [SerializeField] private int onSuccessDoneBgmIdx;
    
    [SerializeField] private Script_LevelBehavior_23 LB23;
    [SerializeField] private SpriteRenderer alchemistCircle;
    
    [SerializeField] private float alchemistCircleCompleteAlpha;

    [SerializeField] private Script_LevelBehavior_44 XXXWorldBehavior;

    [SerializeField] private Vector3 screenShakeVals;

    private Script_BgThemePlayer heartBeatBgThemePlayer;
    private bool isTimelineControlled = false;
    
    public bool IsCurrentPuzzleComplete
    {
        get => isCurrentPuzzleComplete;
        set => isCurrentPuzzleComplete = value;
    }

    private void OnValidate()
    {
        pillars     = pillarParent.GetChildren<Script_Tracker>();
    }
    
    protected override void OnEnable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess     += OnPuzzleSuccess;
        Script_GameEventsManager.OnLevelInitComplete    += CheckSuccessCase;

        // enable pillars renderers
        SetPillarsVisibility(true);
        // set triggers and pillars to active
        ActivateTriggersAndPillars(true);
        
        if (!isTimelineControlled)
        {
            game.PauseBgMusic();
            if (!isCurrentPuzzleComplete)
            {
                heartBeatBgThemePlayer = Instantiate(heartBeatBgThemePlayerPrefab, Vector3.zero, Quaternion.identity);
            }
        }
    }

    protected override void OnDisable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess     -= OnPuzzleSuccess;
        Script_GameEventsManager.OnLevelInitComplete    -= CheckSuccessCase;

        ActivateTriggersAndPillars(false);
        SetPillarsVisibility(false);   
        
        if (!isTimelineControlled)
        {
            if (heartBeatBgThemePlayer != null)
            {
                DestroyBgThemePlayer();
            }
            game.UnPauseBgMusic();
        }
    }

    private void DestroyBgThemePlayer()
    {
        heartBeatBgThemePlayer.GetComponent<AudioSource>().volume = 0f;
        heartBeatBgThemePlayer.GetComponent<AudioSource>().Stop();
        Destroy(heartBeatBgThemePlayer.gameObject);
        heartBeatBgThemePlayer = null;
    }

    private void ActivateTriggersAndPillars(bool isActive)
    {
        // set triggers and pillars to active
        if (triggersPuzzleController != null)
            triggersPuzzleController.gameObject.SetActive(isActive);

        if (pillarParent != null)
            pillarParent.gameObject.SetActive(isActive);
    }

    private void SetPillarsVisibility(bool isOn)
    {
        foreach (Script_Tracker pillar in pillars)  pillar.SetVisibility(isOn);
    }

    private void CheckSuccessCase()
    {
        // check for completion state (only complete if still correct)
        // to ensure none were moved since player came from KTV Room 1
        triggersPuzzleController.CheckSuccessCase();
    }
    public void OnPuzzleSuccess(string Id)
    {
        if (!isCurrentPuzzleComplete)
        {
            Dev_Logger.Debug("PUZZLE SUCCESS SCENE!!!");
            IsPuzzleComplete        = true;
            isCurrentPuzzleComplete = true;
            
            // stop heartbeat bg music and stop pulsing animation of pillars
            if (heartBeatBgThemePlayer != null)
            {
                // Fade out bgThemePlayer first to avoid sound click
                heartBeatBgThemePlayer.FadeOutStop(DestroyBgThemePlayer, FadeSpeeds.XFast.ToFadeTime());
            }

            foreach (Script_Tracker tracker in pillars)
                tracker.Done();
            
            game.ChangeStateCutScene();
            StartCoroutine(TransmutationScene());
        }

        IEnumerator TransmutationScene()
        {
            // camera to SummerStone via VCam, 2 sec
            Script_VCamManager.VCamMain.SetNewVCam(staticZoomOutVCam);
            yield return new WaitForSeconds(Script_VCamManager.defaultBlendTime);
            
            yield return new WaitForSeconds(beforeSpawnWaitTime);
            
            // spawn sunstone & SFX
            GetComponent<Script_TimelineController>().PlayableDirectorPlay(0);
        }
    }

    public void PuzzleFinishedState()
    {
        transformingRock.gameObject.SetActive(false);

        if (heartBeatBgThemePlayer != null)
            DestroyBgThemePlayer();
        
        triggersPuzzleController.CompleteState();
        LB23.CompletedState();
    }

    private void AlchemistCircleCompleteState()
    {
        Color newColor = alchemistCircle.color;
        newColor.a = alchemistCircleCompleteAlpha;

        alchemistCircle.color = newColor;
    }

    public override void InitialState()
    {
        transformingRock.gameObject.SetActive(true);
        
        scarletCipherPiece.gameObject.SetActive(false);
        scarletCipherPiece.SetAlpha(0f);
    }

    // ------------------------------------------------------------------
    // Timeline Signals START

    // Success (LB24PointLight0) Timeline
    public void ShakeScreen()
    {
        var activeShakeCamera = staticZoomOutVCam.GetComponent<Script_CameraShake>();
        activeShakeCamera.Shake(screenShakeVals.x, screenShakeVals.y, screenShakeVals.z, null);
    }
    
    // Do not reveal if the puzzle has previously been completed.
    public void HandleScarletCipherReveal()
    {
        // Transforming rock's alpha is adjusted to 0.

        // Only spawn Scarlet Cipher Piece if it hasn't been picked up already.
        if (!scarletCipherPiece.DidPickUp())
        {
            scarletCipherPiece.gameObject.SetActive(true);
        }
    }

    public void OnSuccessTimelineDone()
    {
        StartCoroutine(EndingAnimations());

        IEnumerator EndingAnimations()
        {
            PuzzleFinishedState();
            yield return new WaitForSeconds(afterSpawnWaitTime);
            
            Script_VCamManager.VCamMain.SwitchToMainVCam(staticZoomOutVCam);
            yield return new WaitForSeconds(Script_VCamManager.defaultBlendTime);

            isTimelineControlled = true;
            
            var transitionManager = Script_TransitionManager.Control;
            transitionManager.OnCurrentQuestDone(
                allQuestsDoneCb: () =>
                {
                    isTimelineControlled = false;
                    
                    transitionManager.FinalCutSceneAwakening();
                }, 
                defaultCb: () =>
                {
                    isTimelineControlled = false;
                    
                    HandlePlayFaceOff(OnSuccessDone);
                },
                waitType: Script_TransitionManager.FinalNotificationWaitTimes.XXXWorld
            );
        }

        // Face Off is not played if Final Cut Scene Sequence should play.
        void HandlePlayFaceOff(Action cb)
        {
            if (!didPlayFaceOff)
            {
                var PRCSManager = Script_PRCSManager.Control;

                PRCSManager.TalkingSelfSequence(() => {
                    PRCSManager.PlayFaceOffTimeline(() => {
                        cb();
                        didPlayFaceOff = true;
                    });
                },
                isFaceOff: true);
            }
            else
            {
                cb();
            }
        }

        // Fade in Lobby Bgm to foreshadow the final event there
        void OnSuccessDone()
        {
            Script_BackgroundMusicManager.Control.PlayFadeIn(
                onSuccessDoneBgmIdx,
                game.ChangeStateInteract,
                forcePlay: true,
                Script_TransitionManager.FadeTimeSlow,
                Const_AudioMixerParams.ExposedBGVolume
            );
        }
    }

    public void FinishQuestPaintings()
    {
        XXXWorldBehavior.FinishQuestPaintings();
    }
    
    // ------------------------------------------------------------------

    void Awake()
    {
        if (isCurrentPuzzleComplete)
        {
            PuzzleFinishedState();
            AlchemistCircleCompleteState();
        }
        else
        {
            InitialState();
        }
    }

    public override void Setup()
    {

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_LevelBehavior_24))]
    public class Script_LevelBehavior_24Tester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LevelBehavior_24 t = (Script_LevelBehavior_24)target;
            if (GUILayout.Button("Place Trackers On Triggers"))
            {
                t.triggersPuzzleController.DevPlaceTrackersOnTriggers();
            }
            
            if (GUILayout.Button("Puzzle Success"))
            {
                t.triggersPuzzleController.DevPlaceTrackersOnTriggers();
                t.OnPuzzleSuccess(null);
            }

            GUILayout.Space(12);
                
            if (GUILayout.Button("Setup True Ending On Complete"))
            {
                Dev_GameHelper gameHelper = Script_Game.Game.gameObject.GetComponent<Dev_GameHelper>();
                gameHelper.SetQuestsDoneExplicit(5);
            }
        }
    }
#endif
}
