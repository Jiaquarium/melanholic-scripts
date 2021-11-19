using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ExitMetadataObject : Script_ExitMetadata
{
    void OnValidate()
    {
        UpdatePosition();
    }

    void OnEnable()
    {
        UpdatePosition();
    }

    void OnDisable()
    {
        UpdatePosition();
    }
}
