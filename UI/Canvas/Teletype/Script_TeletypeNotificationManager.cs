using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_TeletypeNotificationManager : MonoBehaviour
{
    public static Script_TeletypeNotificationManager Control;
    
    [SerializeField] private Script_TeletypeDialogueContainer[] dialogues;
    
    [SerializeField] private Script_TeletypeTextContainer[] EileensMindDialogue;
    [SerializeField] private Script_TeletypeTextContainer[] CatWalk2Dialogue;
    [SerializeField] private Script_CanvasGroupController[] CatWalk2CanvasGroups;

    [SerializeField] private Script_CanvasGroupController canvasGroupController;

    public void ShowEileensMindDialogue(int i)
    {
        canvasGroupController.Open();

        EileensMindDialogue[i].Open();
    }

    public void ShowCatWalk2Dialogue(int i)
    {
        canvasGroupController.Open();

        CatWalk2CanvasGroups[i].Open();
        CatWalk2Dialogue[i].Open();
    }

    public void FadeOutCatWalk2Dialogue(int i)
    {
        CatWalk2CanvasGroups[i].FadeOut();        
    }

    public void InitialState()
    {
        if (canvasGroupController != null)
            canvasGroupController.Close();
        
        // Eileen's Mind
        foreach (var dialogue in dialogues)
        {
            if (dialogue != null)
            {
                dialogue.InitialState();
                dialogue.gameObject.SetActive(true);
            }
        }

        // Catwalk2
        foreach (var canvasGroupCtrl in CatWalk2CanvasGroups)
        {
            if (canvasGroupCtrl != null)
                canvasGroupCtrl.Close();
        }
        
        // Catwalk2
        foreach (var textContainer in CatWalk2Dialogue)
        {
            textContainer.Close();
        }
    }
    
    public void Setup()
    {
        if (Control == null)
            Control = this;
        else if (Control != this)
            Destroy(this.gameObject);
        
        InitialState();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_TeletypeNotificationManager))]
public class Script_TeletypeNotificationManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_TeletypeNotificationManager t = (Script_TeletypeNotificationManager)target;
        if (GUILayout.Button("Show Eileen Dialogue 0"))
            t.ShowEileensMindDialogue(0);
        
        if (GUILayout.Button("Show Eileen Dialogue 1"))
            t.ShowEileensMindDialogue(1);
        
        if (GUILayout.Button("Show Eileen Dialogue 2"))
            t.ShowEileensMindDialogue(2);
        
        if (GUILayout.Button("Show Eileen Dialogue 3"))
            t.ShowEileensMindDialogue(3);
        
        if (GUILayout.Button("Initial State"))
            t.InitialState();
    }
}
#endif