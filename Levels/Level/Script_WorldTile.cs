using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Script_WorldTile : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    
    public Tilemap TileMap
    {
        get => tileMap;
    }
    
    public Vector3Int Offset
    {
        get
        {
            int x = (int)transform.position.x;
            int y = (int)transform.position.z;
            int z = (int)transform.position.y;

            return new Vector3Int(x, y, z);
        }
    }
}
