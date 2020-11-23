using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MeshColorController : MonoBehaviour
{
    [Tooltip("Useful for timeline")]
    [SerializeField] private Color color;
    [SerializeField] private bool forceUpdate = true;
    [SerializeField] private bool isParent;
    [SerializeField] private MeshRenderer[] meshChildren;
    
    void Start()
    {
        if (isParent)
        {
            meshChildren = transform.GetComponentsInChildren<MeshRenderer>(true);
        }

        UpdateColor();
    }

    void Update()
    {
        if (forceUpdate)    UpdateColor();
    }
    
    void UpdateColor()
    {
        if (isParent)
        {
            foreach (MeshRenderer m in meshChildren)
            {
                m.material.color = color;
            }
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = color;
        }
    }
}
