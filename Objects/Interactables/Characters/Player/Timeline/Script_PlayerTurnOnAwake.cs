using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activate this as Activation Track from timeline to make player face directions
/// </summary>
public class Script_PlayerTurnOnAwake : MonoBehaviour
{
    public Directions faceDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        Script_Game.Game.GetPlayer().FaceDirection(faceDirection);        
    }
}
