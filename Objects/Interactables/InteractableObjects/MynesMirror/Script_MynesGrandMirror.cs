using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Not included in the Scarlet Cipher mirrors.
/// 
/// After Grand Mirror activation is done; switch graphic to broken.
/// 
/// Section is switched to decide which Dialogue to use after a non-mirror cut scene.
/// </summary>
public class Script_MynesGrandMirror : Script_MynesMirror
{
    private enum Section
    {
        Intro = 0,
        NewWorldPaintings = 1,
        ItemGive = 2
    }
    
    [SerializeField] private Script_StickerObject stickerObject;
    [SerializeField] private Script_DialogueNode onNewWorldPaintingsTimelineDoneNode;
    [SerializeField] private Script_DialogueNode onStickerGiveDoneNode;
    [SerializeField] private Script_ExitMetadataObject exit;

    [SerializeField] private PlayableDirector newWorldPaintingsDirector;

    [SerializeField] private SpriteRenderer[] grandMirrorGlassGraphics;

    private Section currentSection;
    private bool isActivated;
    
    protected override Sprite MirrorGraphicsSprite
    {
        get => grandMirrorGlassGraphics != null && grandMirrorGlassGraphics.Length > 0
            ? grandMirrorGlassGraphics[0].sprite
            : null;
        set
        {
            foreach (var spriteRenderer in grandMirrorGlassGraphics)
                spriteRenderer.sprite = value;
        }
    }

    private SpriteRenderer[] GrandMirrorGlassGraphics
    {
        get => grandMirrorGlassGraphics;
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

    protected override void Awake()
    {
        base.Awake();
        
        Initialize();
    }

    public override void StartDialogue()
    {
        switch (currentSection)
        {
            case (Section.Intro):
                Script_DialogueManager.DialogueManager.StartDialogueNode(HintNode);
                break;
            case (Section.NewWorldPaintings):
                Script_DialogueManager.DialogueManager.StartDialogueNode(onNewWorldPaintingsTimelineDoneNode);
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
            SetWeekendCycleState();
            
            Script_ArtFrameManager.Control.Close(() => {
                game.ChangeStateInteract();
            });
        });

        void SetWeekendCycleState()
        {
            Script_Game.Game.SetBayV1ToSaveState(Script_LevelBehavior_33.State.SaveAndStartWeekendCycle);
        }
    }

    public void SetMirrorGraphics(bool isDefault, int i)
    {
        GrandMirrorGlassGraphics[i].sprite = isDefault ? defaultMirrorSprite : brokenMirrorSprite;
    }

    // ------------------------------------------------------------------
    // Next Node Actions

    public void PlayNewWorldPaintingsTimeline()
    {
        Script_PRCSManager.Control.ClosePRCSCustom(Script_PRCSManager.CustomTypes.MynesMirror, () => {
            newWorldPaintingsDirector.Play();
        });
    }

    // ------------------------------------------------------------------
    // Timeline Signals

    public void GiveSticker()
    {
        Script_PRCSManager.Control.ClosePRCSCustom(Script_PRCSManager.CustomTypes.MynesMirror, () => {
            game.HandleItemReceive(stickerObject);
        });
    }
    
    public void OnNewWorldPaintingsTimelineDone()
    {
        currentSection = Section.NewWorldPaintings;
        Script_PRCSManager.Control.OpenPRCSCustom(Script_PRCSManager.CustomTypes.MynesMirrorMidConvo);

        // This will then trigger the Script_MynesMirrorEventsManager.OnEndTimeline event which will
        // call StartDialogue, starting dialogue based on the Current Section.
    }

    // ------------------------------------------------------------------

    protected override bool CheckDisabled()
    {
        return
        (
            isActivated || CheckDisabledDirections()
        );
    }
    
    // Grand Mirror doesn't need this mechanic. Will always be an unbroken mirror.
    protected override void HandleIsActivatedGraphics(bool isActivated) {}

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

#if UNITY_EDITOR
[CustomEditor(typeof(Script_MynesGrandMirror))]
public class Script_MynesGrandMirrorTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_MynesGrandMirror t = (Script_MynesGrandMirror)target;
        if (GUILayout.Button("On New World Paintings Timeline Done"))
        {
            t.OnNewWorldPaintingsTimelineDone();
        }
    }
}
#endif