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

    public delegate void OnUnequipDelegate(Script_Sticker sticker, int prevHash, float prevNormalizedTime);
    public static event OnUnequipDelegate OnUnequip;
    public static void Unequip(Script_Sticker sticker, int prevHash, float prevNormalizedTime)
    {
        if (OnUnequip != null)
            OnUnequip(sticker, prevHash, prevNormalizedTime);
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

    public delegate void OnMyMaskForceFaceDirDelegate(Directions dir);
    public static event OnMyMaskForceFaceDirDelegate OnMyMaskForceFaceDir;
    public static void MyMaskForceFaceDir(Directions dir)
    {
        if (OnMyMaskForceFaceDir != null)
            OnMyMaskForceFaceDir(dir);
    }

    public delegate void OnMyMaskStopFaceDirDelegate();
    public static event OnMyMaskStopFaceDirDelegate OnMyMaskStopFaceDir;
    public static void MyMaskStopFaceDir()
    {
        if (OnMyMaskStopFaceDir != null)
            OnMyMaskStopFaceDir();
    }
}
