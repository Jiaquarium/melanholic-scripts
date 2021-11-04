using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Shatter : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] float shatterRadius;
    
    [SerializeField] private Transform forceOrigin;
    
    [SerializeField] private List<GameObject> fulls;
    [SerializeField] private List<GameObject> shatters;

    [SerializeField] private Script_PhysicsController physicsController;
    
    public void Shatter()
    {
        Debug.Log($"{name} Shatter");
        
        // Hide fulls
        fulls.ForEach(full => full.SetActive(false));
        
        // Show shatters and make non-ragdoll (not kinematic).
        shatters.ForEach(shatter => shatter.SetActive(true));
        physicsController.IsKinematic = false;

        // Apply force
        physicsController.AddExplosionForce(force, forceOrigin.position, shatterRadius);
    }

    /// <summary>
    /// Anime-like diagonal cut. Reveal all the cracks.
    /// </summary>
    public void DiagonalCut()
    {
        Debug.Log($"{name} Diagonal Cut");
        
        // Hide fulls
        fulls.ForEach(full => full.SetActive(false));
        
        // Show shatters
        shatters.ForEach(shatter => shatter.SetActive(true));
    }
}
