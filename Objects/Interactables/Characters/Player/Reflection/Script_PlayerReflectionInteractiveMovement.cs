using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handled interaction boxes by reflecting players' exactly
/// </summary>
[RequireComponent(typeof(Script_ReflectionCheckCollisions))]
[RequireComponent(typeof(Script_InteractionBoxController))]
public class Script_PlayerReflectionInteractiveMovement : Script_PlayerReflectionMovement
{
    [SerializeField] private Script_InteractionBoxController interactionBoxController;
    [SerializeField] private Script_InteractionBox[] interactionBoxes;
    [SerializeField] private Script_InteractionBox[] playerIBoxes;
    [SerializeField] private Vector3 iBoxOffset;

    private Dictionary<Directions, Vector3> directionToVector;
    
    void Awake()
    {
        interactionBoxes    = interactionBoxController.InteractionBoxes;
        directionToVector   = Script_Utils.GetDirectionToVectorDict();
    }

    void Update()
    {
        ReflectCopyPlayerIBoxTransforms();
    }

    /// <summary>
    /// Player will dictate the moves by checking CanMove()
    /// ActuallyMove() here will set the proper active interaction box
    /// and match/"reflect" the player's position
    /// </summary>
    protected override void ActuallyMove()
    {
        Directions myFacingDir = ToOppositeDirectionZ(player.FacingDirection);
        HandleActiveInteractionBox(myFacingDir);
        base.ActuallyMove();
    }

    public bool CanMove()
    {
        Directions myFacingDir  = ToOppositeDirectionZ(player.FacingDirection);
        Vector3 desiredMove     = directionToVector[myFacingDir];
        HandleActiveInteractionBox(myFacingDir);

        if (
            GetComponent<Script_CheckCollisions>().CheckCollisions(transform.position, myFacingDir, ref desiredMove)
        )
        {
            return false;
        }

        return true;
    }

    private void HandleActiveInteractionBox(Directions dir)
    {
        interactionBoxController.HandleActiveInteractionBox(dir);
    }

    private void ReflectCopyPlayerIBoxTransforms()
    {
        playerIBoxes = Script_Game.Game.GetPlayer()
            .interactionBoxController.InteractionBoxes;
        
        // player N is now reflection's S
        interactionBoxes[0].transform.position = GetReflectionPosition(playerIBoxes[2]
            .transform.position) + iBoxOffset;
        interactionBoxes[0].BoxSize = playerIBoxes[2].BoxSize;
        
        // & vise versa; player S is now reflection's N
        interactionBoxes[2].transform.position = GetReflectionPosition(playerIBoxes[0]
            .transform.position) + iBoxOffset;
        interactionBoxes[2].BoxSize = playerIBoxes[0].BoxSize;
        
        interactionBoxes[1].transform.position = GetReflectionPosition(playerIBoxes[1]
            .transform.position) + iBoxOffset;
        interactionBoxes[1].BoxSize = playerIBoxes[1].BoxSize;

        interactionBoxes[3].transform.position = GetReflectionPosition(playerIBoxes[3]
            .transform.position) + iBoxOffset;
        interactionBoxes[3].BoxSize = playerIBoxes[3].BoxSize;
    }
}
