using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Exposed box to detect HurtBoxes
/// Note: Does NOT follow physics matrix
/// </summary>

public interface IHitBoxResponder
{
    void CollisionedWith(Collider collider, Script_HitBox hitBox);
}

public class Script_HitBox : MonoBehaviour
{
    public string Id;
    public Script_GameOverController.DeathTypes deathType = Script_GameOverController.DeathTypes.Impaled;
    [SerializeField] protected Collider[] colliders;
    [SerializeField] private Vector3 boxSize; // half extants
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color collisionOpenColor;
    [SerializeField] private Color collidingColor;
    [SerializeField] private ColliderState state;

    [SerializeField] private int max = 10;

    private IHitBoxResponder responder = null;
    
    void Start()
    {
        colliders = new Collider[max];
    }
    
    void Update()
    {
        if (state == ColliderState.Closed)
            return;
        
        ExposeBox();
        CheckColliding();
    }
    
    void ExposeBox()
    {
        Array.Clear(colliders, 0, colliders.Length);
        int size = Physics.OverlapBoxNonAlloc(transform.position, boxSize, colliders, transform.rotation, layerMask);
    }

    void CheckColliding()
    {
        bool isCollision = false;

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider aCollider = colliders[i];

            if (aCollider != null)
            {
                responder?.CollisionedWith(aCollider, this);
                isCollision = true;
            }
        }

        state = isCollision ? state = ColliderState.Colliding : state = ColliderState.Open;
    }

    public void SetResponder(IHitBoxResponder _responder)
    {
        responder = _responder;
    }

    void OnDrawGizmos() {
        CheckGizmoColor();
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawCube(Vector3.zero, new Vector3(boxSize.x * 2, boxSize.y * 2, boxSize.z * 2)); // size is halfExtents
    }

    public void StartCheckingCollision() {
        state = ColliderState.Open;
    }

    public void StopCheckingCollision() {
        state = ColliderState.Closed;
    }

    private void CheckGizmoColor() {
        switch(state) {
            case ColliderState.Closed:
                Gizmos.color = inactiveColor;
                break;
            case ColliderState.Open:
                Gizmos.color = collisionOpenColor;
                break;
            case ColliderState.Colliding:
                Gizmos.color = collidingColor;
                break;
        }
    }
}