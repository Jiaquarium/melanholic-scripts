using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If given an exit, the lock sprite will update according to exit disabled state
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Script_DoorLock : MonoBehaviour
{
    public int id;
    public float unlockSFXVolScale;
    [SerializeField] private Animator a;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip unlockClip;
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Script_TileMapExitEntrance exit;
    private Script_Game game;
    
    void Awake()
    {
        game = Script_Game.Game;
        a = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        unlockClip = audioSource.clip;
    }

    public void Unlock()
    {
        a.SetTrigger("unlock");
        audioSource.PlayOneShot(unlockClip, unlockSFXVolScale);
    }

    public void UnlockCallback()
    {
        game.OnDoorLockUnlock(id);
    }

    public void Setup()
    {
        a = GetComponent<Animator>();
        
        if (exit != null)
        {
            if (exit.IsDisabled)
            {
                a.enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                a.enabled = false;
                if (unlockedSprite != null)
                    GetComponent<SpriteRenderer>().sprite = unlockedSprite;
                else
                    GetComponent<SpriteRenderer>().enabled = false;                                          
            }
        }
    }
}
