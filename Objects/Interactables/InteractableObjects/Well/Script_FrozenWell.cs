using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_CrackableStats))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Script_TimelineController))]
public class Script_FrozenWell : Script_Well
{
    [SerializeField] private Script_CrackableStats crackableIceStats;
    [SerializeField] private Script_HurtBox hurtBox;
    [SerializeField] private Transform iceBlock;

    private bool isSFXOn;
    private Script_TimelineController timelineController;
    
    protected override void Awake()
    {
        timelineController = GetComponent<Script_TimelineController>();
        
        base.Awake();

        InitialState();
    }
    
    // Freeze the Well and enable it to be shattered by Ice Spike attack.
    public void Freeze(bool _isSFXOn = true)
    {
        crackableIceStats.enabled = true;
        hurtBox.gameObject.SetActive(true);

        isSFXOn = _isSFXOn;

        // Freezing animation and set IceBlock active.
        timelineController.PlayableDirectorPlayFromTimelines(0, 0);
    }

    // ----------------------------------------------------------------------
    // Timeline Signals

    public void PlayFreezeSFX()
    {
        if (isSFXOn)
        {
            GetComponent<AudioSource>().PlayOneShot(
                Script_SFXManager.SFX.Freeze, Script_SFXManager.SFX.FreezeVol
            );
        }
    }

    // ----------------------------------------------------------------------

    // Turn into a normal well.
    public void InitialState()
    {
        iceBlock.gameObject.SetActive(false);
        crackableIceStats.enabled = false;
        hurtBox.gameObject.SetActive(false);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_FrozenWell))]
public class Script_FrozenWellTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_FrozenWell t = (Script_FrozenWell)target;
        if (GUILayout.Button("Freeze"))
        {
            t.Freeze();
        }
    }
}
#endif