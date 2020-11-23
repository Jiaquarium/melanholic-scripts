using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_SpecterMovement))]
public class Script_SpecterBehavior_StaticFacePlayer : Script_SpecterBehavior
{
    private enum TurnSpeeds {
        None,
        Slow,
        Med,
        Fast
    }
    public static float TurnSpeedSlow = 1f;
    public static float TurnSpeedMed = 0.5f;
    public static float TurnSpeedFast = 0.25f;
    [SerializeField] private TurnSpeeds turnSpeed;
    [SerializeField] private float turnDelay;
    private Directions facingDirection;
    private Coroutine facingCoroutine;

    void Awake()
    {
        switch (turnSpeed)
        {
            case (TurnSpeeds.Slow):
                turnDelay = TurnSpeedSlow;
                break;
            case (TurnSpeeds.Med):
                turnDelay = TurnSpeedMed;
                break;
            case (TurnSpeeds.Fast):
                turnDelay = TurnSpeedFast;
                break;
            default:
                turnDelay = 0f;
                break;
        }
    }
    
    void Start()
    {
        GetComponent<Script_SpecterMovement>().FacePlayer();
    }
    
    void Update()
    {
        DelayedFacePlayer();
    }

    /// <summary>
    /// Start a coroutine to face the player
    /// </summary>
    private void DelayedFacePlayer()
    {
        if (turnDelay == 0f)
        {
            GetComponent<Script_SpecterMovement>().FacePlayer();
            return;
        }
        
        Directions newFacingDirection = GetComponent<Script_SpecterMovement>().GetDirectionToPlayer();
        
        if (newFacingDirection != facingDirection)
        {
            facingDirection = newFacingDirection;
            if (facingCoroutine != null)    StopCoroutine(facingCoroutine);
            facingCoroutine = StartCoroutine(WaitToFacePlayer());
        }

        IEnumerator WaitToFacePlayer()
        {
            yield return new WaitForSeconds(turnDelay);
            GetComponent<Script_SpecterMovement>().FaceDirection(facingDirection);
        }
    }
}
