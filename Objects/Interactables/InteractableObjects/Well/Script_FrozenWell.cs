using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_CrackableStats))]
[RequireComponent(typeof(AudioSource))]
public class Script_FrozenWell : Script_Well
{
    [SerializeField] private Script_CrackableStats crackableIceStats;
    [SerializeField] private Script_HurtBox hurtBox;
    [SerializeField] private Transform iceBlock;
    
    protected override void Awake()
    {
        base.Awake();

        InitialState();
    }
    
    // Freeze the Well and enable it to be shattered by Ice Spike attack.
    public void Freeze(bool isSFXOn = true)
    {
        // Animate freezing effect.
        iceBlock.gameObject.SetActive(true);

        crackableIceStats.enabled = true;
        hurtBox.gameObject.SetActive(true);

        if (isSFXOn)
        {
            GetComponent<AudioSource>().PlayOneShot(
                Script_SFXManager.SFX.Freeze, Script_SFXManager.SFX.FreezeVol
            );
        }
    }

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