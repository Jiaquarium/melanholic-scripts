using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadPianos : MonoBehaviour
{
    [SerializeField] private Script_PianoManager pianoManager;
    
    public void SavePianos(Model_SaveData data)
    {
        Model_PianoStateData[] pianosStateData = new Model_PianoStateData[Script_PianoManager.NumPianos];
        
        for (int i = 0; i < Script_PianoManager.NumPianos; i++)
        {
            pianosStateData[i] = new Model_PianoStateData(pianoManager.Pianos[i].IsRemembered);
        }

        data.pianosStateData = pianosStateData;
    }

    public void LoadPianos(Model_SaveData data)
    {
        for (int i = 0; i < Script_PianoManager.NumPianos; i++)
        {
            pianoManager.Pianos[i].IsRemembered = data.pianosStateData[i].isRemembered;
        }
    }
}
