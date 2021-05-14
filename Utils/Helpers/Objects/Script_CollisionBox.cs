using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Expose method to return the colliders
/// Used primarily for general controllers that need reference to colliders (e.g. Disablers)
/// Note: Does NOT follow physics matrix
/// </summary>
public class Script_CollisionBox : Script_InteractableBox
{
    [SerializeField] protected Collider[] _colliders;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private int max = 10;
    
    public Collider[] Colliders
    {
        get => _colliders;
        set => _colliders = value;
    }   

    public virtual void Start()
    {
        _colliders = new Collider[max];
    }
    
    public virtual void ExposeBox()
    {
        Array.Clear(_colliders, 0, _colliders.Length);
        int size = Physics.OverlapBoxNonAlloc(transform.position, boxSize, _colliders, transform.rotation, layerMask);
    }
}
