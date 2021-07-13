using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_PianoChoiceText : MonoBehaviour
{
    private const string DefaultText = "???";
    
    [SerializeField] private int Id;
    [SerializeField] private TextMeshProUGUI TMP;

    [SerializeField] private Script_PianoManager pianoManager;

    void OnValidate()
    {
        UpdateText();
    }
    
    void OnEnable()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        if (pianoManager?.GetPianoIsRemembered(Id) ?? false)
        {
            string mapName = pianoManager.GetPianoMapName(Id);
            Debug.Log($"Id: {Id}, mapName: {mapName}");
            
            TMP.text = string.IsNullOrEmpty(mapName) ? DefaultText : mapName;
        }
        else
        {
            TMP.text = DefaultText;
        }
    }
}
