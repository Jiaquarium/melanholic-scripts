using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadScarletCipher : MonoBehaviour
{
    [SerializeField] private Script_ScarletCipher scarletCipher;
    
    public void SaveScarletCipher(Model_SaveData data)
    {
        Model_ScarletCipher ScarletCipher = new Model_ScarletCipher(
            _scarletCipher: scarletCipher.ScarletCipher,
            _visibility: scarletCipher.ScarletCipherVisibility
        );
        data.scarletCipherData = ScarletCipher;
    }

    public void LoadScarletCipher(Model_SaveData data)
    {
        if (data.scarletCipherData == null)
        {
            Debug.Log("There is no ScarletCipher state data to load.");
            return;
        }

        Model_ScarletCipher ScarletCipher = new Model_ScarletCipher(
            _scarletCipher:     data.scarletCipherData.scarletCipher,
            _visibility:        data.scarletCipherData.visibility 
        );

        scarletCipher.ScarletCipher             = ScarletCipher.scarletCipher;
        scarletCipher.ScarletCipherVisibility   = ScarletCipher.visibility;
    }
}
