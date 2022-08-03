using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;

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
    public const int QuestionCount = 10;
    public static int IntroRoomNotesCount = 4;

    [SerializeField] private int[] _scarletCipher = new int[QuestionCount];
    [SerializeField] private bool[] _scarletCipherVisibility = new bool[QuestionCount];
    [Tooltip("The relevant 10 question conversation nodes")]
    [SerializeField] private bool[] _mynesMirrorsActivationStates = new bool[QuestionCount];
    [SerializeField] private bool[] _mynesMirrorsSolved = new bool[QuestionCount];

    [SerializeField] private Script_ScarletCipherNotification _scarletCipherNotification;
    [SerializeField] private PlayableDirector _notificationDirector;
    
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
    /// The current viewable state of the Scarlet Cipher in which the Player can see.
    /// </summary>
    public int[] ScarletCipherPublic
    {
        get
        {
            int[] publicScarletCipher = new int[10];
            
            for (var i = 0; i < ScarletCipher.Length; i++)
                publicScarletCipher[i] = ScarletCipherVisibility[i] ? ScarletCipher[i] : -1;

            return publicScarletCipher;
        }
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

    public Script_ScarletCipherNotification ScarletCipherNotification
    {
        get => _scarletCipherNotification;
    }

    /// <summary>
    /// Use this to reveal pieces of Scarlet Cipher
    /// </summary>
    public int RevealScarletCipherSlot(int i, bool isVisible = true)
    {
        ScarletCipherVisibility[i] = isVisible;

        return ScarletCipher[i];
    }

    public bool HandleCipherSlot(int cipherSlot, int choiceIdx)
    {
        Debug.Log($"Checking cipher slot {cipherSlot} with {choiceIdx}. Expected: {ScarletCipher[cipherSlot]}");

        bool isSolved = ScarletCipher[cipherSlot] == choiceIdx;
        MynesMirrorsSolved[cipherSlot] = isSolved;

        return isSolved;
    }

    public void Dev_ForceSolveAllMirrors()
    {
        for (int i = 0; i < MynesMirrorsSolved.Length; i++)
        {
            MynesMirrorsSolved[i] = true;
        }
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

    public bool CheckCCTVCode(string codeInput)
    {
        Debug.Log($"Scarlet Cipher Length: {ScarletCipher.Length}");
        
        if (codeInput.Length < QuestionCount)   return false;
        
        for (int i = 0; i < ScarletCipher.Length; i++)
        {
            int codeValue;
            
            try
            {
                codeValue = Int32.Parse(codeInput[i].ToString());
            }
            catch (FormatException e)
            {
                Debug.LogError($"You need to pass in only digits so it can be parsed properly: {e}");
                return false;
            }
            
            Debug.Log($"codeValue {codeValue}; ScarletCipher[{i}] {ScarletCipher[i]}");

            if (codeValue != ScarletCipher[i])
            {
                return false;
            }
        }

        return true;
    }

    public bool CheckAllMirrorsSolved()
    {
        for (int i = 0; i < MynesMirrorsSolved.Length; i++)
        {
            if (!MynesMirrorsSolved[i])     return false;
        }

        return true;
    }

    // ------------------------------------------------------------------
    // View
    public void PlayScarletCipherNotification(int revealedNum)
    {
        var game = Script_Game.Game;
        var player = game.GetPlayer();
        
        // Must switch to nonmoving state here to force Player to stop movement.
        player.SetIsStandby();
        game.ChangeStateCutScene();
        
        int n = revealedNum;
        ScarletCipherNotification.FinalRevealDigit = n;
        ScarletCipherNotification.InitialState();
        
        _notificationDirector.Play();
    }
    
    // ------------------------------------------------------------------
    // Timeline Signals

    public void ScarletCipherNotificationDone()
    {
        var game = Script_Game.Game;
        var player = game.GetPlayer();
        
        player.SetIsInteract();
        game.ChangeStateInteract();
    }

    public void PlayTakeNote()
    {
        var SFX = Script_SFXManager.SFX;
        
        SFX.PlayTakeNote();
    }

    // ------------------------------------------------------------------
    
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
            newCipher[i] = UnityEngine.Random.Range(0, 10);
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

        _scarletCipherNotification.Setup();

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_ScarletCipherManager))]
public class Script_ScarletCipherEditor : Editor
{
    private SerializedProperty scarletCipherNotification;
    private SerializedProperty notificationDirector;
    private SerializedProperty scarletCipher;
    private SerializedProperty scarletCipherVisibility;
    private SerializedProperty mynesMirrorsActivationStates;
    private SerializedProperty mynesMirrorsSolved;
    private static bool[] showItemSlots = new bool[Script_ScarletCipherManager.QuestionCount];

    private const string ScarletCipherNotificationName      = "_scarletCipherNotification";
    private const string NotificationDirector               = "_notificationDirector";
    private const string ScarletCipherName                  = "_scarletCipher";
    private const string ScarletCipherVisibilityName        = "_scarletCipherVisibility";
    private const string DialoguesName                      = "dialogues";
    private const string MynesMirrorsActivationStatesName   = "_mynesMirrorsActivationStates";
    private const string MynesMirrorsSolvedName             = "_mynesMirrorsSolved";

    private void OnEnable()
    {
        scarletCipherNotification       = serializedObject.FindProperty(ScarletCipherNotificationName);
        notificationDirector            = serializedObject.FindProperty(NotificationDirector);
        scarletCipher                   = serializedObject.FindProperty(ScarletCipherName);
        scarletCipherVisibility         = serializedObject.FindProperty(ScarletCipherVisibilityName);
        mynesMirrorsActivationStates    = serializedObject.FindProperty(MynesMirrorsActivationStatesName);
        mynesMirrorsSolved              = serializedObject.FindProperty(MynesMirrorsSolvedName);
    }

    public override void OnInspectorGUI()
    {
        // ensure our serialized object is up-to-date with the Inventory
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(scarletCipherNotification, new GUIContent("Notification"));
        EditorGUILayout.PropertyField(notificationDirector, new GUIContent("Director"));
        
        for (int i = 0; i < Script_ScarletCipherManager.QuestionCount; i++)
        {
            ItemSlotGUI(i);
        }

        Script_ScarletCipherManager t = (Script_ScarletCipherManager)target;
        if (GUILayout.Button("Check Mirrors Solved"))
        {
            Debug.Log($"All mirrors solved: {t.CheckAllMirrorsSolved()}");
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
            EditorGUILayout.PropertyField(mynesMirrorsActivationStates.GetArrayElementAtIndex(i),   new GUIContent("Mirror Activated"));
            EditorGUILayout.PropertyField(mynesMirrorsSolved.GetArrayElementAtIndex(i),             new GUIContent("Mirror Solved"));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
#endif
