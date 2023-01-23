using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TMProWavySettingsUI : MonoBehaviour
{
    private float sinTimeRateDivisor = 1.5f;
    private float cosTimeRateDivisor = 2f;

    private float sinMultiplier = 1f;
    private float cosMultiplier = 0.5f;

    private float verticeMultipler0 = 0.1f;
    private float verticeMultipler1 = 0.15f;
    private float verticeMultipler2 = 0.2f;
    private float verticeMultipler3 = 0.25f;

    private float curveScale = 0.5f;

    public float SinTimeRateDivisor => sinTimeRateDivisor;
    public float CosTimeRateDivisor => cosTimeRateDivisor;
    
    public float SinMultiplier => sinMultiplier;
    public float CosMultiplier => cosMultiplier;

    public float VerticeMultipler0 => verticeMultipler0;
    public float VerticeMultipler1 => verticeMultipler1;
    public float VerticeMultipler2 => verticeMultipler2;
    public float VerticeMultipler3 => verticeMultipler3;

    public float CurveScale => curveScale;
}
