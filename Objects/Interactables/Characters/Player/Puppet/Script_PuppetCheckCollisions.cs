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
    [SerializeField] private Script_PuppetMovement puppetMovement;
    
    protected override bool CheckNotOffTilemap(Vector3Int tileWorldLocation)
    {
        Tilemap tileMap = Script_Game.Game.TileMap;
        Vector3Int tileLocation = tileMap.WorldToCell(tileWorldLocation);

        // tiles map from (xyz) to (xz)
        return !tileMap.HasTile(tileLocation);
    }

    protected override bool CheckUniqueBlocking(Directions dir)
    {
        foreach (var uniqueBlockingTag in UniqueBlockingTags)
        {
            string tag = Const_Tags.TagsMap[uniqueBlockingTag];
            
            List<Transform> uniqueBlocking = interactionBoxController.GetUniqueBlocking(dir, tag);
            
            if (uniqueBlocking.Count > 0)
            {
                Dev_Logger.Debug($"{name} Detected unique blocking with tag {tag}");
                if (puppetMovement != null)
                {
                    puppetMovement.StopMovingAnimations();
                    puppetMovement.isBlockedByUnique = true;   
                }
                
                return true;
            }
        }

        if (puppetMovement != null)
            puppetMovement.isBlockedByUnique = false;

        return false;
    }
}
