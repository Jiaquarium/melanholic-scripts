using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_GameHelper : MonoBehaviour
{
    public Vector3Int playerSpawn;
    public int level;
    public Directions facingDirection;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (!Debug.isDebugBuild || !Const_Dev.IsDevMode)
        {
            Destroy(this);
        }
    }
}
