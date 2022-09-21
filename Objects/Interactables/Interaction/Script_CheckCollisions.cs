using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Script_CheckCollisions : MonoBehaviour
{
    [SerializeField] protected Script_InteractionBoxController interactionBoxController;
    [SerializeField] protected List<Const_Tags.Tags> UniqueBlockingTags;

    /// <summary>
    /// Check if allowed to move to desired grid tile
    /// </summary>
    /// <param name="desiredDirection"></param>
    /// <returns>true if collision (not allowed to move to that space)</returns>
    public bool CheckCollisions(Vector3 currentLocation, Directions dir, ref Vector3 desiredMove)
    {
        Vector3 desiredDirection = Script_Utils.GetDirectionToVectorDict()[dir];
        Vector3Int tileWorldLocation = (currentLocation + desiredDirection).ToVector3Int();
        
        bool isStairs = ModifyElevation(currentLocation, dir, ref desiredMove);
        if (isStairs)
            return false;

        if (CheckNotOffTilemap(tileWorldLocation))
        {
            Dev_Logger.Debug($"{name} Tilemap Collision at tileWorldLocation {tileWorldLocation}");
            return true;
        }

        if (CheckInteractableBlocking(dir))
        {
            Dev_Logger.Debug($"{name} Interactable Collision at dir {dir}");
            return true;
        }
        if (CheckPushableBlocking(dir))
        {
            Dev_Logger.Debug($"{name} Pushable Collision at dir {dir}");
            return true;
        }
        if (CheckUniqueBlocking(dir))
        {
            Dev_Logger.Debug($"{name} Unique Collision at dir {dir}");
            return true;
        }

        return false;
    }

    protected virtual bool CheckNotOffTilemap(Vector3Int tileWorldLocation)
    {
        Tilemap tileMap         = Script_Game.Game.TileMap;

        if (!tileMap.HasTile(tileWorldLocation))        return true;
        else                                            return false;
    }

    protected virtual bool ModifyElevation(Vector3 currentLoc, Directions directions, ref Vector3 desiredMove)
    {
        return false;
    }

    protected virtual bool CheckInteractableBlocking(Directions dir)
    {
        List<Script_Interactable> interactables = interactionBoxController.GetInteractablesBlocking(dir);
        return interactables.Count > 0;
    }

    // Meant to be overriden; Pushables care between difference of interactables and pushables
    // when they are in IgnoreInteractables mode
    protected virtual bool CheckPushableBlocking(Directions dir) { return false; }

    /// <summary>
    /// Use to specify if a certain Character should be blocked by particular obstacles, perhaps invisibile to others.
    /// </summary>
    protected virtual bool CheckUniqueBlocking(Directions dir)
    {
        foreach (var uniqueBlockingTag in UniqueBlockingTags)
        {
            string tag = Const_Tags.TagsMap[uniqueBlockingTag];
            
            List<Transform> uniqueBlocking = interactionBoxController.GetUniqueBlocking(dir, tag);
            
            if (uniqueBlocking.Count > 0)
            {
                Dev_Logger.Debug($"{name} Detected unique blocking with tag {tag}");
                return true;    
            }
        }

        return false;
    }
}
