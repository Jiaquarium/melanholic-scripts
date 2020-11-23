using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ParticleSystemColor : MonoBehaviour
{
    public Color newColor;
    [SerializeField] bool isParent;
    void Awake()
    {
        SetParticleColor(GetComponent<ParticleSystem>());
    }

    public void SetChildrenParticleColor()
    {
        ParticleSystem[] children = transform.GetChildren<ParticleSystem>();
        foreach (ParticleSystem ps in children)
        {
            SetParticleColor(ps);
        }
    }

    public void SetParticleColor(ParticleSystem particleSystem)
    {
        ParticleSystem.MainModule settings = particleSystem.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(newColor);
    }
}
