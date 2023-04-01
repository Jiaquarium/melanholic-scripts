using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_CanvasParentSortOrder : MonoBehaviour
{
    /// <summary>
    /// Set sorting order of all children canvases.
    /// </summary>
    [SerializeField] private int sortingOrder;
    
    void OnValidate()
    {
        // SetChildrenCanvasesSortingOrder();
    }

    private void SetChildrenCanvasesSortingOrder()
    {
        Canvas[] children = GetComponentsInChildren<Canvas>(true);

        foreach (Canvas c in children)
        {
            c.sortingOrder = sortingOrder;
        }
    }
}
