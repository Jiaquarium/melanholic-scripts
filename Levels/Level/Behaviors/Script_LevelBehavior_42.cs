using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// When snowing, Player should react to it, hinting at the Fireplace exit.
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
[RequireComponent(typeof(Script_WorldTilesController))]
[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_42 : Script_LevelBehavior
{
    public const string MapName = "Wells World";
    
    // ==================================================================
    // State Data
    public bool didPickUpLastWellMap;
    public bool isMooseQuestDone;
    public bool didPlayFaceOff;
    // ==================================================================
    
    public bool isCurrentMooseQuestComplete;

    [SerializeField] private float waitTimeBeforeCastMooseFinalSpell;

    [SerializeField] private Script_WellsPuzzleController wellsPuzzleController;
    
    [SerializeField] private Script_FrozenWell[] frozenWells;
    
    [SerializeField] private Script_InteractablePaintingEntrance[] paintingEntrances;
    [SerializeField] private Script_InteractablePaintingEntrance ballroomPaintingEntrance;
    
    [SerializeField] private Script_CollectibleObject[] lastWellMaps;
    
    [SerializeField] private Script_Item lastSpellRecipeBookItem;
    
    [SerializeField] private Script_DialogueNode OnHasLastSpellRecipeBookNode;
    [SerializeField] private Script_DialogueNode OnMissingLastSpellRecipeBookNode;
    [SerializeField] private Script_DialogueNode OnMooseGiveItemDoneNode;
    
    [SerializeField] private Script_DialogueNode OnSnowReactionNode;
    [SerializeField] private float beforeSnowReactionWaitTime;
    [SerializeField] private Script_LevelBehavior_13 CatWalkBehavior;

    [SerializeField] private Script_Moose[] Mooses;
    [SerializeField] private Script_DemonNPC[] Suzettes;
    
    [SerializeField] private Script_ScarletCipherPiece[] scarletCipherPieces;

    [SerializeField] private Script_WeatherFXManager weatherFXManager;
    [SerializeField] private List<Script_WellsWorldBehavior> wellsWorldBehaviors;

    private bool didMapNotification;
    private bool didSnowReaction;

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete                            += OnLevelInitCompleteEvent;
        
        Script_PuzzlesEventsManager.OnPuzzleSuccess                             += OnPuzzleSuccess;
        Script_ItemsEventsManager.OnItemPickUp                                  += OnItemPickUp;

        Script_ScarletCipherEventsManager.OnScarletCipherPiecePickUp            += OnScarletCipherPickUp;

        Script_InteractableObjectEventsManager.OnFrozenWellDie                  += OnFrozenWellDie;
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete                            -= OnLevelInitCompleteEvent;
        
        Script_PuzzlesEventsManager.OnPuzzleSuccess                             -= OnPuzzleSuccess;
        Script_ItemsEventsManager.OnItemPickUp                                  -= OnItemPickUp;
        
        Script_ScarletCipherEventsManager.OnScarletCipherPiecePickUp            -= OnScarletCipherPickUp;

        Script_InteractableObjectEventsManager.OnFrozenWellDie                  -= OnFrozenWellDie;
    }

    void Awake()
    {
        if (weatherFXManager.IsSnowDay)
            SetSmallSnowActive(true);
        else
            SetSmallSnowActive(false);
    }
    
    private void OnLevelInitCompleteEvent()
    {
        if (!didMapNotification)
        {
            Script_MapNotificationsManager.Control.PlayMapNotification(MapName, () => {
                if (weatherFXManager.IsSnowDay)
                    HandleSnowReaction(beforeSnowReactionWaitTime);
            });
            didMapNotification = true;
        }
        else
        {
            if (weatherFXManager.IsSnowDay)
                HandleSnowReaction(beforeSnowReactionWaitTime);
        }
    }
    
    private bool HandleSnowReaction(float pauseTime)
    {
        if (didSnowReaction || CatWalkBehavior.didPickUpLightSticker)
            return false;
        
        game.ChangeStateCutScene();

        StartCoroutine(WaitToReact());

        didSnowReaction = true;
        return true;

        IEnumerator WaitToReact()
        {
            yield return new WaitForSeconds(pauseTime);
            Script_DialogueManager.DialogueManager.StartDialogueNode(OnSnowReactionNode);
        }
    }

    private void OnItemPickUp(string itemId)
    {
        if (itemId == lastWellMaps[0].Item.id)
        {
            didPickUpLastWellMap = true;
            SetMapsActive(false);
        }
    }

    private void OnPuzzleSuccess(string puzzleId)
    {
        if (puzzleId == wellsPuzzleController.PuzzleId)
        {
            game.ChangeStateCutScene();

            // Play Freeze Timeline
            var currentWorldTileVCam = GetComponent<Script_WorldTilesController>()
                .OriginWorldTile?.GetComponent<Script_WellsWorldBehavior>()?.VirtualCamera;
            
            GetComponent<Script_TimelineController>().BindVirtualCameraAndPlayFromDirector(
                0, 0, currentWorldTileVCam
            );

            StartHeavySnow();
        }
    }

    private void OnScarletCipherPickUp(int scarletCipherId)
    {
        if (scarletCipherId == scarletCipherPieces[0].ScarletCipherId)
        {
            foreach (var scarletCipherPiece in scarletCipherPieces)
            {
                scarletCipherPiece.UpdateActiveState();
            }
        }
    }

    public void SetFireplaceExitActive(bool isActive)
    {
        wellsWorldBehaviors.ForEach(
            behavior => behavior.FireplaceExit.SetInteractionActive(isActive)
        );
    }

    // Will also open Fireplace Exit.
    public void StartHeavySnow()
    {
        wellsWorldBehaviors.ForEach(
            behavior => behavior.HeavySnow.gameObject.SetActive(true)
        );

        SetFireplaceExitActive(true);
    }

    public void StopHeavySnow()
    {
        wellsWorldBehaviors.ForEach(
            behavior => behavior.HeavySnow.gameObject.SetActive(false)
        );

        SetFireplaceExitActive(false);
    }

    // Will also affect Fireplace Exit state.
    public void SetSmallSnowActive(bool isActive)
    {
        wellsWorldBehaviors.ForEach(
            behavior => behavior.SmallSnow.gameObject.SetActive(isActive)
        );

        SetFireplaceExitActive(isActive);
    }

    /// <summary>
    /// When Player destroys one Well, they should all be destroyed so all the World
    /// Tiles can remain in sync.
    /// </summary>
    public void OnFrozenWellDie(Script_FrozenWellCrackableStats iceStats)
    {
        Script_FrozenWell destroyedFrozenWell = iceStats.GetComponent<Script_FrozenWell>();

        if (destroyedFrozenWell == null)
            return;

        // If one of the frozen wells destroyed is 1 we are tracking, then destroy
        // all frozen wells.
        Script_FrozenWell matchingFrozenWell = frozenWells.FirstOrDefault(
            frozenWell => frozenWell == destroyedFrozenWell
        );

        if (matchingFrozenWell != null)
        {
            foreach (var frozenWell in frozenWells)
            {
                Debug.Log($"Setting frozenWell inactive: {frozenWell.gameObject.name}");
                frozenWell.gameObject.SetActive(false);
            }
        }
    }

    // ----------------------------------------------------------------------
    // Next Node Action START

    public void SnowReactionDone()
    {
        game.ChangeStateInteract();
    }
    
    public void CheckPlayerHasLastSpellRecipeBook()
    {
        game.ChangeStateCutScene();
        
        int slot = -1;
        bool hasBook = game.GetItemsInventoryItem(lastSpellRecipeBookItem.id, out slot) != null;

        if (hasBook)
        {
            Script_DialogueManager.DialogueManager.StartDialogueNodeNextFrame(OnHasLastSpellRecipeBookNode);

            game.RemoveItemFromInventory(lastSpellRecipeBookItem);
        }
        else
        {
            Script_DialogueManager.DialogueManager.StartDialogueNodeNextFrame(OnMissingLastSpellRecipeBookNode);
        }
    }

    public void GiveSticker()
    {
        Debug.Log("-------- MOOSE GIVES PLAYER AESTHETIC STICKER --------");

        Script_DialogueManager.DialogueManager.StartDialogueNode(OnMooseGiveItemDoneNode);
    }

    public void UpdateMooseName()
    {
        Debug.Log("-------- UPDATING MOOSE NAME --------");
        Script_Names.UpdateMoose();
    }

    // Moose and Suzette exit. Cut scene of paintings being done.
    public void MooseQuestDone()
    {
        game.ChangeStateCutScene();
        
        isMooseQuestDone                = true;
        isCurrentMooseQuestComplete     = true;
        
        StartCoroutine(WaitPlayPaintingQuestTimeline());

        IEnumerator WaitPlayPaintingQuestTimeline()
        {
            yield return new WaitForSeconds(waitTimeBeforeCastMooseFinalSpell);            
            
            // Play Timeline for finishing Moose's quest
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
        }
    }

    public void OnMooseCheckItemDialogueDone()
    {
        game.NextFrameChangeStateInteract();
    }

    public void WellsInitialState()
    {
        foreach (Script_FrozenWell frozenWell in frozenWells)
            frozenWell.InitialState();
    }
    
    // ----------------------------------------------------------------------
    // Timeline Signals
    public void FreezeWells()
    {
        // Play SFX Once here and disable for the Wells so all don't play at once.
        foreach (Script_FrozenWell frozenWell in frozenWells)
            frozenWell.Freeze(_isSFXOn: false);

        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.Freeze, Script_SFXManager.SFX.FreezeVol
        );
    }

    /// <summary>
    /// If haven't played the Snow Reaction yet, play it first, and let it handle game state.
    /// </summary>
    public void EndFreezeCutScene()
    {
        if (!HandleSnowReaction(beforeSnowReactionWaitTime))
            game.ChangeStateInteract();
    }

    // Called after Moose checks Player has Last Spell Book.
    public void MooseExit()
    {
        foreach (var Moose in Mooses)
            Moose.FinalSpellExit();
    }

    public void HandleScarletCipherPieces()
    {
        foreach (var scarletCipherPiece in scarletCipherPieces)
        {
            scarletCipherPiece?.UpdateActiveState();
        }
    }

    public void FinishQuestPaintings()
    {
        ballroomPaintingEntrance.DonePainting();

        foreach (var paintingEntrance in paintingEntrances)
        {
            paintingEntrance.DonePainting();
        }
    }

    public void OnWellsWorldPaintingQuestDone()
    {
        Script_TransitionManager.Control.OnCurrentQuestDone(() => {
            // Play TalkingSelf Timeline if haven't played yet.
            if (!didPlayFaceOff)
            {
                var PRCSManager = Script_PRCSManager.Control;

                PRCSManager.TalkingSelfSequence(() => {
                    PRCSManager.PlayFaceOffTimeline(() => {
                        game.ChangeStateInteract();
                        didPlayFaceOff = true;
                    });
                });
            }
            else
            {
                game.ChangeStateInteract();
            }
        });
    }
    // ----------------------------------------------------------------------

    private void SetMapsActive(bool isActive)
    {
        for (int i = 0; i < lastWellMaps.Length; i++)
        {
            if (lastWellMaps[i] != null)
                lastWellMaps[i].gameObject.SetActive(isActive);
        }
    }

    private void SetMoosesActive(bool isActive)
    {
        foreach (var Moose in Mooses)
            Moose.gameObject.SetActive(isActive);
    }

    private void SetSuzettesActive(bool isActive)
    {
        foreach (var Suzette in Suzettes)
            Suzette.gameObject.SetActive(isActive);
    }

    public override void Setup()
    {
        wellsPuzzleController.InitialState();

        // Only Spawn Last Well Map if Player has not picked it up.
        if (didPickUpLastWellMap)
            SetMapsActive(false);
        else
            SetMapsActive(true);

        if (isCurrentMooseQuestComplete)
        {
            SetMoosesActive(false);
            SetSuzettesActive(false);
        }
        else
        {
            SetMoosesActive(true);
            SetSuzettesActive(true);
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_LevelBehavior_42))]
    public class Script_LevelBehavior_42Tester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LevelBehavior_42 t = (Script_LevelBehavior_42)target;
            if (GUILayout.Button("Activate Fireplace Exit"))
            {
                t.SetFireplaceExitActive(true);
            }

            if (GUILayout.Button("Disable Fireplace Exit"))
            {
                t.SetFireplaceExitActive(false);
            }

            if (GUILayout.Button("Start Heavy Snow"))
            {
                t.StartHeavySnow();
            }

            if (GUILayout.Button("Stop Heavy Snow"))
            {
                t.StopHeavySnow();
            }

            GUILayout.Space(12);

            if (GUILayout.Button("Destroy Frozen Wells"))
            {
                t.OnFrozenWellDie(t.frozenWells[0].GetComponent<Script_FrozenWellCrackableStats>());
            }

            GUILayout.Space(12);

            if (GUILayout.Button("Moose Quest Done"))
            {
                t.MooseQuestDone();
            }

            if (GUILayout.Button("Wells Initial State"))
            {
                t.WellsInitialState();
            }
        }
    }
    #endif
}
