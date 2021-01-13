using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Expose method to return the colliders
/// </summary>
public class Script_CollisionBox : Script_InteractableBox
{
    [SerializeField] protected Collider[] _colliders;
    [SerializeField] private LayerMask layerMask;
    
    public Collider[] Colliders
    {
        get => _colliders;
        set => _colliders = value;
    }   

    public virtual void ExposeBox()
    {
        Colliders = Physics.OverlapBox(transform.position, boxSize, transform.rotation, layerMask);
    }
}
