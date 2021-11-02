using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Quest completion: talk to Ursie after finishing LB24 puzzle
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_29 : Script_LevelBehavior
{
    [SerializeField] private Script_PuppetMaster puppetMaster;
    [SerializeField] private Script_Marker puppetMasterSpawn;

    [SerializeField] private Script_TileMapExitEntrance KTVLobbyDoor;
    [SerializeField] private Script_Marker sideOfRoomMarker;
    
    // Needed to initialize "Player" copy.
    [SerializeField] private Script_LevelGrid levelGrid;
    
    private bool isInitialized = false;

    private bool LeftSideOfRoom
    {
        get => puppetMaster.transform.position.x  < sideOfRoomMarker.Position.x;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        HandleMelbaFlipX();
    }
    
    // ------------------------------------------------------------------
    // Unity Events
    
    // Puppeteer Deactivate Event
    public void HandleMelbaFlipX()
    {
        // Melba Sprite should be flipped if is left of flipMarker.
        puppetMaster.FlipSprite(LeftSideOfRoom, puppetMaster.SpriteYFlip);
    }

    // Puppeteer Activate Event
    public void FlipXDefault()
    {
        puppetMaster.FlipSprite(false, false);

        // Handle Player coming out only left or right depending on side of room.
        puppetMaster.FaceDirection(LeftSideOfRoom ? Directions.Right : Directions.Left);
    }
    
    // Trigger.
    public void HandleDoorState(bool isOpen)
    {
        KTVLobbyDoor.IsDisabled = !isOpen;
    }

    // ------------------------------------------------------------------
    
    public override void Setup()
    {
        if (!isInitialized)
        {
            InitializePuppets();
        }

        void InitializePuppets()
        {
            Model_PlayerState puppetMasterStartState = new Model_PlayerState(
                (int)puppetMasterSpawn.transform.position.x,
                (int)puppetMasterSpawn.transform.position.y,
                (int)puppetMasterSpawn.transform.position.z,
                puppetMasterSpawn.Direction
            );
            
            puppetMaster.Setup(puppetMasterStartState.faceDirection, puppetMasterStartState);
            puppetMaster.InitializeOnLevel(puppetMasterStartState, levelGrid.transform);
            
            isInitialized = true;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_29))]
public class Script_LevelBehavior_29Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_29 t = (Script_LevelBehavior_29)target;
        
        if (GUILayout.Button("HandleMelbaFlipX"))
        {
            t.HandleMelbaFlipX();
        }
    }
}
#endif   