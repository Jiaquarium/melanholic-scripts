using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_HandlePlayerGraphicsMaterial : MonoBehaviour
{
    [SerializeField] private Script_PlayerGraphics.Materials levelMaterial;
    
    private Material startingMaterial;

    public Script_PlayerGraphics.Materials LevelMaterial => levelMaterial;
    
    void OnEnable()
    {
        try
        {
            var player = Script_Game.Game?.GetPlayer();
            
            if (player != null)
            {
                startingMaterial = player.MyMaterial;
                player.ChangeMaterial(levelMaterial);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }
    
    void OnDisable()
    {
        var player = Script_Game.Game?.GetPlayer();

        if (player != null)
            player.MyMaterial = startingMaterial;
    }
}
