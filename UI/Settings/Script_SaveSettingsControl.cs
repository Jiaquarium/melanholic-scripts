using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_SaveSettingsControl : MonoBehaviour
{
    public static Script_SaveSettingsControl Instance;
    
    [SerializeField] private Script_SettingsSystemController systemHandler;
    
    public void Save()
    {
        try
        {
            string path = Application.persistentDataPath;
            string filePath = $"{path}/{Script_Utils.SettingsFile}";

            Model_SettingsData settingsData = new Model_SettingsData();

            // Save handlers
            Script_PlayerInputManager.Instance.Save(settingsData);
            systemHandler.Save(settingsData);

            string json = JsonUtility.ToJson(settingsData);
            File.WriteAllText(filePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed Settings Save: " + e.ToString());
        }
    }   

    public void Load()
    {
        try
        {
            string path = Application.persistentDataPath;
            string filePath = $"{path}/{Script_Utils.SettingsFile}";

            if(!File.Exists(filePath))
                return;

            Model_SettingsData settingsData = JsonUtility.FromJson<Model_SettingsData>(File.ReadAllText(filePath));
            
            // Load handlers
            Script_PlayerInputManager.Instance.Load(settingsData);
            systemHandler.Load(settingsData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed Settings Load: " + e.ToString());
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

#if UNITY_EDITOR
[CustomEditor(typeof(Script_SaveSettingsControl))]
public class Script_SaveSettingsControlTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_SaveSettingsControl t = (Script_SaveSettingsControl)target;
        
        if (GUILayout.Button("Save"))
        {
            t.Save();
        }

        if (GUILayout.Button("Load"))
        {
            t.Load();
        }
    }
}
#endif