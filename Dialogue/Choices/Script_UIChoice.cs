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
            isSelected = true;
            
            /// Give Buttonhighlighter precedence to hide cursor
            /// if the button is not in an active state
            Script_ButtonHighlighter myHighlighter = GetComponent<Script_ButtonHighlighter>();
            if (myHighlighter != null)
            {
                if (myHighlighter.isLoading)    cursor.enabled = false;
            }
            else            
            {
                cursor.enabled = true;
            }
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
