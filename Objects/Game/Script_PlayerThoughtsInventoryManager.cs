using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Script_PlayerThoughtsInventoryManager : MonoBehaviour
{
    public CanvasGroup inventoryCanvasGroup;
    public CanvasGroup thoughtsCanvasGroup;
    public CanvasGroup sBookCanvasGroup;
    public Script_MenuController menuController;
    public Script_ThoughtSlotHolder thoughtSlotHolder;
    private Script_Game game;

    [SerializeField] private AudioSource inventoryAudioSource;

    public void OpenInventory(bool noSFX = false)
    {
        if (!noSFX)
        {
            var sfx = Script_SFXManager.SFX;
            inventoryAudioSource.PlayOneShot(sfx.OpenMenu, sfx.OpenMenuVol);
        }
        
        inventoryCanvasGroup.gameObject.SetActive(true);
        // inventoryCanvasGroup.alpha = 1f;
        // inventoryCanvasGroup.blocksRaycasts = true;

        StartCoroutine(WaitCanvasVisible());

        /// <summary>
        /// To avoid canvas flicker at beginning when setting state from outside
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitCanvasVisible()
        {
            yield return new WaitForEndOfFrame();

            inventoryCanvasGroup.alpha = 1f;
        }
    }

    public void CloseInventory(bool noSFX = false)
    {
        if (!noSFX)
        {
            var sfx = Script_SFXManager.SFX;
            inventoryAudioSource.PlayOneShot(sfx.CloseMenu, sfx.CloseMenuVol);
        }

        inventoryCanvasGroup.gameObject.SetActive(false);
        inventoryCanvasGroup.alpha = 0f;
        // inventoryCanvasGroup.blocksRaycasts = false;   
    }

    public void EnableSBook(bool isActive)
    {
        menuController.EnableSBook(isActive);
    }

    public void InitializeState()
    {
        CloseInventory(noSFX: true);
        thoughtsCanvasGroup.gameObject.SetActive(false);
        sBookCanvasGroup.gameObject.SetActive(false);
    }

    public void Setup()
    {
        menuController.Setup();
        game = Script_Game.Game;

        InitializeState();
    }
}
