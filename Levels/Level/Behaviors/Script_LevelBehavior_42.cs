using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Playables;

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
    public const string MapName = Script_Names.WellsWorld;
    
    // ==================================================================
    // State Data
    public bool didPickUpLastWellMap;
    public bool didPickUpSpeedSeal;
    public bool isMooseQuestDone;
    public bool didPlayFaceOff;
    // ==================================================================
    
    public bool isCurrentMooseQuestComplete;

    [SerializeField] private float waitTimeBeforeCastMooseFinalSpell;

    [SerializeField] private Script_WellsPuzzleController wellsPuzzleController;
    
    [SerializeField] private Script_FrozenWell[] frozenWells;
    
    [SerializeField] private Script_InteractablePaintingEntrance[] paintingEntrances;
    [SerializeField] private Script_InteractablePaintingEntrance ballroomPaintingEntrance;
    
    [SerializeField] private Script_Item lastSpellRecipeBookItem;
    
    [SerializeField] private Script_DialogueNode OnHasLastSpellRecipeBookNode;
    [SerializeField] private Script_DialogueNode OnMissingLastSpellRecipeBookNode;
    [SerializeField] private Script_DialogueNode OnMooseGiveItemDoneNode;
    
    [SerializeField] private Script_DialogueNode OnSnowReactionNode;
    
    // A bit longer wait time to give Player time to absorb the snow effect
    [SerializeField] private float beforeSnowReactionWaitTime;
    
    [SerializeField] private Script_LevelBehavior_13 CatWalkBehavior;

    [SerializeField] private Script_Moose[] Mooses;
    [SerializeField] private Script_DemonNPC[] Suzettes;
    
    [SerializeField] private Script_ScarletCipherPiece[] scarletCipherPieces;

    [SerializeField] private Script_WeatherFXManager weatherFXManager;
    [SerializeField] private List<Script_WellsWorldBehavior> wellsWorldBehaviors;

    // ------------------------------------------------------------------
    // Trailer Only
    
    [SerializeField] private PlayableDirector trailerDirector;
    [SerializeField] private float waitToPlayTrailerDirectorTime;
    
    // ------------------------------------------------------------------

    private bool didMapNotification;
    private bool didSnowReaction;

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete                            += OnLevelInitCompleteEvent;
        
        Script_PuzzlesEventsManager.OnPuzzleSuccess                             += OnPuzzleSuccess;
        Script_ItemsEventsManager.OnItemPickUp                                  += OnItemPickUp;

        Script_ScarletCipherEventsManager.OnScarletCipherPiecePickUp            += OnScarletCipherPickUp;

        Script_InteractableObjectEventsManager.OnFrozenWellDie                  += OnFrozenWellDie;
        Script_InteractableObjectEventsManager.OnShatter                        += OnShatter;
        Script_InteractableObjectEventsManager.OnWellInteraction                += OnWellInteraction;
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete                            -= OnLevelInitCompleteEvent;
        
        Script_PuzzlesEventsManager.OnPuzzleSuccess                             -= OnPuzzleSuccess;
        Script_ItemsEventsManager.OnItemPickUp                                  -= OnItemPickUp;
        
        Script_ScarletCipherEventsManager.OnScarletCipherPiecePickUp            -= OnScarletCipherPickUp;

        Script_InteractableObjectEventsManager.OnFrozenWellDie                  -= OnFrozenWellDie;
        Script_InteractableObjectEventsManager.OnShatter                        -= OnShatter;
        Script_InteractableObjectEventsManager.OnWellInteraction                -= OnWellInteraction;
    }

    void Awake()
    {
        if (weatherFXManager.IsSnowDay)
            SetSmallSnowActive(true);
        else
            SetSmallSnowActive(false);
    }

    protected override void Update()
    {
        if (Const_Dev.IsTrailerMode)
            HandleTrailerPan();
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

    /// <summary>
    /// Always give hint to find the fireplace if didn't finish Lantern quest.
    /// </summary>
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
        if (itemId == wellsWorldBehaviors[0].LastWellMap.Item.id)
        {
            didPickUpLastWellMap = true;
            SetMapsActive(false);
        }

        if (itemId == wellsWorldBehaviors[0].SpeedSeal.Item.id)
        {
            didPickUpSpeedSeal = true;
            SetSpeedSealActive(false);
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
            behavior => {
                behavior.FireplaceExit.SetInteractionActive(isActive);
                
                // If the Exit is enabled, then there is no fire, so
                // the text commenting on the fire should be hidden.
                behavior.FireText.gameObject.SetActive(!isActive);
            }
        );
    }

    // Will also open Fireplace Exit.
    public void StartHeavySnow()
    {
        wellsWorldBehaviors.ForEach(
            behavior => behavior.HeavySnow.gameObject.SetActive(true)
        );
    }

    public void StopHeavySnow()
    {
        wellsWorldBehaviors.ForEach(
            behavior => behavior.HeavySnow.gameObject.SetActive(false)
        );
    }

    // Will also affect Fireplace Exit state.
    public void SetSmallSnowActive(bool isActive)
    {
        wellsWorldBehaviors.ForEach(
            behavior => behavior.SmallSnow.gameObject.SetActive(isActive)
        );
    }

    /// <summary>
    /// When Player destroys one Well, hide all others that are currently not visible,
    /// so Tiles remain in sync. The current Well will be handled by its respective Timeline sequence.
    /// </summary>
    public void OnFrozenWellDie(Script_FrozenWellCrackableStats iceStats)
    {
        Script_FrozenWell destroyedFrozenWell = iceStats.GetComponent<Script_FrozenWell>();

        Dev_Logger.Debug($"Frozen Well destroyed: {destroyedFrozenWell}");

        if (destroyedFrozenWell == null)
            return;

        // If one of the frozen wells destroyed is 1 we are tracking, then destroy
        // all frozen wells.
        Script_FrozenWell matchingFrozenWell = frozenWells.FirstOrDefault(
            frozenWell => frozenWell == destroyedFrozenWell
        );

        Dev_Logger.Debug($"Matching Frozen Well destroyed: {matchingFrozenWell}");

        if (matchingFrozenWell != null)
        {
            foreach (Script_FrozenWell frozenWell in frozenWells)
            {
                // Skip the current Well which will be handled by its Timeline.
                if (matchingFrozenWell == frozenWell)
                    continue;

                Dev_Logger.Debug($"Setting frozenWell inactive: {frozenWell.gameObject.name}");
                frozenWell.gameObject.SetActive(false);
            }
        }
    }

    // When an ice block shatters, check which one it is, hide all the rest of them.
    public void OnShatter(Script_CrackableStats crackableStats)
    {
        wellsWorldBehaviors.ForEach(behavior => {
            if (behavior.SpeedSealIceBlock == crackableStats)
            {
                wellsWorldBehaviors.ForEach(_behavior => {
                    if (_behavior.SpeedSealIceBlock != crackableStats)
                        _behavior.SpeedSealIceBlock.gameObject.SetActive(false);
                });
            }
        });

        wellsWorldBehaviors.ForEach(behavior => {
            if (behavior.LastWellMapIceBlock == crackableStats)
            {
                wellsWorldBehaviors.ForEach(_behavior => {
                    if (_behavior.LastWellMapIceBlock != crackableStats)
                        _behavior.LastWellMapIceBlock.gameObject.SetActive(false);
                });
            }
        });
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
        Dev_Logger.Debug("-------- MOOSE GIVES PLAYER AESTHETIC STICKER --------");

        Script_DialogueManager.DialogueManager.StartDialogueNode(OnMooseGiveItemDoneNode);
    }

    public void UpdateMooseName()
    {
        Dev_Logger.Debug("-------- UPDATING MOOSE NAME --------");
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
        var transitionManager = Script_TransitionManager.Control;
        transitionManager.OnCurrentQuestDone(
            allQuestsDoneCb: () =>
            {
                transitionManager.FinalCutSceneAwakening();
            }, 
            defaultCb: () =>
            {
                HandlePlayFaceOff(OnSuccessDone);
            }
        );

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

        void OnSuccessDone()
        {
            // Fade back in level bgm unless it's KTV2 puzzle
            var bgm = Script_BackgroundMusicManager.Control;
            bgm.UnPause();
            bgm.FadeIn(game.ChangeStateInteract, Script_TransitionManager.FadeTimeSlow, Const_AudioMixerParams.ExposedBGVolume);
        }
    }
    // ----------------------------------------------------------------------

    private void SetMapsActive(bool isActive)
    {
        wellsWorldBehaviors.ForEach((behavior) => {
            if (behavior.LastWellMap != null)
                behavior.LastWellMap.gameObject.SetActive(isActive);
        });
    }

    private void SetSpeedSealActive(bool isActive)
    {
        wellsWorldBehaviors.ForEach((behavior) => {
            if (behavior.SpeedSeal != null)
                behavior.SpeedSeal.gameObject.SetActive(isActive);
        });
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

    private void OnWellInteraction(Script_Well well)
    {
        // Handle Well achievement
        var achievementsManager = Script_AchievementsManager.Instance;
        if (!achievementsManager.achievementsState.achWell)
            Script_AchievementsManager.Instance.UnlockWell();
    }

    // ------------------------------------------------------------------
    // Trailer Only

    private void HandleTrailerPan()
    {
        if (Input.GetButtonDown(Const_KeyCodes.TrailerCam))
        {
            trailerDirector.Stop();

            StartCoroutine(WaitToPlay());
        }

        IEnumerator WaitToPlay()
        {
            yield return new WaitForSeconds(waitToPlayTrailerDirectorTime);

            trailerDirector.Play();
        }
    }

    // ------------------------------------------------------------------

    public override void Setup()
    {
        wellsPuzzleController.InitialState();

        // Always leave fireplace open
        SetFireplaceExitActive(true);

        // Only Spawn Last Well Map if Player has not picked it up.
        if (didPickUpLastWellMap)
            SetMapsActive(false);
        else
            SetMapsActive(true);
        
        if (didPickUpSpeedSeal)
            SetSpeedSealActive(false);
        else
            SetSpeedSealActive(true);

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
