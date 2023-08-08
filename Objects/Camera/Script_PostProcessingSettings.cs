using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_PostProcessingSettings : MonoBehaviour
{
    // Defaults
    private static float VignetteIntensityDefault = 0.75f;
    private static float VignetteSmoothnessDefault = 0.5f;

    // Good Ending The End
    private static float VignetteIntensityGoodEndingTheEnd = 0.42f;
    private static FilmGrainLookup FilmGrainTypeGoodEndingTheEnd = FilmGrainLookup.Medium2;
    private static float FilmGrainResponseGoodEndingTheEnd = 0.8f;

    // Take a Bow
    private static float VignetteIntensityTakeABow = 0.42f;
    private static FilmGrainLookup FilmGrainTypeTakeABow = FilmGrainLookup.Medium3;
    private static float FilmGrainResponseTakeABow = 0.8f;

    private Volume Volume {
        get
        {
            if (volume == null)
                volume = GetComponent<Volume>();

            return volume;
        }
        set => volume = value;
    }

    private Volume volume;

    void Awake()
    {
        Volume = GetComponent<Volume>();
    }

    // Use after setting vignette values (to avoid having to check in volume.profile again)
    static public void SetRefVignetteActive(ref Vignette vignette, bool isActive)
    {
        if (vignette != null)
            vignette.active = isActive;
    }

    // Use after setting filmgrain values (to avoid having to check in volume.profile again)
    static public void SetRefFilmGrainActive(ref FilmGrain filmgrain, bool isActive)
    {
        if (filmgrain != null)
            filmgrain.active = isActive;
    }

    public static void SetFilmGrainIntensity(ref FilmGrain filmgrain, float intensity)
    {
        if (filmgrain != null)
            filmgrain.intensity.value = intensity;
    }
    
    /// <summary>
    /// Use only as standalone if not setting vignette values
    /// </summary>
    /// <param name="isActive">Property active within Volume</param>
    public Vignette CloseVignette()
    {
        Vignette vignette;
        Volume.profile.TryGet(out vignette);
        
        if (vignette != null)
            vignette.active = false;
        
        return vignette;
    }

    public FilmGrain CloseFilmGrain()
    {
        FilmGrain filmgrain;
        Volume.profile.TryGet(out filmgrain);

        if (filmgrain != null)
            filmgrain.active = false;
        
        return filmgrain;
    }

    public Vignette SetVignetteDefault()
    {
        Vignette vignette;
        Volume.profile.TryGet(out vignette);
        
        if (vignette != null)
        {
            vignette.intensity.value = VignetteIntensityDefault;
            vignette.smoothness.value = VignetteSmoothnessDefault;
        }

        return vignette;
    }

    public Vignette SetVignetteGoodEndingTheEnd()
    {
        Vignette vignette;
        Volume.profile.TryGet(out vignette);
        
        if (vignette != null)
        {
            vignette.intensity.value = VignetteIntensityGoodEndingTheEnd;
            vignette.smoothness.value = VignetteSmoothnessDefault;
        }

        return vignette;
    }

    public Vignette SetVignetteTakeABow()
    {
        Vignette vignette;
        Volume.profile.TryGet(out vignette);
        
        if (vignette != null)
        {
            vignette.intensity.value = VignetteIntensityTakeABow;
            vignette.smoothness.value = VignetteSmoothnessDefault;
        }

        return vignette;
    }

    public FilmGrain SetFilmGrainGoodEndingTheEnd(float startingIntensity)
    {
        FilmGrain filmgrain;
        Volume.profile.TryGet(out filmgrain);

        if (filmgrain != null)
        {
            filmgrain.type.value = FilmGrainTypeGoodEndingTheEnd;
            filmgrain.intensity.value = startingIntensity;
            filmgrain.response.value = FilmGrainResponseGoodEndingTheEnd;
        }

        return filmgrain;
    }

    public FilmGrain SetFilmGrainTakeABow(float startingIntensity)
    {
        FilmGrain filmgrain;
        Volume.profile.TryGet(out filmgrain);

        if (filmgrain != null)
        {
            filmgrain.type.value = FilmGrainTypeTakeABow;
            filmgrain.intensity.value = startingIntensity;
            filmgrain.response.value = FilmGrainResponseTakeABow;
        }

        return filmgrain;
    }

    public void InitialStateWeight()
    {
        Volume.weight = 1f;
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_PostProcessingSettings))]
    public class Script_PostProcessingSettingsTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_PostProcessingSettings t = (Script_PostProcessingSettings)target;
            if (GUILayout.Button("Vignette Default On"))
            {
                var vignette = t.SetVignetteDefault();
                SetRefVignetteActive(ref vignette, true);
            }

            if (GUILayout.Button("Vignette Off"))
            {
                t.CloseVignette();
            }

            if (GUILayout.Button("Vignette Good Ending On"))
            {
                var vignette = t.SetVignetteGoodEndingTheEnd();
                SetRefVignetteActive(ref vignette, true);
            }
        }
    }
    #endif
}