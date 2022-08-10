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
    [SerializeField] private Script_CanvasGroupController demoTextCanvasGroup;
    [SerializeField] private Script_CanvasGroupController choicesCanvasGroup;
    [SerializeField] private Script_CanvasGroupController demoBg;
    [SerializeField] private float fadeTime;
    [SerializeField] private float waitForDemoTextTime;
    [SerializeField] private float waitForChoicesTime;
    [SerializeField] private float transitionExitFadeTime;
    [SerializeField] private float waitToRevealScrollingBgTime;
    [SerializeField] private float demoBgFadeOutTime;
    
    [SerializeField] private Script_LevelBehavior_26 EileensMindBehavior;    
    [SerializeField] private GameObject EileensMindContainer;
    [SerializeField] private GameObject EileensMindScrollingBg;
    [SerializeField] private GameObject scrollingBG;
    [SerializeField] private int introThemeIdx;

    public void ActivateDemoText()
    {
        Script_Game.Game.ChangeStateCutScene();
        EileensMindBehavior.IsDemoEnd = true;
        
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.FadeOutSlow(bgm.Stop, outputMixer: Const_AudioMixerParams.ExposedBGVolume);
        
        FadeTo(1f, FadeInIntroTheme);

        void FadeInIntroTheme()
        {
            bgm.PlayFadeIn(
                i: introThemeIdx,
                cb: () => {
                    demoTextCanvasGroup.Open();
                    HandleScrollingBGReveal();
                },
                forcePlay: true,
                fadeTime: waitForDemoTextTime,
                outputMixer: Const_AudioMixerParams.ExposedBGVolume
            );
        }

        void HandleScrollingBGReveal()
        {
            // Hide Eileens Mind to show scrolling BG
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
        StartCoroutine(Script_Game.Game.TransitionFadeIn(
            transitionExitFadeTime, Script_TransitionManager.Control.ToTitleScreen)
        );
    }

    public void QuitToDesktop()
    {
        DisableInput();
        StartCoroutine(Script_Game.Game.TransitionFadeIn(
            transitionExitFadeTime, Application.Quit)
        );
    }

    // ----------------------------------------------------------------------

    private void FadeTo(float alpha, Action cb)
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

        controller.FadeIn(fadeTime, cb);
    }
    
    private void DisableInput()
    {
        EventSystem.current.sendNavigationEvents = false;
    }

    public void InitialState()
    {
        controller.Close();
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
