using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Saves for toplevel buttons where the nav came from
/// so once user wants to go back down, the nav will go back down
/// the same path it came up from
/// </summary>
[RequireComponent(typeof(Selectable))]
[RequireComponent(typeof(Button))]
public class Script_ButtonNavSaver : MonoBehaviour, ISelectHandler
{
    public GameObject[] buttons;
    private GameObject lastSelectedNotThis;

    private void Update() {
        if (EventSystem.current == null)    return;
        
        if (
            EventSystem.current.currentSelectedGameObject != lastSelectedNotThis
            && EventSystem.current.currentSelectedGameObject != this.gameObject
        )
        {
            lastSelectedNotThis = EventSystem.current.currentSelectedGameObject;
        }
    }

    // OnSelect always happens before Update but before safe I added the line
    // && EventSystem.current.currentSelectedGameObject != this.gameObject
    public void OnSelect(BaseEventData e)
    {
        foreach (GameObject button in buttons)
        {
            if (button == lastSelectedNotThis)
            {
                Navigation btnNav = GetComponent<Selectable>().navigation;
                btnNav.selectOnDown = button.GetComponent<Button>();
                GetComponent<Selectable>().navigation = btnNav;    
            }
        }
    }
}
