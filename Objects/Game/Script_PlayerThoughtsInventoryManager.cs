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

    public void OpenInventory()
    {
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

    public void CloseInventory()
    {
        inventoryCanvasGroup.gameObject.SetActive(false);
        inventoryCanvasGroup.alpha = 0f;
        // inventoryCanvasGroup.blocksRaycasts = false;   
    }

    public void EnableSBook(bool isActive)
    {
        menuController.EnableSBook(isActive);
    }

    /// <summary>
    /// Handles copying thoughts into slots
    /// </summary>
    public void UpdatePlayerThoughts(
        Model_Thought thought,
        Model_PlayerThoughts thoughts,
        Script_PlayerThoughtsInventoryButton[] thoughtSlots
    )
    {
        foreach (Script_PlayerThoughtsInventoryButton ts in thoughtSlots)
        {
            ts.text.text = string.Empty;
        }

        // works ONLY if thoughts is exactly equal to maxHP
        for (int i = 0; i < thoughts.uglyThoughts.Count; i++)
        {
            string newThoughtText = thoughts.uglyThoughts[i].thought;
            thoughtSlots[i].text.text = Script_Utils.FormatString(newThoughtText);
        }
    }

    public void InitializeState()
    {
        CloseInventory();
        thoughtsCanvasGroup.gameObject.SetActive(false);
        sBookCanvasGroup.gameObject.SetActive(false);
    }

    public void Setup()
    {
        menuController.Setup();
        
        // setup number of slots
        Transform slotHolder = thoughtSlotHolder.transform;
        Script_PlayerThoughtsInventoryButton[] thoughtSlots = new Script_PlayerThoughtsInventoryButton[
            slotHolder.childCount
        ];
        for (int i = 0; i < thoughtSlots.Length; i++)
        {
            thoughtSlots[i] = slotHolder.GetChild(i)
                .GetComponent<Script_PlayerThoughtsInventoryButton>();
        }
        game = Script_Game.Game;
        game.thoughtSlots = thoughtSlots; 

        InitializeState();
    }
}
