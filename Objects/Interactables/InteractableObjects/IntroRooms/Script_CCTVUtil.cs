using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_TimelineController))]
[RequireComponent(typeof(PlayableDirector))]
// Note: PlayableDirector's wrap mode should be set to Loop for this behavior
public class Script_CCTVUtil : MonoBehaviour
{
    public static readonly int IsStaticAnimatorParam   = Animator.StringToHash("IsStatic");
    
    [SerializeField] private Animator cctvCpuAnimator;
    
    [Tooltip("The speaker referenced by the CCTVCPUStaticTimeline")]
    [SerializeField] private Script_ProximitySpeaker timelineSpeaker;

    private Script_TimelineController timelineController;

    void Awake()
    {
        timelineController = GetComponent<Script_TimelineController>();
    }
    
    public void PlayStaticTimeline()
    {
        timelineSpeaker.IsDisabled = false;
        timelineController.PlayableDirectorPlayFromTimelines(0, 0);
    }

    /// <summary>
    /// Only want SFX on first time to give lobby a more relaxing feel
    /// </summary>
    public void StopStaticSFX()
    {
        timelineSpeaker.IsDisabled = true;
        timelineSpeaker.Pause();
    }

    public void StopStaticTimeline()
    {
        StopStaticSFX();
        timelineController.StopAllPlayables();
    }
    
    /// <summary>
    /// Handle speaker state when game is not in Interact state
    /// </summary>
    public void SpeakerForceOnNonInteractState(bool isOn)
    {
        timelineSpeaker.IsForceOnNonInteractState = isOn;
    }

    private void IsStatic(bool isActive)
    {
        cctvCpuAnimator.SetBool(IsStaticAnimatorParam, isActive);
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    
    public void OnSFXDone()
    {
        Script_InteractableObjectEventsManager.CCTVSFXDone(this);
    }

    // ------------------------------------------------------------------

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_CCTVUtil))]
    public class Script_CCTVUtilTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_CCTVUtil t = (Script_CCTVUtil)target;
            if (GUILayout.Button("Start Static Timeline"))
            {
                t.PlayStaticTimeline();
            }

            if (GUILayout.Button("Stop Static Timeline"))
            {
                t.StopStaticTimeline();
            }
        }
    }
    #endif
}
