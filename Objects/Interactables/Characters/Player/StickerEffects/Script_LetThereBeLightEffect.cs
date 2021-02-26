using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LetThereBeLightEffect : Script_StickerEffect
{
    private Script_LanternFollower lanternFollower;
    
    void Awake()
    {
        lanternFollower = Script_Game.Game.LanternFollower;
    }
    
    public override void Effect()
    {
        lanternFollower.SwitchLightState();
    }

    protected override void OnEquip()
    {
        base.OnEquip();
        
        // Set Lantern Follower active.
        lanternFollower.Activate();
    }

    protected override void OnUnequip()
    {
        base.OnUnequip();
        
        // Set Lantern Follower inactive.
        lanternFollower.LightOff();
        lanternFollower.Deactivate();
    }
}
