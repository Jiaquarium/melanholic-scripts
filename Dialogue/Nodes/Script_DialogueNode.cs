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
            data.dialogue.name = languages.speaker;
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
        }
    }
}
