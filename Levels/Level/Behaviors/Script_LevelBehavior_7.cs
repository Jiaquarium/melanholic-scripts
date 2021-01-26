using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using UnityEngine.Playables;

/// <summary>
/// This uses old method for next node action
/// </summary>
public class Script_LevelBehavior_7 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isDone = false;
    public bool isActivated = false;
    public bool[] demonSpawns;
    /* ======================================================================= */
    
    public Script_CutSceneNPC cutSceneNPC;
    [SerializeField] private float afterMelzRevealWaitTime;
    public float demonRevealWaitTime;
    public float postDemonRevealWaitTime;
    public float cagesRevealWaitTime;
    public float postCagesRevealWaitTime;
    public float postZoomInWaitTime;
    public float zoomInOnMelzSize;
    public float moveToMelzSmoothTime;
    public float zoomSmoothTime;
    public float postFinalWordsWaitTime;
    public float hintTimeBeforeShowing;
    public Vector3 zoomPosition;
    public float spotlightVolumeScale;
    public float zoomOutDemonsSize;
    public float zoomOutCagesSize;
    
    public Script_BgThemePlayer MelzBgThemePlayerPrefab;
    public AudioSource audioSource;
    public AudioClip SpotlightSFX;
    public GameObject MelzSpotlight;
    public GameObject demonsAndSpotlights;
    public GameObject demonSpotLights;
    public GameObject cages;
    public Script_DialogueManager dm;
    public Script_DialogueNode ateDemonsNode;
    public Script_DialogueNode sparedDemonsNode;
    [SerializeField] private Script_DialogueNode afterIntroRevealNode;
    public Script_DialogueNode showDemonsNode;
    public Script_DialogueNode showCagesNode;
    public Script_DialogueNode noseRingNode;
    public Script_DialogueNode finalWordsNode;
    public Script_LevelBehavior_3 level3;
    [SerializeField] private Script_VCamera VCamLB7;
    [SerializeField] private Script_VCamera VCamLB7Zoomed;
    [SerializeField] private Script_Hint hint;

    [SerializeField] private bool didEatDemons = false;
    [SerializeField] private bool shownHint = false;
    [SerializeField] private Script_PRCSPlayer namePlatePRCSPlayer;
    [SerializeField] private PlayableDirector nameplateDirector;
    
    private Coroutine hintCoroutine;
    
    void OnValidate()
    {
        // DefaultVCams();
    }

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete += OnEntrance;
        nameplateDirector.stopped += OnNameplateDone;
    }

    protected override void OnDisable() {
        Script_GameEventsManager.OnLevelInitComplete += OnEntrance;
        nameplateDirector.stopped -= OnNameplateDone;
        
        DefaultVCams();
    }

    public override void HandleDialogueNodeAction(string action)
    {
        if (action == "reveal-melz")            MelzReveal();
        else if (action == "reveal-demons")     StartCoroutine(WaitToRevealDemons());
        else if (action == "reveal-cages")      StartCoroutine(WaitToRevealCages());
        else if (action == "nose-ring-zoom")    StartCoroutine(WaitToZoomOnNoseRing());
        else if (action == "final-words")       StartCoroutine(WaitToMelzExit());
    }

    public override void HandleDialogueNodeUpdateAction(string action)
    {
        if (action == "inventory-open")         HandleInventoryOpenAndClose();
    }

    void OnEntrance()
    {
        if (game.state == "interact" && !isActivated && !isDone)
        {
            isActivated = true;

            // foreach(bool demonSpawn in level3.demonSpawns)
            // {
            //     if (!demonSpawn)    didEatDemons = true;
            // }
            // if (didEatDemons)   dm.StartDialogueNode(ateDemonsNode);
            // else                dm.StartDialogueNode(sparedDemonsNode);
            /// Always use Ate node because must EAT on lvl0 Woods
            dm.StartDialogueNode(ateDemonsNode);
            
            game.ChangeStateCutScene();
        }
    }

    void MelzReveal()
    {
        Script_VCamManager.VCamMain.SetNewVCam(VCamLB7);

        audioSource.PlayOneShot(SpotlightSFX, spotlightVolumeScale);
        cutSceneNPC.SetVisibility(true);
        MelzSpotlight.SetActive(true);
        game.PauseBgMusic();
        game.PlayNPCBgTheme(MelzBgThemePlayerPrefab);

        StartCoroutine(WaitAfterRevealDialogue());
        
        IEnumerator WaitAfterRevealDialogue()
        {
            yield return new WaitForSeconds(afterMelzRevealWaitTime);

            /// Play Nameplate PRCS
            // dm.StartDialogueNode(afterIntroRevealNode, SFXOn: false);
            NameplateTimeline();
        }

        void NameplateTimeline()
        {
            Debug.Log("Starting Melz nameplate cut scene ");
            game.ChangeStateCutScene();
            
            namePlatePRCSPlayer.Play();
        }
    }

    private void OnNameplateDone(PlayableDirector aDirector)
    {
        namePlatePRCSPlayer.Stop();
        dm.StartDialogueNode(afterIntroRevealNode, SFXOn: false);
    }
    
    IEnumerator WaitToRevealDemons()
    {
        audioSource.PlayOneShot(SpotlightSFX, spotlightVolumeScale);
        
        Script_VCamManager.SetCameraOrthoSize(VCamLB7, zoomOutDemonsSize);

        demonsAndSpotlights.SetActive(true);
        demonSpotLights.SetActive(true);

        yield return new WaitForSeconds(postDemonRevealWaitTime);
        dm.StartDialogueNode(showCagesNode, false);
    }

    IEnumerator WaitToRevealCages()
    {
        audioSource.PlayOneShot(SpotlightSFX, spotlightVolumeScale);
        
        Script_VCamManager.SetCameraOrthoSize(VCamLB7, zoomOutCagesSize);
        cages.SetActive(true);
        demonSpotLights.SetActive(false);

        yield return new WaitForSeconds(postCagesRevealWaitTime);
        dm.StartDialogueNode(noseRingNode, false);
    }

    IEnumerator WaitToZoomOnNoseRing()
    {
        Script_StaticNPC npc = cutSceneNPC;
        
        Script_VCamManager.VCamMain.SwitchBetweenVCams(
            Script_VCamManager.ActiveVCamera, VCamLB7Zoomed
        );

        // wait for lerp and zoom smooth times
        yield return new WaitForSeconds(moveToMelzSmoothTime + zoomSmoothTime);
        npc.Freeze(true);        
        npc.Glimmer();

        yield return new WaitForSeconds(postZoomInWaitTime);
        npc.Freeze(false);
        dm.StartDialogueNode(finalWordsNode, false);
    }
    
    IEnumerator WaitToMelzExit()
    {
        yield return new WaitForSeconds(postFinalWordsWaitTime);
        
        MelzExit();
    }

    void ChangeGameStateInteract()
    {
        game.ChangeStateInteract();
    }

    void MelzExitCallback()
    {
        MelzSpotlight.SetActive(false);
        // dm.OnEndDialogueSections();
        game.ChangeStateInteract();
        game.StopMovingNPCThemes();
        game.UnPauseBgMusic();

        Script_VCamManager.VCamMain.SwitchToMainVCam(Script_VCamManager.ActiveVCamera);
        
        isDone = true;
        cutSceneNPC.gameObject.SetActive(false);
        game.ClearNPCs();
    }

    void MelzExit()
    {
        cutSceneNPC.FadeOut(MelzExitCallback);
    }

    // handle inventory here since player is still in talking mode
    void HandleInventoryOpenAndClose()
    {
        if (isDone)     return;
        
        if (!shownHint)
        {
            shownHint = true;
            hintCoroutine = StartCoroutine(WaitToShowHint());
        }
        
        if (Input.GetButtonDown(Const_KeyCodes.Inventory) && game.state == Const_States_Game.CutScene)
        {
            game.OpenInventory();
            StopCoroutine(hintCoroutine);
            hint.Hide();
            Script_DialogueManager.DialogueManager.HideDialogue();
        }

        IEnumerator WaitToShowHint()
        {
            yield return new WaitForSeconds(hintTimeBeforeShowing);
            hint.Show();
        }
    }

    public override void OnCloseInventory()
    {
        if (!isDone)
        {
            dm.StartDialogueNode(showDemonsNode, false);
        }
    }

    private void DefaultVCams()
    {
        Script_VCamManager.SetCameraOrthoSize(VCamLB7, Const_Camera.OrthoSizes.DefaultSize);
    }

    protected override void HandleAction()
    {
        base.HandleDialogueAction();
    }

    /// <summary> 
    /// Demon Handlers: handle state
    /// </summary> 
    public override void EatDemon(int Id) {
        demonSpawns[Id] = false;
    }

    public override void Setup()
    {
        /// <summary> 
        /// Demon Handlers: initialize state
        /// </summary> 
        if (!isActivated)
        {
            demonSpawns = new bool[demonsAndSpotlights.transform.childCount];
            for (int i = 0; i < demonSpawns.Length; i++)
                demonSpawns[i] = true;
        }
        game.SetupDemons(demonsAndSpotlights.transform, demonSpawns);
        
        if (!isDone)
        {
            game.SetupCutSceneNPC(cutSceneNPC);
            cutSceneNPC.SetVisibility(false);
            MelzSpotlight.SetActive(false);
            demonsAndSpotlights.SetActive(false);
            demonSpotLights.SetActive(false);
            cages.SetActive(false);
        }
        else
        {
            demonSpotLights.SetActive(false);
            cutSceneNPC.gameObject.SetActive(false);
            demonsAndSpotlights.SetActive(true);
            MelzSpotlight.SetActive(false);
            cages.SetActive(true);
        }
    }
}
