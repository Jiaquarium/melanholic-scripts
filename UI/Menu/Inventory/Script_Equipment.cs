using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_Equipment : MonoBehaviour
{
    [SerializeField] private Image[] stickerImages = new Image[numItemSlots]; 
    [SerializeField] private Script_Sticker[] stickers = new Script_Sticker[numItemSlots];
    public const int numItemSlots = 9;
    public Script_Inventory inventory;

    public Script_Sticker[] Items
    {
        get { return stickers; }
    }
    
    public Script_Sticker GetStickerInSlot(int Id)
    {
        return stickers[Id];
    }
    
    /// <returns>false, if stickers are full</returns>
    public bool AddSticker(Script_Sticker stickerToAdd, out int stickerSlotId)
    {
        for (int i = 0; i < stickers.Length; i++)
        {
            if (stickers[i] == null)
            {
                stickers[i] = stickerToAdd;
                stickerImages[i].sprite = stickerToAdd.sprite;
                stickerImages[i].enabled = true;
                stickerSlotId = i;

                HandleStickerHolsterAdd(stickerToAdd, i);

                return true;
            }
        }

        stickerSlotId = -1;
        return false;
    }

    public bool AddStickerInSlot(Script_Sticker stickerToAdd, int i)
    {
        if (stickers[i] != null)
            Debug.LogWarning($"You are about to overwrite sticker in slot {i}. Be careful this isn't a bug.");
        
        stickers[i] = stickerToAdd;
        stickerImages[i].sprite = stickerToAdd.sprite;
        stickerImages[i].enabled = true;

        HandleStickerHolsterAdd(stickerToAdd, i);

        return true;
    }

    /// Must remove by slot in case there are duplicates of that item
    public bool RemoveStickerInSlot(int i)
    {
        if (!inventory.HasSpace())  return false;
        
        stickers[i] = null;
        stickerImages[i].sprite = null;
        stickerImages[i].enabled = false;
        
        HandleStickerHolsterRemove(i);

        return true;
    }

    public bool SearchForSticker(Script_Sticker stickerToFind)
    {
        for (int i = 0; i < stickers.Length; i++)
            if (stickers[i] == stickerToFind)   return true;

        return false;
    }

    public bool SearchForStickerById(string IdToFind)
    {
        for (int i = 0; i < stickers.Length; i++)
            if (stickers[i] != null && stickers[i].id == IdToFind)   return true;

        return false;
    }

    void HandleStickerHolsterAdd(Script_Sticker stickerToAdd, int i)
    {
        Script_StickerHolsterManager.Control.AddSticker(stickerToAdd, i);   
    }

    void HandleStickerHolsterRemove(int i)
    {
        Script_StickerHolsterManager.Control.RemoveSticker(i);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_Equipment))]
public class Script_EquipmentEditor : Editor
{
    private SerializedProperty stickerProperty;
    private SerializedProperty stickerImagesProperty;
    private SerializedProperty inventoryProperty;
    static private bool[] showItemSlots = new bool[Script_Equipment.numItemSlots];

    private const string EquipmentPropItemImagesName = "stickerImages";
    private const string EquipmentPropItemName = "stickers";
    private const string EquipmentPropInventory = "inventory";

    private void OnEnable()
    {
        stickerImagesProperty = serializedObject.FindProperty(EquipmentPropItemImagesName);
        stickerProperty = serializedObject.FindProperty(EquipmentPropItemName);
        inventoryProperty = serializedObject.FindProperty(EquipmentPropInventory);
    }

    public override void OnInspectorGUI()
    {
        // ensure our serialized object is up-to-date with the Inventory
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(inventoryProperty, new GUIContent("Inventory"));   
        for (int i = 0; i < Script_Equipment.numItemSlots; i++)
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
            EditorGUILayout.PropertyField(stickerImagesProperty.GetArrayElementAtIndex(i), new GUIContent("StickerImage"));
            EditorGUILayout.PropertyField(stickerProperty.GetArrayElementAtIndex(i), new GUIContent("Sticker"));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
#endif
