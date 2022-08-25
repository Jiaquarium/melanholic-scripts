using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Glimmer : MonoBehaviour
{
    private const string IsGlimmer = "IsGlimmer";
    
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
        a.SetTrigger("glimmer");
        audioSource.PlayOneShot(glimmerSFX, glimmerSFXVolScale);
    }
}
