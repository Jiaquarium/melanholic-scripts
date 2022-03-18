using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_HideHUDOnPlayerLocation : MonoBehaviour
{
    [SerializeField] private Script_Marker marker;
    [SerializeField] private Script_Game game;
    
    void OnDisable()
    {
        game.IsHideHUD = false;
    }

    // Run this on Fixed Update because player / triggers work on physics clock. 
    void FixedUpdate()
    {
        if (game.GetPlayer().transform.position.z >= marker.Position.z)
            game.IsHideHUD = true;
        else
            game.IsHideHUD = false;
    }
}
