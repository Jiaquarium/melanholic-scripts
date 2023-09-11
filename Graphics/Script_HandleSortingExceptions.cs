using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Script_HandleSortingExceptions : MonoBehaviour
{
    static private int interval = 3;
    
    [SerializeField] private Directions directionToPlayer;
    [SerializeField] protected Material myStencil;
    [SerializeField] protected Material playerStencil;
    [SerializeField] protected Transform myTransform;
    [Tooltip("Reference object if comparing to transform that is NOT player; otherwise, leave null")]
    [SerializeField] protected Transform playerOverride;
    [Tooltip("Reference renderer to change material of if using player override; otherwise, leave null")]
    [SerializeField] protected Renderer playerOverrideRenderer;

    [Tooltip("Define the World Tile to save performance. Only make calls when Player is on the specified World Tile")]
    [SerializeField] private Script_WorldTile worldTile;

    protected Vector3 myComparisonLoc;
    protected Vector3 playerLocation;
    protected Vector3 diffVector;
    private bool isMyWorldTile;
    
    protected Material playerDefaultMaterial;
    protected Material myDefaultMaterial;
    private Material playerStencilMaterial;
    private Material myStencilMaterial;
    private Dictionary<Directions, Vector3> DirectionsToVectorDict;
    protected Renderer myRenderer;

    protected bool isStencils;

    void Awake()
    {
        myRenderer = GetComponent<Renderer>();
    }
    
    protected virtual void Start()
    {
        if (playerOverride == null)
            playerDefaultMaterial = Script_Game.Game.GetPlayer().MySharedMaterial;
        else
            playerDefaultMaterial = playerOverrideRenderer.material;
        
        myDefaultMaterial = myRenderer.sharedMaterial;
        myStencilMaterial = myStencil;
        playerStencilMaterial = playerStencil;
        
        DirectionsToVectorDict = Script_Utils.GetDirectionToVectorDict();

#if UNITY_EDITOR
        DevCheckGraphics();
#endif
    }

    void LateUpdate()
    {
        // Do only every interval frame
        if (Time.frameCount % interval == 0)
            HandleSortingMaterials();
    }

    protected virtual void HandleSortingMaterials()
    {
        if (directionToPlayer == Directions.None)
            return;

        if (!IsOnMyWorldTile())
            return;
        
        Script_Player player = null;
        
        if (playerOverride == null)
        {
            player = Script_Game.Game.GetPlayer();
            playerLocation = player.location;
        }
        else
        {
            playerLocation = playerOverride.transform.position;
        }
        
        myComparisonLoc = myTransform.position;
        diffVector = playerLocation - myComparisonLoc;

        bool isPlayerInDirection = diffVector == DirectionsToVectorDict[directionToPlayer];
        
        // Test if Player is directly in that direction.
        if (isPlayerInDirection && !isStencils)
        {
            myRenderer.material = myStencilMaterial;
            SetPlayerMaterial(playerOverride, player, playerStencilMaterial);
            
            isStencils = true;
        }
        else if (!isPlayerInDirection && isStencils)
        {
            myRenderer.material = myDefaultMaterial;
            SetPlayerMaterial(playerOverride, player, playerDefaultMaterial);
            
            isStencils = false;
        }
    }

    protected void SetPlayerMaterial(Transform playerOverride, Script_Player player, Material mat)
    {
        if (playerOverride == null && player != null)
            player.MyMaterial = mat;
        else if (playerOverride != null)
            playerOverrideRenderer.material = mat;
    }

    /// <summary>
    /// If referencing World Tile, save performance by only handling sorting when on current World Tile
    /// </summary>
    /// <returns>True if should check sorting, False if should skip checking sorting</returns>
    protected bool IsOnMyWorldTile()
    {
        if (worldTile == null)
            return true;
        
        isMyWorldTile = worldTile.WorldTilesController.OriginWorldTile == worldTile;
        return isMyWorldTile;
    }

#if UNITY_EDITOR
    protected virtual void DevCheckGraphics()
    {
        Script_HandlePlayerGraphicsMaterial playerLevelGraphics = null;
        if (Script_Game.Game.levelBehavior != null)
        {
            playerLevelGraphics = Script_Game.Game.levelBehavior
                .GetComponent<Script_HandlePlayerGraphicsMaterial>();
        }
        
        bool isLitMaterialMap = playerLevelGraphics != null
            && playerLevelGraphics.LevelMaterial == Script_PlayerGraphics.Materials.Lit;
        var mats = isLitMaterialMap ? Script_GraphicsManager.Control.PlayerLitStencils
            : Script_GraphicsManager.Control.PlayerUnlitShadowsStencils;
        var materialType = isLitMaterialMap ? Script_PlayerGraphics.Materials.Lit
            : Script_PlayerGraphics.Materials.UnlitShadows;

        if (!DevCheckStencil(playerStencil, mats))
            Debug.LogWarning($"{name} Player stencil does not match -- {materialType}");
    }
    
    protected bool DevCheckStencil(Material mat, Material[] mats) => mats
        .FirstOrDefault(stencil => stencil == mat) != null;
#endif
}
