using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_StopAnimation : MonoBehaviour
{
    [SerializeField] private Animator[] animators;
    
    public void StopAnimation()
    {
        foreach (Animator a in animators)
        {
            a.speed = 0f;
        }
    }
}
