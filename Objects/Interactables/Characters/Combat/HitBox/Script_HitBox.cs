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
    [SerializeField] private Color inactiveColor = new Color(255, 255, 0, 75);
    [SerializeField] private Color collisionOpenColor = new Color(255, 0, 0, 125);
    [SerializeField] private Color collidingColor = new Color(255, 0, 0, 215);
    [SerializeField] private Color disabledColor = new Color(0, 0, 0, 125);
    [SerializeField] private ColliderState state;

    [SerializeField] private int max = 10;

    // Allow for optimization for hitboxes that will never collide with Player.
    [SerializeField] private bool isDisabled;

    private IHitBoxResponder responder = null;
    
    public bool IsDisabled
    {
        get => isDisabled;
        set => isDisabled = value;
    }
    
    void Start()
    {
        colliders = new Collider[max];

        if (
            (transform.position.x != 0f || transform.position.y != 0f || transform.position.z != 0f)
            && (transform.localScale.x != 1f || transform.localScale.y != 1f || transform.localScale.z != 1f)
        )
        {
            Debug.LogWarning($"{transform.parent.name} {name} needs to have an origin local position for scale change");
        }
    }
    
    void Update()
    {
        if (state == ColliderState.Closed || isDisabled)
            return;
        
        ExposeBox();
        CheckColliding();
    }
    
    /// <summary>
    /// Fill colliders Array with "hit" overlapping colliders.
    /// </summary>
    void ExposeBox()
    {
        Array.Clear(colliders, 0, colliders.Length);
        int size = Physics.OverlapBoxNonAlloc(transform.position, boxSize, colliders, transform.rotation, layerMask);
    }

    /// <summary>
    /// Tell the responder (Attack) how to handle the hit.
    /// </summary>
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
        if (isDisabled)
        {
            Gizmos.color = disabledColor;
            return;
        }
        
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