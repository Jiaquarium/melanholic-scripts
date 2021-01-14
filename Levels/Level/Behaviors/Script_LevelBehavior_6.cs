using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Script_LevelBehavior_6 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool[] switchesStates;
    public bool isPuzzleCompleted;
    /* ======================================================================= */

    public AudioClip puzzleCompleteSFX;
    public AudioSource audioSource;
    public GameObject mirrorReflection;
    public float mirrorRevealWaitTime;
    public float cameraMoveTime;
    public float postMirrorRevealWaitTime;
    public float switchToMainVCamWaitTime;

    public bool[] puzzleCompleteSwitchesStates;
    public Tilemap completionGround;
    public GameObject completionGrid;
    public float volumeScale;
    public int switchesSortingLayerOffset;
    public Transform lightSwitchesParent;
    public Transform[] ptgTextParents;
    [SerializeField] private Transform playerReflectionEro;
    [SerializeField] private Script_VCamera VCamLB6;
    [SerializeField] private Transform fullArtParent;

    private Script_LBSwitchHandler switchHandler;
    private List<Script_InteractableObject> interactableObjects;
    
    private bool isInit = true;

    /// =======================================================================
    /// Light Up Paintings Puzzle START
    /// =======================================================================
    public Transform LightUpPaintingsPuzzleNoteParent;
    /// Light Up Paintings Puzzle END
    /// =======================================================================
    

    protected override void OnDisable()
    {
        // game.RemovePlayerReflection();
    }
    
    public override void SetSwitchState(int Id, bool isOn)
    {
        switchHandler.SetSwitchState(switchesStates, Id, isOn);
    }

    protected override void HandlePuzzle()
    {
        if (game.Run.dayId == Script_Run.DayId.none)
        {
            HandleLightupPaintingsPuzzle();
        }
        
        void HandleLightupPaintingsPuzzle()
        {
            // check switchesStates with winState
            if (switchesStates == null)    return;

            for (int i = 0; i < puzzleCompleteSwitchesStates.Length; i++)
            {
                if (switchesStates[i] != puzzleCompleteSwitchesStates[i])
                {
                    return;
                }
            }

            if (!isPuzzleCompleted)    OnPuzzleCompletion();
        }
    }

    private void OnPuzzleCompletion()
    {
        isPuzzleCompleted = true;
        game.StopBgMusic();

        game.ChangeStateCutScene();
        
        // wait a bit before mirror reveal
        StartCoroutine(WaitToForMirrorReveal());
    }

    IEnumerator WaitToForMirrorReveal()
    {
        yield return new WaitForSeconds(mirrorRevealWaitTime);
        
        Script_VCamManager.VCamMain.SetNewVCam(VCamLB6);
        
        yield return new WaitForSeconds(cameraMoveTime);
        
        // play puzzle complete SFX and fade in grid
        audioSource.PlayOneShot(puzzleCompleteSFX, volumeScale);
        StartCoroutine(
            completionGround.GetComponent<Script_TileMapFadeIn>().FadeInCo(
                () => {
                    StartCoroutine(WaitToMoveAfterReveal());
                })
        );
        StartCoroutine(
            mirrorReflection.GetComponent<Script_SpriteFadeOut>().FadeOutCo(null)
        );
        completionGrid.SetActive(true);
        game.SetNewTileMapGround(completionGround);
        game.RemovePlayerReflection();
    }

    IEnumerator WaitToMoveAfterReveal()
    {
        yield return new WaitForSeconds(postMirrorRevealWaitTime);

        // return camera to player and change game state back
        // to interact so player can move again
        Script_VCamManager.VCamMain.SwitchToMainVCam(VCamLB6);

        yield return new WaitForSeconds(switchToMainVCamWaitTime);
        
        game.ChangeStateInteract();
    }
    
    public override void Setup()
    {
        switchHandler = GetComponent<Script_LBSwitchHandler>();
        switchHandler.Setup(game);
        switchesStates = switchHandler.SetupSwitchesState(
            lightSwitchesParent,
            switchesStates,
            isInitialize: isInit
        );      
        foreach(Transform t in ptgTextParents)  game.SetupInteractableObjectsText(t, isInit);
        
        // for HandleWallTextActive polling
        interactableObjects = game.GetInteractableObjects();

        completionGrid.SetActive(isPuzzleCompleted);
        if (isPuzzleCompleted)
        {
            mirrorReflection.SetActive(false);
            // game.RemovePlayerReflection();
            if (playerReflectionEro != null)
                playerReflectionEro.gameObject.SetActive(false);
            game.SetNewTileMapGround(completionGround);
        }
        else
        {
            // create "mirror"
            // game.CreatePlayerReflection(game.Levels.levelsData[6].playerData.reflectionVector);
            if (playerReflectionEro != null)
                playerReflectionEro.gameObject.SetActive(true);
            game.SetupPlayerReflection(playerReflectionEro);
            mirrorReflection.SetActive(true);
        }

        if (game.Run.dayId == Script_Run.DayId.none)
        {
            game.SetupInteractableFullArt(fullArtParent, isInit);

            LightUpPaintingsPuzzleNoteParent.gameObject.SetActive(true);
        }
        else
        {
            LightUpPaintingsPuzzleNoteParent.gameObject.SetActive(false);
        }
        
        isInit = false;
    }
}
