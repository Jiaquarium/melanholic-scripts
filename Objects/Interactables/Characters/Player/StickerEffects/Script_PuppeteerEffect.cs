using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PuppeteerEffect : Script_StickerEffect
{
    [SerializeField] private Script_Player player;
    
    public override void Effect()
    {
        Debug.Log("&&&&PUPPETEER EFFECT&&&&");
        
        player.SetIsPuppeteer();
    }

    protected override void OnEquip()
    {
        base.OnEquip();
    }

    protected override void OnUnequip()
    {
        base.OnUnequip();
    }
}
