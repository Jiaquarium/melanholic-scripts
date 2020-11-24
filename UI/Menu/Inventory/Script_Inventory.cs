using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Handles inventory State data
/// Should only be done by Script_InventoryManager
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
        return items[Id];
    }

    public bool HasSpace()
    {
        foreach (Script_Item item in items)
            if (item == null)   return true;
        return false;
    }
    
    /// <summary>
    /// Add item to inventory.
    /// </summary>
    /// <param name="itemToAdd">item ScriptableObject</param>
    /// <returns>
    /// True if successfully added; False means it was full
    /// </returns>
    public bool AddItem(Script_Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = itemToAdd;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// For loading into specific slots
    /// </summary>
    public bool AddItemInSlot(Script_Item itemToAdd, int i)
    {
        if (items[i] != null)
            Debug.LogWarning($"You are about to overwrite item in slot {i}. Be careful this isn't a bug.");
        items[i] = itemToAdd;
        itemImages[i].sprite = itemToAdd.sprite;
        itemImages[i].enabled = true;
        return true;
    }

    /// Must remove by slot in case there are duplicates of that item
    public bool RemoveItemInSlot(int i)
    {
        items[i] = null;
        itemImages[i].sprite = null;
        itemImages[i].enabled = false;
        return true;
    }

    public void HighlightItem(int i, bool isFocus)
    {
        if (items[i] == null)   return;
        if (isFocus)    itemImages[i].sprite = items[i].focusedSprite;
        else            itemImages[i].sprite = items[i].sprite;   
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_Inventory))]
public class Script_InventoryEditor : Editor
{
    private SerializedProperty itemsProperty;
    private SerializedProperty itemImagesProperty;
    private bool[] showItemSlots = new bool[Script_Inventory.numItemSlots];

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
