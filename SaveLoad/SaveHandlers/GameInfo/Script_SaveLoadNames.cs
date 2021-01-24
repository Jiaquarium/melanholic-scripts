using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadNames : MonoBehaviour
{
    public void SaveNames(Model_SaveData data)
    {
        Model_Names names = new Model_Names(
            _Player:                    Script_Names.Player,
            _Ids:                       Script_Names.Ids,
            _Ero:                       Script_Names.Ero,
            _Myne:                      Script_Names.Myne,
            _Eileen:                    Script_Names.Eileen,
            _Ellenia:                   Script_Names.Ellenia,
            _ElleniaPassword:           Script_Names.ElleniaPassword,
            _Tedwich:                   Script_Names.Tedwich,
            _Ursie:                     Script_Names.Ursie,
            _Kaffe:                     Script_Names.Kaffe,
            _Latte:                     Script_Names.Latte,
            _KingEclaire:               Script_Names.KingEclaire,
            _Suzette:                   Script_Names.Suzette,
            _Peche:                     Script_Names.Peche,
            _Melba:                     Script_Names.Melba,
            _Moose:                     Script_Names.Moose
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
            _Player:                    data.namesData.Player,
            _Ids:                       data.namesData.Ids,
            _Ero:                       data.namesData.Ero,
            _Myne:                      data.namesData.Myne,
            _Eileen:                    data.namesData.Eileen,
            _Ellenia:                   data.namesData.Ellenia,
            _ElleniaPassword:           data.namesData.ElleniaPassword,
            _Tedwich:                   data.namesData.Tedwich,
            _Ursie:                     data.namesData.Ursie,
            _Kaffe:                     data.namesData.Kaffe,
            _Latte:                     data.namesData.Latte,
            _KingEclaire:               data.namesData.KingEclaire,
            _Suzette:                   data.namesData.Suzette,
            _Peche:                     data.namesData.Peche,
            _Melba:                     data.namesData.Melba,
            _Moose:                     data.namesData.Moose
        );

        Script_Names.LoadNames(names);

        Debug.Log($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(names);
    }
}
