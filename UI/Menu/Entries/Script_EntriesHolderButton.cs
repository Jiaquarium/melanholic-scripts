using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EntriesHolderButton : MonoBehaviour
{
    public Script_EntriesViewController entriesViewController;

    /// <summary>
    /// called from OnClick handler
    /// </summary>
    public void OnEnter()
    {
        entriesViewController.EnterEntriesView();
    }
}
