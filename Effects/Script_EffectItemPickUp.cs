﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EffectItemPickUp : MonoBehaviour
{
    [SerializeField] private SpriteRenderer itemImage;
    [SerializeField] private AudioSource audioSource;
    
    public void ShowItem(Script_Item item)
    {
        Dev_Logger.Debug($"Player showing item {item.id}");
        itemImage.sprite = item.sprite;
        itemImage.gameObject.SetActive(true);
        audioSource.PlayOneShot(Script_SFXManager.SFX.ItemPickUp, Script_SFXManager.SFX.ItemPickUpVol);
    }

    public void HideItem()
    {
        Dev_Logger.Debug("Hide Item being called");
        itemImage.gameObject.SetActive(false);
    }

    public void Setup()
    {
        itemImage.gameObject.SetActive(false);
    }
}
