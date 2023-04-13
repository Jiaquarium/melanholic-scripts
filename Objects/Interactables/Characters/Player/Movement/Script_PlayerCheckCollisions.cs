using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

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

        // Option to use multiple Infinite World Tiles.
        if (worldTileMaps != null && worldTileMaps.Length > 0)
        {
            Script_WorldTile worldTile = GetCurrentWorldTile(worldTileMaps, tileWorldLocation);
            
            if (worldTile != null)
            {
                worldTile.SetAsNewOrigin();
                return false;
            }

            Dev_Logger.Debug($"tileWorldLocation: {tileWorldLocation} NOT in World Tile Maps");
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
    }

    public Script_WorldTile GetCurrentWorldTile(Script_WorldTile[] worldTileMaps, Vector3Int tileWorldLocation)
    {
        return worldTileMaps.FirstOrDefault(worldTile => {
            // https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.GetCellCenterWorld.html   
            // Getting grid location based on tileMap assumes entrances and exits
            // are relatively in the same world space.
            Vector3Int tileLoc = worldTile.TileMap.WorldToCell(tileWorldLocation);
            
            return !IsOutOfBounds(worldTile.TileMap, tileLoc);
        });
    }
    
    private bool IsOutOfBounds(Tilemap tileMap, Vector3Int tileLocation)
    {
        // tiles map from (xyz) to (xz)
        return !tileMap.HasTile(tileLocation);
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

    /// <summary>
    /// Handles if Player Movement should freeze on the pushable or continue with walking animation.
    /// Gets pushables from colliders stored from the previous call to CheckCollisions.
    /// </summary>
    /// <param name="dir"></param>
    /// <returns>True, if no pushable is collided OR if pushable is detected but it's colliding with something
    /// in the desired direction. False, if a pushable is detected and you are pushing it (continue walking anim)</returns>
    public bool IsFreezeOnCollisionPushable(Directions dir)
    {
        List<Script_Pushable> pushables = interactionBoxController.GetCurrentPushablesCached();
        
        if (pushables.Count == 0)
            return true;

        bool isPushableBlocked = pushables[0].CheckCollisions(dir, out Vector3 desiredDir);
        
        return isPushableBlocked;
    }
}
