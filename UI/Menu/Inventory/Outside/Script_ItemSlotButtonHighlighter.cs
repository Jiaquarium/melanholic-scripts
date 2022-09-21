using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Script_InventoryViewSlot))]
public class Script_ItemSlotButtonHighlighter : Script_ButtonHighlighter, ISelectHandler, IDeselectHandler
{
    public bool isEnterPressed;
    
    private Script_InventoryManager.Types type;
    
    void Awake()
    {
        type = GetComponent<Script_InventoryViewSlot>().Type;
    }
    
    /// Allow to keep item slot highlighted when moving to itemChoices
    public void ForceHighlightItemChoices()
    {
        HighlightAndShowDescription(true, false, type);
    }
    
    private void HighlightAndShowDescription(
        bool isOn,
        bool isUpdateDescription,
        Script_InventoryManager.Types type
    )
    {
        Dev_Logger.Debug($"HighlightAndShowDescription({isOn}, {isUpdateDescription}, {type})");
        
        foreach (Image img in outlines)
        {
            img.enabled = isOn;
            int myId = GetComponent<Script_InventoryViewSlot>().Id;
            Script_Game.Game.HighlightItem(myId, isOn, isUpdateDescription, type);
        }
        
        isHighlighted = isOn;
    }

    /// <summary>
    /// inventory slots should stay selected when player goes "deeper"
    /// into one via itemChoices
    /// </summary>
    /// <param name="e"></param>
    public override void OnSelect(BaseEventData e)
    {
        Dev_Logger.Debug("Slot OnSelect()");
        // after itemChoices, active will return after "Enter" sequence
        HighlightAndShowDescription(true, true, type);
        isEnterPressed = false;
    }
    public override void OnDeselect(BaseEventData e)
    {
        if (!isEnterPressed)
        {
            Dev_Logger.Debug("Slot Deselect()");
            HighlightAndShowDescription(false, false, type);
        }
    }
}
