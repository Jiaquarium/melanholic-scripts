using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_VCamera))]
[RequireComponent(typeof(Script_CameraShake))]
public class Script_DistanceVCamera : MonoBehaviour
{
    public Script_CameraShake CameraShake => GetComponent<Script_CameraShake>();
    public Script_VCamera VCamera => GetComponent<Script_VCamera>();
}
