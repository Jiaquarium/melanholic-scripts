using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_HandlePlayerGraphicsMaterial : MonoBehaviour
{
    [SerializeField] private Script_PlayerGraphics.Materials levelMaterial;
    
    private Material startingMaterial;
    
    void OnEnable()
    {
        var player = Script_Game.Game.GetPlayer();
        
        startingMaterial = player.MyMaterial;
        player.ChangeMaterial(levelMaterial);
    }
    
    void OnDisable()
    {
        var player = Script_Game.Game.GetPlayer();

        player.MyMaterial = startingMaterial;
    }
}
