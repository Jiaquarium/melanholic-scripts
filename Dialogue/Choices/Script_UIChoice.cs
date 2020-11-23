using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Script_UIChoice : MonoBehaviour
{
    public Image cursor;
    public bool isSelected;
    public int Id;

    void Start()
    {
        InitializeState();
    }

    /// to prevent FOUC on showing if using same menu and changing active button while inactive
    void OnDisable()
    {
        InitializeState();
    }
    
    void Update()
    {
        if (EventSystem.current == null)    return;
        
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            cursor.enabled = true;
            isSelected = true;
        }
        else
        {
            cursor.enabled = false;
            isSelected = false;
        }
    }

    public virtual void HandleSelect() {}

    public void InitializeState()
    {
        cursor.enabled = false;
    }
}
