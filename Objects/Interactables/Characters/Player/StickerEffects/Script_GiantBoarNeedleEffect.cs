using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(PlayableDirector))]
public class Script_GiantBoarNeedleEffect : Script_StickerEffect
{
    [SerializeField] private Script_Player player;
    [SerializeField] private Script_PlayerAction playerActionHandler;
    [SerializeField] private Script_InteractionBoxController interactionBoxController;
    [SerializeField] private TimelineAsset timeline;
    private PlayableDirector myDirector;

    void Awake()
    {
        myDirector = GetComponent<PlayableDirector>();
    }

    public override void Effect()
    {
        Debug.Log($"{name} Effect()");
        
        Script_InteractableObject[] objs = interactionBoxController.GetInteractableObject(player.FacingDirection);
        if (objs.Length == 0)    return;

        foreach (Script_InteractableObject obj in objs)
        {
            if (obj is Script_InteractablePaintingEntrance)
            {
                Debug.Log($"Detected Painting Entrance {obj.name}");
                var paintingEntrance = (Script_InteractablePaintingEntrance)obj;
                paintingEntrance.InitiatePaintingEntrance();
                myDirector.Play(timeline);
            }
        }
    }

    protected override void OnEquip()
    {
        base.OnEquip();
        OnEquipControllerSynced();
    }

    protected override void OnUnequip()
    {
        base.OnEquip();
        OnUnequipControllerSynced();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_GiantBoarNeedleEffect))]
public class Script_GiantBoarNeedleEffectTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_GiantBoarNeedleEffect bn = (Script_GiantBoarNeedleEffect)target;
        if (GUILayout.Button("Play Boar Needle Effect"))
        {
            bn.Effect();
        }
    }
}
#endif
