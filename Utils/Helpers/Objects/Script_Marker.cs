using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Marker : MonoBehaviour
{
    [SerializeField] protected Vector3 boxSize; // half extants
    [SerializeField] Color color;
    [SerializeField] protected Vector3 drawOffset;
    
    private void OnDrawGizmos() {
        Vector3 drawPos = new Vector3(
            transform.position.x + drawOffset.x,
            transform.position.y + drawOffset.y,
            transform.position.z + drawOffset.z
        );
        
        Gizmos.color = color;
        Gizmos.matrix = Matrix4x4.TRS(drawPos, transform.rotation, transform.localScale);
        Gizmos.DrawCube(Vector3.zero, new Vector3(boxSize.x * 2, boxSize.y * 2, boxSize.z * 2)); // size is halfExtents
    }
}
