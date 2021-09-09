using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// spikes [0, 1, 2, 3] => [N, E, S, W] depending on how it's laid out in Inspector
/// </summary>
public class Script_IceSpikeAttack : Script_EnergySpikeAttack
{
    [SerializeField] private Script_Player player;
    [SerializeField] private Script_EnergySpike spikeN;
    [SerializeField] private Script_EnergySpike spikeE;
    [SerializeField] private Script_EnergySpike spikeS;
    [SerializeField] private Script_EnergySpike spikeW;
    
    protected override void Awake()
    {
        base.Awake();
        HideSpikes();       
    }
    
    public override void Spike(Directions dir)
    {
        player.SetIsEffect();
        didHit = false;
        
        switch (dir)
        {
            case (Directions.Up):
                spikeN.gameObject.SetActive(true);
                break;
            case (Directions.Right):
                spikeE.gameObject.SetActive(true);
                break;
            case (Directions.Down):
                spikeS.gameObject.SetActive(true);
                break;
            case (Directions.Left):
                spikeW.gameObject.SetActive(true);
                break;
        }

        SpikeSequence();
    }

    /// <summary>
    /// Uses the Ice Spike timeline to call when to end spike instead of by coroutine and timer
    /// which is inexact, which is fine for enemies but not the player
    /// </summary>
    protected override void SpikeSequence()
    {
        SetHitBoxes();
        SpikesSFX();
        ResetSpikesElevation();
        GetComponent<Script_TimelineController>().PlayAllPlayables();
    }

    private void HideSpikes()
    {
        foreach (Transform spike in spikes) spike.gameObject.SetActive(false);
    }

    // ------------------------------------------------------------------------------------
    // Timeline Signals Start
    public void EndPlayerIceSpikeFromTimeline()
    {
        Debug.Log($"{name} EndPlayerIceSpikeFromTimeline() called from Timeline signal");
        
        HideSpikes();
        isInUse = false;

        player.SetIsInteract();
    }
    // Timeline Signals End
    // ------------------------------------------------------------------------------------
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_IceSpikeAttack))]
public class Script_IceSpikeAttackTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_IceSpikeAttack attack = (Script_IceSpikeAttack)target;
        if (GUILayout.Button("Spike(Up)"))
        {
            attack.Spike(Directions.Up);
        }
    }
}
#endif
