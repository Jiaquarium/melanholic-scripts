using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Script_StickerEffect : MonoBehaviour
{
    public abstract void Effect();

    public virtual void EquipEffect(bool isEquip)
    {
        if (isEquip)    OnEquip();
        else            OnUnequip();
    }

    protected virtual void OnEquip()
    {
        Debug.Log($"{name} OnEquip()");
    }

    protected virtual void OnUnequip()
    {
        Debug.Log($"{name} OnUnequip()");
    }
}
