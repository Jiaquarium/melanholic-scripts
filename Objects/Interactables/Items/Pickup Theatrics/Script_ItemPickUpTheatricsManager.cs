using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// When we need theatrics to introduce important items (e.g. the concept of stickers)
/// 
/// Changes game state to cut scene and then back to what was saved as prev state
/// 
/// Very similar to PRCSManager
/// No fading here
/// </summary>
public class Script_ItemPickUpTheatricsManager : MonoBehaviour
{
    public static Script_ItemPickUpTheatricsManager Control;
    [SerializeField] private CanvasGroup ItemPickUpTheatricsCanvasGroup;
    [SerializeField] private Canvas ItemPickUpTheatricsCanvas;
    [SerializeField] private string prevState;
    
    public void Start()
    {
        Initialize();
    }

    public void ShowItemPickUpTheatric(Script_ItemPickUpTheatric theatric)
    {
        ItemPickUpTheatricsCanvasGroup.alpha = 1f;
        ItemPickUpTheatricsCanvasGroup.gameObject.SetActive(true);

        theatric.gameObject.SetActive(true);

        /// Must do this at end of frame or the "space" press will cause the item to be hidden
        StartCoroutine(WaitToChangeToCutScene());
        IEnumerator WaitToChangeToCutScene()
        {
            yield return new WaitForEndOfFrame();
            Script_Game.Game.ChangeStateCutScene();
            /// Need to disable player's ability to move forward into item dialogue
            Script_Game.Game.GetPlayer().SetIsStandby();
        }
    }

    public void HideItemPickUpTheatric(Script_ItemPickUpTheatric theatric)
    {
        ItemPickUpTheatricsCanvasGroup.alpha = 0f;
        ItemPickUpTheatricsCanvasGroup.gameObject.SetActive(false);

        theatric.gameObject.SetActive(false);

        Script_Game.Game.ChangeStateLastState(null);
        /// Return the player back to Picking Up State and then immediately force done
        Script_Player p = Script_Game.Game.GetPlayer();
        p.SetIsPickingUp(p.GetItemShown());
        p.HandleEndItemDescriptionDialogue();
        p.SetItemShown(null);
    }

    public void Initialize()
    {
        /// Hide CanvasGroup but ensure the ItemPickUpTheatrics canvas and ready to use
        ItemPickUpTheatricsCanvasGroup.gameObject.SetActive(false);
        ItemPickUpTheatricsCanvasGroup.alpha = 0f;
        ItemPickUpTheatricsCanvas.gameObject.SetActive(true);

        Script_ItemPickUpTheatric []allTheatrics = ItemPickUpTheatricsCanvasGroup.GetComponentsInChildren<Script_ItemPickUpTheatric>(true);
        foreach (Script_ItemPickUpTheatric theatric in allTheatrics)
        {
            theatric.gameObject.SetActive(false);
        }
    }

    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }
    }
}
