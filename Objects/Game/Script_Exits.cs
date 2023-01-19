using System.Collections;
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
                Script_BackgroundMusicManager.Control.HandleStopLevelBgmFade(levelToGo);
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
                Script_BackgroundMusicManager.Control.HandleStopLevelBgmFade(levelToGo);
                StartFadeOut();
                break;
            }
            case (FollowUp.SaveAndRestartOnLevel):
            {
                Dev_Logger.Debug("SaveAndRestartOnLevel Exit Follow Up");
                Script_BackgroundMusicManager.Control.HandleStopLevelBgmFade(levelToGo);
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
            
            // Slow, dramatic fade on for default Woods entrances
            if (game.IsDefaultWoodsEntranceFromHotel)
                currentLevelFadeInTime = woodsFromHotelFadeInTime;
            else if (
                customFadeBehavior != null
                && customFadeBehavior.FadeInTime > 0
                && customFadeBehavior.CheckLastBehavior(game.LastLevelBehavior)
            )
            {
                currentLevelFadeInTime = customFadeBehavior.GetFadeInTime();

                Dev_Logger.Debug($"{behavior.name} using custom fadeTime: {currentLevelFadeInTime}");
            }
            else
                currentLevelFadeInTime = defaultLevelFadeInTime;
            
            fadeTimer = currentLevelFadeInTime;
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
            
            switch (currentFollowUp)
            {
                case (FollowUp.CutSceneNoFade):
                {
                    ChangeLevel();
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

            StartFadeIn();
        }

        /// <summary>
        /// Describe custom cases for waiting in black screen between levels
        /// </summary>
        void HandleCustomLevelWait(Script_LevelBehavior behavior)
        {
            var customFadeBehavior = behavior?.GetComponent<Script_LevelCustomFadeBehavior>();
            
            // Slow, dramatic on the default Woods entrance. Do default wait time if is
            // opening PRCS.
            if (game.IsDefaultWoodsEntranceFromHotel)
                currentWaitToFadeInLevelTime = woodsFromHotelWaitTime;
            else if (
                customFadeBehavior != null
                && customFadeBehavior.WaitInBlackTime > 0
                && customFadeBehavior.CheckLastBehavior(game.LastLevelBehavior)
            )
            {
                currentWaitToFadeInLevelTime = customFadeBehavior.GetWaitInBlackTime();

                Dev_Logger.Debug($"{behavior.name} using custom waitToFade: {currentWaitToFadeInLevelTime}");
            }
            else
                currentWaitToFadeInLevelTime = defaultLevelWaitToFadeInTime;
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
