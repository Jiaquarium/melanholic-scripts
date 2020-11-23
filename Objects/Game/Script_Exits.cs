using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;
public class Script_Exits : MonoBehaviour
{
    public CanvasGroup canvas;

    private AudioSource audioSource;
    private Script_Game game;
    private IEnumerator coroutine;


    public bool isFadeIn;
    public float InitiateLevelWaitTime;
    public float fadeSpeed;

    private bool exitsDisabled;
    private bool isFadeOut;
    private bool isHandlingExit;
    private int levelToGo;
    
    void Update()
    {
        if (isFadeOut)  FadeOut();
        if (isFadeIn)   FadeIn();    
    }

    public void Exit(
        int level,
        Vector3 playerNextSpawnPosition,
        Directions playerFacingDirection,
        bool isExit,
        bool isSilent = false
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
        
        isFadeOut = true;
        levelToGo = level;

        if (!isSilent)  audioSource.PlayOneShot(Script_SFXManager.SFX.exitSFX, Script_SFXManager.SFX.exitSFXVol);
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
        isFadeOut = true;
    }

    void FadeOut()
    {
        canvas.alpha += fadeSpeed * Time.deltaTime;

        if (canvas.alpha >= 1f)
        {
            canvas.alpha = 1f;
            isFadeOut = false;
            
            Script_GameEventsManager.LevelBeforeDestroy();
            game.DestroyLevel();
            
            // isHandlingExit = false;
            game.level = levelToGo;

            Script_Player p = game.GetPlayer();
            Vector3 playerPrevPosition = p.transform.position;
            // player state has loaded here
            game.InitiateLevel();
            // cut to player spawn to avoid slow camera tracking
            game.SnapToPlayer(playerPrevPosition);
            
            isFadeIn = true;
        }
    }

    void FadeIn()
    {
        canvas.alpha -= fadeSpeed * Time.deltaTime;

        if (canvas.alpha <= 0f)
        {
            isHandlingExit = false;
            canvas.alpha = 0f;

            // after faded in, player can then move
            // change from initiate-level state
            game.ChangeStateInteract();
            
            /// Event fires after setting game state
            /// Allows us to define new initial game state in level behavior
            Script_GameEventsManager.LevelInitComplete();
            
            // must happen last so handlers can interact with fade in sequence.
            isFadeIn = false;
        }
    }

    public bool GetIsExitsDisabled()
    {
        return exitsDisabled;
    }

    public void Setup(Script_Game _game)
    {
        game = _game;
        audioSource = GetComponent<AudioSource>();
    }
}
