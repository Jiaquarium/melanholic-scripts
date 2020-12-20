using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_FadePoint : MonoBehaviour
{
    
    public Model_FadePoint data;
    
    void OnValidate()
    {
        SetMyTransform();
    }

    void Awake()
    {
        SetMyTransform();
    }

    private void SetMyTransform()
    {
        if (data != null)
        {
            data.target = transform;
        }
    }
}
