using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Handles Active Sticker data
/// Should only be called by ActiveStickerManager
/// </summary>
[RequireComponent(typeof(Script_CanvasGroupController))]
public class Script_ActiveSticker : MonoBehaviour
{
    [SerializeField] private Image[] stickerImages = new Image[numItemSlots]; 
    [SerializeField] private Script_Sticker[] stickers = new Script_Sticker[numItemSlots];
    public const int numItemSlots = 1;
    
    public Script_Sticker GetSticker()
    {
        return stickers[0];
    }
    
    public bool AddSticker(Script_Sticker stickerToAdd)
    {
        stickers[0] = stickerToAdd;
        stickerImages[0].sprite = stickerToAdd.sprite;
        stickerImages[0].enabled = true;
        return true;
    }

    public bool RemoveSticker()
    {
        stickers[0] = null;
        stickerImages[0].sprite = null;
        stickerImages[0].enabled = false;
        return true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_ActiveSticker))]
public class Script_ActiveStickerEditor : Editor
{
    private SerializedProperty stickersProperty;
    private SerializedProperty stickerImagesProperty;
    private static bool[] showItemSlots = new bool[Script_ActiveSticker.numItemSlots];

    private const string InventoryPropItemImagesName = "stickerImages";
    private const string InventoryPropItemName = "stickers";

    private void OnEnable()
    {
        stickerImagesProperty = serializedObject.FindProperty(InventoryPropItemImagesName);
        stickersProperty = serializedObject.FindProperty(InventoryPropItemName);
    }

    public override void OnInspectorGUI()
    {
        // ensure our serialized object is up-to-date with the Inventory
        serializedObject.Update();
        
        for (int i = 0; i < Script_ActiveSticker.numItemSlots; i++)
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
        
        showItemSlots[i] = EditorGUILayout.Foldout(showItemSlots[i], $"Active Sticker {i}");

        if (showItemSlots[i])
        {
            // show default
            EditorGUILayout.PropertyField(stickerImagesProperty.GetArrayElementAtIndex(i), new GUIContent("ItemImage"));
            EditorGUILayout.PropertyField(stickersProperty.GetArrayElementAtIndex(i), new GUIContent("Item"));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
#endif
