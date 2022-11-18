using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Script_PhysicsBox : MonoBehaviour
{
    public bool isExposed;
    [SerializeField] protected Collider[] colliders;
    [SerializeField] private Vector3 boxSize; // half extants
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] private Color color;
    
    [SerializeField] private int max = 10;

    public Vector3 BoxSize
    {
        get => boxSize;
        set => boxSize = value;
    }

    public Collider[] Colliders { get => colliders; }
    
    protected virtual void Start()
    {
        colliders = new Collider[max];
    }
    
    protected virtual void Update()
    {
        if (isExposed)  ExposeBox();
    }
    
    public void ExposeBox()
    {
        Array.Clear(colliders, 0, colliders.Length);
        int size = Physics.OverlapBoxNonAlloc(transform.position, boxSize, colliders, transform.rotation, layerMask);
    }

    private void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawCube(Vector3.zero, new Vector3(boxSize.x * 2, boxSize.y * 2, boxSize.z * 2)); // size is halfExtents
    }
}
