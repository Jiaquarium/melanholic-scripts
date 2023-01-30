using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioClip))]
public class Script_TriggerEventsPassthroughUtil : MonoBehaviour
{
    [SerializeField] private Directions exitDirection;

    [SerializeField] private bool isLightGlow;
    [SerializeField] private bool isSFX;
    
    [SerializeField] private Script_LightFadeIn lightFadeIn;
    [SerializeField] private float lightFadeTime;
    [SerializeField] private float lightHoldTime;
    
    private AudioSource audioSource;
    private bool isExiting;
    private Script_Game game;
    private Script_SFXManager sfxManager;

    private Coroutine lightFadeCoroutine;

    void OnDisable()
    {
        StopMyCoroutines();
    }
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Do this in Start in case game starts on level where this component is present (only in Dev)
        game = Script_Game.Game;
        sfxManager = Script_SFXManager.SFX;

        lightFadeIn.gameObject.SetActive(false);
        lightFadeIn.SetIntensity(0f);
    }

    // ------------------------------------------------------------------
    // Unity Event Utils
    
    public void HandleExitEffects()
    {
        var exittingDir = game.GetPlayer().FacingDirection;
        
        Dev_Logger.Debug($"{name} HandleExitSFX exittingDir {exittingDir}");
        
        if (
            exitDirection != Directions.None
            && (exittingDir == exitDirection && !isExiting)
        )
        {
            if (isLightGlow)
                LightGlow();

            if (isSFX)
                PlayPassthroughSFX();

            // Prevent multiple calls
            isExiting = true;

            StartCoroutine(WaitEndFrameSetFlag());
        }

        IEnumerator WaitEndFrameSetFlag()
        {
            yield return new WaitForEndOfFrame();
            isExiting = false;
        }       
    }

    // ------------------------------------------------------------------

    private void PlayPassthroughSFX()
    {
        audioSource.PlayOneShot(sfxManager.ToriiPass, sfxManager.ToriiPassVol);
    }

    private void LightGlow()
    {
        StopMyCoroutines();
        lightFadeIn.gameObject.SetActive(true);
        lightFadeCoroutine = StartCoroutine(lightFadeIn.FadeInLightOnTarget(lightFadeTime, null, HoldLight));

        void HoldLight()
        {
            lightFadeCoroutine = StartCoroutine(WaitToFadeOutLight());
        }
        
        IEnumerator WaitToFadeOutLight()
        {
            yield return new WaitForSeconds(lightHoldTime);

            lightFadeCoroutine = StartCoroutine(
                lightFadeIn.FadeOutLight(
                    lightFadeTime,
                    () => lightFadeIn.gameObject.SetActive(false)
                )
            );
        }
    }

    private void StopMyCoroutines()
    {
        if (lightFadeCoroutine != null)
        {
            StopCoroutine(lightFadeCoroutine);
            lightFadeCoroutine = null;
        }
    }
}
