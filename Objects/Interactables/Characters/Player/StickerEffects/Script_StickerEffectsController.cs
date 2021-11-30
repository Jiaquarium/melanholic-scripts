﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// The holster stickers map to the order of the sticker in equipment
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_StickerEffectsController : MonoBehaviour
{
    [SerializeField] private Script_PsychicDuckEffect psychicDuckEffect;
    [SerializeField] private Script_GiantBoarNeedleEffect boarNeedleEffect;
    [SerializeField] private Script_PlayerAttackEat eatAttack; // TBD combine into Effect
    [SerializeField] private Script_AnimalWithinEffect animalWithinEffect;
    [SerializeField] private Script_IceSpikeEffect iceSpikeEffect;
    [SerializeField] private Script_MelancholyPianoEffect melancholyPianoEffect;
    [SerializeField] private Script_LastElevatorEffect lastElevatorEffect;
    [SerializeField] private Script_LetThereBeLightEffect letThereBeLightEffect;
    [SerializeField] private Script_PuppeteerEffect puppeteerEffect;
    [SerializeField] private Script_MyMaskEffect myMaskEffect;
    [SerializeField][Range(0f, 1f)] private float errorVol;

    [SerializeField] private float coolDown;

    private float coolDownTimer;

    public bool IsLanternLightOn
    {
        get => letThereBeLightEffect.IsLanternOn;
    }

    public bool IsPuppeteerEffectHoldOn
    {
        get => puppeteerEffect.IsEffectHoldActive;
    }

    void Update()
    {
        coolDownTimer = Mathf.Max(0, coolDownTimer - Time.deltaTime);
    }
    
    /// <summary>
    /// Error SFX if there is none to switch with
    /// Using the same slot as already-equipped unequips the active sticker
    /// Switch the active sticker with the selected
    /// </summary>
    public void Switch(int i, bool isBackground = false)
    {
        /// Disable for when in Hotel
        if (Script_Game.Game.IsInHotel())
        {
            return;
        }

        if (coolDownTimer > 0f)
        {
            return;
        }
        
        Script_Sticker stickerToSwitch = Script_StickerHolsterManager.Control.GetSticker(i);
        Script_Sticker activeSticker = Script_ActiveStickerManager.Control.ActiveSticker;

        if (stickerToSwitch == null)
        {
            Debug.Log("No sticker in sticker holster slot");
            NullSFX();
        }
        else
        {
            if (activeSticker != null && activeSticker.id == stickerToSwitch.id)
            {
                UnequipActionSticker();
            }
            else
            {
                SwitchActionSticker();
            }

            if (!isBackground)
                SwitchSFX();

            coolDownTimer = coolDown;
        }

        // When pressing a Sticker Holster slot that is currently the Action Sticker.
        void UnequipActionSticker()
        {
            EquipEffect(activeSticker, Script_StickerEffect.EquipType.Unequip);
                
            Script_ActiveStickerManager.Control.RemoveSticker(i);
        }

        // When pressing another Sticker Holster slot that is currently not the Action Sticker.
        void SwitchActionSticker()
        {
            // If there is an existing Action Sticker, call its Unequip Effect.
            if (activeSticker != null)  EquipEffect(activeSticker, Script_StickerEffect.EquipType.UnequipSwitch);

            Script_ActiveStickerManager.Control.AddSticker(stickerToSwitch, i);
            
            EquipEffect(stickerToSwitch, Script_StickerEffect.EquipType.Equip);
        }
    }

    /// <summary>
    /// To be called when need to return Player to Default state in the background.
    /// </summary>
    public void DefaultStateNoEffect()
    {
        Script_Sticker activeSticker = Script_ActiveStickerManager.Control.ActiveSticker;

        if (activeSticker == null)
            return;

        EquipEffect(activeSticker, Script_StickerEffect.EquipType.UnequipState);
        
        Script_ActiveStickerManager.Control.RemoveActiveSticker();
    }
    
    /// <summary>
    /// Use the Active Sticker Effect
    /// </summary>
    /// <param name="i">Sticker Holster Slot</param>
    public void Effect(Directions dir)
    {
        Script_Sticker activeSticker = Script_ActiveStickerManager.Control.ActiveSticker;
        if (activeSticker == null)
        {
            NullSFX();
            return;
        }
        
        switch (activeSticker.id)
        {
            case Const_Items.PsychicDuckId:
                Debug.Log($"{activeSticker} Effect Activated");
                psychicDuckEffect.Effect();
                break;
            case Const_Items.BoarNeedleId:
                Debug.Log($"{activeSticker} Effect Activated");
                boarNeedleEffect.Effect();
                break;
            case Const_Items.AnimalWithinId:
                Debug.Log($"{activeSticker} Effect Activated");
                animalWithinEffect.Effect();
                break;
            case Const_Items.IceSpikeId:
                Debug.Log($"{activeSticker} Effect Activated");
                iceSpikeEffect.Effect();
                break;
            case Const_Items.MelancholyPianoId:
                Debug.Log($"{activeSticker} Effect Activated");
                melancholyPianoEffect.Effect();
                break;
            case Const_Items.LastElevatorId:
                Debug.Log($"{activeSticker} Effect Activated");
                lastElevatorEffect.Effect();
                break;
            case Const_Items.LetThereBeLightId:
                Debug.Log($"{activeSticker} Effect Activated");
                letThereBeLightEffect.Effect();
                break;
            case Const_Items.PuppeteerId:
                Debug.Log($"{activeSticker} Effect Activated");
                puppeteerEffect.Effect();
                break;
            case Const_Items.MyMaskId:
                Debug.Log($"{activeSticker} Effect Activated");
                myMaskEffect.Effect();
                break;
        }

        // On Successful Active Sticker Use, Show Animation
        Script_ActiveStickerManager.Control.AnimateActiveStickerSlot();
    }

    public void EquipEffect(Script_Sticker sticker, Script_StickerEffect.EquipType equipType)
    {
        switch (sticker.id)
        {
            case Const_Items.PsychicDuckId:
                Debug.Log($"{sticker} equipType {equipType} Effect");
                psychicDuckEffect.EquipEffect(equipType);
                break;
            case Const_Items.BoarNeedleId:
                Debug.Log($"{sticker} equipType {equipType} Effect");
                boarNeedleEffect.EquipEffect(equipType);
                break;
            case Const_Items.AnimalWithinId:
                Debug.Log($"{sticker} equipType {equipType} Effect");
                animalWithinEffect.EquipEffect(equipType);
                break;
            case Const_Items.IceSpikeId:
                Debug.Log($"{sticker} equipType {equipType} Effect");
                iceSpikeEffect.EquipEffect(equipType);
                break;
            case Const_Items.MelancholyPianoId:
                Debug.Log($"{sticker} equipType {equipType} Effect");
                melancholyPianoEffect.EquipEffect(equipType);
                break;
            case Const_Items.LastElevatorId:
                Debug.Log($"{sticker} equipType {equipType} Effect");
                lastElevatorEffect.EquipEffect(equipType);
                break;
            case Const_Items.LetThereBeLightId:
                Debug.Log($"{sticker} equipType {equipType} Effect");
                letThereBeLightEffect.EquipEffect(equipType);
                break;
            case Const_Items.PuppeteerId:
                Debug.Log($"{sticker} equipType {equipType} Effect");
                puppeteerEffect.EquipEffect(equipType);
                break;
            case Const_Items.MyMaskId:
                Debug.Log($"{sticker} equipType {equipType} Effect");
                myMaskEffect.EquipEffect(equipType);
                break;
        }
    }

    void NullSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.empty, Script_SFXManager.SFX.emptyVol
        );
    }

    void SwitchSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.PlayerStashItem, Script_SFXManager.SFX.PlayerStashItemVol
        );
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_StickerEffectsController))]
public class Script_StickerEffectsControllerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_StickerEffectsController t = (Script_StickerEffectsController)target;
        if (GUILayout.Button("Default State No Effect"))
        {
            t.DefaultStateNoEffect();
        }
    }
}
#endif
