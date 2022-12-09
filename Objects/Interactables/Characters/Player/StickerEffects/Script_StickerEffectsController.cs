using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// The holster stickers map to the order of the sticker in equipment
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_StickerEffectsController : MonoBehaviour
{
    [SerializeField] private Script_Player player;
    [SerializeField] private Script_PlayerMovement playerMovement;
    
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

    [SerializeField] private List<Script_StickerEffect> effects;
    
    [SerializeField][Range(0f, 1f)] private float errorVol;

    [SerializeField] private float coolDown;
    [SerializeField] private float mutationTime;

    private float coolDownTimer;
    private float mutationTimer;
    private Script_PlayerMutation playerMutation;

    public bool IsLanternLightOn
    {
        get => letThereBeLightEffect.IsLanternOn;
    }

    public bool IsPuppeteerEffectHoldOn
    {
        get => puppeteerEffect.IsEffectHoldActive;
    }

    void OnEnable()
    {
        InitialState();
    }
    
    void Update()
    {
        coolDownTimer = Mathf.Max(0, coolDownTimer - Time.deltaTime);

        if (player.IsFinalRound)
        {
            if (Script_MaskEffectsDirectorManager.Instance.IsMyMaskMutationOff)
            {
                mutationTimer = 0f;
                return;
            }
            
            mutationTimer = Mathf.Max(0, mutationTimer - Time.deltaTime);
            
            if (mutationTimer <= 0f)
            {
                Mutate();
                mutationTimer = mutationTime;
            }
        }
    }
    
    /// <summary>
    /// Error SFX if there is none to switch with
    /// Using the same slot as already-equipped unequips the active sticker
    /// Switch the active sticker with the selected
    /// </summary>
    public void Switch(int i, bool isBackground = false)
    {
        /// Disable for when in Hotel
        var game = Script_Game.Game;
        
        if (game.IsInHotel() || game.IsHideHUD || game.IsDisableMasksOnly)
        {
            NullSFX();    
            return;
        }

        if (coolDownTimer > 0f)
            return;
        
        Script_Sticker stickerToSwitch = Script_StickerHolsterManager.Control.GetSticker(i);
        Script_Sticker activeSticker = Script_ActiveStickerManager.Control.ActiveSticker;

        if (stickerToSwitch == null)
        {
            Dev_Logger.Debug("No sticker in sticker holster slot");
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

            // If is force called in BG needs to be silent.
            // My Mask switch SFX will be handled later.
            if (!isBackground && stickerToSwitch.id != Const_Items.MyMaskId)
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
            if (activeSticker != null)
                EquipEffect(activeSticker, Script_StickerEffect.EquipType.UnequipSwitch);

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

        if (activeSticker == null || player.IsFinalRound)
        {
            NullSFX();
            return;
        }
        
        switch (activeSticker.id)
        {
            case Const_Items.PsychicDuckId:
                Dev_Logger.Debug($"{activeSticker} Effect Activated");
                psychicDuckEffect.Effect();
                break;
            case Const_Items.BoarNeedleId:
                Dev_Logger.Debug($"{activeSticker} Effect Activated");
                boarNeedleEffect.Effect();
                break;
            case Const_Items.AnimalWithinId:
                Dev_Logger.Debug($"{activeSticker} Effect Activated");
                animalWithinEffect.Effect();
                break;
            case Const_Items.IceSpikeId:
                Dev_Logger.Debug($"{activeSticker} Effect Activated");
                iceSpikeEffect.Effect();
                break;
            case Const_Items.MelancholyPianoId:
                Dev_Logger.Debug($"{activeSticker} Effect Activated");
                melancholyPianoEffect.Effect();
                break;
            case Const_Items.LastElevatorId:
                Dev_Logger.Debug($"{activeSticker} Effect Activated");
                lastElevatorEffect.Effect();
                break;
            case Const_Items.LetThereBeLightId:
                Dev_Logger.Debug($"{activeSticker} Effect Activated");
                letThereBeLightEffect.Effect();
                break;
            case Const_Items.PuppeteerId:
                Dev_Logger.Debug($"{activeSticker} Effect Activated");
                puppeteerEffect.Effect();
                break;
            case Const_Items.MyMaskId:
                Dev_Logger.Debug($"{activeSticker} Effect Activated");
                myMaskEffect.Effect();
                break;
        }

        // On Successful Active Sticker Use, Show Animation
        Script_ActiveStickerManager.Control.AnimateActiveStickerSlot();
    }

    private void EquipEffect(Script_Sticker sticker, Script_StickerEffect.EquipType equipType)
    {
        if (
            player.IsFinalRound
            && Script_ActiveStickerManager.Control.ActiveSticker?.id != Const_Items.MyMaskId
        )
        {
            return;
        }   
        
        switch (sticker.id)
        {
            case Const_Items.PsychicDuckId:
                Dev_Logger.Debug($"{sticker} equipType {equipType} Effect");
                psychicDuckEffect.EquipEffect(equipType);
                break;
            case Const_Items.BoarNeedleId:
                Dev_Logger.Debug($"{sticker} equipType {equipType} Effect");
                boarNeedleEffect.EquipEffect(equipType);
                break;
            case Const_Items.AnimalWithinId:
                Dev_Logger.Debug($"{sticker} equipType {equipType} Effect");
                animalWithinEffect.EquipEffect(equipType);
                break;
            case Const_Items.IceSpikeId:
                Dev_Logger.Debug($"{sticker} equipType {equipType} Effect");
                iceSpikeEffect.EquipEffect(equipType);
                break;
            case Const_Items.MelancholyPianoId:
                Dev_Logger.Debug($"{sticker} equipType {equipType} Effect");
                melancholyPianoEffect.EquipEffect(equipType);
                break;
            case Const_Items.LastElevatorId:
                Dev_Logger.Debug($"{sticker} equipType {equipType} Effect");
                lastElevatorEffect.EquipEffect(equipType);
                break;
            case Const_Items.LetThereBeLightId:
                Dev_Logger.Debug($"{sticker} equipType {equipType} Effect");
                letThereBeLightEffect.EquipEffect(equipType);
                break;
            case Const_Items.PuppeteerId:
                Dev_Logger.Debug($"{sticker} equipType {equipType} Effect");
                puppeteerEffect.EquipEffect(equipType);
                break;
            case Const_Items.MyMaskId:
                Dev_Logger.Debug($"{sticker} equipType {equipType} Effect");
                myMaskEffect.EquipEffect(equipType);
                break;
        }
    }

    // ------------------------------------------------------------------
    // Final Round

    // My Mask will not do equip effects when switching via key input; instead, will play timeline
    // that sends a signal to do the actual switch effect.
    public void MyMaskEquipEffectTimeline()
    {
        myMaskEffect.EquipEffectTimeline();
    }

    /// <summary>
    /// Rotates the player's controller.
    /// </summary>
    private void Mutate()
    {
        var randomEffectIdx = Random.Range(0, effects.Count);
        var currentEffect = effects[randomEffectIdx];
        RuntimeAnimatorController animatorController = currentEffect.StickerAnimatorController;
        AnimatorStateInfo animatorStateInfo = playerMovement.MyAnimator.GetCurrentAnimatorStateInfo(Script_PlayerMovement.Layer);

        playerMovement.MyAnimator.runtimeAnimatorController = animatorController;
        playerMovement.SyncAnimatorState(animatorStateInfo);

        playerMovement.MyAnimator.AnimatorSetDirection(playerMovement.FacingDirection);

        if (player.IsFinalRound)
        {
            if (playerMutation == null)
                playerMutation = Script_Game.Game.grandMirrorRoomBehavior.PlayerMutation;
            
            var _randomEffectIdx = Random.Range(0, effects.Count);
            var _currentEffect = effects[_randomEffectIdx];
            RuntimeAnimatorController _animatorController = _currentEffect.StickerAnimatorController;

            playerMutation.HandleAnimatorState(
                _animatorController,
                animatorStateInfo,
                playerMovement.FacingDirection,
                playerMovement.MyAnimator.GetBool(Script_PlayerMovement.PlayerMovingAnimatorParam)
            );
        }
    }
    
    // ------------------------------------------------------------------
    // SFX

    private void NullSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.empty, Script_SFXManager.SFX.emptyVol
        );
    }

    private void SwitchSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.PlayerStashItem, Script_SFXManager.SFX.PlayerStashItemVol
        );
    }

    private void InitialState()
    {
        mutationTimer = 0f;
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

            if (GUILayout.Button("Mutate"))
            {
                t.Mutate();
            }
        }
    }
    #endif
}

