using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_UsableKeyTarget))]
public class Script_TreasureChestLocked : Script_TreasureChest
{
    static private float WaitForTreasureChestOpenTimelineTime = 2.15f;

    [SerializeField] private bool _isLocked = true;
    
    [Tooltip("Dialogue object to react to object when locked.")]
    [SerializeField] private Script_InteractableObjectText objectText;
    [SerializeField] private PlayableDirector director;

    public bool IsLocked
    {
        get => _isLocked;
        private set => _isLocked = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (IsOpen)
            IsLocked = false;
        
        if (objectText != null)
        {
            // Ensure the object text is disabled, so it doesn't react to interaction events.
            objectText.SetInteractionActive(false);

            // Warn if text object is on the same object to avoid it eating up interactions meant for this.
            if (objectText.GetComponent<Script_TreasureChestLocked>() != null)
                Debug.LogWarning("You need to move object text further down the hierarchy to avoid handling actions");
        }
    }

    // Via Inventory UI
    public void UnlockWithKey()
    {
        ActionDefault();
    }

    // Via Interact (and Inventory UI)
    protected override void ActionDefault()
    {
        if (CheckDisabledDirections())
        {
            Dev_Logger.Debug($"{name}: Action default from Disabled Direction");
            return;
        }
        
        if (IsLocked)
        {
            var myKey = GetComponent<Script_UsableKeyTarget>().MyKey;
            
            if (!IsOpen && game.TryUseKey(myKey))
            {
                // If there is an item, Item receive will play its SFX instead.
                if (IsEmpty)
                {
                    var sfx = Script_SFXManager.SFX;
                    audioSource.PlayOneShot(sfx.useKey, sfx.useKeyVol);
                    OnUnlockHandleState();
                }
                else
                {
                    Script_Game.Game.GetPlayer().SetIsStandby();
                    StartCoroutine(WaitToShowItemPickUp());
                }

                IEnumerator WaitToShowItemPickUp()
                {
                    director.Play();
                    
                    // Must wait at least as long as TreasureChestHalfOpenTimeline
                    yield return new WaitForSeconds(WaitForTreasureChestOpenTimelineTime);

                    Script_Game.Game.HandleItemReceive(item);
                    OnUnlockHandleState();
                }

                void OnUnlockHandleState()
                {
                    IsOpen = true;
                    IsLocked = false;
                    Script_ItemsEventsManager.Unlock(myKey, myKey.id);
                }
            }
            else if (!IsOpen && !game.TryUseKey(myKey))
            {
                if (objectText != null)
                    objectText.ForceAction();
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
