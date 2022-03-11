using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EnergySpikeParent : MonoBehaviour
{
    public string hitBoxId;

    [SerializeField] private Material material;

    void OnValidate()
    {
        SetChildrenMaterial();   
    }

    void Awake()
    {
        SetChildrenMaterial();   
    }

    private void SetChildrenMaterial()
    {
        if (material != null)
        {
            foreach (var r in GetComponentsInChildren<Renderer>(true))
                r.material = material;
        }
    }
}
