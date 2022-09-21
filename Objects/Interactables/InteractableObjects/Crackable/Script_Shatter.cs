using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ensure the shattered ice have
/// - RigidBody
/// - MeshCollider
/// - Layer set to Environment_Remains
/// </summary>
public class Script_Shatter : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] float shatterRadius;
    [SerializeField] float upwardsModifier;
    
    [SerializeField] private Transform forceOrigin;
    
    [SerializeField] private List<GameObject> fulls;
    [SerializeField] private List<GameObject> shatters;

    [SerializeField] private Script_PhysicsController physicsController;
    
    public void Shatter()
    {
        Dev_Logger.Debug($"{name} Shatter");
        
        // Hide fulls
        fulls.ForEach(full => full.SetActive(false));
        
        // Show shatters and make non-ragdoll (not kinematic).
        shatters.ForEach(shatter => shatter.SetActive(true));
        physicsController.IsKinematic = false;

        // Apply force
        physicsController.AddExplosionForce(force, forceOrigin.position, shatterRadius, upwardsModifier);
    }

    /// <summary>
    /// Anime-like diagonal cut. Reveal all the cracks.
    /// </summary>
    public void DiagonalCut()
    {
        Dev_Logger.Debug($"{name} Diagonal Cut");
        
        // Hide fulls
        fulls.ForEach(full => full.SetActive(false));
        
        // Show shatters
        shatters.ForEach(shatter => shatter.SetActive(true));
    }
}
