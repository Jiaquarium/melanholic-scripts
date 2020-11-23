using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ExitMetadataObject : Script_ExitMetadata
{
    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        data.playerSpawn = transform.position;
    }
}
