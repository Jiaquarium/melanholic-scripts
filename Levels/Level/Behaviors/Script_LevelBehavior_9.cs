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
    [Space]
    [SerializeField] private Script_ProximitySpeaker speakerPrefab;
    [SerializeField] private Script_Marker speakerLoc;
    [SerializeField] private Script_InteractableFullArt IdsNote;
    [SerializeField] private Script_TileMapExitEntrance exitToIdsRoom;
    private bool isInitialized;
    
    void Awake()
    {
        speaker = null; // needs to be null so we can instantiate a global speaker
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
        }
    }
    
    public override void Setup()
    {
        HandleSpeakerRegen();
        
        if (game.RunCycle == Script_RunsManager.Cycle.Weekday)
        {
            // Disable exit and show Ids note if not Wed (last day) on Weekday Cycle.
            if (Script_EventCycleManager.Control.IsIdsHome())   HandleIdsHome();
            else                                                HandleIdsNotHomeLocked();
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

        isInitialized = true;

        void HandleIdsHome()
        {
            exitToIdsRoom.IsDisabled = false;
            IdsNote.gameObject.SetActive(false);
            speaker.gameObject.SetActive(true);
        }

        void HandleIdsNotHomeLocked()
        {
            exitToIdsRoom.IsDisabled = true;
            IdsNote.gameObject.SetActive(true);
            speaker.gameObject.SetActive(false);
        }

        void HandleIdsNotHomeNotLocked()
        {
            exitToIdsRoom.IsDisabled = false;
            IdsNote.gameObject.SetActive(false);
            speaker.gameObject.SetActive(false);
        }
    }
}
