using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The holster stickers map to the order of the sticker in equipment
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_StickerEffectsController : MonoBehaviour
{
    [SerializeField] private Script_PsychicDuckEffect psychicDuckEffect;
    [SerializeField][Range(0f, 1f)] private float errorVol;

    /// <summary>
    /// Error SFX if there is none to switch with
    /// Using the same slot as already-equipped unequips the active sticker
    /// Switch the active sticker with the selected
    /// </summary>
    public void Switch(int i)
    {
        Script_Sticker stickerToSwitch = Script_StickerHolsterManager.Control.GetSticker(i);
        Script_Sticker activeSticker = Script_ActiveStickerManager.Control.GetSticker();

        if (stickerToSwitch == null)
        {
            Debug.Log("No sticker in sticker holster slot");
            NullSFX();
        }
        else if (activeSticker != null && activeSticker.id == stickerToSwitch.id)
        {
            Script_ActiveStickerManager.Control.RemoveSticker();
        }
        else
        {
            Script_ActiveStickerManager.Control.AddSticker(stickerToSwitch);
        }
    }
    
    /// <summary>
    /// Use the Active Sticker Effect
    /// </summary>
    /// <param name="i">Sticker Holster Slot</param>
    public void Effect()
    {
        Script_Sticker activeSticker = Script_ActiveStickerManager.Control.GetSticker();
        if (activeSticker == null)  return;
        
        switch (activeSticker.id)
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

    void NullSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.empty, Script_SFXManager.SFX.emptyVol
        );
    }
}
