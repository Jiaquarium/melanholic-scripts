using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only play Ids music if home.
/// 
/// Weekday:
///     - only there on Last Day.
/// 
/// Weekend:
///     - if didn't talk to Ids on First Day, won't be there on second day.
/// </summary>
public class Script_LevelBehavior_9 : Script_LevelBehavior
{
    public Script_ProximitySpeaker speaker; // to be set by code
    [SerializeField] private float speakerFadeOutTime;
    
    [Space]
    
    [SerializeField] private float speakerDistance;
    [SerializeField] private Script_ProximitySpeaker speakerPrefab;
    [SerializeField] private Script_Marker speakerLoc;

    [SerializeField] private Script_InteractableFullArt IdsNote;
    [SerializeField] private Script_TileMapExitEntrance exitToIdsRoom;

    [SerializeField] private Script_LevelBehavior_10 IdsRoom;

    private bool didFadeOutSpeaker;
    
    void Awake()
    {
        speaker = null; // needs to be null so we can instantiate a global speaker
    }

    protected override void Update()
    {
        base.Update();

        if (game.RunCycle == Script_RunsManager.Cycle.Weekend)
        {
            var eventCycleManager = Script_EventCycleManager.Control;
            
            if (
                speaker != null
                && speaker.gameObject.activeInHierarchy
                && (eventCycleManager.IsIdsInSanctuary() || eventCycleManager.IsIdsDead())
                && !didFadeOutSpeaker
            )
            {
                speaker.GetComponent<Script_AudioSourceFader>().FadeOut(speakerFadeOutTime);
                didFadeOutSpeaker = true;
            }
        }
    }
    
    void HandleSpeakerRegen()
    {
        // if speaker was destroyed in lvl 10, then create a new one
        if (speaker == null)
        {
            speaker = Instantiate(
                speakerPrefab,
                speakerLoc.transform.position,
                Quaternion.identity
            );
            speaker.transform.SetParent(game.bgThemeSpeakersContainer, false);
            speaker.MaxDistance = speakerDistance;
            
            speaker.audioSource.clip = Script_BackgroundMusicManager.Control.GetClip(IdsRoom.BGMIdx);

            if (!speaker.audioSource.isPlaying)
                speaker.audioSource.Play();
        }
    }
    
    public override void Setup()
    {
        if (!didFadeOutSpeaker)
            HandleSpeakerRegen();
        
        HandleSpeaker();

        if (game.RunCycle == Script_RunsManager.Cycle.Weekday)
        {
            HandleIdsHome();
        }
        else
        {
            // If is second day (Fri) and did not talk, then Ids is not home but is unlocked.
            if (
                Script_EventCycleManager.Control.IsIdsInSanctuary()
                || Script_EventCycleManager.Control.IsIdsDead()
            )
            {
                HandleIdsNotHomeNotLocked();
            }
            else
            {
                HandleIdsHome();
            }
        }

        /// <summary>
        /// Note: if Player finished DDR quest, then no BGM.
        /// </summary>
        void HandleIdsHome()
        {
            exitToIdsRoom.IsDisabled = false;
            IdsNote.gameObject.SetActive(false);
        }

        void HandleIdsNotHomeNotLocked()
        {
            exitToIdsRoom.IsDisabled = false;
            IdsNote.gameObject.SetActive(false);
        }

        void HandleSpeaker()
        {
            var eventCycleManager = Script_EventCycleManager.Control;
            
            var isSpeakerOnWeekday = !IdsRoom.isCurrentPuzzleComplete;
            var isSpeakerOnWeekend = !IdsRoom.isCurrentPuzzleComplete
                && !eventCycleManager.IsIdsInSanctuary()
                && !eventCycleManager.IsIdsDead();
            
            var isSpeakerOn = game.RunCycle == Script_RunsManager.Cycle.Weekend ?
                isSpeakerOnWeekend : isSpeakerOnWeekday;

            speaker.gameObject.SetActive(isSpeakerOn);
        }
    }
}
