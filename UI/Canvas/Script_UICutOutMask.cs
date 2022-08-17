using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Script_UICutOutMask : Image
{
    public const string StencilCompId = "_StencilComp";
    
    public override Material materialForRendering
    {
        get
        {
            Material material = new Material(base.materialForRendering);
            material.SetInt(StencilCompId, (int)CompareFunction.NotEqual);
            return material;
        }
    }
}
