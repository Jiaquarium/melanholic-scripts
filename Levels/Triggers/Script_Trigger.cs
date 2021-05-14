using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Trigger : MonoBehaviour
{
    public string Id;
    protected bool isInitializing = true;
    protected bool isColliding;

    [SerializeField] bool reuseCollisionCallbacks = true;
    
    protected virtual void Start()
    {
        Physics.reuseCollisionCallbacks = reuseCollisionCallbacks;
    }
    
    void Update()
    {
        // to prevent multiple OnTriggerEnters for same event;
        // set isColliding to true in OnTriggerEnter and check if isColliding 
        // physics is done before Update, so it'll be reset here
        isColliding = false;
    }
    
    // allows triggers to know if an object has entered it on initialization of scene
    void LateUpdate()
    {
        if (isInitializing)     isInitializing = false;
    }
}
