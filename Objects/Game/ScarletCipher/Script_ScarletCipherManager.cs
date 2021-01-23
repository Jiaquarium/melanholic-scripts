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
public class Script_ScarletCipherManager : MonoBehaviour
{
    public static Script_ScarletCipherManager Control;
    public const int QuestionCount = 20;
    [SerializeField] private int[] _scarletCipher = new int[QuestionCount];
    [SerializeField] private bool[] _scarletCipherVisibility = new bool[QuestionCount];
    [Tooltip("The relevant 20 question conversation nodes")]
    [SerializeField] private Script_DialogueNode_MynesConversationChoiceParent[] dialogues = new Script_DialogueNode_MynesConversationChoiceParent[QuestionCount];
    [SerializeField] private bool[] _mynesMirrorsActivationStates = new bool[QuestionCount];
    [SerializeField] private bool[] _mynesMirrorsSolved = new bool[QuestionCount];
    
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

    public bool[] MynesMirrorsActivationStates
    {
        get => _mynesMirrorsActivationStates;
        set => _mynesMirrorsActivationStates = value;
    }

    public bool[] MynesMirrorsSolved
    {
        get => _mynesMirrorsSolved;
        set => _mynesMirrorsSolved = value;
    }

    /// <summary>
    /// Use this to reveal pieces of Scarlet Cipher
    /// </summary>
    public bool RevealScarletCipherSlot(int i, bool isVisible = true)
    {
        ScarletCipherVisibility[i] = isVisible;

        return ScarletCipherVisibility[i];
    }

    public bool HandleCipherSlot(int cipherSlot, int choiceIdx)
    {
        Debug.Log($"Checking cipher slot {cipherSlot} with {choiceIdx}. Expected: {ScarletCipher[cipherSlot]}");

        bool isSolved = ScarletCipher[cipherSlot] == choiceIdx;
        MynesMirrorsSolved[cipherSlot] = isSolved;

        return isSolved;
    }

    /// <summary>
    /// Call when starting a new run
    /// </summary>
    public void ResetMynesMirrors()
    {
        for (int i = 0; i < QuestionCount; i++)
        {
            MynesMirrorsActivationStates[i] = false;
            MynesMirrorsSolved[i]           = false;
        }
    }

    public void InitialState()
    {
        ResetMynesMirrors();
    }

    public void Initialize()
    {
        int[] newCipher                             = new int[QuestionCount];
        bool[] newVisibility                        = new bool[QuestionCount];
        bool[] newMynesMirrorsActivationStates      = new bool[QuestionCount];
        bool[] newMynesMirrorsSolved                = new bool[QuestionCount];

        for (int i = 0; i < QuestionCount; i++)
        {
            if (dialogues[i] == null)
            {
                newCipher[i] = -1;
                Debug.Log($"You need to reference a dialogue node parent for cipher slot {i}");
                continue;
            }

            int choicesCount = dialogues[i].data.children.Length;
            
            /// Choose a random choice for the node Random.Range(inclusive, exclusive)
            int choice = Random.Range(0, choicesCount);
            
            newCipher[i] = choice;
        }

        ScarletCipher                   = newCipher;
        ScarletCipherVisibility         = newVisibility;
        MynesMirrorsActivationStates    = newMynesMirrorsActivationStates;
        MynesMirrorsSolved              = newMynesMirrorsSolved;
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

#if UNITY_EDITOR
[CustomEditor(typeof(Script_ScarletCipherManager))]
public class Script_ScarletCipherEditor : Editor
{
    private SerializedProperty scarletCipher;
    private SerializedProperty scarletCipherVisibility;
    private SerializedProperty dialogue;
    private SerializedProperty mynesMirrorsActivationStates;
    private SerializedProperty mynesMirrorsSolved;
    private static bool[] showItemSlots = new bool[Script_ScarletCipherManager.QuestionCount];

    private const string ScarletCipherName                  = "_scarletCipher";
    private const string ScarletCipherVisibilityName        = "_scarletCipherVisibility";
    private const string DialoguesName                      = "dialogues";
    private const string MynesMirrorsActivationStatesName   = "_mynesMirrorsActivationStates";
    private const string MynesMirrorsSolvedName             = "_mynesMirrorsSolved";

    private void OnEnable()
    {
        scarletCipher                   = serializedObject.FindProperty(ScarletCipherName);
        scarletCipherVisibility         = serializedObject.FindProperty(ScarletCipherVisibilityName);
        dialogue                        = serializedObject.FindProperty(DialoguesName);
        mynesMirrorsActivationStates    = serializedObject.FindProperty(MynesMirrorsActivationStatesName);
        mynesMirrorsSolved              = serializedObject.FindProperty(MynesMirrorsSolvedName);
    }

    public override void OnInspectorGUI()
    {
        // ensure our serialized object is up-to-date with the Inventory
        serializedObject.Update();
        
        for (int i = 0; i < Script_ScarletCipherManager.QuestionCount; i++)
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
            EditorGUILayout.PropertyField(scarletCipher.GetArrayElementAtIndex(i),                  new GUIContent("Cipher"));
            EditorGUILayout.PropertyField(scarletCipherVisibility.GetArrayElementAtIndex(i),        new GUIContent("Visibility"));
            EditorGUILayout.PropertyField(dialogue.GetArrayElementAtIndex(i),                       new GUIContent("Dialogue"));
            EditorGUILayout.PropertyField(mynesMirrorsActivationStates.GetArrayElementAtIndex(i),   new GUIContent("Mirror Activated"));
            EditorGUILayout.PropertyField(mynesMirrorsSolved.GetArrayElementAtIndex(i),              new GUIContent("Mirror Solved"));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
#endif
