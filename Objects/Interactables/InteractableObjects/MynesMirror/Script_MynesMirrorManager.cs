using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for Mynes Mirror Game Objects
/// States will be handled by Scarlet Cipher 
/// </summary>
public class Script_MynesMirrorManager : MonoBehaviour
{
    public static Script_MynesMirrorManager Control;

    [SerializeField] private int interactionCount;
    [SerializeField] private bool didSealingDialogue;

    [SerializeField] private Script_DialogueNode[] interactionNodes;

    [SerializeField] private Script_CanvasGroupController mynePortraitDefault;
    [SerializeField] private Script_CanvasGroupController mynePortraitGlare;
    [SerializeField] private Script_CanvasGroupController mynePortraitFeignedConcern;
    [SerializeField] private Script_CanvasGroupController mynePortraitConfident;
    
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
    
    public void InitializeMynePortrait(Script_DialogueNode node)
    {
        CloseAllPortraits();

        Model_DialogueSection[] sections = node.data.dialogue.sections;
        if (sections.Length > 0 && sections[0].fullArtOverride != FullArtPortrait.None)
        {
            var fullArtPortrait = sections[0].fullArtOverride;
            OpenMynePortrait(fullArtPortrait);
        }
        else
        {
            mynePortraitDefault.Open();
        }
    }

    public void HandleMidConvoPortraitOverride(FullArtPortrait portraitType)
    {
        CloseAllPortraits();
        OpenMynePortrait(portraitType);
    }

    void CloseAllPortraits()
    {
        mynePortraitDefault.Close();
        mynePortraitGlare.Close();
        mynePortraitFeignedConcern.Close();
        mynePortraitConfident.Close();
    }

    void OpenMynePortrait(FullArtPortrait portraitType)
    {
        switch (portraitType)
        {
            case FullArtPortrait.MyneGlare:
                mynePortraitGlare.Open();
                break;
            case FullArtPortrait.MyneFeignedConcern:
                mynePortraitFeignedConcern.Open();
                break;
            case FullArtPortrait.MyneConfident:
                mynePortraitConfident.Open();
                break;
            default:
                mynePortraitDefault.Open();
                break;
        }
    }
    
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
