using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// For Special Intro (Player comes in with Lantern)
/// 1.  OnEnable: OnEnableLanternSFXReactionSetup()
///     This will wait waitAfterOnEnableFadeOutBgmTime (1.5s) and take 0.25s to fade out BGM. Because the custom
///     special fade is 2s, Bgm will already be set to 0f before OnBlackScreenDone event is fired which sets vol to 0f
///     and plays Lantern On SFX.
/// 2.  After level fades in and player is set to interact, OnLevelInitComplete is called, calling HandleLanternSFXReaction
///     HandleLanternSFXReaction then waits onEntranceWaitBeforeQuestSFXTime to player Quest SFX 
/// Note:
///   - For this Special Intro, specialCaseFadeInTime and specialCaseWaitInBlackTime should define the transition times
///     always regardless if it's initial entrance or not; thus we 
/// </summary>
public class Script_LevelBehavior_45 : Script_LevelBehavior
{
    // ==================================================================
    // State Data

    // ==================================================================

    [SerializeField] private Light directionalLight;
    [SerializeField] private Script_CollectibleObject lastSpellRecipeBook;

    [SerializeField] private bool isFinalTrueEndingTimeline;

    [SerializeField] private Transform cursedOnes;
    
    // ------------------------------------------------------------------
    // Lantern SFX
    [Space][Header("Lantern SFX Reaction")][Space]
    [SerializeField] private bool didLightsOn;
    [Tooltip("Default wait time after playing Lantern SFX e.g. turning Lantern On while in Underworld")]
    [SerializeField] private float waitBeforeQuestSFXTime;
    [Space]
    [Tooltip("Special Intro: override fade in time when entering with Lantern Effect on")]
    [SerializeField] private float specialCaseFadeInTime;
    [Tooltip("Special Intro: override black screen time when entering with Lantern Effect on")]
    [SerializeField] private float specialCaseWaitInBlackTime;
    [Tooltip("Special Intro: time to wait after OnEnable to fade out bgm. Must be less than specialCaseWaitInBlackTime, since on Black Screen Done event Lantern SFX plays")]
    [SerializeField] private float waitAfterOnEnableFadeOutBgmTime;
    [Tooltip("Special Intro: time to fade out bgm before Lantern SFX")]
    [SerializeField] private float bgmFadeOutTime;
    [Tooltip("Special Intro: time to fade bgm back in after Lantern SFX")]
    [SerializeField] private float bgmFadeInTime;
    [Tooltip("Special Intro: wait time after level init (fade in done) for Quest Complete SFX.\nNote should be less than waitBeforeQuestSFXTime since this accounts for specialCaseFadeInTime")]
    [SerializeField] private float onEntranceWaitBeforeQuestSFXTime;
    [SerializeField] private Script_LevelCustomFadeBehavior levelCustomFadeBehavior;

    // ------------------------------------------------------------------
    // Nazca Totem
    [Space][Header("Nazca Totem")][Space]
    
    private static string CryTrigger = "Cry";
    
    [SerializeField] private float totemReactionWaitTime;
    [SerializeField] private Vector3 screenShakeVals;
    [SerializeField] private float totemSFXExtraWaitTime;
    [SerializeField] private Animator totemAnimator;
    private Script_CameraShake activeShakeCamera;

    // Should not be saved in state because this quest should be repeatable on
    // subsequent runs.
    private bool didPickUpLastSpellRecipeBook;

    public bool IsFinalTrueEndingTimeline
    {
        get => isFinalTrueEndingTimeline;
        set => isFinalTrueEndingTimeline = value;
    }

    private bool IsSpecialIntro => !didLightsOn && Script_Game.Game.GetPlayer().IsLightOn;
    
    protected override void OnEnable()
    {
        Script_ItemsEventsManager.OnItemPickUp += OnItemPickUp;
        Script_GameEventsManager.OnLevelBlackScreenDone += OnBlackScreenDoneLanternSFXReaction;

        if (IsFinalTrueEndingTimeline)
            HandleLanternLightReaction(true);
        else
            HandleLanternLightReaction(game.GetPlayer().IsLightOn);
        
        OnEnableLanternSFXReactionSetup();

        var vCamManager = Script_VCamManager.VCamMain;
        if (vCamManager != null)
            vCamManager.SetNewVCam(distanceVCam);

        // Set shadow distance longer to account for this farther distance camera
        Script_GraphicsManager.Control.SetUnderworldShadowDistance(); 
    }

    protected override void OnDisable()
    {
        Script_ItemsEventsManager.OnItemPickUp -= OnItemPickUp;
        Script_GameEventsManager.OnLevelBlackScreenDone -= OnBlackScreenDoneLanternSFXReaction;

        Dev_Logger.Debug($"On Disable: Setting IsFinalTrueEndingTimeline {IsFinalTrueEndingTimeline} to false");
        IsFinalTrueEndingTimeline = false;

        var vCamManager = Script_VCamManager.VCamMain;
        if (vCamManager != null)
            vCamManager.SwitchToMainVCam(distanceVCam);

        // Revert shadow distance
        Script_GraphicsManager.Control.SetDefaultShadowDistance();
    }

    void Awake()
    {
        activeShakeCamera = distanceVCam.GetComponent<Script_CameraShake>();
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
            HandleLanternSFXReaction(isOnLevelInit: true);
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
        game.ChangeStateCutScene();

        StartCoroutine(WaitToReact());

        IEnumerator WaitToReact()
        {
            yield return new WaitForSeconds(totemReactionWaitTime);

            totemAnimator.SetTrigger(CryTrigger);
            StartScreenShake();
        
            Script_SFXManager.SFX.PlayTotemCry(() => {
                StartCoroutine(WaitToInteract());
            });
        }

        void StartScreenShake() => activeShakeCamera.
            Shake(screenShakeVals.x, screenShakeVals.y, screenShakeVals.z, null);

        IEnumerator WaitToInteract()
        {
            yield return new WaitForSeconds(totemSFXExtraWaitTime);

            StopScreenShake();
            game.ChangeStateInteract();
        }
        
        void StopScreenShake() => activeShakeCamera.InitialState();
    }

    private void HandleLanternLightReaction(bool isLightOn)
    {
        directionalLight.gameObject.SetActive(isLightOn);
    }

    private void HandleLanternSFXReaction(bool isOnLevelInit = false)
    {
        var sfx = Script_SFXManager.SFX;
        var bgm = Script_BackgroundMusicManager.Control;
        
        didLightsOn = true;
        game.ChangeStateCutScene();

        // Fade out bgm
        bgm.FadeOutFast(() => {
            StartCoroutine(WaitToPlayQuestProgressSFX());
        }, Const_AudioMixerParams.ExposedBGVolume);

        IEnumerator WaitToPlayQuestProgressSFX()
        {
            if (isOnLevelInit)
            {
                yield return new WaitForSeconds(onEntranceWaitBeforeQuestSFXTime);

                // This SFX waits after lantern SFX = Bgm FadeOut (above) + specialCaseFadeInTime + onEntranceWaitBeforeQuestSFXTime
                // Note: When coming in on entrance the Bgm Fade can be cancelled by if setting bgm OnEnable
                // so must reset Bgm here.
                sfx.PlayQuestProgress(() => {
                    bgm.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
                    bgm.PlayFadeIn(
                        bgm.CurrentClipIndex,
                        OnQuestSFXDone,
                        forcePlay: true,
                        fadeTime: bgmFadeInTime,
                        outputMixer: Const_AudioMixerParams.ExposedBGVolume
                    );
                });
            }
            else
            {
                yield return new WaitForSeconds(waitBeforeQuestSFXTime);
                
                // This SFX waits after lantern SFX = Bgm FadeOut (above) + waitBeforeQuestSFXTime
                // + onEntranceWaitBeforeQuestSFXTime
                sfx.PlayQuestProgress(() => {
                    bgm.FadeInFast(
                        OnQuestSFXDone,
                        Const_AudioMixerParams.ExposedBGVolume
                    );
                });
            }
        }

        void OnQuestSFXDone()
        {
            game.ChangeStateInteract();
        }
    }

    // This will always occur before OnBlackScreenDoneLanternSFXReaction to cancel level bgm
    private void OnEnableLanternSFXReactionSetup()
    {
        if (IsSpecialIntro)
        {
            // Between OnEnable and Black Screen Done event, you have approximately specialCaseWaitInBlackTime
            // (i.e. 2f). So after 1.5f sec after OnEnable, start fading out Bgm.
            StartCoroutine(WaitAfterOnEnableFadeOutBgm());
        }

        IEnumerator WaitAfterOnEnableFadeOutBgm()
        {
            yield return new WaitForSecondsRealtime(waitAfterOnEnableFadeOutBgmTime);
            Script_BackgroundMusicManager.Control.FadeOut(
                null,
                bgmFadeOutTime,
                Const_AudioMixerParams.ExposedBGVolume
            );
        }
    }

    private void OnBlackScreenDoneLanternSFXReaction()
    {
        if (IsSpecialIntro)
        {
            Script_BackgroundMusicManager.Control.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            Script_SFXManager.SFX.PlayLanternOnXL();
        }
    }

    // ------------------------------------------------------------------
    // Intro w/ Lantern Effect On Only
    
    /// <summary>
    /// Fade Special Case, for Intro setup
    /// To mark it a special case, MUST set custom fade behavior's IsSpecialFadeIn & SpecialCaseFadeInTime
    /// </summary>
    public void SpecialCaseFadeIn()
    {
        if (IsSpecialIntro)
        {
            // Note: MUST SET for custom fade behavior to work properly
            levelCustomFadeBehavior.SpecialCaseFadeInTime = specialCaseFadeInTime;
            // Note: MUST SET for custom fade behavior to work properly
            levelCustomFadeBehavior.IsSpecialFadeIn = true;
            levelCustomFadeBehavior.IsOptOutInitial = true;
            levelCustomFadeBehavior.DidCheckSpecialCaseFadeIn = true;
        }
    }
    
    /// <summary>
    /// Wait In Black Special Case, for Intro setup
    /// To mark it a special case, MUST set custom fade behavior's IsSpecialWaitInBlack & SpecialCaseWaitInBlackTime
    /// </summary>
    public void SpecialCaseWaitInBlack()
    {
        if (IsSpecialIntro)
        {
            // Note: MUST SET for custom fade behavior to work properly
            levelCustomFadeBehavior.SpecialCaseWaitInBlackTime = specialCaseWaitInBlackTime;
            // Note: MUST SET for custom fade behavior to work properly
            levelCustomFadeBehavior.IsSpecialWaitInBlack = true;
            levelCustomFadeBehavior.IsOptOutInitial = true;
            levelCustomFadeBehavior.DidCheckSpecialCaseWaitInBlack = true;
        }
    }
    
    // ------------------------------------------------------------------

    public override void Setup()
    {
        if (lastSpellRecipeBook != null)
        {
            if (didPickUpLastSpellRecipeBook)   lastSpellRecipeBook.gameObject.SetActive(false);
            else                                lastSpellRecipeBook.gameObject.SetActive(true);
        }
    }
}