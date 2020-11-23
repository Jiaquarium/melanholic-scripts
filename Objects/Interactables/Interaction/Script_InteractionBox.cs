using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Exposed box to detect interactable box (hurtBox)
/// handles all interactions on this layer and ONLY this layer (e.g. talking, pushing)
/// NOTE: DOES NOT FOLLOW THE PHYSICS MATRIX
/// </summary>
public class Script_InteractionBox : MonoBehaviour
{
    public bool isExposed;
    [SerializeField] protected Collider[] colliders;
    [SerializeField] private Vector3 boxSize; // half extants
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] private Color color;
    protected virtual void Update()
    {
        if (isExposed)  ExposeBox();
    }
    
    protected void ExposeBox()
    {
        colliders = Physics.OverlapBox(transform.position, boxSize, transform.rotation, layerMask);
    }

    private void OnDrawGizmos() {
        if (!isExposed)     return;
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawCube(Vector3.zero, new Vector3(boxSize.x * 2, boxSize.y * 2, boxSize.z * 2)); // size is halfExtents
    }

    /// <summary>
    /// Searches recursively up the hierarchy for a parent with Script_Interactable Component
    /// </summary>
    public List<Script_Interactable> GetInteractablesBlocking(bool ignoreText = true)
    {
        List<Script_Interactable> interactables = new List<Script_Interactable>();
        
        ExposeBox();

        foreach (Collider col in colliders)
        {
            Script_Interactable interactable = col.transform.GetParentRecursive<Script_Interactable>();
            
            if (interactable != null)
            {
                 if (ignoreText && interactable is Script_InteractableObjectText)
                 {
                    if (!((Script_InteractableObjectText)interactable).isBlocking)
                        continue;
                 }
                
                interactables.Add(interactable);
            }
        }

        return interactables;
    }
    /// <summary>
    /// Searches recursively up the hierarchy for a parent with Script_Pushable Component
    /// </summary>
    public virtual List<Script_Pushable> GetPushables()
    {
        List<Script_Pushable> pushables = new List<Script_Pushable>();
        
        ExposeBox();

        foreach (Collider col in colliders)
        {
            if (col.transform.GetParentRecursive<Script_Pushable>() != null)
            {
                pushables.Add(col.transform.GetParentRecursive<Script_Pushable>());
            }
        }

        return pushables;
    }
    public virtual Script_SavePoint GetSavePoint() { return null; }
    public virtual Script_StaticNPC GetNPC() { return null; }
    public virtual Script_InteractableObject GetInteractableObject() { return null; }
    public virtual Script_InteractableObject[] GetInteractableObjects() { return null; }
    public virtual Script_ItemObject GetItem() { return null; }
    public virtual Script_UsableTarget GetUsableTarget() { return null; }
}
