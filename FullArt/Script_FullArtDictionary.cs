using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// For collectible fullArts to view out of world space
/// Allows to find fullArt via Script_Item Id
/// </summary>
public class Script_FullArtDictionary : MonoBehaviour
{
    // Unity doesn't know how to serialize Dictionary
    public Dictionary<string, Script_FullArt> myDictionary = new Dictionary<string, Script_FullArt>{};
    public string[] fullArtIds = new string[numFullArt];
    public Script_FullArt[] fullArtPrefabs = new Script_FullArt[numFullArt];
    public const int numFullArt = 50;

    /// Need to use Awake and not OnValidate as OnValidate won't be called without the editor script below
    void Awake()
    {
        myDictionary = new Dictionary<string, Script_FullArt>();
        string[] noNullsfullArtIds              = fullArtIds.Where(q => !string.IsNullOrEmpty(q)).ToArray();
        Script_FullArt[] noNullsfullArtPrefabs  = fullArtPrefabs.Where(q => q != null).ToArray();

        for (int i = 0; i < Mathf.Min(noNullsfullArtIds.Length, noNullsfullArtPrefabs.Length); i++)
        {
            myDictionary.Add(noNullsfullArtIds[i], noNullsfullArtPrefabs[i]);
            Debug.Log($"fullArtDict key: {fullArtIds[i]}, value: {myDictionary[fullArtIds[i]]}");
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_FullArtDictionary))]
public class Script_FullArtDictionaryEditor : Editor
{
    private SerializedProperty keysProperty;
    private SerializedProperty fullArtPrefabsProperty;
    static private bool[] showFullArtSlots = new bool[Script_FullArtDictionary.numFullArt];

    private const string fullArtIdsName = "fullArtIds";
    private const string fullArtPrefabsName = "fullArtPrefabs";

    private void OnEnable()
    {
        fullArtPrefabsProperty = serializedObject.FindProperty(fullArtIdsName);
        keysProperty = serializedObject.FindProperty(fullArtPrefabsName);
    }

    public override void OnInspectorGUI()
    {
        // ensure our serialized object is up-to-date with the Inventory
        serializedObject.Update();
        
        for (int i = 0; i < Script_FullArtDictionary.numFullArt; i++)
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

