﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerEffectQuestionMarkAnimate : MonoBehaviour
{
    private Animator a;
    private AudioSource audioSource;
    private AudioClip audioClip;
    public float questionMarkSFXVolScale;
    
    public void QuestionMark()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        a.SetTrigger("question-mark");
        audioSource.PlayOneShot(audioClip, questionMarkSFXVolScale);
    }

    public void HideQuestionMark()
    {
        // instantly hides
        a.SetTrigger("question-mark-hide");
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Setup()
    {
        a = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioClip = audioSource.clip;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
