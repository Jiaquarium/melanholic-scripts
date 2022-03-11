using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set the Render Queue of the referenced material. The change
/// will propagate through to every Renderer that uses the material.
/// </summary>
public class Script_SetRenderQueue : MonoBehaviour {
 
    [SerializeField] private int queue = 2000;
 
    void OnValidate()
    {
        SetRenderQueue();
    }
    
    void Awake()
    {
        SetRenderQueue();
    }

    private void SetRenderQueue()
    {
        Material[] materials = GetComponent<Renderer>()?.sharedMaterials;
		
        for (int i = 0; i < materials.Length; i++) {
            materials[i].renderQueue = queue;
		}
    }
}