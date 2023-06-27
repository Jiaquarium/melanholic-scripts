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
                if (!IsOutOfBounds(extraTileMap, tileLoc))
                    return false;
            }
        }

        // Check the default Ground Tilemap.
        Vector3Int tileLocation = tileMap.WorldToCell(tileWorldLocation);
        return IsOutOfBounds(tileMap, tileLocation);
    }

    /// <summary>
    /// Check if the desired position for the attack will land off tilemaps (anywhere player cannot move).
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="currentLocation"></param>
    /// <returns>True, if attack will land off tilemaps; False, if the attack will land on tilemap</returns>
    public bool CheckAttackOffTilemap(Directions dir, Vector3 currentLocation)
    {
        Script_WorldTile[] worldTileMaps = Script_Game.Game.WorldTiles;
        Vector3 desiredDirection = Script_Utils.GetDirectionToVectorDict()[dir];
        Vector3Int tileWorldLocation = (currentLocation + desiredDirection).ToVector3Int();

        // If using World Tiles, there should not be any off tilemap area
        if (worldTileMaps != null && worldTileMaps.Length > 0)
        {
            Script_WorldTile worldTile = GetCurrentWorldTile(worldTileMaps, tileWorldLocation);
            return worldTile == null;
        }

        Tilemap currentTileMap = GetCurrentTileMapOn(tileWorldLocation);
        return currentTileMap == null;
    }

    public Tilemap GetCurrentTileMapOn(Vector3Int tileWorldLocation)
    {
        Tilemap tileMap = GetMainTilemap(Script_Game.Game.TileMap, tileWorldLocation);

        if (tileMap == null)
            tileMap = GetExtraTilemap(Script_Game.Game.ExtraTileMaps, tileWorldLocation);
        
        if (tileMap == null)
            tileMap = GetStairsTilemap(Script_Game.Game.StairsTileMaps, tileWorldLocation);
        
        return tileMap;
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

    /// <summary>
    /// Get the Main Tilemap the location is on.
    /// </summary>
    /// <param name="tileMap">Main tilemap (not extra e.g. Ballroom Upper Deck)</param>
    /// <param name="tileWorldLocation">Location to check on tilemap</param>
    /// <returns>Main Tilemap (Game) the location is on; null, if not on the Main Tilemap</returns>
    public Tilemap GetMainTilemap(Tilemap tileMap, Vector3Int tileWorldLocation)
    {
        Vector3Int tileLocation = tileMap.WorldToCell(tileWorldLocation);
        if (!IsOutOfBounds(tileMap, tileLocation))
            return tileMap;
        
        return null;
    }

    /// <summary>
    /// Get the Extra Tilemap (e.g. Ballroom Upper Deck) that the location is on.
    /// </summary>
    /// <param name="extraTileMaps">Extra tile maps from Game</param>
    /// <param name="tileWorldLocation">Location to check on tilemap</param>
    /// <returns>Extra Tilemap the location is on; null, if not on an extra tilemap</returns>
    public Tilemap GetExtraTilemap(Tilemap[] extraTileMaps, Vector3Int tileWorldLocation)
    {
        if (extraTileMaps != null && extraTileMaps.Length > 0)
        {
            foreach (var extraTileMap in extraTileMaps)
            {
                Vector3Int tileLoc = extraTileMap.WorldToCell(tileWorldLocation);
                if (!IsOutOfBounds(extraTileMap, tileLoc))
                    return extraTileMap;
            }
        }

        return null;
    }

    /// <summary>
    /// Get the Stairs Tilemap (e.g. Ballroom Stairs) that the location is on.
    /// </summary>
    /// <param name="stairsTileMaps">Extra Stairs maps from Game</param>
    /// <param name="tileWorldLocation">Location to check on tilemap</param>
    /// <returns>Extra Tilemap the location is on; null, if not on an extra tilemap</returns>
    public Tilemap GetStairsTilemap(Script_StairsTilemap[] stairs, Vector3Int tileWorldLocation)
    {
        if (stairs != null && stairs.Length > 0)
        {
            foreach (var stair in stairs)
            {
                Tilemap stairTileMap = stair.GetComponent<Tilemap>();
                Vector3Int tileLoc = stairTileMap.WorldToCell(tileWorldLocation);
                if (!IsOutOfBounds(stairTileMap, tileLoc))
                    return stairTileMap;
            }
        }

        return null;
    }
    
    /// <summary>
    /// Check if location is on tilemap. Tile map from (xyz) to (xz).
    /// </summary>
    /// <param name="tileMap"></param>
    /// <param name="tileLocation"></param>
    /// <returns>True if tilemap does not contain tileLocation; False if tilemap contains tileLocation</returns>
    private bool IsOutOfBounds(Tilemap tileMap, Vector3Int tileLocation)
    {
        return !tileMap.HasTile(tileLocation);
    }

    /// <summary>
    /// Overrides Check Collisions if handling desired movement onto Stairs.
    /// </summary>
    /// <param name="loc"></param>
    /// <param name="dir"></param>
    /// <param name="desiredMove"></param>
    /// <returns>True if using stairs handler for movement</returns>
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
