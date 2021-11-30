using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Dev_InventoryTester : MonoBehaviour
{
    public Dev_InventoryTester Control;
    
    public string itemId;

    void Awake()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }
    }
    
    void Start()
    {
        if (Const_Dev.GiveItems)
        {
            AddStickers();
            AddKeys();
        }
    }

    public void AddItemById()
    {
        Script_Game.Game.AddItemById(itemId);
    }

    public void AddStickers()
    {
        AddPsychicDuck();
        AddAnimalWithin();
        AddBoarNeedle();
        AddIceSpike();
        AddMelancholyPiano();
        AddLastElevator();
        AddLetThereBeLight();
        AddPuppeteer();
    }

    public void AddKeys()
    {
        AddSuperSmallKey();
    }

    public void GrandMirror()
    {
        AddPsychicDuck();
        AddAnimalWithin();
        AddBoarNeedle();
        AddIceSpike();
        AddMelancholyPiano();
    }
    
    public void WeekendCycle()
    {
        AddPsychicDuck();
        AddAnimalWithin();
        AddBoarNeedle();
        AddIceSpike();
        AddMelancholyPiano();
        AddLastElevator();
    }

    public void UnequipAll()
    {
        Script_Game.Game.UnequipAll();
    }
    
    // ------------------------------------------------------------------------
    //  Stickers
    public void AddPsychicDuck()
    {
        Script_Game.Game.AddItemById(Const_Items.PsychicDuckId);
    }

    public void AddBoarNeedle()
    {
        Script_Game.Game.AddItemById(Const_Items.BoarNeedleId);
    }

    public void AddAnimalWithin()
    {
        Script_Game.Game.AddItemById(Const_Items.AnimalWithinId);
    }

    public void AddIceSpike()
    {
        Script_Game.Game.AddItemById(Const_Items.IceSpikeId);
    }

    public void AddMelancholyPiano()
    {
        Script_Game.Game.AddItemById(Const_Items.MelancholyPianoId);
    }

    public void AddLastElevator()
    {
        Script_Game.Game.AddItemById(Const_Items.LastElevatorId);
    }

    public void AddLetThereBeLight()
    {
        Script_Game.Game.AddItemById(Const_Items.LetThereBeLightId);
    }

    public void AddPuppeteer()
    {
        Script_Game.Game.AddItemById(Const_Items.PuppeteerId);
    }

    public void AddMyMask()
    {
        Script_Game.Game.AddItemById(Const_Items.MyMaskId);
    }

    // ------------------------------------------------------------------------
    //  Usables
    public void AddMasterKey()
    {
        Script_Game.Game.AddItemById(Const_Items.MasterKeyId);
    }

    public void AddSuperSmallKey()
    {
        Script_Game.Game.AddItemById(Const_Items.SuperSmallKeyId);
    }

    // ------------------------------------------------------------------------
    //  Collectibles

    public void AddWinterStone()
    {
        Script_Game.Game.AddItemById(Const_Items.WinterStoneId);
    }

    public void AddSpringStone()
    {
        Script_Game.Game.AddItemById(Const_Items.SpringStoneId);
    }

    public void AddSummerStone()
    {
        Script_Game.Game.AddItemById(Const_Items.SummerStoneId);
    }

    public void AddAutumnStone()
    {
        Script_Game.Game.AddItemById(Const_Items.AutumnStoneId);
    }

    public void AddLastWellMap()
    {
        Script_Game.Game.AddItemById(Const_Items.LastWellMapId);
    }
    
    public void AddLastSpellRecipeBook()
    {
        Script_Game.Game.AddItemById(Const_Items.LastSpellRecipeBookId);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Dev_InventoryTester))]
public class Dev_InventoryTesterTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Dev_InventoryTester t = (Dev_InventoryTester)target;
        
        var style = EditorStyles.foldoutHeader;

        EditorGUILayout.LabelField("Item Utils");

        if (GUILayout.Button("Add Item By Id"))
        {
            t.AddItemById();
        }

        if (GUILayout.Button("Unequip All"))
        {
            t.UnequipAll();
        }
        
        GUILayout.Space(8);

        EditorGUILayout.LabelField("Masks");
        
        if (GUILayout.Button("Add: Psychic Duck"))
        {
            t.AddPsychicDuck();
        }

        if (GUILayout.Button("Add: Boar Needle"))
        {
            t.AddBoarNeedle();
        }

        if (GUILayout.Button("Add: Animal Within"))
        {
            t.AddAnimalWithin();
        }

        if (GUILayout.Button("Add: Ice Spike"))
        {
            t.AddIceSpike();
        }

        if (GUILayout.Button("Add: Melancholy Piano"))
        {
            t.AddMelancholyPiano();
        }

        if (GUILayout.Button("Add: Last Elevator"))
        {
            t.AddLastElevator();
        }

        if (GUILayout.Button("Add: Let There Be Light"))
        {
            t.AddLetThereBeLight();
        }

        if (GUILayout.Button("Add: Puppeteer"))
        {
            t.AddPuppeteer();
        }

        if (GUILayout.Button("Add: MyMask"))
        {
            t.AddMyMask();
        }

        GUILayout.Space(8);

        EditorGUILayout.LabelField("Usables");

        if (GUILayout.Button("Add: Super Small Key"))
        {
            t.AddSuperSmallKey();
        }

        GUILayout.Space(8);

        EditorGUILayout.LabelField("Collectibles");

        if (GUILayout.Button("Add: Last Well Map"))
        {
            t.AddLastWellMap();
        }

        if (GUILayout.Button("Add: Last Spell Recipe Book"))
        {
            t.AddLastSpellRecipeBook();
        }
    }
}
#endif