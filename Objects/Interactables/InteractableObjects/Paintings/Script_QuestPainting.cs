using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_QuestPainting : Script_InteractableObjectText
{
    [SerializeField] private Sprite donePainting;
    [SerializeField] private SpriteRenderer questPaintingGraphics;

    [SerializeField] private Script_DialogueNode[] donePaintingNodes;
    [SerializeField] private Script_DialogueNode[] defaultNodes;

    protected bool isDonePainting;
    private Sprite defaultPainting;
    
    protected override void Awake() {
        base.Awake();

        if (defaultPainting == null)    defaultPainting = questPaintingGraphics.sprite;
    }

    // ----------------------------------------------------------------------
    // Timeline Signals
    public void DonePainting()
    {
        questPaintingGraphics.sprite    = donePainting;
        isDonePainting                  = true;

        defaultNodes = dialogueNodes;
        SwitchDialogueNodes(donePaintingNodes);
    }

    public void DefaultPainting()
    {
        questPaintingGraphics.sprite    = defaultPainting;
        isDonePainting                  = false;

        SwitchDialogueNodes(defaultNodes);
    }
    // ----------------------------------------------------------------------
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_QuestPainting))]
public class Script_QuestPaintingTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_QuestPainting t = (Script_QuestPainting)target;
        if (GUILayout.Button("Done Painting"))
        {
            t.DonePainting();
        }

        if (GUILayout.Button("Default Painting"))
        {
            t.DefaultPainting();
        }
    }
}
#endif