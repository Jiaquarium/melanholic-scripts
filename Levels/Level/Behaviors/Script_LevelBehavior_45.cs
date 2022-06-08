using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_45 : Script_LevelBehavior
{
    // ==================================================================
    // State Data

    // ==================================================================

    [SerializeField] private Light directionalLight;
    [SerializeField] private Script_CollectibleObject lastSpellRecipeBook;

    [SerializeField] private bool isFinalTrueEndingTimeline;

    [SerializeField] private Transform cursedOnes;

    [SerializeField] private float waitBeforeQuestSFXTime;
    [SerializeField] private bool didLightsOn;

    // Should not be saved in state because this quest should be repeatable on
    // subsequent runs.
    private bool didPickUpLastSpellRecipeBook;

    public bool IsFinalTrueEndingTimeline
    {
        get => isFinalTrueEndingTimeline;
        set => isFinalTrueEndingTimeline = value;
    }
    
    protected override void OnEnable()
    {
        Script_ItemsEventsManager.OnItemPickUp      += OnItemPickUp;

        if (IsFinalTrueEndingTimeline)
            HandleLanternLightReaction(true);
        else
            HandleLanternLightReaction(game.GetPlayer().IsLightOn);

        OnEnableLanternSFXReaction();
    }

    protected override void OnDisable()
    {
        Script_ItemsEventsManager.OnItemPickUp      -= OnItemPickUp;

        Debug.Log($"On Disable: Setting IsFinalTrueEndingTimeline {IsFinalTrueEndingTimeline} to false");
        IsFinalTrueEndingTimeline = false;
    }
    
    protected override void Update()
    {
        base.Update();

        if (IsFinalTrueEndingTimeline)
            HandleLanternLightReaction(true);
        else
            HandleLanternLightReaction(game.GetPlayer().IsLightOn);
    }

    private void OnItemPickUp(string itemId)
    {
        if (itemId == lastSpellRecipeBook.Item.id)
        {
            didPickUpLastSpellRecipeBook = true;
        }
    }

    /// <summary>
    /// Must also handle the case if Player comes into level the first time with Lantern already on.
    /// </summary>
    public override void OnLevelInitComplete()
    {
        if (!didLightsOn && Script_Game.Game.GetPlayer().IsLightOn)
        {
            HandleLanternSFXReaction();
        }
    }
    
    public override bool OnLanternEffectOn()
    {
        var sfx = Script_SFXManager.SFX;
        sfx.PlayLanternOnXL();

        if (!didLightsOn)
        {
            HandleLanternSFXReaction();
        }
        
        return true;
    }

    public override bool OnLanternEffectOff()
    {
        Script_SFXManager.SFX.PlayLanternOffXL();
        
        return true;
    }

    public void TotemCry()
    {
        Script_SFXManager.SFX.PlayTotemCry();
    }

    private void HandleLanternLightReaction(bool isLightOn)
    {
        directionalLight.gameObject.SetActive(isLightOn);
    }

    private void HandleLanternSFXReaction()
    {
        var sfx = Script_SFXManager.SFX;
        
        didLightsOn = true;
        game.ChangeStateCutScene();

        // Fade out bgm
        Script_BackgroundMusicManager.Control.FadeOutFast(() => {
            StartCoroutine(WaitToPlayQuestProgressSFX());
        }, Const_AudioMixerParams.ExposedBGVolume);

        IEnumerator WaitToPlayQuestProgressSFX()
        {
            yield return new WaitForSeconds(waitBeforeQuestSFXTime);

            sfx.PlayQuestProgress(() => {
                Script_BackgroundMusicManager.Control.FadeInFast(() => {
                    game.ChangeStateInteract();
                }, Const_AudioMixerParams.ExposedBGVolume);
            });
        }
    }

    private void OnEnableLanternSFXReaction()
    {
        if (!didLightsOn && Script_Game.Game.GetPlayer().IsLightOn)
        {
            Script_BackgroundMusicManager.Control.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            Script_SFXManager.SFX.PlayLanternOnXL();
        }
    }

    public override void Setup()
    {
        if (lastSpellRecipeBook != null)
        {
            if (didPickUpLastSpellRecipeBook)   lastSpellRecipeBook.gameObject.SetActive(false);
            else                                lastSpellRecipeBook.gameObject.SetActive(true);
        }
    }
}