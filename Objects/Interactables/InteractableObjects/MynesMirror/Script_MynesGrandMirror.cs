using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Not included in the Scarlet Cipher mirrors.
/// 
/// After Grand Mirror activation is done; switch graphic to broken.
/// </summary>
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
    private bool isActivated;
    
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

    protected override void Awake()
    {
        base.Awake();
        
        Initialize();
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

        // Change mirror to broken state.
        mirrorGraphics.sprite = brokenMirrorSprite;
        
        Script_PRCSManager.Control.ClosePRCSCustom(Script_PRCSManager.CustomTypes.MynesMirror, () => {
            isActivated = true;
            game.ChangeStateInteract();
            SetWeekendCycleState();
        });

        void SetWeekendCycleState()
        {
            Script_Game.Game.SetBayV1ToSaveState(Script_LevelBehavior_33.State.SaveAndStartWeekendCycle);
        }
    }

    protected override bool CheckDisabled()
    {
        return
        (
            isActivated || CheckDisabledDirections()
        );
    }
    
    // Grand Mirror doesn't need this mechanic. Will always be an unbroken mirror.
    protected override void HandleIsSolvedGraphics(bool isSolved) { }

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
