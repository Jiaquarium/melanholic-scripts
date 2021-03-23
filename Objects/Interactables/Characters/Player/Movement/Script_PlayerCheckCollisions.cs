using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Script_PlayerCheckCollisions : Script_CheckCollisions
{
    /// <summary>
    /// in addition to checking if on tilemap, player needs to verify
    /// entrance and exits. allow player to go onto exits and entrance tiles
    /// </summary>
    /// <param name="desiredX">int of x player is trying to move to</param>
    /// <param name="desiredZ">int of z player is trying to move to</param>
    /// <param name="tileLocation">world location</param>
    /// <returns>true if not on tilemap</returns>
    protected override bool CheckNotOffTilemap(
        int desiredX,
        int desiredZ,
        Vector3Int tileWorldLocation
    )
    {
        Tilemap tileMap                         = Script_Game.Game.TileMap;
        Script_WorldTile[] worldTileMaps        = Script_Game.Game.WorldTiles;
        Tilemap entrancesTileMap                = Script_Game.Game.EntranceTileMap;
        Tilemap[] exitsTileMaps                 = Script_Game.Game.ExitTileMaps;

        // Option to use multiple Infinite World Tiles.
        if (worldTileMaps != null && worldTileMaps.Length != 0)
        {
            foreach (var worldTile in worldTileMaps)
            {
                // https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.GetCellCenterWorld.html   
                // Getting grid location based on tileMap assumes entrances and exits
                // are relatively in the same world space.
                Vector3Int tileLoc = worldTile.TileMap.WorldToCell(tileWorldLocation);
                
                if (!IsOutOfBounds(worldTile.TileMap, tileLoc))
                {
                    worldTile.SetAsNewOrigin();
                    return false;
                }
            }

            return true;
        }

        // Default just using single Tilemap.
        Vector3Int tileLocation = tileMap.WorldToCell(tileWorldLocation);
        return IsOutOfBounds(tileMap, tileLocation);

        bool IsOutOfBounds(Tilemap tileMap, Vector3Int tileLocation)
        {
            // tiles map from (xyz) to (xz)
            if (!tileMap.HasTile(tileLocation))
            {
                // tile may not be in current tilemap but could still be in an entrance tilemap
                if (
                    entrancesTileMap != null
                    && entrancesTileMap.HasTile(tileLocation)
                    && !entrancesTileMap.GetComponent<Script_TileMapExitEntrance>().IsDisabled
                    && entrancesTileMap.gameObject.activeSelf
                )
                {
                    return false;
                }
                
                foreach(Tilemap tm in exitsTileMaps)
                {
                    // tile may not be in current tilemap but could still be in an exit tilemap
                    if (
                        tm != null
                        && tm.HasTile(tileLocation)
                        && !tm.GetComponent<Script_TileMapExitEntrance>().IsDisabled
                        && tm.gameObject.activeSelf
                    )
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
