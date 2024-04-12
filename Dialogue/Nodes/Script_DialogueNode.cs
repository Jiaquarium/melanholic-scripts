using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_DialogueNode : MonoBehaviour
{
    public string Id;
    public Model_DialogueNode data;

    private string LastUpdatedLang;
    private bool didFirstLoad;

#if UNITY_EDITOR
    // Revert any state changes back to default EN to clean up Editor View
    void OnApplicationQuit()
    {
        Script_Game.ChangeLangToEN();
        Refresh(isForce: true);
    }   
#endif
    
    void OnValidate()
    {
        Refresh(isForce: true);
    }

    public void Refresh(bool isForce = false) {
        if (
            !didFirstLoad
            || isForce
            || LastUpdatedLang != Script_Game.Lang
        )
        {
            HandlePopulateById();
            LastUpdatedLang = Script_Game.Lang;
            didFirstLoad = true;
        }
    }
    
    private void HandlePopulateById()
    {
        if (!String.IsNullOrEmpty(Id))
        {
            Model_Languages languages = Script_Dialogue.Dialogue[Id];
            string[] sections;
            
            // Get Sections in respective language. Handle using an unsupported language code.
            if (!languages.HasProp(Script_Game.Lang))
                return;
            
            // Sections will always be populated because in dialogue-exporter.py we fall back
            // to English if the localized line is empty
            sections = languages.GetProp<string[]>(Script_Game.Lang);
            
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

            string choiceTextLocalized = Script_LocalizationUtils.SwitchTextOnLang(
                languages.choiceText,
                languages.choiceTextCN,
                languages.choiceTextJP
            );

            // if choice text is undefined it will return null (e.g. languages.choiceText == null)
            if (String.IsNullOrEmpty(choiceTextLocalized))
                choiceTextLocalized = string.Empty;
            
            data.choiceText = choiceTextLocalized;
        }
    }
}
