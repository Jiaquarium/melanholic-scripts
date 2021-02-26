using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Manages exiting and entering levels
/// 
/// Pass in FollowUp.CutScene to Exit if you want to exit into a black screen
/// Then call StartFadeIn() to fade back into World
/// </summary>
public class Script_Exits : MonoBehaviour
{
    public enum ExitType
    {
        Default                     = 0,
        SaveAndRestart              = 1,
        Elevator                    = 2,
        CutScene                    = 3,
        Piano                       = 4,
        SaveAndStartWeekendCycle    = 5,
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
    }
    public CanvasGroup canvas;

    private AudioSource audioSource;
    private Script_Game game;
    private IEnumerator coroutine;


    public bool isFadeIn;
    public float InitiateLevelWaitTime;
    public float fadeSpeed;
    [SerializeField] private bool isHandlingExit; /// Used to prevent multiple exitting and crashing

    private bool exitsDisabled;
    private bool isDefaultFadeOut;
    private int levelToGo;
    private FollowUp currentFollowUp;
    
    void Update()
    {
        if (isDefaultFadeOut)   ChangeLevelFade();
        if (isFadeIn)           FadeInLevel();
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
                Debug.Log("Changing Level without Fade");
                ChangeLevelNoFade();
                break;
            }
            case (FollowUp.SaveAndRestart):
            {
                Debug.Log("SaveAndRestart Exit Follow Up");
                isDefaultFadeOut = true; // triggers ChangeLevelFade(); 
                break;
            }
            case (FollowUp.Piano):
            {
                isDefaultFadeOut = true; // triggers ChangeLevelFade(); 
                break;
            }
            case (FollowUp.SaveAndStartWeekendCycle):
            {
                Debug.Log("SaveAndStartWeekendCycle Exit Follow Up");
                isDefaultFadeOut = true; // triggers ChangeLevelFade(); 
                break;
            }
            default:
            {
                Debug.Log("Default Fading Out");
                isDefaultFadeOut = true; // triggers ChangeLevelFade(); 
                break;
            }
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
        game.GetExitsTileMaps()[i]
            .GetComponent<Script_TileMapExitEntrance>()
            .IsDisabled = isDisabled;
    }

    public void DisableAllExitsEntrances(bool isDisabled)
    {
        foreach (Tilemap tm in game.GetExitsTileMaps())
        {
            tm.GetComponent<Script_TileMapExitEntrance>().IsDisabled = isDisabled;
        }

        game.GetEntrancesTileMap().GetComponent<Script_TileMapExitEntrance>().IsDisabled = isDisabled;
    }

    public void StartFadeIn()
    {
        isFadeIn = true;
    }

    public void StartFadeOut()
    {
        isDefaultFadeOut = true;
    }

    public bool GetIsExitsDisabled()
    {
        return exitsDisabled;
    }

    // -------------------------------------------------------------------------------------
    // FOR CHANGING OUT LEVELS: START
    void ChangeLevelFade()
    {
        canvas.alpha += fadeSpeed * Time.deltaTime;

        if (canvas.alpha >= 1f)
        {
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
                    Debug.Log("------------ SAVE STATE AND RESTART ------------");
                    game.ShowSaveAndRestartMessageDefault();
                    game.NextRunSaveInitialize();
                    break;
                }
                case (FollowUp.SaveAndStartWeekendCycle):
                {
                    Debug.Log("------------ SAVE STATE AND START WEEKEND CYCLE ------------");
                    game.ShowSaveAndStartWeekendMessage();
                    game.StartWeekendCycleSaveInitialize();
                    break;
                }
                default:
                {
                    ChangeLevel();
                    isFadeIn = true; // OnDoneExitingTransition will be called after fadeIn is complete
                    break;
                }
            }

            /// OnDoneExitingTransition() to be called in FadeInLevel()
        }
    }

    /// <summary>
    /// Don't put up the canvas
    /// Use when covering the screen with another cut scene before
    /// changing levels
    /// </summary>
    private void ChangeLevelNoFade()
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
        game.SnapToPlayer(playerPrevPosition);
    }
    
    // FOR CHANGING OUT LEVELS: END
    // -------------------------------------------------------------------------------------

    /// <summary>
    /// Fade in the level (fade out the canvas)
    /// </summary>
    void FadeInLevel()
    {
        canvas.alpha -= fadeSpeed * Time.deltaTime;

        if (canvas.alpha <= 0f && isFadeIn)
        {
            canvas.alpha = 0f;
            
            OnDoneExitingTransition();
            // must happen last so handlers can interact with fade in sequence.
            isFadeIn = false;
        }
    }

    void OnDoneExitingTransition()
    {
        isHandlingExit = false;
        
        // after faded in, player can then move
        // leave the state as cut scene if the exit FollowUp is a cut scene though
        if (game.state == Const_States_Game.InitiateLevel)
        {
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

    public void Setup(Script_Game _game)
    {
        game = _game;
        audioSource = GetComponent<AudioSource>();
    }
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
    }
}
#endif