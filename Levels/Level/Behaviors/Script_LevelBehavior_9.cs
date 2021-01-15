using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_9 : Script_LevelBehavior
{
    public Script_ProximitySpeaker speaker; // to be set by code
    [Space]
    [SerializeField] private Script_ProximitySpeaker speakerPrefab;
    [SerializeField] private Script_Marker speakerLoc;
    [SerializeField] private Transform signTextParent;
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
        game.SetupInteractableObjectsText(signTextParent, !isInitialized);
        /// Only play music on Sunday when Ids is there
        HandleSpeakerRegen();
        
        /// Disable exit and show Ids note if not Sunday
        if (game.Run.dayId == Script_Run.DayId.sun)
        {
            IdsNote.gameObject.SetActive(false);
            exitToIdsRoom.IsDisabled = false;
            speaker.gameObject.SetActive(true);
        }
        else
        {
            IdsNote.gameObject.SetActive(true);
            exitToIdsRoom.IsDisabled = true;
            speaker.gameObject.SetActive(false);
        }

        isInitialized = true;
    }
}
