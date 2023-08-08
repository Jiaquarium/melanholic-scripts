using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Script_PostProcessingManager : MonoBehaviour
{
    public static Script_PostProcessingManager Instance;
    
    [SerializeField] private Script_PostProcessingSettings globalVolume;

    private FilmGrain currentFilmGrain;
    private float currentFilmGrainEndingIntensity;
    private float currentFilmGrainMaxTime;
    private float filmGrainTimer;

    void Update()
    {
        if (filmGrainTimer > 0f)
        {
            filmGrainTimer -= Time.deltaTime;
            
            if (filmGrainTimer <= 0f)
                filmGrainTimer = 0f;
            
            // Set filmgrain
            var timeElapsed = currentFilmGrainMaxTime - filmGrainTimer;
            var timeElapsedPercent = timeElapsed / currentFilmGrainMaxTime;
            SetFilmGrainIntensity(currentFilmGrain, timeElapsedPercent * currentFilmGrainEndingIntensity);
        }
    }
    
    public Vignette SetVignetteGoodEndingTheEnd()
    {
        Vignette vignette = globalVolume.SetVignetteGoodEndingTheEnd();
        Script_PostProcessingSettings.SetRefVignetteActive(ref vignette, true);

        return vignette;
    }
    
    public FilmGrain SetFilmGrainGoodEndingTheEnd(float startingIntensity)
    {
        FilmGrain filmgrain = globalVolume.SetFilmGrainGoodEndingTheEnd(startingIntensity);
        Script_PostProcessingSettings.SetRefFilmGrainActive(ref filmgrain, true);
        
        return filmgrain;
    }

    public FilmGrain SetFilmGrainTakeABow(float startingIntensity)
    {
        FilmGrain filmgrain = globalVolume.SetFilmGrainTakeABow(startingIntensity);
        Script_PostProcessingSettings.SetRefFilmGrainActive(ref filmgrain, true);
        
        return filmgrain;
    }

    public void BlendInFilmGrainIntensity(FilmGrain filmgrain, float endingIntensity, float blendInTime)
    {
        currentFilmGrain = filmgrain;
        currentFilmGrainMaxTime = blendInTime;
        filmGrainTimer = currentFilmGrainMaxTime;
        currentFilmGrainEndingIntensity = endingIntensity;
    }

    private void SetFilmGrainIntensity(
        FilmGrain filmgrain,
        float intensity
    ) => Script_PostProcessingSettings.SetFilmGrainIntensity(ref filmgrain, intensity);
    
    public void InitialState()
    {
        var vignette = globalVolume.SetVignetteDefault();
        Script_PostProcessingSettings.SetRefVignetteActive(ref vignette, false);
        
        globalVolume.CloseFilmGrain();
        filmGrainTimer = 0f;
    }

    public void Setup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }   
        
        InitialState();
    }
}
