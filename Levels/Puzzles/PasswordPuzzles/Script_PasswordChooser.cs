using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_PasswordChooser : MonoBehaviour
{
    [SerializeField] private string[] words;
    
    public string GetRandomWord()
    {
        string randomWord = words[Random.Range(0, words.Length)];
        return randomWord;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_PasswordChooser))]
public class Script_PasswordChooserTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_PasswordChooser pwChooser = (Script_PasswordChooser)target;
        if (GUILayout.Button("GetRandomWord()"))
        {
            pwChooser.GetRandomWord();
        }
    }
}
#endif