using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use if you need the Animator to maintain its last state.
/// E.g. Animators that are children of containers.
/// </summary>
[RequireComponent(typeof(Animator))]
public class Script_KeepAnimatorControllerStateOnDisable : MonoBehaviour
{
    [SerializeField] private bool keepAnimatorControllerStateOnDisable;
    
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.keepAnimatorControllerStateOnDisable = keepAnimatorControllerStateOnDisable;
    }
}
