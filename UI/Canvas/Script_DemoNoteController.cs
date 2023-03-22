using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_DemoNoteController : MonoBehaviour
{
    public enum Levels
    {
        SpikeRoom = 0,
        IdsRoom = 1
    }

    private static int IntroThemeIdx = 23;

    [SerializeField] private Script_CanvasGroupController controller;
    [SerializeField] private Script_CanvasGroupController headerCanvasGroup;
    [SerializeField] private Script_CanvasGroupController demoNoteCanvasGroup;
    [SerializeField] private Script_CanvasGroupController demoTextCanvasGroup;
    [SerializeField] private Script_CanvasGroupController choicesCanvasGroup;
    [SerializeField] private Script_CanvasGroupController demoBg;
    [SerializeField] private float introThemeFadeInTime;
    [SerializeField] private float introThemeStartTime;
    [SerializeField] private float waitForDemoTextTime;
    [SerializeField] private float waitForChoicesTime;
    [SerializeField] private float waitToRevealScrollingBgTime;
    [SerializeField] private float demoBgFadeOutTime;
    
    [Space][Header("Spike Room Ending")][Space]
    [SerializeField] private Script_LevelBehavior_26 EileensMindBehavior;    
    [SerializeField] private GameObject EileensMindContainer;
    [SerializeField] private GameObject EileensMindScrollingBg;
    [SerializeField] private GameObject scrollingBG;
    
    [Space][Header("Ids' Room Ending")][Space]
    [SerializeField] private GameObject IdsRoomContainer;
    [SerializeField] private GameObject IdsRoomScrollingBg;
    [SerializeField] private GameObject IdsRoomDemoScrollingBG;

    private Levels activeLevel = Levels.SpikeRoom;

    private GameObject LevelContainer => activeLevel switch
    {
        Levels.IdsRoom => IdsRoomContainer,
        _ => EileensMindContainer
    };

    private GameObject ScrollingBg => activeLevel switch
    {
        Levels.IdsRoom => IdsRoomScrollingBg,
        _ => EileensMindScrollingBg
    };

    private GameObject DemoScrollingBg => activeLevel switch
    {
        Levels.IdsRoom => IdsRoomDemoScrollingBG,
        _ => scrollingBG
    };

    public void ActivateDemoText(Levels _activeLevel)
    {
        activeLevel = _activeLevel;
        
        // Note: Ensure bgm fade out time less than FadeTo time
        float fadeToBlackTime = FadeSpeeds.XXSlow.GetFadeTime();
        float fadeOutBgmTime = Mathf.Clamp(fadeToBlackTime - 1f, 0f, fadeToBlackTime);

        Script_Game.Game.ChangeStateCutScene();
        
        if (activeLevel == Levels.SpikeRoom)
            EileensMindBehavior.IsDemoEnd = true;
        
        var bgm = Script_BackgroundMusicManager.Control;
        
        // Fade out Bgm in 4 sec, while simultaneously fading in this CanvasGroup (shown as black BG)
        bgm.FadeOut(
            bgm.PauseAll,
            fadeOutBgmTime,
            outputMixer: Const_AudioMixerParams.ExposedBGVolume
        );
        
        // Fade in this canvas group in 5 sec (only Black BG will be showing)
        // On completion fade in new Bgm
        FadeIn(1f, FadeInIntroTheme);

        void FadeIn(float alpha, Action cb)
        {
            gameObject.SetActive(true);
            
            controller.IsUseMaxAlpha = true;
            controller.MaxAlpha = alpha;
            
            // Force Fade In
            if (controller.FadeInCoroutine != null)
            {
                StopCoroutine(controller.FadeInCoroutine);
                controller.FadeInCoroutine = null;
            }

            controller.FadeIn(fadeToBlackTime, cb);
        }

        // Then fade in new Bgm (intro theme)
        void FadeInIntroTheme()
        {
            // In the case, bgm fade out coroutine is still running, calling Stop will end it
            bgm.Stop();
            bgm.PlayFadeIn(
                i: IntroThemeIdx,
                cb: () => StartCoroutine(FadeInText()),
                forcePlay: true,
                fadeTime: introThemeFadeInTime,
                outputMixer: Const_AudioMixerParams.ExposedBGVolume,
                startTime: introThemeStartTime
            );
        }

        // Once new Bgm (intro theme) is fully faded in, wait 1 second to fade in text
        IEnumerator FadeInText()
        {
            yield return new WaitForSeconds(waitForDemoTextTime);
            
            // Fade in Header and Note Border first, followed by text
            var fadeTimeFast = FadeSpeeds.Fast.ToFadeTime();

            headerCanvasGroup.FadeIn(fadeTimeFast);
            demoNoteCanvasGroup.FadeIn(fadeTimeFast, () => {
                demoTextCanvasGroup.FadeIn(FadeSpeeds.XXSlow.ToFadeTime(), ActivateChoices);
                HandleScrollingBGReveal();
            });

            Script_SFXManager.SFX.PlaySubmitTransition();
        }

        // Wait 1 second to start fading in the scrolling background
        void HandleScrollingBGReveal()
        {
            // Hide Eileens Mind to show scrolling BG.
            LevelContainer.SetActive(false);
            ScrollingBg.SetActive(false);
            Script_Game.Game.GetPlayer().SetInvisible(true);
            DemoScrollingBg.SetActive(true);

            StartCoroutine(WaitToFadeOutDemoBG());
        }

        IEnumerator WaitToFadeOutDemoBG()
        {
            yield return new WaitForSeconds(waitToRevealScrollingBgTime);

            demoBg.FadeOut(demoBgFadeOutTime);
        }
    }

    // ----------------------------------------------------------------------
    // Unity Events
    
    public void ActivateChoices()
    {
        StartCoroutine(WaitToShowChoices());

        IEnumerator WaitToShowChoices()
        {
            yield return new WaitForSeconds(waitForChoicesTime);
            choicesCanvasGroup.Open();
        }
    }

    public void ReturnToMainMenu()
    {
        DisableInput();
        StartCoroutine(
            Script_Game.Game.TransitionFadeIn(
                FadeSpeeds.MedFast.ToFadeTime(), Script_TransitionManager.Control.ToTitleScreen
            )
        );
    }

    public void QuitToDesktop()
    {
        DisableInput();
        StartCoroutine(
            Script_Game.Game.TransitionFadeIn(
                FadeSpeeds.MedFast.ToFadeTime(), Application.Quit
            )
        );
    }

    /// <summary>
    /// Wishlist on Steam
    /// </summary>
    public void GoToWishlist()
    {
        Application.OpenURL(Script_Utils.SteamClientStoreURL);
    }

    // ----------------------------------------------------------------------
    
    private void DisableInput()
    {
        EventSystem.current.sendNavigationEvents = false;
    }

    public void InitialState()
    {
        controller.Close();
        demoNoteCanvasGroup.Close();
        demoTextCanvasGroup.Close();
        choicesCanvasGroup.Close();
        headerCanvasGroup.Close();
        DemoScrollingBg.SetActive(false);

        demoBg.Open();
    }

    public void Setup()
    {
        InitialState();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_DemoNoteController))]
    public class Script_DemoNoteControllerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_DemoNoteController t = (Script_DemoNoteController)target;
            if (GUILayout.Button("Activate (Spike Room)"))
            {
                t.ActivateDemoText(Script_DemoNoteController.Levels.SpikeRoom);
            }

            if (GUILayout.Button("Activate (Ids' Room)"))
            {
                t.ActivateDemoText(Script_DemoNoteController.Levels.IdsRoom);
            }
        }
    }
#endif
}
