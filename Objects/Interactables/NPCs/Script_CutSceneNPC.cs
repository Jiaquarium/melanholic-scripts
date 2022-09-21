using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    cut scene NPCs just show up for cut scene and disappear afterwards
*/
public class Script_CutSceneNPC : Script_StaticNPC
{
    private static readonly int IsFrozen = Animator.StringToHash("isFrozen");
    
    public int CutSceneNPCId;

    public override void Freeze(bool isFrozen)
    {
        rendererChild.GetComponent<Animator>().SetBool(IsFrozen, isFrozen);
    }
    
    public override void Setup
    ()
    {
        base.Setup();
    }
}
