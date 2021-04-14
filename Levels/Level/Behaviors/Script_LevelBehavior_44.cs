using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_44 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    
    // ==================================================================

    [SerializeField] private Script_InteractablePaintingEntrance[] paintingEntrances;   

    public void PaintingsDone()
    {
        foreach (var painting in paintingEntrances)
        {
            painting.DonePainting();
        }
    }
    
    public override void Setup()
    {
        base.Setup();
    }
}