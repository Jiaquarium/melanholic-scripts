using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The holster stickers map to the order of the sticker in equipment
/// </summary>
public class Script_StickerEffectsController : MonoBehaviour
{
    [SerializeField] private Script_PsychicDuckEffect psychicDuckEffect;

    public void Effect(int i)
    {
        Script_Sticker stickerInSlot = Script_StickerHolsterManager.Control.GetSticker(i);
        if (stickerInSlot == null)  return;
        
        switch (stickerInSlot.id)
        {
            case Const_Items.PsychicDuckId:
                Debug.Log("Psychic Duck Effect Activated");
                psychicDuckEffect.Effect();
                break;
            case Const_Items.BoarNeedleId:
                Debug.Log("Boar Needle Effect Activated");
                break;
        }
    }
}
