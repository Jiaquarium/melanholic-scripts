using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_HandleSortingExceptions : MonoBehaviour
{
    static private int interval = 3;
    
    [SerializeField] private Directions directionToPlayer;
    [UnityEngine.Serialization.FormerlySerializedAs("stencil1Material")]
    [SerializeField] private Material myStencil;
    [UnityEngine.Serialization.FormerlySerializedAs("stencil2Material")]
    [SerializeField] private Material playerStencil;
    [SerializeField] private Transform myTransform;
    [SerializeField] private Vector3 myComparisonLoc;

    [Tooltip("Define the World Tile to save performance. Only make calls when Player is on the specified World Tile")]
    [SerializeField] private Script_WorldTile worldTile;
    private bool isMyWorldTile;
    
    private Material playerDefaultMaterial;
    private Material myDefaultMaterial;
    private Material playerStencilMaterial;
    private Material myStencilMaterial;
    private Dictionary<Directions, Vector3> DirectionsToVectorDict;
    private Renderer myRenderer;

    private bool isStencils;

    void Awake()
    {
        myRenderer = GetComponent<Renderer>();
    }
    
    void Start()
    {
        playerDefaultMaterial = Script_Game.Game.GetPlayer().MySharedMaterial;
        myDefaultMaterial = myRenderer.sharedMaterial;
        myStencilMaterial = myStencil;
        playerStencilMaterial = playerStencil;
        
        DirectionsToVectorDict = Script_Utils.GetDirectionToVectorDict();
    }

    void LateUpdate()
    {
        // Do only every interval frame
        if (Time.frameCount % interval == 0)
            HandleSortingMaterials();
    }

    private void HandleSortingMaterials()
    {
        if (directionToPlayer == Directions.None)
            return;

        // Check if current world tile is active to make calls
        if (worldTile != null)
        {
            isMyWorldTile = worldTile.WorldTilesController.OriginWorldTile == worldTile;
            if (!isMyWorldTile)
                return;
        }
        
        var player = Script_Game.Game.GetPlayer();
        myComparisonLoc = myTransform.position;
        
        // Test if Player is directly in that direction.
        if (
            player.location - myComparisonLoc == DirectionsToVectorDict[directionToPlayer]
            && !isStencils
        )
        {
            myRenderer.material = myStencilMaterial;
            player.MyMaterial = playerStencilMaterial;
            isStencils = true;
        }
        else if (
            player.location - myComparisonLoc != DirectionsToVectorDict[directionToPlayer]
            && isStencils
        )
        {
            myRenderer.material = myDefaultMaterial;
            player.MyMaterial = playerDefaultMaterial;
            isStencils = false;
        }
    }
}
