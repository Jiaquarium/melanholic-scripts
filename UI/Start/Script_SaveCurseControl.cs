using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Script_SaveCurseControl : MonoBehaviour
{
    public static Script_SaveCurseControl Instance;
    public const string CurseTag = "cursetag";

    // For Start Scene
    [SerializeField] private Script_Start startHandler;

    private string path;
    private string filePath;

    private void SetPath()
    {
        path = Application.persistentDataPath;
        filePath = $"{path}/{Script_Utils.CurseFile}";
    }
    
    public void Save()
    {
        try 
        {
            SetPath();
            
            // Load save file
            BinaryFormatter bf = new BinaryFormatter();

            Model_CurseData curseData = new Model_CurseData(CurseTag);

            // Will overwrite existing file
            FileStream curseFile = File.Create(filePath);
            bf.Serialize(curseFile, curseData);
            curseFile.Close();

            if (Debug.isDebugBuild)
                Dev_Logger.Debug($"Successfully saved curse at existing file: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{name} Failed Save with exception: " + e.ToString());
        }
    }   

    public void Load()
    {
        SetPath();
        
        try 
        {
            if (File.Exists(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(filePath, FileMode.Open);
                Model_CurseData curseData = (Model_CurseData)bf.Deserialize(file);

                // Load data
                if (curseData != null)
                    startHandler.curse = curseData.curse;

                file.Close();

                if (Debug.isDebugBuild)
                    Dev_Logger.Debug($"Successful load at: {filePath}; data: {curseData}");
            }
            else
            {
                if (Debug.isDebugBuild)
                    Dev_Logger.Debug($"{name} Did not load; file not found at {filePath}.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{name} Failed Load with exception: " + e.ToString());
        }
    }

    public void Delete()
    {
        SetPath();
        
        try 
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);

                Dev_Logger.Debug($"{name} Curse data file deleted at: {filePath}.");
            }
            else
            {
                Dev_Logger.Debug($"{name} Could not delete curse data file; not found at: {filePath}.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{name} Failed Load with exception: " + e.ToString());
        }
    }

    public void Setup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}
