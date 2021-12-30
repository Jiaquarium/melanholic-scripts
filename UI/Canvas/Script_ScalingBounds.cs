using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ScalingBounds : MonoBehaviour
{
    [SerializeField] private int bound1;
    [SerializeField] private int bound2;
    [SerializeField] private int bound3;

    public int Bound1 { get => bound1; }
    public int Bound2 { get => bound2; }
    public int Bound3 { get => bound3; }
}
