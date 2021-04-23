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
/// 
/// Handles adding and removing "Skills" into the holster;
/// NOTE: StickerEffectsController manages actually calling them!
/// </summary>
[RequireComponent(typeof(Script_CanvasGroupController))] // Used by StickerHolsterManager
public class Script_StickerHolster : MonoBehaviour
{
    public enum States
    {
        Active
    }
    [SerializeField] private States _state = States.Active;
    [SerializeField] private Image[] stickerImages = new Image[numItemSlots]; 
    [SerializeField] private Script_Sticker[] stickers = new Script_Sticker[numItemSlots];
    public const int numItemSlots = 9;
    public States State
    {
        get { return _state; }
        set { _state = value;}
    }
    
    public Script_Sticker GetStickerInSlot(int Id)
    {
        return stickers[Id];
    }

    /// <summary>
    /// Add item to the Sticker Holster.
    /// 
    /// NOTE: These should work regardless of State
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

    // Only 1 Sticker can be highlighted at a time.
    public void HighlightStickerInSlot(int slotIdx, bool isHighlight)
    {
        for (int i = 0; i < stickers.Length; i++)
        {
            if (stickers[i] != null)
            {
                if (i == slotIdx && isHighlight)
                    stickerImages[slotIdx].sprite = stickers[i].focusedSprite;
                else
                    stickerImages[i].sprite = stickers[i].sprite;
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_StickerHolster))]
public class Script_StickerHolsterEditor : Editor
{
    private SerializedProperty stickersProperty;
    private SerializedProperty stickerImagesProperty;
    private SerializedProperty stateProperty;
    private static bool[] showItemSlots = new bool[Script_StickerHolster.numItemSlots];

    private const string InventoryPropItemImagesName = "stickerImages";
    private const string InventoryPropItemName = "stickers";
    private const string StateName = "_state";

    private void OnEnable()
    {
        stickerImagesProperty = serializedObject.FindProperty(InventoryPropItemImagesName);
        stickersProperty = serializedObject.FindProperty(InventoryPropItemName);
        stateProperty = serializedObject.FindProperty(StateName);
    }

    public override void OnInspectorGUI()
    {
        /// Ensure our serialized object is up-to-date with the Inventory
        serializedObject.Update();

        /// Show other properties we want
        EditorGUILayout.PropertyField(stateProperty);
        
        for (int i = 0; i < Script_StickerHolster.numItemSlots; i++)
        {
            ItemSlotGUI(i);
        }

        /// Ensure changes in our serialized object go back into the game object
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
