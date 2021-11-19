using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ExitMetadata : MonoBehaviour
{
    public Model_Exit data;

    // Should always update position before being used as Exit
    // in case the Exit changed positions (e.g. World Tile changes position).
    public void UpdatePosition()
    {
        data.playerSpawn = transform.position;
    }
}
