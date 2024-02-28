using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Purchasing.MiniJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_SaveLanguagePreferenceControl : MonoBehaviour
{
    public static Script_SaveLanguagePreferenceControl Instance;
    
    private string path;
    private string filePath;
    
    private void SetPath()
    {
        path = Application.persistentDataPath;
        filePath = $"{path}/{Script_Utils.LangPrefFile}";
    }

    public void Save()
    {
        try
        {
            SetPath();

            Model_LanguagePreference langPref = new Model_LanguagePreference();

            Script_LocalizationUtils.SaveLangPref(langPref);

            string json = JsonUtility.ToJson(langPref);
            File.WriteAllText(filePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed Language Preference Save: " + e.ToString());
        }
    }

    public bool Load()
    {
        try
        {
            SetPath();

            if (!File.Exists(filePath))
                return false;

            Model_LanguagePreference langPref = JsonUtility
                .FromJson<Model_LanguagePreference>(File.ReadAllText(filePath));
            
            if (langPref == null)
                throw new System.Exception("The .dat file either has been modified or is empty");
            
            if (!Script_LocalizationUtils.CheckValidLang(langPref.lang))
            {
                string errorMessage = langPref.lang == null
                    ? errorMessage = $"The key is missing, langPref.lang is null"
                    : errorMessage = $"{langPref.lang} is not a valid language; string may also be empty.";

                throw new System.Exception(errorMessage);
            }
            
            Script_LocalizationUtils.SwitchGameLangByLang(langPref.lang);

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Failed Language Preference Load: " + e.ToString());

            return false;
        }
    }

    public void Delete()
    {
        try
        {
            SetPath();

            if (File.Exists(filePath))
            {
                File.Delete(filePath);

                Dev_Logger.Debug($"{name} Language Preference data file deleted at: {filePath}.");
            }
            else
            {
                Dev_Logger.Debug($"{name} Could not delete Language Preference data file; not found at: {filePath}.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{name} Failed Delete with exception: " + e.ToString());
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
[CustomEditor(typeof(Script_SaveLanguagePreferenceControl))]
public class Script_SaveLanguagePreferenceControlTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_SaveLanguagePreferenceControl t = (Script_SaveLanguagePreferenceControl)target;
        
        if (GUILayout.Button("Save"))
        {
            t.Save();
        }

        if (GUILayout.Button("Load"))
        {
            t.Load();
        }

        if (GUILayout.Button("Delete"))
        {
            t.Delete();
        }
    }
}
#endif