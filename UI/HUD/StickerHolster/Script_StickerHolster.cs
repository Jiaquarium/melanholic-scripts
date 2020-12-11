using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Handles Sticker Holster data
/// Should only be called by StickerHolsterManager and InventoryManager
/// </summary>
[RequireComponent(typeof(Script_CanvasGroupController))] // Used by StickerHolsterManager
public class Script_StickerHolster : MonoBehaviour
{
    [SerializeField] private Image[] stickerImages = new Image[numItemSlots]; 
    [SerializeField] private Script_Sticker[] stickers = new Script_Sticker[numItemSlots];
    public const int numItemSlots = 9;
    
    public Script_Sticker GetStickerInSlot(int Id)
    {
        return stickers[Id];
    }

    /// <summary>
    /// Add item to the Sticker Holster.
    /// </summary>
    public bool AddStickerInSlot(Script_Sticker stickerToAdd, int i)
    {
        if (stickers[i] != null)
            Debug.LogWarning($"You are about to overwrite sticker in sticker holster slot {i}. Be careful this isn't a bug.");
        stickers[i] = stickerToAdd;
        stickerImages[i].sprite = stickerToAdd.sprite;
        stickerImages[i].enabled = true;
        return true;
    }

    /// Must remove by slot in case there are duplicates of that item
    public bool RemoveStickerInSlot(int i)
    {
        stickers[i] = null;
        stickerImages[i].sprite = null;
        stickerImages[i].enabled = false;
        return true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_StickerHolster))]
public class Script_StickerHolsterEditor : Editor
{
    private SerializedProperty stickersProperty;
    private SerializedProperty stickerImagesProperty;
    private static bool[] showItemSlots = new bool[Script_StickerHolster.numItemSlots];

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
        
        for (int i = 0; i < Script_StickerHolster.numItemSlots; i++)
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
        
        showItemSlots[i] = EditorGUILayout.Foldout(showItemSlots[i], $"Sticker Holster Slot {i}");

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
