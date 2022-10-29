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
    
    public void StartStaticFX()
    {
        // Change canvas to Screen Space - Camera
        endingsCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        endingsCanvas.worldCamera = mainCamera;

        // Start static high screen FX
        glitchFXManager.SetHigh(useCurrentBlend: true);
        glitchFXManager.BlendTo(1f);
    }

    public void StopStaticFX()
    {
        // Switch Camera render mode back to default
        endingsCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        glitchFXManager.BlendTo(0f);
    }

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
