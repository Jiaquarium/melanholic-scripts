using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadNames : MonoBehaviour
{
    public void SaveNames(Model_SaveData data)
    {
        Model_Names names = new Model_Names(
            _Player:                    Script_Names.PlayerUnbold,
            _Ids:                       Script_Names.IdsNameStateEN(),
            _Ero:                       Script_Names.Ero,
            _Myne:                      Script_Names.MyneNameStateEN(),
            _Eileen:                    Script_Names.EileenNameStateEN(),
            _Ellenia:                   Script_Names.ElleniaNameStateEN(),
            _Tedwich:                   Script_Names.Tedwich,
            _Ursie:                     Script_Names.UrsieNameStateEN(),
            _Kaffe:                     Script_Names.KaffeNameStateEN(),
            _Latte:                     Script_Names.LatteNameStateEN(),
            _KingEclaire:               Script_Names.KingEclaireNameStateEN(),
            _Suzette:                   Script_Names.SuzetteNameStateEN(),
            _Peche:                     Script_Names.PecheNameStateEN(),
            _Melba:                     Script_Names.MelbaNameStateEN(),
            _Moose:                     Script_Names.MooseNameStateEN(),
            _Flan:                      Script_Names.FlanNameStateEN()
        );
        data.namesData = names;
    }

    public void LoadNames(Model_SaveData data)
    {
        if (data.namesData == null)
        {
            Dev_Logger.Debug("There is no names state data to load.");
            return;
        }

        Model_Names names = new Model_Names(
            _Player:                    data.namesData.Player,
            _Ids:                       data.namesData.Ids,
            _Ero:                       data.namesData.Ero,
            _Myne:                      data.namesData.Myne,
            _Eileen:                    data.namesData.Eileen,
            _Ellenia:                   data.namesData.Ellenia,
            _Tedwich:                   data.namesData.Tedwich,
            _Ursie:                     data.namesData.Ursie,
            _Kaffe:                     data.namesData.Kaffe,
            _Latte:                     data.namesData.Latte,
            _KingEclaire:               data.namesData.KingEclaire,
            _Suzette:                   data.namesData.Suzette,
            _Peche:                     data.namesData.Peche,
            _Melba:                     data.namesData.Melba,
            _Moose:                     data.namesData.Moose,
            _Flan:                      data.namesData.Flan
        );

        Script_Names.LoadNames(names);

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(names);
    }
}
