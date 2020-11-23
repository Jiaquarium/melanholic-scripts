using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    }
}
