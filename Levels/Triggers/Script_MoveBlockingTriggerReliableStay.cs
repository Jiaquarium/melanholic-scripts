using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_MoveBlockingTriggerReliableStay : Script_TriggerReliableStay
{
    [SerializeField] private Directions pushDirection;
    [SerializeField] private List<Script_Interactable> blockingInteractables;

    void OnEnable()
    {
        Script_PuzzlesEventsManager.OnClearDoorways += PushInteractables;
    }

    void OnDisable()
    {
        Script_PuzzlesEventsManager.OnClearDoorways -= PushInteractables;
    }    

    public void PushInteractables()
    {
        foreach (Script_Interactable interactable in blockingInteractables)
        {
            bool isDetectEverything = detectTags.FindIndex(tag => tag == DetectTags.Everything) != -1;
            // Results in DetectTags.None if none is found.
            DetectTags result = detectTags.FirstOrDefault(tag => HandleDetectionTag(tag, interactable.tag));

            if (isDetectEverything || !result.Equals(default(DetectTags)))
                interactable.ForcePush(pushDirection);
        }
    }
    
    protected override void OnEnter(Collider other)
    {
        var parent = other.transform.GetParentRecursive<Script_Interactable>();
        if (parent == null)     return;

        bool isNewObject = blockingInteractables.Find(obj => obj == parent) == null;
        if (isNewObject)    blockingInteractables.Add(parent);
    }

    protected override void OnExit(Collider other)
    {
        var parent = other.transform.GetParentRecursive<Script_Interactable>();
        if (parent == null)     return;

        blockingInteractables.Remove(parent);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_MoveBlockingTriggerReliableStay))]
public class Script_MoveBlockingTriggerReliableStayTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_MoveBlockingTriggerReliableStay t = (Script_MoveBlockingTriggerReliableStay)target;
        if (GUILayout.Button("Push Interactables"))
        {
            t.PushInteractables();
        }
    }
}
#endif