using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_48 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    [SerializeField] private bool isDone;
    // ==================================================================

    [SerializeField] private int glitchZoneSteps;
    [SerializeField] private float waitTimeAfterAwakening;
    
    [SerializeField] private Script_Snow snowEffectAlways;

    [SerializeField] private PlayableDirector newWorldPaintingsDirector;

    [SerializeField] private Script_BgThemePlayer northWindPlayer;

    [SerializeField] private Script_CrackableStats iceBlockStatsLeft;
    [SerializeField] private Script_CrackableStats iceBlockStatsMid;
    [SerializeField] private Script_CrackableStats iceBlockStatsRight;

    [SerializeField] private bool isIceBlockLeftCracked;
    [SerializeField] private bool isIceBlockMidCracked;
    [SerializeField] private bool isIceBlockRightCracked;

    [SerializeField] private List<GameObject> iceBlockTimelineObjects;
    [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;
    [SerializeField] private List<Script_InteractablePaintingEntrance> worldPaintings;

    [SerializeField] private TimelineAsset newWorldPaintingRevealTimeline;

    [SerializeField] private Script_LevelBehavior_20 ballroomBehavior;

    [SerializeField] private Script_Sticker lastElevatorMask;
    [SerializeField] private int targetEquipmentSlot;

    [SerializeField] private Script_ItemDialogueNode lastElevatorMaskDialogue;

    private Script_TimelineController timelineController;
    private Script_CrackableStats currentIceBlockStats;
    private float currentTargetBlend;
    private Script_GlitchFXManager glitchManager;

    public bool IsDone
    {
        get => isDone;
        set => isDone = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Script_InteractableObjectEventsManager.OnIceCrackingTimelineDone += PlayRevealNewWorldTimeline;

        currentTargetBlend = 0f;
        glitchManager.InitialState();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Script_InteractableObjectEventsManager.OnIceCrackingTimelineDone -= PlayRevealNewWorldTimeline;

        glitchManager.InitialState();
    }
    
    void Awake()
    {
        glitchManager = Script_GlitchFXManager.Control;
        
        timelineController = GetComponent<Script_TimelineController>();
        
        snowEffectAlways.gameObject.SetActive(true);
        
        // Initialize World Painting States for Reveal Cut Scenes.
        ballroomBehavior.SetPaintingEntrancesActive(false);
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void SetWorldPaintingSketch()
    {
        if (currentIceBlockStats == iceBlockStatsLeft)
        {
            worldPaintings[0].SetSketchAnimation();   
        }
        else if (currentIceBlockStats == iceBlockStatsMid)
        {
            worldPaintings[1].SetSketchAnimation();   
        }
        else if (currentIceBlockStats == iceBlockStatsRight)
        {
            worldPaintings[2].SetSketchAnimation();
        }
    }

    public void SetWorldPaintingStateActive()
    {
        if (currentIceBlockStats == iceBlockStatsLeft)
        {
            worldPaintings[0].SetStateActive();   
        }
        else if (currentIceBlockStats == iceBlockStatsMid)
        {
            worldPaintings[1].SetStateActive();   
        }
        else if (currentIceBlockStats == iceBlockStatsRight)
        {
            worldPaintings[2].SetStateActive();
        }
    }

    // Called from end of NewWorldRevealPaintingTimeline
    public void OnNewWorldRevealPaintingDone()
    {
        // If all three ice blocks have been destroyed, start the Awakening scene.
        if (IsAllIceBlocksCracked())
        {
            AwakeningCutscene();
        }
        else
        {
            game.ChangeStateInteract();
        }
        
        void AwakeningCutscene()
        {
            // Start heavy glitch effect if was third ice stat.
            glitchManager.SetHigh();
            glitchManager.BlendTo(1f, 5f, () => Script_TransitionManager.Control.FadeInCoroutine(
                Script_TransitionManager.FadeTimeSlow,
                () => FadeInStartTimeline()
            ));

            void FadeInStartTimeline()
            {
                glitchManager.SetDefault();
                glitchManager.SetBlend(0f);
                
                // Show Awakening canvas immediately (fader transition will handle fading).
                Script_PRCSManager.Control.SetAwakeningActive(true);

                Script_TransitionManager.Control.FadeOutCoroutine(Script_TransitionManager.FadeTimeSlow);
                
                // Awakening Timeline's TimelineTeletypeReveal's UnityEvents will enable/disable
                // Eye's animator during Pause/Play states (Timeline loses control when paused).
                timelineController.PlayableDirectorPlayFromTimelines(1, 1);
            }
        }
    }

    public void OnAwakeningTimelineDone()
    {
        game.ChangeStateCutScene();
        
        // Set Fade Canvas Active
        Script_TransitionManager.Control.TimelineBlackScreen();

        Script_PRCSManager.Control.SetAwakeningActive(false);
        
        Debug.Log("Waiting for next part in sequence!!!");

        // Wait a few seconds for black screen to stay up and then fade out.
        EquipLastElevatorMaskBackground();

        glitchManager.SetLow();
        glitchManager.SetBlend(1f);

        StartCoroutine(WaitToFadeOut());

        IEnumerator WaitToFadeOut()
        {
            yield return new WaitForSeconds(waitTimeAfterAwakening);

            Script_TransitionManager.Control.TimelineFadeOut(
                Script_TransitionManager.FadeTimeSlow,
                () => Script_DialogueManager.DialogueManager.StartDialogueNode(lastElevatorMaskDialogue)
            );            
        }
    }

    private void EquipLastElevatorMaskBackground()
    {
        // Unequip all Prepped Masks.
        game.UnequipAll();

        // Give Last Elevator mask in background.
        game.AddItemById(lastElevatorMask.id);

        // Set as Prepped Mask in background.
        game.AddEquippedItemInSlotById(lastElevatorMask.id, targetEquipmentSlot);

        // Set as active in background.
        game.GetPlayer().ForceStickerSwitchBackground(targetEquipmentSlot);
    }
    
    // ------------------------------------------------------------------
    // Unity Events

    public void OnIceBlockCracked(Script_CrackableStats ice)
    {
        Debug.Log($"Cracked Left Ice Block <{ice}>");

        game.ChangeStateCutScene();
    }

    public void PlayRevealNewWorldTimeline(Script_CrackableStats iceStats)
    {
        Debug.Log($"Reacting to IceCrackingTimelineDone event with iceStats <{iceStats}>");
        
        currentIceBlockStats = iceStats;

        // Bind the proper objects depending on which crackable stats.
        if (currentIceBlockStats == iceBlockStatsLeft)
        {
            isIceBlockLeftCracked = true;
            newWorldPaintingsDirector.BindTimelineTracks(newWorldPaintingRevealTimeline, iceBlockTimelineObjects);
            timelineController.BindVirtualCameraAndPlayFromDirector(0, 0, virtualCameras[0]);
        }
        else if (currentIceBlockStats == iceBlockStatsMid)
        {
            isIceBlockMidCracked = true;
            newWorldPaintingsDirector.BindTimelineTracks(newWorldPaintingRevealTimeline, iceBlockTimelineObjects);
            timelineController.BindVirtualCameraAndPlayFromDirector(0, 0, virtualCameras[1]);
        }
        else if (currentIceBlockStats == iceBlockStatsRight)
        {
            isIceBlockRightCracked = true;
            newWorldPaintingsDirector.BindTimelineTracks(newWorldPaintingRevealTimeline, iceBlockTimelineObjects);
            timelineController.BindVirtualCameraAndPlayFromDirector(0, 0, virtualCameras[2]);
        }
    }

    // Zone trigger events
    public void SetNorthWindActive(bool isActive)
    {
        game.GetPlayer().IsNorthWind = isActive;

        if (isActive)
            northWindPlayer.FadeInPlay();
        else
        {
            // Abrubtly stop music when player clears the top of wind zone.
            Directions lastMove = game.GetPlayer().LastMove;
            
            if (lastMove == Directions.Up)
                northWindPlayer.SoftStop();
            else
                northWindPlayer.FadeOutStop();
        }
    }

    public void IncreaseGlitchBlend()
    {
        float blendStep = 1f / glitchZoneSteps;
        currentTargetBlend += blendStep;

        // Ensure current settings is Default.
        glitchManager.SetDefault(useCurrentBlend: true);
        glitchManager.BlendTo(currentTargetBlend);
    }

    public void DecreaseGlitchBlend()
    {
        float blendStep = 1f / glitchZoneSteps;
        currentTargetBlend -= blendStep;

        // Ensure current settings is Default.
        glitchManager.SetDefault(useCurrentBlend: true);
        
        // Once player clears the top of the wind zone, immediately set Blend for abrubt effect.
        Directions lastMove = game.GetPlayer().LastMove;
        
        if (lastMove == Directions.Up)
            glitchManager.SetBlend(currentTargetBlend);
        else
            glitchManager.BlendTo(currentTargetBlend);
        }

    // ------------------------------------------------------------------
    // Next Node Actions

    public void OnLastElevatorDialogueDone()
    {
        game.ChangeStateInteract();
    }

    // ------------------------------------------------------------------

    private bool IsAllIceBlocksCracked()
    {
        return isIceBlockLeftCracked && isIceBlockMidCracked && isIceBlockRightCracked;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_LevelBehavior_48))]
    public class Script_LevelBehavior_48Tester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_LevelBehavior_48 t = (Script_LevelBehavior_48)target;
            
            if (GUILayout.Button("Switch to Last Elevator Background"))
            {
                t.EquipLastElevatorMaskBackground();
            }

            if (GUILayout.Button("On Awakening Timeline Done"))
            {
                t.OnAwakeningTimelineDone();
            }
        }
    }
#endif   
}