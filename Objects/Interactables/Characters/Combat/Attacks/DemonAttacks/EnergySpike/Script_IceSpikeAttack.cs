using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Player Spike Attack
/// spikes [0, 1, 2, 3] => [N, E, S, W] depending on how it's laid out in Inspector
/// </summary>
public class Script_IceSpikeAttack : Script_EnergySpikeAttack
{
    [SerializeField] private Script_Player player;
    [SerializeField] private Script_EnergySpike spikeN;
    [SerializeField] private Script_EnergySpike spikeE;
    [SerializeField] private Script_EnergySpike spikeS;
    [SerializeField] private Script_EnergySpike spikeW;
    
    public List<Script_EnergySpike> Spikes
    {
        get => new List<Script_EnergySpike>{
            spikeN,
            spikeE,
            spikeS,
            spikeW
        };
    }
    
    protected override void Awake()
    {
        base.Awake();
        HideSpikes();       
    }
    
    public override void Spike(Directions dir)
    {
        player.SetIsEffect();
        didHit = false;

        activeHitBox = GetHitBoxDirection(dir);
        
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

    // Compare with PlayerAttackEat.
    public override void CollisionedWith(Collider collider, Script_HitBox hitBox)
    {
        if (didHit)
        {
            activeHitBox.StopCheckingCollision();
            return;
        }
        
        Script_HurtBox hurtBox = collider.GetComponent<Script_HurtBox>();
        
        if (hurtBox != null)
        {
            int dmg = GetAttackStat().GetVal();
            Dev_Logger.Debug($"CollisionedWith with {hurtBox} inflicting dmg: {dmg}");
            
            int dmgActuallyGiven = hurtBox.Hurt(dmg, hitBox, hitBoxBehavior);
            if (dmgActuallyGiven > 0)
                HitSFX();
            
            didHit = true;

            if (hitBoxBehavior != null)
                hitBoxBehavior.Hit(collider);
        }
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
        foreach (Transform spike in spikes)
            spike.gameObject.SetActive(false);
    }

    // ------------------------------------------------------------------------------------
    // Timeline Signals Start
    public void EndPlayerIceSpikeFromTimeline()
    {
        Dev_Logger.Debug($"{name} EndPlayerIceSpikeFromTimeline() called from Timeline signal");
        
        HideSpikes();
        isInUse = false;

        player.SetIsInteract();

        base.EndAttack();
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
