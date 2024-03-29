﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Exit Manager
/// 
/// Manages exiting and entering levels
/// 
/// Pass in FollowUp.CutScene to Exit if you want to exit into a black screen
/// Then call StartFadeIn() to fade back into World
/// </summary>
public class Script_Exits : MonoBehaviour
{
    public static Script_Exits Control;
    
    public enum ExitType
    {
        Default                     = 0,
        SaveAndRestart              = 1,
        Elevator                    = 2,
        CutScene                    = 3,
        Piano                       = 4,
        SaveAndStartWeekendCycle    = 5,
        // Dev only
        SaveAndRestartOnLevel       = 6,
        StairsUp                    = 10,
    }
    
    public enum FollowUp
    {
        Default                     = 0,
        CutSceneNoFade              = 1,
        CutScene                    = 2,
        SaveAndRestart              = 3,
        Piano                       = 4,
        SaveAndStartWeekendCycle    = 5,
        SaveAndRestartOnLevel       = 6,
    }

    public enum TransitionFades
    {
        None = 0,
        Light = 1,
        Medium = 2,
        Heavy = 3,
        XHeavy = 4
    }
    
    public Model_LevelTransitions xHeavyTransitions;
    public Model_LevelTransitions heavyTransitions;
    public Model_LevelTransitions medTransitions;
    public Model_LevelTransitions lightTransitions;

    public CanvasGroup canvas;
    [SerializeField] private Script_ExitToWeekendCutScene exitToWeekendCutScene;

    private AudioSource audioSource;
    private Script_Game game;
    private IEnumerator coroutine;

    public bool isFadeIn;
    
    [Tooltip("Wait time after fading in level until player can interact")]
    [SerializeField] private float initiateLevelWaitTime;
    
    [Tooltip("Wait time for black screen between rooms")]
    [SerializeField] private float defaultLevelWaitToFadeInTime;
    
    [Tooltip("Fade time upon entering a room")]
    [SerializeField] private float defaultLevelFadeInTime;
    
    [Tooltip("Fade time for exiting a room")]
    [SerializeField] private float defaultLevelFadeOutTime;
    
    [Space][Header("Custom Level Fade & Wait Times")][Space]

    [SerializeField] private float woodsFromHotelFadeInTime;
    [SerializeField] private float woodsFromHotelWaitTime;

    [Space][Header("Dev Flags Display")][Space]
    [SerializeField] private bool isHandlingExit; /// Used to prevent multiple exitting and crashing

    private float currentLevelFadeInTime;
    private float currentWaitToFadeInLevelTime;
    
    private bool exitsDisabled;
    private bool isDefaultFadeOut;
    private int levelToGo;
    private FollowUp currentFollowUp;

    private float fadeTimer;
    
    public Script_CanvasGroupController ExitCanvasGroupController => canvas.GetComponent<Script_CanvasGroupController>();
    
    public bool IsHandlingExit
    {
        get => isHandlingExit;
    }

    public float InitiateLevelWaitTime
    {
        get => initiateLevelWaitTime;
        set => initiateLevelWaitTime = value;
    }

    public float TotalLevelTransitionTime => defaultLevelWaitToFadeInTime
        + defaultLevelFadeInTime + initiateLevelWaitTime;

    public float DefaultLevelFadeOutTime => defaultLevelFadeOutTime;
    public float DefaultLevelFadeInTime => defaultLevelFadeInTime;
    
    void Update()
    {
        if (isDefaultFadeOut)
            ChangeLevelFade();
        if (isFadeIn)
            FadeInLevel();
    }

    public void Exit(
        int level,
        Vector3 playerNextSpawnPosition,
        Directions playerFacingDirection,
        bool isExit,
        bool isSilent = false,
        FollowUp followUp = FollowUp.Default
    )
    {
        if (isHandlingExit)             return;
        // still allow player to go back where they came from
        if (isExit && exitsDisabled)    return;
        
        if (!isHandlingExit)    isHandlingExit = true;

        int x = (int)playerNextSpawnPosition.x;
        int y = (int)playerNextSpawnPosition.y;
        int z = (int)playerNextSpawnPosition.z;

        game.ChangeStateToInitiateLevel();
        game.SetPlayerState(
            new Model_PlayerState(x, y, z, playerFacingDirection)
        );
        game.RemoveMapNotification();
        
        currentFollowUp = followUp;
        levelToGo = level;

        if (!isSilent)  HandleSFX(currentFollowUp);
        
        switch (currentFollowUp)
        {
            case (FollowUp.CutSceneNoFade):
            {
                Dev_Logger.Debug("Changing Level without Fade");
                Script_BackgroundMusicManager.Control.HandleStopLevelBgmNoFade(levelToGo);
                HandleChangeLevelNoFade();
                break;
            }
            case (FollowUp.SaveAndRestart):
            {
                Dev_Logger.Debug("SaveAndRestart Exit Follow Up");
                Script_BackgroundMusicManager.Control.HandleStopLevelBgmFade(levelToGo, isFadeBGMUI: true);
                StartFadeOut();
                break;
            }
            case (FollowUp.Piano):
            {
                Script_BackgroundMusicManager.Control.HandleStopLevelBgmFade(levelToGo);
                StartFadeOut();
                break;
            }
            case (FollowUp.SaveAndStartWeekendCycle):
            {
                Dev_Logger.Debug("SaveAndStartWeekendCycle Exit Follow Up");
                Script_BackgroundMusicManager.Control.HandleStopLevelBgmFade(levelToGo, isFadeBGMUI: true);
                StartFadeOut();
                break;
            }
            case (FollowUp.SaveAndRestartOnLevel):
            {
                Dev_Logger.Debug("SaveAndRestartOnLevel Exit Follow Up");
                Script_BackgroundMusicManager.Control.HandleStopLevelBgmFade(levelToGo, isFadeBGMUI: true);
                StartFadeOut();
                break;
            }
            default:
            {
                Dev_Logger.Debug("Default Fading Out");
                Script_BackgroundMusicManager.Control.HandleStopLevelBgmFade(levelToGo);
                StartFadeOut();
                break;
            }
        }

        void StartFadeOut()
        {
            fadeTimer = defaultLevelFadeOutTime;
            isDefaultFadeOut = true;
        }
    }

    private void HandleSFX(FollowUp followUp)
    {
        switch(currentFollowUp)
        {
            case (FollowUp.Piano):
                audioSource.PlayOneShot(Script_SFXManager.SFX.piano, Script_SFXManager.SFX.pianoVol);
                break;
            default:
                audioSource.PlayOneShot(Script_SFXManager.SFX.exitSFX, Script_SFXManager.SFX.exitSFXVol);
                break;
        }
    }

    public void DisableExits(bool isDisabled, int i)
    {
        game.ExitTileMaps[i]
            .GetComponent<Script_TileMapExitEntrance>()
            .IsDisabled = isDisabled;
    }

    public void DisableAllExitsEntrances(bool isDisabled)
    {
        foreach (Tilemap tm in game.ExitTileMaps)
        {
            tm.GetComponent<Script_TileMapExitEntrance>().IsDisabled = isDisabled;
        }

        game.EntranceTileMap.GetComponent<Script_TileMapExitEntrance>().IsDisabled = isDisabled;
    }

    public void StartFadeIn()
    {
        HandleCustomLevelFading(game.levelBehavior);

        isFadeIn = true;

        /// <summary>
        /// Describe custom cases for fading into levels
        /// </summary>
        void HandleCustomLevelFading(Script_LevelBehavior behavior)
        {
            var customFadeBehavior = behavior?.GetComponent<Script_LevelCustomFadeBehavior>();
            float specialCaseFadeInTime = -1f;
            bool didEventSetSpecial = false;
            
            // Slow, dramatic fade on for default Woods entrances
            if (game.IsDefaultWoodsEntranceFromHotel)
                currentLevelFadeInTime = woodsFromHotelFadeInTime;
            else if (customFadeBehavior != null)
            {
                if (CheckSpecial())
                {
                    currentLevelFadeInTime = specialCaseFadeInTime;
                    Dev_Logger.Debug($@"{behavior.name} using Special Fade In: {currentLevelFadeInTime}
                        customFadeBehavior.IsSpecialCase {customFadeBehavior.IsSpecialCase}
                        customFadeBehavior.DidCheckSpecialCaseFadeIn {customFadeBehavior.DidCheckSpecialCaseFadeIn}
                        specialCaseFadeInTime {specialCaseFadeInTime}
                        didEventSetSpecial {didEventSetSpecial}"
                    );
                }
                else if (CheckCustomFadeNonSpecial())
                {
                    currentLevelFadeInTime = customFadeBehavior.GetFadeInTime();
                    Dev_Logger.Debug($@"{behavior.name} using custom Fade In: {currentLevelFadeInTime}
                        customFadeBehavior.FadeInTime {customFadeBehavior.FadeInTime}
                        customFadeBehavior.CheckLastBehavior(game.LastLevelBehavior) {customFadeBehavior.CheckLastBehavior(game.LastLevelBehavior)}"
                    );
                }
                else
                    DefaultFadeInTime();
            }
            else
                DefaultFadeInTime();
            
            fadeTimer = currentLevelFadeInTime;

            bool CheckSpecial() => customFadeBehavior.IsSpecialCase
                && !customFadeBehavior.DidCheckSpecialCaseFadeIn
                && customFadeBehavior.InvokeSettersFadeInTime(out specialCaseFadeInTime, out didEventSetSpecial)
                // Checks if fadeInTime & didEventSetSpecial flag were set from the setter invoked by InvokeSettersFadeInTime
                && specialCaseFadeInTime >= 0f
                && didEventSetSpecial;
            
            bool CheckCustomFadeNonSpecial() => customFadeBehavior.FadeInTime > 0
                && customFadeBehavior.CheckLastBehavior(game.LastLevelBehavior);

            void DefaultFadeInTime()
            {
                currentLevelFadeInTime = defaultLevelFadeInTime;
                Dev_Logger.Debug($@"{behavior.name} using default Fade In: {currentLevelFadeInTime}");
            }
        }
    }

    public bool GetIsExitsDisabled()
    {
        return exitsDisabled;
    }

    // -------------------------------------------------------------------------------------
    // FOR CHANGING OUT LEVELS: START
    void ChangeLevelFade()
    {
        // Start Fading in Black Fader
        canvas.gameObject.SetActive(true);
        
        fadeTimer -= Time.deltaTime;
        canvas.alpha = Mathf.Min(1f, 1 - fadeTimer / defaultLevelFadeOutTime);

        if (fadeTimer <= 0f)
        {
            fadeTimer = 0f;
            canvas.alpha = 1f;
            isDefaultFadeOut = false;

            Dev_Logger.Debug($"Time: {Time.time}");
            
            switch (currentFollowUp)
            {
                case (FollowUp.CutSceneNoFade):
                {
                    // 1/22/23 CASE DEPRECATED
                    // Note: OnDoneExitingTransition() will not be called in this case.
                    // ChangeLevel();
                    Debug.LogError("This case should not be in use. Instead HandleChangeLevelNoFade() should be called.");
                    break;
                }
                case (FollowUp.SaveAndRestart):
                {
                    Dev_Logger.Debug("------------ SAVE STATE AND RESTART ------------");
                    game.ShowSaveAndRestartMessageDefault();
                    game.NextRunSaveInitialize();
                    break;
                }
                case (FollowUp.SaveAndStartWeekendCycle):
                {
                    Dev_Logger.Debug("------------ SAVE STATE AND START WEEKEND CYCLE ------------");
                    
                    exitToWeekendCutScene.Play();
                    
                    break;
                }
                case (FollowUp.SaveAndRestartOnLevel):
                {
                    Dev_Logger.Debug("------------ SAVE STATE AND RESTART ON LEVEL ------------");
                    game.ShowSaveAndRestartMessageDefault();
                    game.NextRunSaveInitialize(isLobbySpawn: false);
                    break;
                }
                default:
                {
                    // Wait a frame before changing level
                    // and wait before triggering the FadeInLevel sequence,
                    // keeping screen black for a moment
                    StartCoroutine(WaitBeforeFadeInLevelSequence());
                    break;
                }
            }

            // OnDoneExitingTransition() to be called in FadeInLevel()
        }

        IEnumerator WaitBeforeFadeInLevelSequence()
        {
            yield return null;
            
            ChangeLevel();

            HandleCustomLevelWait(game.levelBehavior);
            yield return new WaitForSeconds(currentWaitToFadeInLevelTime);

            // Snap camera again for LBs where the camera distance has changed, since for unknown reason, when
            // switching back and forth to main VCam after the initial snap, the camera changes positions again
            if (
                game.LastLevelBehavior == game.UnderworldBehavior
                || game.levelBehavior == game.UnderworldBehavior
                || game.LastLevelBehavior == game.XXXWorldBehavior
                || game.levelBehavior == game.XXXWorldBehavior
            )
            {
                game.SnapActiveCam(game.GetPlayer().transform.position);
                yield return null;
            }

            // Should always be called when changing level, after black screen wait time but before 
            // OnDoneExitingTransition, telling us the level is ready for fade in or wait to init
            Script_GameEventsManager.LevelBlackScreenDone();

            StartFadeIn();
        }

        /// <summary>
        /// Describe custom cases for waiting in black screen between levels
        /// </summary>
        void HandleCustomLevelWait(Script_LevelBehavior behavior)
        {
            var customFadeBehavior = behavior?.GetComponent<Script_LevelCustomFadeBehavior>();
            float specialCaseWaitInBlackTime = -1f;
            bool didEventSetSpecial = false;

            // Slow, dramatic on the default Woods entrance. Do default wait time if is
            // opening PRCS.
            if (game.IsDefaultWoodsEntranceFromHotel)
                currentWaitToFadeInLevelTime = woodsFromHotelWaitTime;
            else if (customFadeBehavior != null)
            {
                if (CheckSpecial())
                {
                    currentWaitToFadeInLevelTime = specialCaseWaitInBlackTime;
                    Dev_Logger.Debug($@"{behavior.name} using Special Wait In Black: {currentWaitToFadeInLevelTime}
                        customFadeBehavior.IsSpecialCase {customFadeBehavior.IsSpecialCase}
                        customFadeBehavior.DidCheckSpecialCaseWaitInBlack {customFadeBehavior.DidCheckSpecialCaseWaitInBlack}
                        specialCaseWaitInBlackTime {specialCaseWaitInBlackTime}
                        didEventSetSpecial {didEventSetSpecial}"
                    );
                }
                else if (CheckCustomFadeNonSpecial())
                {
                    currentWaitToFadeInLevelTime = customFadeBehavior.GetWaitInBlackTime();
                    Dev_Logger.Debug($@"{behavior.name} using custom Wait In Black: {currentWaitToFadeInLevelTime}
                        customFadeBehavior.WaitInBlackTime {customFadeBehavior.WaitInBlackTime}
                        customFadeBehavior.CheckLastBehavior(game.LastLevelBehavior) {customFadeBehavior.CheckLastBehavior(game.LastLevelBehavior)}"
                    );
                }
                else
                    DefaultWaitInBlackTime();
            }
            else
                DefaultWaitInBlackTime();
            
            bool CheckSpecial() => customFadeBehavior.IsSpecialCase
                && !customFadeBehavior.DidCheckSpecialCaseWaitInBlack
                && customFadeBehavior.InvokeSettersWaitInBlackTime(out specialCaseWaitInBlackTime, out didEventSetSpecial)
                // Checks if waitInBlackTime & didEventSetSpecial flag were set from the setter invoked by InvokeSettersFadeInTime
                && specialCaseWaitInBlackTime >= 0f
                && didEventSetSpecial;

            bool CheckCustomFadeNonSpecial() => customFadeBehavior.WaitInBlackTime > 0
                && customFadeBehavior.CheckLastBehavior(game.LastLevelBehavior);
            
            void DefaultWaitInBlackTime()
            {
                currentWaitToFadeInLevelTime = defaultLevelWaitToFadeInTime;
                Dev_Logger.Debug($@"{behavior.name} using default Wait In Black: {currentWaitToFadeInLevelTime}");
            }
        }
    }

    /// <summary>
    /// Don't put up the canvas
    /// Use when covering the screen with another cut scene before
    /// changing levels
    /// </summary>
    private void HandleChangeLevelNoFade()
    {
        ChangeLevel();
        
        // Should always be called when changing level, after black screen wait time but before 
        // OnDoneExitingTransition, telling us the level is ready for fade in or wait to init
        Script_GameEventsManager.LevelBlackScreenDone();
        
        OnDoneExitingTransition();
    }

    private void ChangeLevel()
    {
        Script_GameEventsManager.LevelBeforeDestroy();
        game.DestroyLevel();
        
        game.level = levelToGo;

        Script_Player p = game.GetPlayer();
        Vector3 playerPrevPosition = p.transform.position;
        
        // player state has loaded here
        game.InitiateLevel();
        
        /// Cut to player spawn to avoid slow camera tracking
        game.SnapActiveCam(playerPrevPosition);
    }
    
    // FOR CHANGING OUT LEVELS: END
    // -------------------------------------------------------------------------------------

    /// <summary>
    /// Fade in the level (fade out the canvas)
    /// </summary>
    private void FadeInLevel()
    {
        canvas.gameObject.SetActive(true);
        fadeTimer -= Time.deltaTime;
        canvas.alpha = Mathf.Max(0f, fadeTimer / currentLevelFadeInTime);

        if (fadeTimer <= 0)
        {
            fadeTimer = 0f;
            canvas.alpha = 0f;
            
            OnDoneExitingTransition();
            
            // Must happen last so handlers can interact with fade in sequence.
            isFadeIn = false;
        }
    }

    private void OnDoneExitingTransition()
    {
        StartCoroutine(WaitToCompleteInit());
        
        IEnumerator WaitToCompleteInit()
        {
            yield return new WaitForSeconds(InitiateLevelWaitTime);
            
            isHandlingExit = false;
            
            // after faded in, player can then move
            // leave the state as cut scene if the exit FollowUp is a cut scene though
            if (game.state == Const_States_Game.InitiateLevel)
            {
                Dev_Logger.Debug($"{name} OnDoneExitingTransition currentFollowUp: {currentFollowUp}");
                
                switch(currentFollowUp)
                {
                    case (FollowUp.Default):
                        game.ChangeStateInteract();
                        break;
                    case (FollowUp.Piano):
                        game.ChangeStateInteract();
                        break;
                    default:
                        break;
                }
            }
            
            currentFollowUp = FollowUp.Default;

            /// Allows us to define new initial game state in level behavior
            /// Also fire event, so other objects can react
            game.levelBehavior.OnLevelInitComplete();
            Script_GameEventsManager.LevelInitComplete();
        }
    }

    public void InitializeExitFader() => ExitCanvasGroupController.InitialState();
    
    public void Setup(Script_Game _game)
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }
        
        game = _game;
        audioSource = GetComponent<AudioSource>();
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_Exits))]
    public class Script_ExitsTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_Exits lb = (Script_Exits)target;
            if (GUILayout.Button("StartFadeIn()"))
            {
                lb.StartFadeIn();
            }

            if (GUILayout.Button("To Weekend"))
            {
                lb.Exit(
                    Script_Game.Game.level,
                    Script_Game.Game.GetPlayerLocation(),
                    Script_Game.Game.GetPlayer().FacingDirection,
                    false,
                    false,
                    FollowUp.SaveAndStartWeekendCycle
                );
            }
        }
    }
    #endif
}
