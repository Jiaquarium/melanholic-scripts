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
    public enum Saves {
        /// Saves run data and game 
        SavePoint,
        /// Saves game data, erasing run data
        Initialize,
        /// Overwrites game data with the last initialize save
        RestartInitialized
    }
    
    public static Script_SaveGameControl control;
    public Script_Game game;
    public static string path;
    public static int saveSlotId;
    public static string saveFileName;
    public static string saveInitializeFileName;
    public static string saveFilePath;
    public static string saveInitializeFilePath;
    public static string savedGameTitleDataFileName;
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

    public static void SetPath()
    {
        saveFileName                = $"saveData{Application.version}";
        saveInitializeFileName      = $"saveDataInitialize{Application.version}";
        savedGameTitleDataFileName  = $"savedGameTitleData{Application.version}";
        
        path                        = Application.persistentDataPath;

        saveFilePath                = $"{path}/{saveFileName}_{saveSlotId}.dat";
        saveInitializeFilePath      = $"{path}/{saveInitializeFileName}_{saveSlotId}.dat";
        savedGameTitleDataPath      = $"{path}/{savedGameTitleDataFileName}_{saveSlotId}.dat";

        Debug.Log($"Persistent path is: {Application.persistentDataPath}");
        Debug.Log($"Currently using path: {path}");
    }

    /// <param name="type">SavePoint saves within Kelsingor</param>
    /// <param name="playerStateOverride">Option to override player state</param>
    public void Save(
        Saves type,
        Model_PlayerState playerStateOverride = null,
        Model_GameData gameDataOverride = null
    )
    {
        SetPath();

        try
        {
            /// Update total play time
            game.OnSaveTasks();
            
            BinaryFormatter bf  = new BinaryFormatter();

            /// Main data to pass around and modify
            Model_SaveData saveData = new Model_SaveData();

            /// Depending on what type of save, we need to modify Run data appropriately
            switch (type)
            {
                case (Saves.SavePoint):
                    SaveGame(saveData);
                    OverridePlayerData(saveData, playerStateOverride);
                    HandleSaveRun(saveData);
                    WriteSaveDataFile(bf, saveFilePath, saveData);
                    break;
                
                /// Should only be called before beginning a new run 
                case (Saves.Initialize):
                    SaveGame(saveData);
                    OverridePlayerData(saveData, playerStateOverride);
                    HandleInitialSave(saveData);
                    WriteSaveDataFile(bf, saveFilePath, saveData);
                    /// Create a copy of the file at the beginning of the run in case player wants
                    /// to restart back to this point 
                    WriteSaveDataFile(bf, saveInitializeFilePath, saveData);
                    break;
                
                /// Replace current game saved data and with the SaveDataInitialize
                case (Saves.RestartInitialized):
                    HandleRestartFromInitialSave();
                    break;
            }

            /// Create SavedGameTitleData
            FileStream titleFile    = File.Create(savedGameTitleDataPath);            
            Model_SavedGameTitleData savedGameTitleData = savedGameTitleDataHandler.Create();
            bf.Serialize(titleFile, savedGameTitleData);
            titleFile.Close();

            if (Debug.isDebugBuild) Debug.Log("Save successful at: " + saveFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed Save with exception: " + e.ToString());
        }

        void SaveGame(Model_SaveData data)
        {
            gameHandler.SaveGameData(data, gameDataOverride);
            playerHandler.SavePlayerData(data);
            entriesHandler.SaveEntries(data);
            inventoryHandler.SaveInventory(data);
            equipmentHandler.SaveEquipment(data);
            dropsHandler.SaveDrops(data);
            namesHandler.SaveNames(data);
            scarletCipherHandler.SaveScarletCipher(data);
        }

        void WriteSaveDataFile(BinaryFormatter bf, string filePath, Model_SaveData data)
        {
            // will overwrite existing file
            FileStream gameFile = File.Create(filePath);
            bf.Serialize(gameFile, data);
            gameFile.Close();
        }

        void OverridePlayerData(Model_SaveData data, Model_PlayerState playerState)
        {
            data.playerData = playerState ?? data.playerData;
        }

        void HandleSaveRun(Model_SaveData data)
        {
            levelsHandler.SaveLevels(data);
        }
        
        void HandleInitialSave(Model_SaveData data)
        {
            data.runData = new Model_RunData();
        }

        void HandleRestartFromInitialSave()
        {
            try
            {
                if (File.Exists(saveFilePath) && File.Exists(saveInitializeFilePath))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream initFile = File.Open(saveInitializeFilePath, FileMode.Open);
                    Model_SaveData data = (Model_SaveData)bf.Deserialize(initFile);
                    initFile.Close();
                    
                    /// Update playtime
                    gameHandler.UpdatePlayTime(data);
                    
                    /// Update init file with new play time
                    WriteSaveDataFile(bf, saveInitializeFilePath, data);
                    
                    /// Replace saveFile with the updated init file
                    File.Copy(saveInitializeFilePath, saveFilePath, true);
                }
                else
                {
                    Debug.LogError(
                        $"You are missing the save file and/or saveInitialize file when you should have both"
                    );
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed copying saveInitialized file with exception: " + e.ToString());
            }
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
            // copy saveData
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

    public void Setup()
    {
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
}