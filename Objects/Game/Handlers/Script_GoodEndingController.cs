using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_GoodEndingController : MonoBehaviour
{
    [SerializeField] private Canvas endingsCanvas;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Script_GlitchFXManager glitchFXManager;
    [SerializeField] private float fadeOutMainMelodyTime;
    [SerializeField] private Script_TransitionManager transitionManager;
    [SerializeField] private Script_TMProRandomizer TMProRandomizer;
    [SerializeField] private Script_ImageDistorterController theEndImageDistorter;
    [SerializeField] private string newTMProRandomizerId;

    [SerializeField] private List<Script_CanvasGroupController> slashOverlays;

    [SerializeField] private Script_Game game;

    // ------------------------------------------------------------------
    // Timeline Signals
    
    // Good Ending Timeline - Realization Text Ending
    public void StartStaticFX()
    {
        // Change canvas to Screen Space - Camera
        endingsCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        endingsCanvas.worldCamera = mainCamera;
        endingsCanvas.planeDistance = Script_GraphicsManager.CamCanvasPlaneDistance;

        // Start static high screen FX
        glitchFXManager.SetHigh(useCurrentBlend: true);
        glitchFXManager.BlendTo(1f);
    }

    // Good Ending Timeline - Realization Text Ending
    public void StopStaticFX()
    {
        // Switch Camera render mode back to default
        endingsCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
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

    /// <summary>
    /// This image distorter also will control on activation:
    /// - TMPRandomizer on "Good Ending" text
    /// </summary>
    public void StartTheEndGlitching()
    {
        TMProRandomizer.DefaultId = newTMProRandomizerId;
        theEndImageDistorter.enabled = true;
    }

    // ------------------------------------------------------------------

    // Good Ending Timeline Start
    public void InitialState()
    {
        slashOverlays.ForEach(slashOverlay => slashOverlay.Close());
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
