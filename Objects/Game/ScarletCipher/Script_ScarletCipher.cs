using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// When game begins, this will initialize a new "seed" to answer Myne's question as 
/// the final quest
/// </summary>
public class Script_ScarletCipher : MonoBehaviour
{
    public const int QuestionCount = 20;
    [SerializeField] private int[] _scarletCipher = new int[QuestionCount];
    [SerializeField] private bool[] _scarletCipherVisibility = new bool[QuestionCount];
    [SerializeField] private Script_DialogueNode[] dialogues = new Script_DialogueNode[QuestionCount];
    
    public int[] ScarletCipher
    {
        get => _scarletCipher;
        set => _scarletCipher = value;
    }

    public bool[] ScarletCipherVisibility
    {
        get => _scarletCipherVisibility;
        set => _scarletCipherVisibility = value;
    }

    /// <summary>
    /// Use this to reveal pieces of Scarlet Cipher
    /// </summary>
    public bool RevealScarletCipherSlot(int i, bool isVisible = true)
    {
        ScarletCipherVisibility[i] = isVisible;

        return ScarletCipherVisibility[i];
    }

    public void Initialize()
    {
        int[] newCipher         = new int[QuestionCount];
        bool[] newVisibility    = new bool[QuestionCount];

        for (int i = 0; i < QuestionCount; i++)
        {
            int choicesCount = dialogues[i].data.children.Length;
            
            /// Choose a random choice for the node Random.Range(inclusive, exclusive)
            int choice = Random.Range(0, choicesCount);
            
            Debug.Log($"choice: {choice}");
            
            newCipher[i] = choice;
        }

        ScarletCipher           = newCipher;
        ScarletCipherVisibility = newVisibility;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_ScarletCipher))]
public class Script_ScarletCipherEditor : Editor
{
    private SerializedProperty scarletCipher;
    private SerializedProperty scarletCipherVisibility;
    private SerializedProperty dialogue;
    private static bool[] showItemSlots = new bool[Script_ScarletCipher.QuestionCount];

    private const string ScarletCipherName              = "_scarletCipher";
    private const string ScarletCipherVisibilityName    = "_scarletCipherVisibility";
    private const string DialoguesName                  = "dialogues";

    private void OnEnable()
    {
        scarletCipher           = serializedObject.FindProperty(ScarletCipherName);
        scarletCipherVisibility = serializedObject.FindProperty(ScarletCipherVisibilityName);
        dialogue                = serializedObject.FindProperty(DialoguesName);
    }

    public override void OnInspectorGUI()
    {
        // ensure our serialized object is up-to-date with the Inventory
        serializedObject.Update();
        
        for (int i = 0; i < Script_ScarletCipher.QuestionCount; i++)
        {
            ItemSlotGUI(i);
        }

        // ensure changes in our serialized object go back into the game object
        serializedObject.ApplyModifiedProperties();
    }

    private void ItemSlotGUI(int i)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        
        showItemSlots[i] = EditorGUILayout.Foldout(showItemSlots[i], $"Scarlet Cipher Slot {i}");

        if (showItemSlots[i])
        {
            EditorGUILayout.PropertyField(scarletCipher.GetArrayElementAtIndex(i),              new GUIContent("Cipher"));
            EditorGUILayout.PropertyField(scarletCipherVisibility.GetArrayElementAtIndex(i),    new GUIContent("Visibility"));
            EditorGUILayout.PropertyField(dialogue.GetArrayElementAtIndex(i),                   new GUIContent("Dialogue"));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
#endif
