using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Script_MyMaskEffect : Script_StickerEffect
{
    [SerializeField] private List<GameObject> timelineBindings;

    // During white screen, simulate equipping / switching to this mask.
    public void EquipEffectTimeline()
    {
        base.OnEquip();
        OnEquipControllerSynced();
        
        Script_MaskEffectsDirectorManager.Instance.IsMyMaskMutationOff = true;
    }
    
    public override void Effect()
    {
        
    }

    protected override void OnEquip()
    {
        var game = Script_Game.Game;
        game.ChangeStateCutScene();

        Script_MaskEffectsDirectorManager.Instance.PlayMyMaskEffect(timelineBindings);
    }

    protected override void OnUnequip()
    {
        base.OnEquip();
        OnUnequipControllerSynced();

        Script_MaskEffectsDirectorManager.Instance.IsMyMaskMutationOff = false;
        Script_MaskEffectsDirectorManager.Instance.IsForceSheepFaceDirection = false;
    }
}
