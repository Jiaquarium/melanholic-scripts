using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MyMaskEffect : Script_StickerEffect
{
    public override void Effect()
    {
        
    }

    protected override void OnEquip()
    {
        base.OnEquip();
        OnEquipControllerSynced();
    }

    protected override void OnUnequip()
    {
        base.OnEquip();
        OnUnequipControllerSynced();
    }
}
