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

    public bool IsLocked
    {
        get => _isLocked;
        private set => _isLocked = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (IsOpen)     IsLocked = false;
    }

    public void UnlockWithKey()
    {
        ActionDefault();
    }

    protected override void ActionDefault()
    {
        if (CheckDisabledDirections())
        {
            Debug.Log($"{name}: Action default from Disabled Direction");
            return;
        }
        
        if (IsLocked)
        {
            var myKey = GetComponent<Script_UsableKeyTarget>().MyKey;
            
            if (game.TryUseKey(myKey) && !IsOpen)
            {
                // If there is an item, Item receive will play its SFX instead.
                if (IsEmpty)
                {
                    var sfx = Script_SFXManager.SFX;
                    audioSource.PlayOneShot(sfx.useKey, sfx.useKeyVol);
                }
                else
                    Script_Game.Game.HandleItemReceive(item);

                IsOpen = true;
                IsLocked = false;

                Script_ItemsEventsManager.Unlock(myKey, myKey.id);
            }

            return;
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
}
