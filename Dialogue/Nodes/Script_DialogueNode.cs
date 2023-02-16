using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_DialogueNode : MonoBehaviour
{
    public string Id;
    public Model_DialogueNode data;

    void OnValidate()
    {
        HandlePopulateById();
    }

    private void HandlePopulateById()
    {
        if (!String.IsNullOrEmpty(Id))
        {
            Model_Languages languages = Script_Dialogue.Dialogue[Id];
            string[] sections;
            
            // Get Sections in respective language.
            if (!languages.HasProp(Const_Dev.Lang))    return;
            
            sections = languages.GetProp<string[]>(Const_Dev.Lang);

            // Populate node with language data.
            // Speaker
            data.dialogue.name = languages.speaker;
            
            // Sections
            data.dialogue.sections = new Model_DialogueSection[sections.Length];

            for (int i = 0; i < sections.Length; i++)
            {
                data.dialogue.sections[i] = new Model_DialogueSection
                {
                    lines = new string[]
                    {
                        sections[i]
                    }
                };
            }

            // Sections Metadata
            Model_Languages.Metadata[] metadata = languages.metadata;
            if (metadata != null)
            {
                if (metadata.Length != sections.Length)
                {
                    Debug.LogError($"{name} If defining metadata for DialogueNode, need to be equal Length to sections.");
                }
                
                for (int i = 0; i < metadata.Length; i++)
                {
                    if (metadata[i] != null && metadata[i].isUnskippable != null)
                        data.dialogue.sections[i].isUnskippable = (bool)metadata[i].isUnskippable;
                    
                    if (metadata[i] != null && metadata[i].noContinuationIcon != null)
                        data.dialogue.sections[i].noContinuationIcon = (bool)metadata[i].noContinuationIcon;
                    
                    if (metadata[i] != null && metadata[i].waitForTimeline != null)
                        data.dialogue.sections[i].waitForTimeline = (bool)metadata[i].waitForTimeline;
                    
                    if (metadata[i] != null && metadata[i].autoNext != null)
                    {
                        bool isAutoNext = (bool)metadata[i].autoNext;
                        data.dialogue.sections[i].autoNext = isAutoNext;
                        
                        // If autoNext is true, the node should also be unskippable and no continuation icon.
                        if (isAutoNext)
                        {
                            data.dialogue.sections[i].isUnskippable = true;
                            data.dialogue.sections[i].noContinuationIcon = true;
                        }
                    }

                    if (metadata[i] != null && metadata[i].fullArtOverride != null)
                        data.dialogue.sections[i].fullArtOverride = (FullArtPortrait)metadata[i].fullArtOverride;
                }
            }

            // Choice Text
            if (!String.IsNullOrEmpty(languages.choiceText))
            {
                data.choiceText = languages.choiceText;
            }
            else
            {
                data.choiceText = string.Empty;
            }
        }
    }
}
