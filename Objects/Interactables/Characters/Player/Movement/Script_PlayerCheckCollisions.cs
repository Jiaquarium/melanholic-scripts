using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Script_PlayerCheckCollisions : Script_CheckCollisions
{
    [SerializeField] private Script_PlayerHandleStairs stairsHandler;
    
    /// <summary>
    /// in addition to checking if on tilemap, player needs to verify
    /// entrance and exits. allow player to go onto exits and entrance tiles
    /// </summary>
    /// <param name="desiredX">int of x player is trying to move to</param>
    /// <param name="desiredZ">int of z player is trying to move to</param>
    /// <param name="tileLocation">world location</param>
    /// <returns>true if not on tilemap</returns>
    protected override bool CheckNotOffTilemap(Vector3Int tileWorldLocation)
    {
        Tilemap tileMap                         = Script_Game.Game.TileMap;
        Tilemap[] extraTileMaps                 = Script_Game.Game.ExtraTileMaps;
        Script_WorldTile[] worldTileMaps        = Script_Game.Game.WorldTiles;
        Tilemap entrancesTileMap                = Script_Game.Game.EntranceTileMap;
        Tilemap[] exitsTileMaps                 = Script_Game.Game.ExitTileMaps;

        // Option to use multiple Infinite World Tiles.
        if (worldTileMaps != null && worldTileMaps.Length > 0)
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

            Debug.Log($"tileWorldLocation: {tileWorldLocation} NOT in World Tile Maps");
            return true;
        }

        // Check additional default Ground Tilemaps if any.
        if (extraTileMaps != null && extraTileMaps.Length > 0)
        {
            foreach (var extraTileMap in extraTileMaps)
            {
                Vector3Int tileLoc = extraTileMap.WorldToCell(tileWorldLocation);
                if (!IsOutOfBounds(extraTileMap, tileLoc))  return false;
            }
        }

        // Check the default Ground Tilemap.
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

    protected override bool ModifyElevation(Vector3 loc, Directions dir, ref Vector3 desiredMove)
    {
        Vector3? newDesiredMoveWithElevation = stairsHandler?.CheckStairsTilemaps(
            loc, dir, (Vector3)desiredMove
        );
        bool isStairs = newDesiredMoveWithElevation != null;
        
        if (isStairs)   desiredMove = (Vector3)newDesiredMoveWithElevation;

        return isStairs;
    }
}
