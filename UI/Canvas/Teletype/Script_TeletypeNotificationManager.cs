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

    [SerializeField] private Script_CanvasGroupController canvasGroupController;

    public void ShowEileensMindDialogue(int i)
    {
        canvasGroupController.Open();

        EileensMindDialogue[i].Open();
    }

    public void InitialState()
    {
        canvasGroupController.Close();
        
        foreach (var dialogue in dialogues)
        {
            dialogue.InitialState();
            dialogue.gameObject.SetActive(true);
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