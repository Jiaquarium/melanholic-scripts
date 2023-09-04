using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PhysicsController : MonoBehaviour
{
    [SerializeField] private bool isParent;
    [SerializeField] private List<Rigidbody> rigidBodies;

    [SerializeField] private bool isForceSolverIterations;
    [SerializeField] private int solverIterations;
    
    public bool IsKinematic
    {
        set => rigidBodies.ForEach(rigidBody => rigidBody.isKinematic = value);
    }
    
    void OnValidate()
    {
        PopulateRigidBodies();
    }

    void Awake()
    {
        PopulateRigidBodies();
    }

    // ForceMode.Impulse ideal for instant forces like explosions / collisions.
    // https://docs.unity3d.com/ScriptReference/ForceMode.Impulse.html
    public void AddExplosionForce(float force, Vector3 origin, float radius, float upwardsModifier = 0f)
    {
        rigidBodies.ForEach(rigidBody => rigidBody.AddExplosionForce(
            force,
            origin,
            radius,
            upwardsModifier,
            ForceMode.Impulse
        ));
    }

    private void PopulateRigidBodies()
    {
        var _rigidBodies = new List<Rigidbody>();
        
        var myRigidBody = GetComponent<Rigidbody>();
        if (myRigidBody != null)
            _rigidBodies.Add(myRigidBody);
        
        if (isParent)
        {
            foreach (var rigidBody in transform.GetComponentsInChildren<Rigidbody>(true))
            {
                _rigidBodies.Add(rigidBody);

                if (isForceSolverIterations && solverIterations > 0)
                    rigidBody.solverIterations = solverIterations;
            }
        }

        rigidBodies = _rigidBodies;
    }
}
