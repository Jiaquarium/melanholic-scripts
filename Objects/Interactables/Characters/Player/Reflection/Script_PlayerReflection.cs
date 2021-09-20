using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_PlayerReflectionMovement))]
public class Script_PlayerReflection : MonoBehaviour
{
    public Vector3 axis;
    [Tooltip("Sets the reflection axis based on object; preferred as the object will move along with its parents")]
    public Transform axisObject;
    private Script_PlayerReflectionMovement reflectionMovement;
    private Script_Player player;
    [SerializeField] Transform graphics;
    
    void Update()
    {
        reflectionMovement.HandleMove();
    }

    public Directions ToOppositeDirectionZ(Directions desiredDir)
    {
        if      (desiredDir == Directions.Right)     return Directions.Right;
        else if (desiredDir == Directions.Left)      return Directions.Left;
        else if (desiredDir == Directions.Up)        return Directions.Down;
        else                                         return Directions.Up;
    }

    public void Setup(
        Script_Player _player,
        Vector3 _axis
    )
    {
        reflectionMovement = GetComponent<Script_PlayerReflectionMovement>();
        reflectionMovement.Setup(this, _player, _axis);
        
        player = _player;
        axis = axisObject == null ? axis = _axis : axis = axisObject.position;

        transform.position = reflectionMovement.GetReflectionPosition(player.transform.position);
    }
}
