using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_InteractableObjectHandler : MonoBehaviour
{
    public Vector3[] GetLocations(List<Script_InteractableObject> objs)
    {
        if (objs.Count == 0)    return new Vector3[0];
        
        Vector3[] objLocations = new Vector3[objs.Count];

        for (int i = 0; i < objs.Count; i++)
        {
            objLocations[i] = objs[i].transform.position;
        }

        return objLocations;
    }
}
