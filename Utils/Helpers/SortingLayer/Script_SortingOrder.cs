using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set sorting order based on positioning of specified axis;
/// Use with setting player to ForceSortingOrder on respective level
/// </summary>
public class Script_SortingOrder : MonoBehaviour
{
    public static int playerSortOrderOffset = 0;
    
    public int defaultSortingOrder;
    public int sortingOrderBase;
    public int offset;
    public bool runOnlyOnce;
    public bool sortingOrderIsAxisZ = true;
    public bool isParent;
    
    [SerializeField] private Renderer r;

    void Start()
    {
        r = GetComponent<Renderer>();
    }

    void Update()
    {
        SetChildrenSortingOrder();
        SetSortingOrder(r);

        if (runOnlyOnce)    this.enabled = false;
    }

    public void EnableWithOffset(int _offset, bool isAxisZ)
    {
        this.enabled = true;
        offset = _offset;
        sortingOrderIsAxisZ = isAxisZ;
    }

    public void DefaultSortingOrder()
    {
        r = GetComponent<Renderer>();
        r.sortingOrder = defaultSortingOrder;
        this.enabled = false;
    }

    void SetSortingOrder(Renderer r)
    {
        if (r == null)
            return;
        
        if (sortingOrderIsAxisZ)
        {
            r.sortingOrder = (int)(((sortingOrderBase - transform.position.z) * 10) + offset);
        }
        else
        {
            r.sortingOrder = (int)(((sortingOrderBase + transform.position.x) * 10) + offset);
        }
    }

    void SetChildrenSortingOrder()
    {
        Renderer[] childrenRenderers = transform.GetChildren<Renderer>();
        
        foreach (Renderer r in childrenRenderers)
            SetSortingOrder(r);
    }
}
