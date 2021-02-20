using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MynesGrandMirror : Script_MynesMirror
{
    private enum Section
    {
        Intro = 0,
        ItemGive = 1
    }
    
    [SerializeField] private Script_StickerObject stickerObject;
    [SerializeField] private Script_DialogueNode onStickerGiveDoneNode;
    [SerializeField] private Script_ExitMetadataObject exit;

    private Section currentSection;
    
    protected override void Awake()
    {
        base.Awake();
        
        Initialize();
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        Script_ItemsEventsManager.OnItemStash += OnItemPickUpTheatricsDone;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        Script_ItemsEventsManager.OnItemStash -= OnItemPickUpTheatricsDone;
    }
    
    public void GiveSticker()
    {
        Script_PRCSManager.Control.ClosePRCSCustom(Script_PRCSManager.CustomTypes.MynesMirror, () => {
            game.HandleItemReceive(stickerObject);
        });
    }

    public override void StartDialogue()
    {
        switch (currentSection)
        {
            case (Section.Intro):
                Script_DialogueManager.DialogueManager.StartDialogueNode(MynesConversationNode);
                break;
            case (Section.ItemGive):
                Script_DialogueManager.DialogueManager.StartDialogueNode(onStickerGiveDoneNode);
                break;
        }
    }

    /// <summary>
    /// Remove cut scene
    /// Called from last Dialogue Node
    /// Exit to Bay v1
    /// </summary>
    public override void End()
    {
        // Fade out BG Theme Player and Fade in Game BGM
        Script_BackgroundMusicManager.Control.FadeOutMed(() => {
                bgThemePlayer.gameObject.SetActive(false);
                FadeInBGMusic();
            },
            Const_AudioMixerParams.ExposedBGVolume
        );
        
        Script_PRCSManager.Control.ClosePRCSCustom(Script_PRCSManager.CustomTypes.MynesMirror, () => {
            Exit();
        });

        // Exit to Bay v1 but ensure it's Bay V1 Save and Start Weekend Cycle State
        void Exit()
        {
            Script_Game.Game.SetBayV1ToSaveState(Script_LevelBehavior_33.State.SaveAndStartWeekendCycle);
            Script_Game.Game.Exit(
                exit.data.level,
                exit.data.playerSpawn,
                exit.data.facingDirection,
                true
            );
        }
    }

    private void OnItemPickUpTheatricsDone(string itemId)
    {
        if (itemId == stickerObject.Item.id)
        {
            currentSection = Section.ItemGive;
            Script_PRCSManager.Control.OpenPRCSCustom(Script_PRCSManager.CustomTypes.MynesMirrorMidConvo);
            // This will then trigger the Script_MynesMirrorEventsManager.OnEndTimeline event which will
            // call StartDialogue
        }
    }

    private void Initialize()
    {
        currentSection = Section.Intro;
    }
}
