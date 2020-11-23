using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Script_SavedGameTitleControl : MonoBehaviour
{
    public static Script_SavedGameTitleControl Control;
    
    private void Awake()
    {
        Script_SaveGameControl.SetPath();
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }
    }
    
    public Model_SavedGameTitleData Load(int saveSlotId)
    {
        string path = Script_SaveGameControl.path;
        string fileName = Script_SaveGameControl.savedGameTitleDataFileName;
        string filePath = $"{path}{fileName}{saveSlotId}.dat";
        
        try 
        {
            if (File.Exists(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(filePath, FileMode.Open);
                Model_SavedGameTitleData data = (Model_SavedGameTitleData)bf.Deserialize(file);
                file.Close();

                if (Debug.isDebugBuild) Debug.Log("Successful title load at: " + filePath);
                return data;
            }
            else
            {
                if (Debug.isDebugBuild) Debug.Log("Did not load; file not found.");
                return null;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed Load with exception: " + e.ToString());
            return null;
        }      
    } 
}
