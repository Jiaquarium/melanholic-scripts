using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Looks recursively through Script_SaveLoadLevelBehavior children and calls their SaveLoad funcs 
/// You still need to update Model_SaveData.Model_LevelData to reflect the dataz though!!!
/// </summary>
public class Script_SaveLoadLevels : MonoBehaviour
{
    [SerializeField] private Script_SaveLoadLevelBehavior[] levelBehaviors;
    
    public void SaveLevels(Model_SaveData data)
    {
        levelBehaviors = transform.GetComponentsInChildren<Script_SaveLoadLevelBehavior>(true);
        foreach (Script_SaveLoadLevelBehavior LB in levelBehaviors)
        {
            LB.Save(data);
        }
    }

    public void LoadLevels(Model_SaveData data)
    {
        levelBehaviors = transform.GetComponentsInChildren<Script_SaveLoadLevelBehavior>(true);
        foreach (Script_SaveLoadLevelBehavior LB in levelBehaviors)
        {
            LB.Load(data);
        }
    }
}
