using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Dev_InventoryTester : MonoBehaviour
{
    public string itemId;

    public void AddItemById()
    {
        Script_Game.Game.AddItemById(itemId);
    }

    public void AddPsychicDuck()
    {
        Script_Game.Game.AddItemById("sticker_psychic-duck");
    }

    public void AddBoarNeedle()
    {
        Script_Game.Game.AddItemById("sticker_boar-needle");
    }

    public void AddMasterKey()
    {
        Script_Game.Game.AddItemById("usable_master-key");
    }

    public void AddWinterStone()
    {
        Script_Game.Game.AddItemById("collectible_winter-stone");
    }

    public void AddSpringStone()
    {
        Script_Game.Game.AddItemById("collectible_spring-stone");
    }

    public void AddSummerStone()
    {
        Script_Game.Game.AddItemById("collectible_summer-stone");
    }

    public void AddAutumnStone()
    {
        Script_Game.Game.AddItemById("collectible_autumn-stone");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Dev_InventoryTester))]
public class Dev_InventoryTesterTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Dev_InventoryTester inventoryTester = (Dev_InventoryTester)target;
        if (GUILayout.Button("AddItemById()"))
        {
            inventoryTester.AddItemById();
        }

        if (GUILayout.Button("Add: Psychic Duck"))
        {
            inventoryTester.AddPsychicDuck();
        }

        if (GUILayout.Button("Add: Boar Needle"))
        {
            inventoryTester.AddBoarNeedle();
        }

        if (GUILayout.Button("Add: Master Key"))
        {
            inventoryTester.AddMasterKey();
        }

        if (GUILayout.Button("Add: Winter Stone"))
        {
            inventoryTester.AddWinterStone();
        }

        if (GUILayout.Button("Add: Spring Stone"))
        {
            inventoryTester.AddSpringStone();
        }

        if (GUILayout.Button("Add: Summer Stone"))
        {
            inventoryTester.AddSummerStone();
        }

        if (GUILayout.Button("Add: Autumn Stone"))
        {
            inventoryTester.AddAutumnStone();
        }
    }
}
#endif