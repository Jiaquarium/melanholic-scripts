using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_GoodEndingController : MonoBehaviour
{
    public static float WaitToPlayGoodEndingTimeAfterFadeOut = 0.25f;
    
    [SerializeField] private Canvas endingsCanvas;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Script_GlitchFXManager glitchFXManager;
    [SerializeField] private float fadeOutMainMelodyTime;
    [SerializeField] private Script_TransitionManager transitionManager;
    [SerializeField] private Script_TMProRandomizer TMProRandomizer;
    [SerializeField] private Script_ImageDistorterController theEndImageDistorter;
    [SerializeField] private string newTMProRandomizerId;

    [SerializeField] private List<Script_CanvasGroupController> slashOverlays;

    [Tooltip("Overlay canvas group for The End text and End message UI")]
    [SerializeField] private Canvas goodEndingOverlaysCanvas;
    [SerializeField] private CanvasGroup theEndCanvasGroup;
    [SerializeField] private Script_CanvasGroupController symbolsBgCanvasGroup;
    [SerializeField] private Script_PulseImage symbolsPulse;
    [SerializeField] private Script_ShakeImage symbolsShake;

    [Space][Header("The End Glitch - Post Processing")][Space]
    [SerializeField] private float filmGrainEndingIntensity;
    [Tooltip("Ensure this is less than TheEndCliff CanvasGroup DISTORTER's active interval time")]
    [SerializeField] private float filmGrainBlendTime;

    [Space][Header("After Good Ending PRCS")][Space]
    [SerializeField] private float keepAfterGoodEndingTextUpTime;
    [SerializeField][Range(0.25f, 1.0f)] private float waitInBlackAfterPRCSFadeOutTime;
    
    [Space][Header("Managers")][Space]
    [SerializeField] private Script_PostProcessingManager postProcessingManager;
    [SerializeField] private Script_Game game;

    // ------------------------------------------------------------------
    // Timeline Signals
    
    // Good Ending Timeline - Realization Text Ending
    public void StartStaticFX()
    {
        // Change canvas to Screen Space - Camera
        SetRenderModeScreenSpaceCamera(endingsCanvas);

        // Start static high screen FX
        glitchFXManager.SetHigh(useCurrentBlend: true);
        glitchFXManager.BlendTo(1f);
    }

    // Good Ending Timeline - Realization Text Ending
    public void StopStaticFX()
    {
        // Switch Camera render mode back to default
        SetRenderModeOverlay(endingsCanvas);
        
        glitchFXManager.BlendTo(0f);
    }

    /// <summary>
    /// Good Ending Timeline -- once curse file is made, defines if Good Ending has been done or not.
    /// </summary>
    public void SaveCurseData()
    {
        game.didGoodEnding = true;
        game.SaveAutoSaves();
        Script_SaveCurseControl.Instance.Save();
    }

    // Good Ending Timeline - Realization Text Ending
    // Fade out after "my eyes feel incredibly heavy"
    public void FadeOutMainMelody()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        bgm.FadeOut(bgm.Stop, fadeOutMainMelodyTime, Const_AudioMixerParams.ExposedBGVolume);
        transitionManager.FadeOutOceanBgm(fadeOutMainMelodyTime);
    }

    // Good Ending Timeline - GoodEndingTheEnd start
    public void SetupTheEndCanvases()
    {
        postProcessingManager.SetVignetteGoodEndingTheEnd();
        
        // EndingsCanvas with Cliff / Sea Bg set to screenspace camera for PostProcessing
        SetRenderModeScreenSpaceCamera(endingsCanvas);

        // GoodEndingTheEndOverlaysUI canvas set to overlay to be unaffected by PostProcessing
        SetRenderModeOverlay(goodEndingOverlaysCanvas);
    }
    
    /// <summary>
    /// This image distorter also will control on activation:
    /// - TMPRandomizer on "Good Ending" text
    /// This currently will be stopped after 1st interval.
    /// </summary>
    public void StartTheEndGlitching()
    {
        TMProRandomizer.DefaultId = newTMProRandomizerId;
        theEndImageDistorter.enabled = true;
        symbolsBgCanvasGroup.Open();
    }

    /// <summary>
    /// Reset grain and vignette
    /// </summary>
    public void PostProcessingInitialState() => postProcessingManager.InitialState();

    // Good Ending Timeline > AfterGoodEnding Timeline Typing Done
    public void OnAfterGoodEndingPRCSTypingDone()
    {
        StartCoroutine(WaitToTitle());

        IEnumerator WaitToTitle()
        {
            yield return new WaitForSecondsRealtime(keepAfterGoodEndingTextUpTime);

            Script_PRCSManager.Control.ClosePRCSCustom(Script_PRCSManager.CustomTypes.AfterGoodEnding);
            
            yield return new WaitForSecondsRealtime(waitInBlackAfterPRCSFadeOutTime);

            // Transition Manager's ToTitleScreen includes 1s delay, so instead, call scene change immediately,
            // so we can define a shorter delay above with waitInBlackAfterPRCSFadeOutTime (0.25-0.5s). Shorter
            // time feels better to connect the words more with title instead of isolating an entire new scene.
            game.ToTitleFromTimeline();
        }
    }
    
    // ------------------------------------------------------------------
    // Unity Events

    // TheEndCliff CanvasGroup DISTORTER's OnActiveIntervalStartOnce event
    // Start blending in the film grain
    public void StartBlendInFilmGrain()
    {
        FilmGrain filmgrain = postProcessingManager.SetFilmGrainGoodEndingTheEnd(0f);
        postProcessingManager.BlendInFilmGrainIntensity(filmgrain, filmGrainEndingIntensity, filmGrainBlendTime);
    }

    // ------------------------------------------------------------------
    
    private void SetRenderModeScreenSpaceCamera(Canvas canvas)
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = mainCamera;
        canvas.planeDistance = Script_GraphicsManager.CamCanvasPlaneDistance;
    }

    private void SetRenderModeOverlay(Canvas canvas)
    {
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }
    
    public void InitialState()
    {
        slashOverlays.ForEach(slashOverlay => slashOverlay.Close());
    }

    public void Setup()
    {
        theEndCanvasGroup.gameObject.SetActive(false);

        symbolsBgCanvasGroup.Close();
        symbolsPulse.Close();
        symbolsShake.Close();
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_GoodEndingController))]
    public class Script_GoodEndingControllerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_GoodEndingController t = (Script_GoodEndingController)target;
            if (GUILayout.Button("The End Glitching"))
                t.StartTheEndGlitching();
        }
    }
    #endif
}
