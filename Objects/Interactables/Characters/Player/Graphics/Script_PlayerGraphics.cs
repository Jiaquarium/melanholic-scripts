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
                graphics.material                                   = value;
            }
        }
    }

    public void ChangeMaterial(Materials materialType)
    {
        switch (materialType)
        {
            case (Materials.Unlit):
                graphics.material                                   = unlit;
                break;
            case (Materials.Lit):
                graphics.material                                   = lit;
                break;
            case (Materials.SimpleUnlit):
                graphics.material                                   = simpleUnlit;
                break;
        }
    }

    public bool IsMaterialTransparent()
    {
        // All materials where we are treating the Player material as a mesh
        // and so transparency will not affect it.
        bool noAlpha = PlayerGraphicsMaterial is Material lit
            || PlayerGraphicsMaterial is Material unlit
            || PlayerGraphicsMaterial is Material simpleUnlit;
        
        return !noAlpha;
    }

    public void SetHidden(bool isHidden)
    {
        Debug.Log($"Disabling graphics {name}");
        graphics.enabled = !isHidden;
    }
}
