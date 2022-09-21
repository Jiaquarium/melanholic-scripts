using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Used to find the item when dropping
/// </summary>
public class Script_ItemDictionary : MonoBehaviour
{
    // Unity doesn't know how to serialize Dictionary
    public Dictionary<string, Script_ItemObject> myDictionary = new Dictionary<string, Script_ItemObject>{};
    public string[] itemIds = new string[numItems];
    public Script_ItemObject[] itemPrefabs = new Script_ItemObject[numItems];
    public const int numItems = 50;

    void Awake()
    {
        myDictionary = new Dictionary<string, Script_ItemObject>();
        string[] noNullsitemIds                      = itemIds.Where(q => !string.IsNullOrEmpty(q)).ToArray();
        Script_ItemObject[] noNullsItemPrefabs       = itemPrefabs.Where(q => q != null).ToArray();

        for (int i = 0; i < Mathf.Min(noNullsitemIds.Length, noNullsItemPrefabs.Length); i++)
        {
            myDictionary.Add(noNullsitemIds[i], noNullsItemPrefabs[i]);
            Dev_Logger.Debug($"itemDict key: {itemIds[i]}, value: {myDictionary[itemIds[i]]} added to ItemDictionary");
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_ItemDictionary))]
public class Script_ItemDictionaryEditor : Editor
{
    private SerializedProperty keysProperty;
    private SerializedProperty fullArtPrefabsProperty;
    private static bool[] showFullArtSlots = new bool[Script_ItemDictionary.numItems];

    private const string itemIdsName = "itemIds";
    private const string itemPrefabsName = "itemPrefabs";

    private void OnEnable()
    {
        fullArtPrefabsProperty = serializedObject.FindProperty(itemIdsName);
        keysProperty = serializedObject.FindProperty(itemPrefabsName);
    }

    public override void OnInspectorGUI()
    {
        // ensure our serialized object is up-to-date with the Inventory
        serializedObject.Update();
        
        for (int i = 0; i < Script_ItemDictionary.numItems; i++)
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
        
        showFullArtSlots[i] = EditorGUILayout.Foldout(showFullArtSlots[i], $"kvp {i}");

        if (showFullArtSlots[i])
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

