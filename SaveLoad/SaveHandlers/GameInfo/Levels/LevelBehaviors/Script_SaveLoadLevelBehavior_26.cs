using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior_26 : Script_SaveLoadLevelBehavior
{
    [SerializeField] private Script_LevelBehavior_26 LB26;

    public override void Save(Model_RunData data)
    {
        Model_LevelBehavior_26 lvlModel = new Model_LevelBehavior_26(
            _isPuzzleComplete               : LB26.isPuzzleComplete,
            _didActivateDramaticThoughts    : LB26.didActivateDramaticThoughts,
            _gotIceSpikeSticker             : LB26.gotIceSpikeSticker
        );
        
        data.levelsData.LB26 = lvlModel;
    }

    public override void Load(Model_RunData data)
    {
        if (data.levelsData == null)
        {
            Dev_Logger.Debug("There is no levels state data to load.");
            return;
        }

        if (data.levelsData.LB26 == null)
        {
            Dev_Logger.Debug("There is no LB26 state data to load.");
            return;
        }

        Model_LevelBehavior_26 lvlModel     = data.levelsData.LB26;
        LB26.isPuzzleComplete               = lvlModel.isPuzzleComplete;
        LB26.didActivateDramaticThoughts    = lvlModel.didActivateDramaticThoughts;
        LB26.gotIceSpikeSticker             = lvlModel.gotIceSpikeSticker;

        Dev_Logger.Debug($"-------- LOADED {name} --------");
        Script_Utils.DebugToConsole(lvlModel);
    }
}
