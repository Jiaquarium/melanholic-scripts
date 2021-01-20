using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_TreasureChestLocked : Script_TreasureChest
{
    [SerializeField] private bool _isLocked = true;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (IsOpen)     IsLocked = false;
    }
    
    public bool IsLocked
    {
        get => _isLocked;
        private set => _isLocked = value;
    }

    public void UnlockWithKey()
    {
        IsLocked = false;
        ActionDefault();
    }

    public override void ActionDefault()
    {
        if (IsLocked)   return;
        
        base.ActionDefault(); // calls one level up in inheritance chain
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_TreasureChestLocked))]
public class Script_TreasureChestLockedTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_TreasureChestLocked t = (Script_TreasureChestLocked)target;
        if (GUILayout.Button("ActionDefault()"))
        {
            t.ActionDefault();
        }
    }
}
#endif