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
    public const string MapName = Script_Names.UrsaSaloonHallway;
    
    [SerializeField] private Script_PuppetMaster puppetMaster;
    [SerializeField] private Script_Marker puppetMasterSpawn;

    [SerializeField] private Script_TileMapExitEntrance KTVLobbyDoor;
    [SerializeField] private Script_Marker sideOfRoomMarker;

    [SerializeField] private Script_DialogueNode[] blockingPsychicNodes;
    [SerializeField] private Script_DialogueNode[] notBlockingPsychicNodes;

    [SerializeField] private Script_DialogueNode exitReactionDefault;
    [SerializeField] private Script_DialogueNode exitReactionPsychic;
    [SerializeField] private float waitBeforeExitReactionTime;
    
    // Needed to initialize "Player" copy.
    [SerializeField] private Script_LevelGrid levelGrid;

    [SerializeField] private Script_PhysicsBox puppeteerAreaOfEffect;
    
    private Script_DemonNPC MelbaDemonNPC;
    private bool isInitialized = false;

    private bool LeftSideOfRoom
    {
        get => puppetMaster.transform.position.x  < sideOfRoomMarker.Position.x;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable() {
        base.OnDisable();
    }

    void Awake()
    {
        MelbaDemonNPC = puppetMaster.GetComponent<Script_DemonNPC>();

        if (MelbaDemonNPC == null)
            Debug.LogWarning($"PuppetMaster <{puppetMaster}> does not have DemonNPC component");
    }

    public bool CheckInsidePuppeteerAreaOfEffect()
    {
        return puppeteerAreaOfEffect.CheckPlayerOverlap();
    }

    // Only switch if they are different nodes. Maintain the talking state.
    private void HandleMelbaDialogueNodes()
    {
        if (KTVLobbyDoor.IsDisabled && MelbaDemonNPC.PsychicNodes != blockingPsychicNodes)
            MelbaDemonNPC.SwitchPsychicNodes(blockingPsychicNodes);
        else if (!KTVLobbyDoor.IsDisabled && MelbaDemonNPC.PsychicNodes != notBlockingPsychicNodes)
            MelbaDemonNPC.SwitchPsychicNodes(notBlockingPsychicNodes);
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
    
    // Trigger
    public void OnTriggerEnter()
    {
        KTVLobbyDoor.IsDisabled = true;
        HandleMelbaDialogueNodes();
    }

    // Trigger
    public void OnTriggerExit()
    {
        KTVLobbyDoor.IsDisabled = false;
        HandleMelbaDialogueNodes();
    }

    // KTV Door Disabled Reaction
    public void OnTryExit()
    {
        game.ChangeStateCutScene();

        StartCoroutine(WaitBeforeExitReaction());

        // Note: this will not trigger "Can't Understand" Dialogue.
        IEnumerator WaitBeforeExitReaction()
        {
            yield return new WaitForSeconds(waitBeforeExitReactionTime);
            
            bool isPsychicDuckActive = Script_ActiveStickerManager.Control.IsActiveSticker(Const_Items.PsychicDuckId);
            
            var node = isPsychicDuckActive ? exitReactionPsychic : exitReactionDefault;
            exitReactionDefault.data.isZalgofy = true;

            Script_DialogueManager.DialogueManager.StartDialogueNode(node);
        }
    }

    // ------------------------------------------------------------------
    // Next Node Actions

    public void SendPlayerBackToXXXWorld()
    {
        game.ChangeStateInteract();
        game.TeleportToXXXWorldSaloonExit();
    }

    // ------------------------------------------------------------------
    
    private void InitializePuppets()
    {
        Model_PlayerState puppetMasterStartState = new Model_PlayerState(
            (int)puppetMasterSpawn.transform.position.x,
            (int)puppetMasterSpawn.transform.position.y,
            (int)puppetMasterSpawn.transform.position.z,
            puppetMasterSpawn.Direction
        );
        
        puppetMaster.Setup(puppetMasterStartState.faceDirection, puppetMasterStartState);
        puppetMaster.InitializeOnLevel(puppetMasterStartState, levelGrid.transform);
        HandleMelbaFlipX();
    }

    public override void InitialState()
    {
        InitializePuppets();
        KTVLobbyDoor.IsDisabled = true;
        isInitialized = true;

        HandleMelbaDialogueNodes();
    }

    public override void Setup()
    {
        if (game.KTVRoom2Behavior.IsCurrentPuzzleComplete)
        {
            puppetMaster.gameObject.SetActive(false);
            KTVLobbyDoor.IsDisabled = false;
            return;
        }
        
        // Re-initialize level when coming back from XXX World to avoid edge case
        // where Melba is placed in front of the Exit and Player enters on top.
        if (!isInitialized || game.LastLevelBehavior != game.SaloonBehavior)
            InitialState();
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

        if (GUILayout.Button("CheckInsidePuppeteerAreaOfEffect"))
        {
            t.CheckInsidePuppeteerAreaOfEffect();
        }
    }
}
#endif   