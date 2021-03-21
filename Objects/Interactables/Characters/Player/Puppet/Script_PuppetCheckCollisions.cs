using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Unlike PlayerCheckCollisions, this will
///     - not allow the Player Copy to access Entrance/Exit tiles.
/// </summary>
public class Script_PuppetCheckCollisions : Script_PlayerCheckCollisions
{
    protected override bool CheckNotOffTilemap(
        int desiredX,
        int desiredZ,
        Vector3Int tileWorldLocation
    )
    {
        Tilemap tileMap = Script_Game.Game.TileMap;
        Vector3Int tileLocation = tileMap.WorldToCell(tileWorldLocation);

        // tiles map from (xyz) to (xz)
        return !tileMap.HasTile(tileLocation);
    }   
}
