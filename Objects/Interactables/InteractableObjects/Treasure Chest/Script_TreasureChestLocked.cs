using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_UsableKeyTarget))]
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

    protected override void ActionDefault()
    {
        if (IsLocked)
        {
            var myKey = GetComponent<Script_UsableKeyTarget>().MyKey;
            
            if (game.TryUseKey(myKey))  
            {
                UnlockWithKey();
                Script_ItemsEventsManager.Unlock(myKey, myKey.id);
            }

            return;
        }
        
        base.ActionDefault();
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
}
