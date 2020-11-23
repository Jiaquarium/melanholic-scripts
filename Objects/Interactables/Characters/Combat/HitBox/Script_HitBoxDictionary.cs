using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_HitBoxDictionary : MonoBehaviour
{
    // Unity doesn't know how to serialize Dictionary
    public Dictionary<string, Script_HitBoxMetadata> myDictionary = new Dictionary<string, Script_HitBoxMetadata>{};
    public string[] hitBoxIds = new string[numItems];
    public Script_HitBoxMetadata[] hitBoxMetadatas = new Script_HitBoxMetadata[numItems];
    public const int numItems = 50;
    public static Script_HitBoxDictionary HitBoxDictionary;

    void Awake()
    {
        myDictionary = new Dictionary<string, Script_HitBoxMetadata>();
        string[] noNullsHitBoxIds                       = hitBoxIds.Where(q => !string.IsNullOrEmpty(q)).ToArray();
        Script_HitBoxMetadata[] noNullsHitBoxMetadaatas = hitBoxMetadatas.Where(q => q != null).ToArray();

        for (int i = 0; i < Mathf.Min(noNullsHitBoxIds.Length, noNullsHitBoxMetadaatas.Length); i++)
        {
            myDictionary.Add(noNullsHitBoxIds[i], noNullsHitBoxMetadaatas[i]);
            // Debug.Log($"itemDict key: {hitBoxIds[i]}, value: {myDictionary[hitBoxIds[i]]}");
        }

        if (HitBoxDictionary == null)
        {
            HitBoxDictionary = this;
        }
        else if (HitBoxDictionary != this)
        {
            Destroy(this.gameObject);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_HitBoxDictionary))]
public class Script_HitBoxDictionaryEditor : Editor
{
    private SerializedProperty keysProperty;
    private SerializedProperty fullArtPrefabsProperty;
    private static bool[] showHitBoxSlots = new bool[Script_HitBoxDictionary.numItems];

    private const string hitBoxIdsName = "hitBoxIds";
    private const string hitBoxMetadatasName = "hitBoxMetadatas";

    private void OnEnable()
    {
        fullArtPrefabsProperty = serializedObject.FindProperty(hitBoxIdsName);
        keysProperty = serializedObject.FindProperty(hitBoxMetadatasName);
    }

    public override void OnInspectorGUI()
    {
        // ensure our serialized object is up-to-date with the Inventory
        serializedObject.Update();
        
        for (int i = 0; i < Script_HitBoxDictionary.numItems; i++)
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
        
        showHitBoxSlots[i] = EditorGUILayout.Foldout(showHitBoxSlots[i], $"kvp {i}");

        if (showHitBoxSlots[i])
        {
            // show default
            EditorGUILayout.PropertyField(fullArtPrefabsProperty.GetArrayElementAtIndex(i), new GUIContent("key"));
            EditorGUILayout.PropertyField(keysProperty.GetArrayElementAtIndex(i), new GUIContent("value"));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
#endif


