using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_MaskEffectsDirectorManager : MonoBehaviour
{
    public enum Effects
    {
        MyMask = 8
    }
    
    public static Script_MaskEffectsDirectorManager Instance;

    [SerializeField] private Script_TimelineController timelineController;
    [SerializeField] private List<GameObject> myMaskInitialBindings;
    [SerializeField] private bool isMyMaskMutationOff;

    [SerializeField] private Script_Game game;

    public bool IsMyMaskMutationOff
    {
        get => isMyMaskMutationOff;
        set => isMyMaskMutationOff = value;
    }

    public bool IsForceSheepFaceDirection { get; set; }

    public void PlayMyMaskEffect(List<GameObject> bindings)
    {
        int myMaskIdx = (int)Effects.MyMask;
        int directorIdx = 0;

        TimelineAsset myMaskTimeline = timelineController.timelines[myMaskIdx];
        PlayableDirector director = timelineController.playableDirectors[directorIdx];
        
        var myMaskBindings = new List<GameObject>();

        // Leave initial bindings as null to retain Timeline bindings
        myMaskBindings.AddRange(myMaskInitialBindings);
        myMaskBindings.AddRange(bindings);
        
        director.BindTimelineTracks(myMaskTimeline, myMaskBindings, true);
        timelineController.PlayableDirectorPlayFromTimelines(directorIdx, myMaskIdx);
    }

    // ------------------------------------------------------------------
    // Timeline Signals

    public void MyMaskEquipEffectTimeline()
    {
        game.GetPlayer().MyMaskEquipEffectTimeline();
    }

    public void MyMaskEffectTimelineDone()
    {
        game.ChangeStateInteract();
    }

    public void ForceSheepFaceDirection()
    {
        IsForceSheepFaceDirection = true;
    }

    // ------------------------------------------------------------------
    
    public void Setup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_MaskEffectsDirectorManager))]
    public class Script_MaskEffectsDirectorManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_MaskEffectsDirectorManager t = (Script_MaskEffectsDirectorManager)target;
            if (GUILayout.Button("Equip Effect Timeline"))
            {
                t.MyMaskEquipEffectTimeline();
            }
        }
    }
    #endif
}
