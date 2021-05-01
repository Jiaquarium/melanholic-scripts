using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Handles inventory State data
/// Should only be used by Script_InventoryManager and its Handlers
/// 
/// Script_Items is the same as this.
/// </summary>
public class Script_Inventory : MonoBehaviour
{
    [SerializeField] private Image[] itemImages = new Image[numItemSlots]; 
    [SerializeField] private Script_Item[] items = new Script_Item[numItemSlots];
    public const int numItemSlots = 9;

    public Script_Item[] Items
    {
        get { return items; }
    }
    
    public Script_Item GetItemInSlot(int Id)
    {
        return Script_InventoryHelpers.GetItemInSlot(Id, items);
    }

    public bool HasSpace()
    {
        return Script_InventoryHelpers.HasSpace(items);
    }
    
    public bool AddItem(Script_Item itemToAdd)
    {
        return Script_InventoryHelpers.AddItem(itemToAdd, items, itemImages);
    }
    
    public bool AddItemInSlot(Script_Item itemToAdd, int i)
    {
        return Script_InventoryHelpers.AddItemInSlot(itemToAdd, i, items, itemImages);
    }

    public bool RemoveItemInSlot(int i)
    {
        return Script_InventoryHelpers.RemoveItemInSlot(i, items, itemImages);
    }

    public bool RemoveItem(Script_Item itemToRemove)
    {
        return Script_InventoryHelpers.RemoveItem(itemToRemove, items, itemImages);
    }

    public void HighlightItem(int i, bool isFocus)
    {
        Script_InventoryHelpers.HighlightItem(i, isFocus, items, itemImages);
    }

    public Script_Item SearchForItemById(string Id, out int slot)
    {
        return Script_InventoryHelpers.SearchForItemById(Id, out slot, items);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_Inventory))]
public class Script_InventoryEditor : Editor
{
    private SerializedProperty itemsProperty;
    private SerializedProperty itemImagesProperty;
    private static bool[] showItemSlots = new bool[Script_Inventory.numItemSlots];

    private const string InventoryPropItemImagesName = "itemImages";
    private const string InventoryPropItemName = "items";

    private void OnEnable()
    {
        itemImagesProperty = serializedObject.FindProperty(InventoryPropItemImagesName);
        itemsProperty = serializedObject.FindProperty(InventoryPropItemName);
    }

    public override void OnInspectorGUI()
    {
        // ensure our serialized object is up-to-date with the Inventory
        serializedObject.Update();
        
        for (int i = 0; i < Script_Inventory.numItemSlots; i++)
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
        
        showItemSlots[i] = EditorGUILayout.Foldout(showItemSlots[i], $"Item Slot {i}");

        if (showItemSlots[i])
        {
            // show default
            EditorGUILayout.PropertyField(itemImagesProperty.GetArrayElementAtIndex(i), new GUIContent("ItemImage"));
            EditorGUILayout.PropertyField(itemsProperty.GetArrayElementAtIndex(i), new GUIContent("Item"));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
#endif
