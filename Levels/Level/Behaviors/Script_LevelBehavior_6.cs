using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Note: the Completion tile map REPLACES the original Ground tilemap, so any changes made to
/// the original Ground Tilemap, ensure to make to completion tilemap.
/// </summary>
public class Script_LevelBehavior_6 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    /* ======================================================================= */

    public bool[] switchesStates;
    public bool isPuzzleCompleted;
    
    [SerializeField] private AudioClip puzzleCompleteSFX;
    [SerializeField] private AudioSource audioSource;
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

    [SerializeField] private Script_MeshFadeController mirrorGlassFader;
    [SerializeField] private float fadeOutMirrorTime;

    private Script_LBSwitchHandler switchHandler;
    
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

    protected override void Update()
    {
        base.Update();
        HandlePuzzle();
    }
    
    public override void SetSwitchState(int Id, bool isOn)
    {
        switchHandler.SetSwitchState(switchesStates, Id, isOn);
    }

    void HandlePuzzle()
    {
        HandleLightupPaintingsPuzzle();
        
        void HandleLightupPaintingsPuzzle()
        {
            // check switchesStates with winState
            if (switchesStates == null)
                return;

            for (int i = 0; i < puzzleCompleteSwitchesStates.Length; i++)
            {
                if (switchesStates[i] != puzzleCompleteSwitchesStates[i])
                    return;
            }

            if (!isPuzzleCompleted)
                OnPuzzleCompletion();
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

        var SFX = Script_SFXManager.SFX;
        audioSource.PlayOneShot(SFX.Secret, SFX.SecretVol);
        yield return new WaitForSeconds(Script_SFXManager.SFX.SecretDuration);
        
        Script_VCamManager.VCamMain.SetNewVCam(VCamLB6);
        
        yield return new WaitForSeconds(cameraMoveTime);
        
        // Play Puzzle Completion SFX, Fade In extra Grid and Fade Out Glass Mesh
        // Reflection Sprite
        audioSource.PlayOneShot(puzzleCompleteSFX, volumeScale);

        StartCoroutine(
            completionGround.GetComponent<Script_TileMapFadeIn>().FadeInCo(
                () => {
                    StartCoroutine(WaitToMoveAfterReveal());
                })
        );
        StartCoroutine(
            mirrorReflection.GetComponent<Script_SpriteFadeOut>().FadeOutCo(null, t: fadeOutMirrorTime)
        );
        mirrorGlassFader.FadeOut(fadeOutMirrorTime);
        
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

        game.SetupInteractableFullArt(fullArtParent, isInit);

        LightUpPaintingsPuzzleNoteParent.gameObject.SetActive(true);
        
        isInit = false;
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_LevelBehavior_6))]
    public class Script_LevelBehavior_6Tester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LevelBehavior_6 t = (Script_LevelBehavior_6)target;
            if (GUILayout.Button("Complete Puzzle"))
            {
                t.OnPuzzleCompletion();
            }
        }
    }
    #endif
}
