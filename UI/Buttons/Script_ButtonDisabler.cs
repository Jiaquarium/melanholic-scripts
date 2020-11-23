using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Script_ButtonHighlighter))]
public class Script_ButtonDisabler : MonoBehaviour
{
    [SerializeField] Script_UIState masterUIState;
    
    // Update is called once per frame
    void Update()
    {
        if (masterUIState != null)
        {
            if (masterUIState.state == UIState.Disabled)
            {
                GetComponent<Button>().enabled = false;
            }
            else
            {
                /// ButtonHighlighter has priority as to whether the button is active or not
                if (GetComponent<Script_ButtonHighlighter>().isActive)
                    GetComponent<Button>().enabled = true;
            }
        }
    }
}
