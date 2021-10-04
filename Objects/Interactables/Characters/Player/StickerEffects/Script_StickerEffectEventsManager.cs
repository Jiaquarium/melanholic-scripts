using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_StickerEffectEventsManager : MonoBehaviour
{
    public delegate void OnEquipDelegate(Script_Sticker sticker);
    public static event OnEquipDelegate OnEquip;
    public static void Equip(Script_Sticker sticker)
    {
        if (OnEquip != null)
            OnEquip(sticker);
    }

    public delegate void OnUnequipDelegate(Script_Sticker sticker);
    public static event OnUnequipDelegate OnUnequip;
    public static void Unequip(Script_Sticker sticker)
    {
        if (OnUnequip != null)
            OnUnequip(sticker);
    }

    // For mimicing animation.
    public delegate void OnAnimatorEffectTriggerDelegate();
    public static event OnAnimatorEffectTriggerDelegate OnAnimatorEffectTrigger;
    public static void AnimatorEffectTrigger()
    {
        if (OnAnimatorEffectTrigger != null)
            OnAnimatorEffectTrigger();
    }

    // For mimicing animation.
    public delegate void OnAnimatorEffectHoldDelegate(bool isActive);
    public static event OnAnimatorEffectHoldDelegate OnAnimatorEffectHold;
    public static void AnimatorEffectHold(bool isActive)
    {
        if (OnAnimatorEffectHold != null)
            OnAnimatorEffectHold(isActive);
    }
}
