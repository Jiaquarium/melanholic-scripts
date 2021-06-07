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
    [SerializeField] private bool isDone;
    // ==================================================================

    [SerializeField] private Script_Snow snowEffectAlways;

    public bool IsDone
    {
        get => isDone;
        set => isDone = value;
    }

    void Awake()
    {
        snowEffectAlways.gameObject.SetActive(true);
    }    
}