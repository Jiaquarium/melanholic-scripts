using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using mySortingLayerNameSpace;
#endif

/// <summary>
/// Set sorting order and layer manually
/// </summary>
public class Script_SetSortingOrder : MonoBehaviour
{
    [SerializeField] private int forceSortingOrder;
    
    #if UNITY_EDITOR
    [SortingLayer]
    #endif
    public string sortingLayerAttribute;
    public string sortingLayerFallbackName = "Default";
    [SerializeField] private bool isParent;
    
    [SerializeField] private bool isUpdate;
    
    private Renderer r;
    
    void OnValidate()
    {
        HandleSortingOrder();
    }

    void Start()
    {
        HandleSortingOrder();
    }

    void Update()
    {
        if (isUpdate)
            HandleSortingOrder();
    }

    private void HandleSortingOrder()
    {
        if (string.IsNullOrEmpty(sortingLayerFallbackName))
            sortingLayerFallbackName = "Default";
        
        r = GetComponent<Renderer>();
        
        if (isParent)   SetChildrenSortingOrder();
        else            SetSortingOrder(r);
    }

    private void SetChildrenSortingOrder()
    {
        Renderer[] childrenRenderers = transform.GetChildren<Renderer>();
         
        foreach (Renderer r in childrenRenderers)
        {
            SetSortingOrder(r);
        }
    }

    private void SetSortingOrder(Renderer r)
    {
        if (!string.IsNullOrEmpty(sortingLayerAttribute))
            r.sortingLayerName = sortingLayerAttribute;
        else
            r.sortingLayerName = sortingLayerFallbackName.ToString();
        r.sortingOrder = forceSortingOrder;
    }
}
