using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_49 : Script_LevelBehavior
{
    [SerializeField] private Script_LevelAttackController attackController;
    
    protected override void Update()
    {
        attackController.AttackTimer(false);
    }
}