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
    [SerializeField] private Script_CanvasGroupController controller;
    [SerializeField] private Script_CanvasGroupController demoNoteCanvasGroup;
    [SerializeField] private Script_CanvasGroupController demoTextCanvasGroup;
    [SerializeField] private Script_CanvasGroupController choicesCanvasGroup;
    [SerializeField] private Script_CanvasGroupController demoBg;
    [SerializeField] private float introThemeFadeInTime;
    [SerializeField] private float waitForDemoTextTime;
    [SerializeField] private float waitForChoicesTime;
    [SerializeField] private float waitToRevealScrollingBgTime;
    [SerializeField] private float demoBgFadeOutTime;
    
    [SerializeField] private Script_LevelBehavior_26 EileensMindBehavior;    
    [SerializeField] private GameObject EileensMindContainer;
    [SerializeField] private GameObject EileensMindScrollingBg;
    [SerializeField] private GameObject scrollingBG;
    [SerializeField] private int introThemeIdx;

    public void ActivateDemoText()
    {
        // Note: Ensure bgm fade out time less than FadeTo time
        float fadeToBlackTime = FadeSpeeds.XXSlow.GetFadeTime();
        float fadeOutBgmTime = Mathf.Clamp(fadeToBlackTime - 1f, 0f, fadeToBlackTime);

        Script_Game.Game.ChangeStateCutScene();
        EileensMindBehavior.IsDemoEnd = true;
        
        var bgm = Script_BackgroundMusicManager.Control;
        
        // Fade out Bgm in 4 sec, while simultaneously fading in this CanvasGroup (shown as black BG)
        bgm.FadeOut(bgm.Stop, fadeOutBgmTime, outputMixer: Const_AudioMixerParams.ExposedBGVolume);
        
        // Fade in this canvas group in 5 sec (only Black BG will be showing)
        // On completion fade in new Bgm
        FadeTo(1f, FadeInIntroTheme);

        void FadeTo(float alpha, Action cb)
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
            // Must set volume to 0 before to avoid Bgm popping
            bgm.SetVolume(0f, Const_AudioMixerParams.ExposedBGVolume);
            bgm.PlayFadeIn(
                i: introThemeIdx,
                cb: () => StartCoroutine(FadeInText()),
                forcePlay: true,
                fadeTime: introThemeFadeInTime,
                outputMixer: Const_AudioMixerParams.ExposedBGVolume
            );
        }

        // Once new Bgm (intro theme) is fully faded in, wait 1 second to fade in text
        IEnumerator FadeInText()
        {
            yield return new WaitForSeconds(waitForDemoTextTime);
            
            // Fade in Note Border first, followed by text
            demoNoteCanvasGroup.FadeIn(FadeSpeeds.Fast.ToFadeTime(), () => {
                demoTextCanvasGroup.FadeIn(FadeSpeeds.XXSlow.ToFadeTime(), ActivateChoices);
                HandleScrollingBGReveal();
            });
        }

        // Wait 1 second to start fading in the scrolling background
        void HandleScrollingBGReveal()
        {
            // Hide Eileens Mind to show scrolling BG.
            EileensMindContainer.SetActive(false);
            EileensMindScrollingBg.SetActive(false);
            Script_Game.Game.GetPlayer().SetInvisible(true);
            scrollingBG.SetActive(true);

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
    
    // - Demo Note Text: OnTypingDone
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
        scrollingBG.SetActive(false);

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
            if (GUILayout.Button("Activate"))
            {
                t.ActivateDemoText();
            }
        }
    }
#endif
}
