using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerGraphics : MonoBehaviour
{
    public enum Materials
    {
        Unlit       = 0,
        Lit         = 1,
        SimpleUnlit = 2
    }
    
    [SerializeField] private Script_Player player;
    [SerializeField] private Renderer graphics;
    [SerializeField] private Material lit;
    [SerializeField] private Material unlit;
    [SerializeField] private Material simpleUnlit;

    public Material PlayerGraphicsMaterial
    {
        get => graphics.material;
        set
        {
            if (graphics != null)
            {
                graphics.material           = value;
                player.GetPlayerGhost().spriteRenderer.material     = value;
            }
        }
    }

    public void ChangeMaterial(Materials materialType)
    {
        switch (materialType)
        {
            case (Materials.Unlit):
                graphics.material                                   = unlit;
                player.GetPlayerGhost().spriteRenderer.material     = unlit;
                break;
            case (Materials.Lit):
                graphics.material                                   = lit;
                player.GetPlayerGhost().spriteRenderer.material     = lit;
                break;
            case (Materials.SimpleUnlit):
                graphics.material                                   = simpleUnlit;
                player.GetPlayerGhost().spriteRenderer.material     = simpleUnlit;
                break;
        }
    }
}
