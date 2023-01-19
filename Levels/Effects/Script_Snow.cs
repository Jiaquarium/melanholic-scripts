using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Snow : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] snowParticles;
    
    private void OnValidate()
    {
        PopulateSnowParticles();
    }

    private void Start()
    {
        PopulateSnowParticles();
    }

    public void SetEmissionEnabled(bool isEnabled)
    {
        foreach (var system in snowParticles)
        {
            var emission = system.emission;
            emission.enabled = isEnabled;
        }
    }
    
    private void PopulateSnowParticles()
    {
        snowParticles = GetComponentsInChildren<ParticleSystem>(true);
    }
}
