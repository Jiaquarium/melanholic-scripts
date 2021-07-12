using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Changes menu top bar state depending
/// </summary>
public class Script_TopBarButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] private Script_MenuController.TopBarStates state;
    [SerializeField] public Script_MenuController mainController;
    
    public virtual void OnSelect(BaseEventData e)
    {
        mainController.ChangeTopBarState(state);
        
        // For Items, the UI allows navigation to TopBar from InventoryState Inventory,
        // so ensure we update InventoryState to Overview if we exit a Submenu without
        // sending a Submenu exit event.
        mainController.InventoryState = Script_MenuController.InventoryStates.Overview;
    }

    // ------------------------------------------------------------------
    // OnClick Handler
    public void OnClick()
    {
        GameObject selectOnDown = GetComponent<Selectable>().navigation.selectOnDown.gameObject;
        EventSystem.current.SetSelectedGameObject(selectOnDown);
    }
}
