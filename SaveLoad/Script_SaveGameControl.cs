using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Must set the Id here first before doing operations
/// </summary>
public class Script_SaveGameControl : MonoBehaviour
{
    public static Script_SaveGameControl control;
    public Script_Game game;
    public static string path;
    public static int saveSlotId;
    public static string saveFileName = "gameInfo";
    public static string saveFilePath;
    public static string savedGameTitleDataFileName = "savedGameTitleData";
    public static string savedGameTitleDataPath;
    
    public Script_SaveSavedGameTitleData savedGameTitleDataHandler;
    public Script_SaveLoadGame gameHandler;
    public Script_SaveLoadPlayer playerHandler;
    public Script_SaveLoadEntries entriesHandler;
    public Script_SaveLoadInventory inventoryHandler;
    public Script_SaveLoadEquipment equipmentHandler;
    public Script_SaveLoadDrops dropsHandler;
    public Script_SaveLoadLevels levelsHandler;
    public Script_SaveLoadNames namesHandler;
    [SerializeField] Script_SaveLoadScarletCipher scarletCipherHandler;

    void Awake() {
        if (control == null)
        {
            control = this;
        }
        else if (control != this)
        {
            Destroy(this.gameObject);
        }

        SetPath();
    }

    public static void SetPath()
    {
        path                    = Application.persistentDataPath;

        saveFilePath            = $"{path}{saveFileName}{saveSlotId}.dat";
        savedGameTitleDataPath  = $"{path}{savedGameTitleDataFileName}{saveSlotId}.dat";

        Debug.Log($"Persistent path is: {Application.persistentDataPath}");
        Debug.Log($"Currently using path: {path}");
    }

    public virtual void OnInspectorGUI(){}

    public void Save()
    {
        SetPath();

        try
        {
            game.OnSaveTasks();
            
            BinaryFormatter bf = new BinaryFormatter();
            // will overwrite existing file
            FileStream gameFile     = File.Create(saveFilePath);

            /// Main data to pass around and modify
            Model_SaveData gameInfo = new Model_SaveData();
            
            // modify with all necessary persistent state gameInfo
            gameHandler.SaveGameData(gameInfo);
            playerHandler.SavePlayerData(gameInfo);
            entriesHandler.SaveEntries(gameInfo);
            inventoryHandler.SaveInventory(gameInfo);
            equipmentHandler.SaveEquipment(gameInfo);
            dropsHandler.SaveDrops(gameInfo);
            levelsHandler.SaveLevels(gameInfo);
            namesHandler.SaveNames(gameInfo);
            scarletCipherHandler.SaveScarletCipher(gameInfo);

            bf.Serialize(gameFile, gameInfo);
            gameFile.Close();

            /// Create SavedGameTitleData
            FileStream titleFile    = File.Create(savedGameTitleDataPath);            
            Model_SavedGameTitleData savedGameTitleData = savedGameTitleDataHandler.Create();
            bf.Serialize(titleFile, savedGameTitleData);
            titleFile.Close();

            // TODO: CREATE FILES AS bleh.dat, then change name once all files have succeeded,
            // so we don't get corrupt files

            if (Debug.isDebugBuild) Debug.Log("Save successful at: " + saveFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed Save with exception: " + e.ToString());
        }
    }

    public bool Load()
    {
        SetPath();
        
        try 
        {
            if (File.Exists(saveFilePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(saveFilePath, FileMode.Open);
                Model_SaveData data = (Model_SaveData)bf.Deserialize(file);
                file.Close();

                gameHandler.LoadGameData(data);
                playerHandler.LoadPlayerData(data);
                entriesHandler.LoadEntries(data);
                inventoryHandler.LoadInventory(data);
                equipmentHandler.LoadEquipment(data);
                dropsHandler.LoadDrops(data);
                levelsHandler.LoadLevels(data);
                namesHandler.LoadNames(data);
                scarletCipherHandler.LoadScarletCipher(data);

                if (Debug.isDebugBuild) Debug.Log("Successful load at: " + saveFilePath);
                return true;
            }
            else
            {
                if (Debug.isDebugBuild) Debug.Log("Did not load; file not found.");
                return false;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed Load with exception: " + e.ToString());
            return false;
        }
    }

    public static bool Delete()
    {
        SetPath();
        
        try
        {
            // delete Save Info
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
                if (Debug.isDebugBuild) Debug.Log($"Save Info file deleted at: {saveFilePath}.");
            }
            else
            {
                if (Debug.isDebugBuild) Debug.Log($"Could not delete Save Info; not found at: {saveFilePath}.");
            }
            
            // delete Saved Game Title Data Info
            if (File.Exists(savedGameTitleDataPath))
            {
                File.Delete(savedGameTitleDataPath);
                if (Debug.isDebugBuild) Debug.Log($"Saved Game Title Data file deleted at: {savedGameTitleDataPath}.");
            }
            else
            {
                if (Debug.isDebugBuild) Debug.Log($"Could not delete Save Game Title Data; not found at: {savedGameTitleDataPath}.");
            }

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed Delete with exception: " + e.ToString());
            return false;
        }
    }

    public static bool Copy(int newSlotId)
    {
        SetPath();

        try
        {
            // copy gameInfo
            if (File.Exists(saveFilePath))
            {
                string newSaveFilePath                = $"{path}{saveFileName}{newSlotId}.dat";
                File.Copy(saveFilePath, newSaveFilePath, true);
            }

            // copy titleData
            if (File.Exists(savedGameTitleDataPath))
            {
                string newSavedGameTitleDataPath      = $"{path}{savedGameTitleDataFileName}{newSlotId}.dat";
                File.Copy(savedGameTitleDataPath, newSavedGameTitleDataPath, true);
            }

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed Copy with exception: " + e.ToString());
            return false;
        }
    }
}