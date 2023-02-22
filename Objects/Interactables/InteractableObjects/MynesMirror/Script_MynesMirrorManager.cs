using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// Manager for Mynes Mirror Game Objects
/// States will be handled by Scarlet Cipher 
/// </summary>
public class Script_MynesMirrorManager : MonoBehaviour
{
    public static Script_MynesMirrorManager Control;

    // ==================================================================
    // State Data
    
    [SerializeField] private int interactionCount;
    [SerializeField] private bool didSealingDialogue;
    
    // ==================================================================

    [SerializeField] private Script_DialogueNode[] interactionNodes;

    [SerializeField] private Script_CanvasGroupController mynePortraitDefault;
    [SerializeField] private Script_CanvasGroupController mynePortraitGlare;
    [SerializeField] private Script_CanvasGroupController mynePortraitFeignedConcern;
    [SerializeField] private Script_CanvasGroupController mynePortraitConfident;

    [SerializeField] private List<GameObject> introTimelineObjectsToBind;
    
    public int InteractionCount
    {
        get => interactionCount;
        set => interactionCount = value;
    }

    public Script_DialogueNode ActiveNode
    {
        get => interactionNodes[interactionCount];
    }

    public bool DidInteract
    {
        get => interactionCount > 0;
    }

    public bool DidSealingDialogue
    {
        get => didSealingDialogue;
        set => didSealingDialogue = value;
    }

    public GameObject MynePortraitToBind { get; set; }
    
    /// <summary>
    /// Set MynePortraitToBind to the object to be bound to Intro timeline
    /// </summary>
    public void InitializeMynePortrait(Script_DialogueNode node)
    {
        CloseAllPortraits();

        Model_DialogueSection[] sections = node.data.dialogue.sections;

        if (sections.Length > 0 && sections[0].fullArtOverride != FullArtPortrait.None)
        {
            FullArtPortrait portraitType = sections[0].fullArtOverride;
            MynePortraitToBind = GetMynePortrait(portraitType).gameObject;
        }
        else
        {
            MynePortraitToBind = mynePortraitDefault.gameObject;
        }
    }

    public void BindIntroTimeline(
        PlayableDirector director,
        TimelineAsset timeline
    )
    {
        introTimelineObjectsToBind[0] = MynePortraitToBind;
        introTimelineObjectsToBind[1] = MynePortraitToBind;
        director.BindTimelineTracks(timeline, introTimelineObjectsToBind);
    }

    public void HandleMidConvoPortraitOverride(FullArtPortrait portraitType)
    {
        CloseAllPortraits();
        GetMynePortrait(portraitType).Open();
    }

    void CloseAllPortraits()
    {
        mynePortraitDefault.Close();
        mynePortraitGlare.Close();
        mynePortraitFeignedConcern.Close();
        mynePortraitConfident.Close();
    }

    private Script_CanvasGroupController GetMynePortrait(FullArtPortrait portraitType) => portraitType switch
    {
        FullArtPortrait.MyneGlare => mynePortraitGlare,
        FullArtPortrait.MyneFeignedConcern => mynePortraitFeignedConcern,
        FullArtPortrait.MyneConfident => mynePortraitConfident,
        _ => mynePortraitDefault,
    };

    // ----------------------------------------------------------------------
    // Timeline Signals
    
    /// <summary>
    /// Call this from Mynes Mirror End of Timeline so Mynes Mirrors can react to it
    /// </summary>
    public void OnEndTimeline()
    {
        Script_MynesMirrorEventsManager.EndTimeline();
    }

    // ----------------------------------------------------------------------
    // Next Node Actions

    /// <summary>
    /// Must call this from Interaction Node.
    /// </summary>
    public void EndInteractionDialogue()
    {
        Script_MynesMirrorEventsManager.EndInteractionNode();
    }

    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }   
    }
}
