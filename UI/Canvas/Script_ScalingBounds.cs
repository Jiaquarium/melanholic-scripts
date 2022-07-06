using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ScalingBounds : MonoBehaviour
{
    [Tooltip("Any screen below this pixel height will return a pixel scale factor of 1")]
    [SerializeField] private int bound1;
    
    [Tooltip("Any screen below this pixel height will return a pixel scale factor of 2")]
    [SerializeField] private int bound2;
    
    [Tooltip("Any screen below this pixel height will return a pixel scale factor of 3")]
    [SerializeField] private int bound3;

    public int Bound1 { get => bound1; }
    public int Bound2 { get => bound2; }
    public int Bound3 { get => bound3; }
}
