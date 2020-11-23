using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_30 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isDone;
    /* ======================================================================= */
    
    [SerializeField] private Script_SyncedTriggerPuzzleController puzzleTriggerController;
    [SerializeField] private Transform pushablesParent;
    [SerializeField] private Transform playerReflectionIds;
    [SerializeField] private Script_Fireplace fireplacePlayer;
    [SerializeField] private Script_Fireplace fireplaceMirror;
    [SerializeField] private AudioSource proximityCracklingFire;
    [SerializeField] private Transform fireplaceBlockingBox;
    [SerializeField] private Transform fullArtParent;

    private bool isInit = true;

    protected override void OnEnable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess += OnPuzzleComplete;
    }

    protected override void OnDisable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess -= OnPuzzleComplete;
    }
    
    private void OnPuzzleComplete(string arg)
    {
        isDone = true;

        PuzzleDoneSetup();
    }

    /// <summary>
    /// Called in Script_SyncedTriggerPuzzleController InitialState()
    /// </summary>
    public void InitializeFire()
    {
        // initialize fire grow in fireplace
        fireplacePlayer.InitializeFire();
        fireplaceMirror.InitializeFire();

        // initialize fire SFX
        proximityCracklingFire.gameObject.SetActive(true);
    }

    private void PuzzleDoneSetup()
    {
        puzzleTriggerController.isComplete = true;
        pushablesParent.gameObject.SetActive(false);
        game.DisableExits(false, 0);
        
        proximityCracklingFire.gameObject.SetActive(false);
        fireplaceBlockingBox.gameObject.SetActive(false);

        fireplacePlayer.Disable();
        fireplaceMirror.Disable();
    }

    private void PuzzleSetup()
    {
        proximityCracklingFire.gameObject.SetActive(true);
        fireplaceBlockingBox.gameObject.SetActive(true);
        
        game.DisableExits(true, 0);
    }
    
    public override void Setup()
    {
        game.SetupPlayerReflection(playerReflectionIds);
        game.SetupInteractableFullArt(fullArtParent, isInit);
        
        if (isDone)
        {
            PuzzleDoneSetup();
        }
        else
        {
            PuzzleSetup();
        }
        
        game.SetupPushables(pushablesParent, isInit);
        puzzleTriggerController.Setup();

        isInit = false;
    }        
}