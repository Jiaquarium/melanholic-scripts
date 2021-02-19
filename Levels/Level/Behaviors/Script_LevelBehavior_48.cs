using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_48 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    
    // ==================================================================

    [SerializeField] private Script_Snow snowEffectAlways;

    protected override void HandleAction()
    {
        base.HandleDialogueAction();
    }
    
    void Awake()
    {
        snowEffectAlways.gameObject.SetActive(true);
    }    
}