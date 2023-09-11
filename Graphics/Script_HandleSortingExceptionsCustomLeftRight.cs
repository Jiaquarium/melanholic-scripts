using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_HandleSortingExceptionsCustomLeftRight : Script_HandleSortingExceptions
{
    [Header("Custom Left or Right Only")]
    
    [SerializeField] private Material playerFrontStencil;
    [SerializeField] private Material playerBehindStencil;
    [SerializeField] private Material myFrontStencil;
    [SerializeField] private Material myBehindStencil;
    [Tooltip("Z bounds when player is right of target (use stencil front). Positive value means player is farther up.")]
    [SerializeField] private Vector2 playerRightZBounds;
    [Tooltip("X bounds when player is right of target (use stencil front). Positive value means player is farther right.")]
    [SerializeField] private Vector2 playerRightXBounds;
    [Tooltip("Z bounds when player is left of target (use stencil behind). Positive value means player is farther up.")]
    [SerializeField] private Vector2 playerLeftZBounds;
    [Tooltip("Z bounds when player is left of target (use stencil behind). Positive value means player is farther right.")]
    [SerializeField] private Vector2 playerLeftXBounds;

    // When this is true, only need playerFrontStencil, myBehindStencil, and playerRightBounds, since not checking left
    // direction, which is the only case that will force sort player behind
    [Tooltip("Only check for bounds on right side of myTransform. When true, if player on L side, will set to default.")]
    [SerializeField] private bool isOnlyCheckRight;

    bool isPlayerInFront;
    
    // Start is called before the first frame update
    protected override void Start()
    {
#if UNITY_EDITOR
        if (myStencil != null || playerStencil != null)
            Debug.LogError($"{name} Only define custom stencil properties when using custom Left Right sorting");
#endif
        
        base.Start();
    }

    protected override void HandleSortingMaterials()
    {
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
        bool isPlayerInDirection;

        // If player is directly in line with target, use default material
        if (diffVector.x == 0f)
        {
            isPlayerInDirection = false;   
        }
        // Determine whether to test R or L
        else if (diffVector.x > 0f)
        {
            // Handle player on R
            bool isZEqualOrApproxPast = diffVector.z >= playerRightZBounds.x
                && diffVector.z <= playerRightZBounds.y;
            bool isXApproxAdjacent = diffVector.x >= playerRightXBounds.x
                && diffVector.x <= playerRightXBounds.y;
            
            isPlayerInDirection = isZEqualOrApproxPast && isXApproxAdjacent;
            isPlayerInFront = true;
        }
        else
        {
            if (isOnlyCheckRight)
            {
                // If only checking right, handle player on L by ignoring further calcs, use default material
                isPlayerInDirection = false;
            }
            else
            {
                // Handle player on L
                bool isZEqualOrApproxPast = diffVector.z >= playerLeftZBounds.x
                    && diffVector.z <= playerLeftZBounds.y;
                bool isXApproxAdjacent = diffVector.x >= playerLeftXBounds.x
                    && diffVector.x <= playerLeftXBounds.y;
                
                isPlayerInDirection = isZEqualOrApproxPast && isXApproxAdjacent;
                isPlayerInFront = false;
            }
        }
        
        // Handle if Player is in direction that needs sorting adjustment.
        if (isPlayerInDirection && !isStencils)
        {
            var myMaterial = isPlayerInFront ? myBehindStencil : myFrontStencil;
            var playerMaterial = isPlayerInFront ? playerFrontStencil : playerBehindStencil;
            
            myRenderer.material = myMaterial;
            SetPlayerMaterial(playerOverride, player, playerMaterial);
            
            isStencils = true;
        }
        else if (!isPlayerInDirection && isStencils)
        {
            myRenderer.material = myDefaultMaterial;
            SetPlayerMaterial(playerOverride, player, playerDefaultMaterial);
            
            isStencils = false;
        }
    }

#if UNITY_EDITOR
    protected override void DevCheckGraphics()
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
        
        bool isFail = (playerFrontStencil != null && !DevCheckStencil(playerFrontStencil, mats))
            || (playerBehindStencil != null && !DevCheckStencil(playerBehindStencil, mats));

        if (isFail)
            Debug.LogWarning($"{name} Player stencil does not match -- {materialType}");
    }
#endif
}
