using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Script_CheckCollisions : MonoBehaviour
{
    [SerializeField] protected Script_InteractionBoxController interactionBoxController;

    /// <summary>
    /// 
    /// check if allowed to move to desired grid tile
    /// </summary>
    /// <param name="desiredDirection"></param>
    /// <returns>true if collision (not allowed to move to that space)</returns>
    public bool CheckCollisions(Vector3 currentLocation, Directions dir)
    {
        Vector3 desiredDirection = Script_Utils.GetDirectionToVectorDict()[dir];

        int desiredX = (int)Mathf.Round((currentLocation + desiredDirection).x);
        int desiredY = (int)Mathf.Round((currentLocation + desiredDirection).y);
        int desiredZ = (int)Mathf.Round((currentLocation + desiredDirection).z);

        // allow grid offsets to affect
        int adjustedDesiredX = desiredX; // - (int)Script_Game.Game.grid.transform.position.x;
        int adjustedDesiredY = desiredY;
        int adjustedDesiredZ = desiredZ; // - (int)Script_Game.Game.grid.transform.position.z;
        
        // Vector3Int tileWorldLocation = new Vector3Int(adjustedDesiredX, adjustedDesiredZ, 0);
        Vector3Int tileWorldLocation = new Vector3Int(adjustedDesiredX, adjustedDesiredY, adjustedDesiredZ);
        
        if (CheckNotOffTilemap(desiredX, desiredZ, tileWorldLocation))      return true;
        if (CheckInteractableBlocking(dir))                                 return true;
        if (CheckPushableBlocking(dir))                                     return true;

        return false;
    }

    protected virtual bool CheckNotOffTilemap(int desiredX, int desiredZ, Vector3Int tileWorldLocation)
    {
        Tilemap tileMap         = Script_Game.Game.GetTileMap();

        if (!tileMap.HasTile(tileWorldLocation))        return true;
        else                                            return false;
    }

    protected virtual bool CheckInteractableBlocking(Directions dir)
    {
        List<Script_Interactable> interactables = interactionBoxController.GetInteractablesBlocking(dir);
        return interactables.Count > 0;
    }

    /// Meant to be overriden; Pushables care between difference of interactables and pushables
    /// when they are in IgnoreInteractables mode
    protected virtual bool CheckPushableBlocking(Directions dir) { return false; }
}
