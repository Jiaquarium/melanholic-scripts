using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadNames : MonoBehaviour
{
    public void SaveNames(Model_SaveData data)
    {
        Model_Names names = new Model_Names(
            _Player: Script_Names.Player,
            _Eileen: Script_Names.Eileen,
            _Ellenia: Script_Names.Ellenia,
            _ElleniaPassword: Script_Names.ElleniaPassword,
            _Tedwich: Script_Names.Tedwich,
            _Ursie: Script_Names.Ursie
        );
        data.namesData = names;
    }

    public void LoadNames(Model_SaveData data)
    {
        if (data.namesData == null)
        {
            Debug.Log("There is no names state data to load.");
            return;
        }

        Model_Names names = new Model_Names(
            _Player: data.namesData.Player,
            _Eileen: data.namesData.Eileen,
            _Ellenia: data.namesData.Ellenia,
            _ElleniaPassword: data.namesData.ElleniaPassword,
            _Tedwich: data.namesData.Tedwich,
            _Ursie: data.namesData.Ursie
        );

        Script_Names.LoadNames(names);
    }
}
