using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Glimmer : MonoBehaviour
{
    private static int IsGlimmer = Animator.StringToHash("IsGlimmer");
    private static int GlimmerTrigger = Animator.StringToHash("glimmer");

    [SerializeField] private bool isGlimmerAlways;
    
    public Animator a;
    public AudioSource audioSource;
    public AudioClip glimmerSFX;
    public float glimmerSFXVolScale;
    
    void OnEnable()
    {
        if (isGlimmerAlways)
            a.SetBool(IsGlimmer, isGlimmerAlways);
    }
    
    public void Glimmer()
    {
        a.SetTrigger(GlimmerTrigger);
        audioSource.PlayOneShot(glimmerSFX, glimmerSFXVolScale);
    }
}
