using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If given an exit, the lock sprite will update according to exit disabled state
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Script_DoorLock : MonoBehaviour
{
    private static readonly int UnlockTrigger = Animator.StringToHash("unlock");
    
    public int id;
    public float unlockSFXVolScale;
    
    [SerializeField] private SpriteRenderer graphics;
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip unlockClip;
    
    [SerializeField] private Script_TileMapExitEntrance exit;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        if (exit != null)
        {
            if (exit.IsDisabled)
            {
                animator.enabled = true;
                graphics.enabled = true;
            }
            else
            {
                animator.enabled = false;
                
                if (unlockedSprite != null)     graphics.sprite = unlockedSprite;
                else                            graphics.enabled = false;                                          
            }
        }
    }

    public void Unlock()
    {
        animator.SetTrigger(UnlockTrigger);
        audioSource.PlayOneShot(unlockClip, unlockSFXVolScale);
    }
}
