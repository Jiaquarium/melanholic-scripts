using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Currently pushables collide when detect another pushable
/// </summary>
public class Script_PushableCheckCollisions : Script_CheckCollisions
{
    [SerializeField] private Tilemap myPushableTilemap;
    [SerializeField] private bool isOnlyPushTilemap;
    /// <summary>
    /// Use this in conjunction with above; allows to push into interactables
    /// (e.g. push into Interactables with convexity like fireplace, tunnels, etc.)
    /// </summary>
    [SerializeField] private bool shouldIgnoreInteractables;
    
    protected override bool CheckNotOffTilemap(
        int desiredX,
        int desiredZ,
        Vector3Int tileWorldLocation
    )
    {
        Tilemap tileMap = Script_Game.Game.TileMap;
        Tilemap pushablesTileMap = myPushableTilemap == null ?
            Script_Game.Game.PushableTileMap
            : myPushableTilemap;
        
        /// <summary>
        /// Getting grid location based on tileMap assumes entrances and exits
        /// are relatively in the same world space
        /// </summary>
        Vector3Int tileLocation = tileMap.WorldToCell(tileWorldLocation);

        if (isOnlyPushTilemap)
        {
            return pushablesTileMap == null || !pushablesTileMap.HasTile(tileLocation);
        }
        
        if (
            !tileMap.HasTile(tileLocation)
            && (pushablesTileMap == null || !pushablesTileMap.HasTile(tileLocation))
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected override bool CheckInteractableBlocking(Directions dir)
    {
        if (shouldIgnoreInteractables)  return false;
        
        return base.CheckInteractableBlocking(dir);
    }

    protected override bool CheckPushableBlocking(Directions dir)
    {
        List<Script_Pushable> pushables = interactionBoxController.GetPushables(dir);
        
        return pushables.Count > 0;
    }
}
