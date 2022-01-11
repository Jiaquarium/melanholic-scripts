using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// NOTE It's assumed the Player will always respawn at the Origin Tile, so loading directly onto a
/// non-Origin tile will make tile shifting break.
/// </summary>
public class Script_WorldTile : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Script_WorldTilesController worldTilesController;
    
    private Vector3 initialPosition;
    
    public Tilemap TileMap
    {
        get => tileMap;
    }
    
    void Awake()
    {
        if (worldTilesController == null)   Debug.LogError($"{name} needs a reference to a WorldTilesController.");

        initialPosition = transform.position;
    }

    public void SetAsNewOrigin()
    {
        worldTilesController.SetNewOriginWorldTile(this);
    }

    public void Move(Vector2 coord)
    {
        float x = Mathf.Round(transform.position.x + (coord.x * worldTilesController.XLength));
        float y = Mathf.Round(transform.position.y);
        float z = Mathf.Round(transform.position.z + (coord.y * worldTilesController.ZLength));
        
        transform.position = new Vector3(x, y, z);
    }

    public void InitialState()
    {
        // Means this is the first initial Setup
        if (initialPosition == null)    return;

        transform.position = initialPosition;
    }
}
