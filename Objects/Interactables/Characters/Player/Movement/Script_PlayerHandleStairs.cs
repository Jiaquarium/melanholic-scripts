using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Script_PlayerHandleStairs : MonoBehaviour
{
    public Vector3? CheckStairsTilemaps(
        Vector3 loc,
        Directions dir,
        in Vector3 desiredMove
    )
    {
        Script_StairsTilemap[] stairsTilemaps = Script_Game.Game.StairsTileMaps;
        
        if (stairsTilemaps != null && stairsTilemaps.Length > 0)
        {
            Vector3 desiredDir = Script_Utils.GetDirectionToVectorDict()[dir];
            
            int desiredX = (int)Mathf.Round((loc + desiredDir).x);
            int desiredZ = (int)Mathf.Round((loc + desiredDir).z);

            foreach (var stairs in stairsTilemaps)
            {
                int stairsY = (int)stairs.transform.position.y;
                Vector3Int desiredTileWorldLocation = new Vector3Int(desiredX, stairsY, desiredZ);

                var tileMap = stairs.GetComponent<Tilemap>();
                Vector3Int tileLoc = tileMap.WorldToCell(desiredTileWorldLocation);
                
                if (tileMap.HasTile(tileLoc))
                {
                    float elevationChange = stairsY - loc.y;
                    
                    return new Vector3(
                        desiredMove.x,
                        desiredMove.y + elevationChange,
                        desiredMove.z
                    );
                }
            }
        }

        return null;
    }
}
