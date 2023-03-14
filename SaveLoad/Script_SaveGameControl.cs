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
    public static string saveFilePath;
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
    
    [SerializeField] private Script_SaveLoadScarletCipher scarletCipherHandler;
    [SerializeField] private Script_SaveLoadEventCycle eventCycleHandler;

    [SerializeField] private Script_SaveLoadMynesMirror mynesMirrorHandler;

    [SerializeField] private Script_SaveLoadPianos pianosHandler;
    [SerializeField] private Script_SaveLoadAchievements achievementsHandler;
    [SerializeField] private Script_SaveLoadAutoSaves autoSavesHandler;

    public static void SetPath()
    {
        saveFileName                = Script_Utils.SaveFile(saveSlotId);
        savedGameTitleDataFileName  = Script_Utils.SaveTitleDataFile(saveSlotId);
        
        path                        = Application.persistentDataPath;

        saveFilePath                = $"{path}/{saveFileName}";
        savedGameTitleDataPath      = $"{path}/{savedGameTitleDataFileName}";

        Dev_Logger.Debug($"Persistent path is: {Application.persistentDataPath}");
        Dev_Logger.Debug($"Currently using path: {path}");
    }

    /// <param name="type">SavePoint saves within Kelsingor</param>
    /// <param name="playerStateOverride">Option to override player state</param>
    public void Save(
        Model_PlayerState playerStateOverride = null,
        Model_GameData gameDataOverride = null
    )
    {
        SetPath();

        try
        {
            /// Update total play time
            game.OnSaveTasks();
            
            BinaryFormatter bf = new BinaryFormatter();

            /// Main data to pass around and modify
            Model_SaveData saveData = new Model_SaveData();

            SaveGame(saveData);
            OverridePlayerData(saveData, playerStateOverride);
            HandleSaveRun(saveData);
            WriteSaveDataFile(bf, saveFilePath, saveData);

            /// Create SavedGameTitleData
            FileStream titleFile = File.Create(savedGameTitleDataPath);            
            Model_SavedGameTitleData savedGameTitleData = savedGameTitleDataHandler.Create();
            bf.Serialize(titleFile, savedGameTitleData);
            titleFile.Close();

            if (Debug.isDebugBuild) Dev_Logger.Debug("Save successful at: " + saveFilePath);
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
            namesHandler.SaveNames(data);
            scarletCipherHandler.SaveScarletCipher(data);
            eventCycleHandler.SaveEventCycle(data);
            mynesMirrorHandler.SaveMynesMirror(data);
            pianosHandler.SavePianos(data);
            achievementsHandler.SaveAchievements(data);
            autoSavesHandler.SaveAutoSaves(data);
        }

        void OverridePlayerData(Model_SaveData data, Model_PlayerState playerState)
        {
            data.playerData = playerState ?? data.playerData;
        }

        void HandleSaveRun(Model_SaveData data)
        {
            levelsHandler.SaveLevels(data);
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
                levelsHandler.LoadLevels(data);
                namesHandler.LoadNames(data);
                scarletCipherHandler.LoadScarletCipher(data);
                eventCycleHandler.LoadEventCycle(data);
                mynesMirrorHandler.LoadMynesMirror(data);
                pianosHandler.LoadPianos(data);
                achievementsHandler.LoadAchievements(data);
                autoSavesHandler.LoadAutoSaves(data);

                if (Debug.isDebugBuild) Dev_Logger.Debug("Successful load at: " + saveFilePath);
                return true;
            }
            else
            {
                if (Debug.isDebugBuild) Dev_Logger.Debug("Did not load; file not found.");
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
                if (Debug.isDebugBuild) Dev_Logger.Debug($"Save Info file deleted at: {saveFilePath}.");
            }
            else
            {
                if (Debug.isDebugBuild) Dev_Logger.Debug($"Could not delete Save Info; not found at: {saveFilePath}.");
            }
            
            // delete Saved Game Title Data Info
            if (File.Exists(savedGameTitleDataPath))
            {
                File.Delete(savedGameTitleDataPath);
                if (Debug.isDebugBuild) Dev_Logger.Debug($"Saved Game Title Data file deleted at: {savedGameTitleDataPath}.");
            }
            else
            {
                if (Debug.isDebugBuild) Dev_Logger.Debug($"Could not delete Save Game Title Data; not found at: {savedGameTitleDataPath}.");
            }

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed Delete with exception: " + e.ToString());
            return false;
        }
    }

    public static bool Copy(int fromSlot, int newSlotId)
    {
        SetPath();

        try
        {
            // copy saveData
            var fromFileName = Script_Utils.SaveFile(fromSlot);
            var fromTitleDataFileName = Script_Utils.SaveTitleDataFile(fromSlot);

            var newFileName = Script_Utils.SaveFile(newSlotId);
            var newTitleDataFileName = Script_Utils.SaveTitleDataFile(newSlotId);

            var fromFilePath = $"{path}/{fromFileName}";
            var fromTitleDataFilePath = $"{path}/{fromTitleDataFileName}";

            var newFilePath = $"{path}/{newFileName}";
            var newTitleDataFilePath = $"{path}/{newTitleDataFileName}";

            Dev_Logger.Debug($"From File Path: {fromFilePath}");
            Dev_Logger.Debug($"To File Path: {newFilePath}");

            Dev_Logger.Debug($"From Title Data File Path: {fromTitleDataFilePath}");
            Dev_Logger.Debug($"To Title Data File Path: {newTitleDataFilePath}");

            if (File.Exists(fromFilePath))
            {
                File.Copy(fromFilePath, newFilePath, true);
            }

            // copy titleData
            if (File.Exists(fromTitleDataFilePath))
            {
                File.Copy(fromTitleDataFilePath, newTitleDataFilePath, true);
            }

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed Copy with exception: " + e.ToString());
            return false;
        }
    }

    public bool SaveAchievements()
    {
        SetPath();
        
        try 
        {
            if (File.Exists(saveFilePath))
            {
                // Load save file
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(saveFilePath, FileMode.Open);
                Model_SaveData data = (Model_SaveData)bf.Deserialize(file);
                file.Close();

                // Save achievements
                achievementsHandler.SaveAchievements(data);
                WriteSaveDataFile(bf, saveFilePath, data);

                if (Debug.isDebugBuild)
                    Dev_Logger.Debug($"Successfully saved achievements at existing file: {saveFilePath}");

                return true;
            }
            else
            {
                if (Debug.isDebugBuild)
                    Dev_Logger.Debug("Did not save; file not found.");

                return false;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed SaveAchievements with exception: " + e.ToString());
            return false;
        }
    }

    public bool SaveAutoSaves()
    {
        SetPath();
        
        try 
        {
            if (File.Exists(saveFilePath))
            {
                // Load save file
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(saveFilePath, FileMode.Open);
                Model_SaveData data = (Model_SaveData)bf.Deserialize(file);
                file.Close();

                // Save autoSaves
                autoSavesHandler.SaveAutoSaves(data);
                WriteSaveDataFile(bf, saveFilePath, data);

                if (Debug.isDebugBuild)
                    Dev_Logger.Debug($"Successfully saved autoSaves at existing file: {saveFilePath}");

                return true;
            }
            else
            {
                if (Debug.isDebugBuild)
                    Dev_Logger.Debug("Did not save; file not found.");

                return false;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed SaveAutoSaves with exception: " + e.ToString());
            return false;
        }
    }

    private void WriteSaveDataFile(BinaryFormatter bf, string filePath, Model_SaveData data)
    {
        // will overwrite existing file
        FileStream gameFile = File.Create(filePath);
        bf.Serialize(gameFile, data);
        gameFile.Close();
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