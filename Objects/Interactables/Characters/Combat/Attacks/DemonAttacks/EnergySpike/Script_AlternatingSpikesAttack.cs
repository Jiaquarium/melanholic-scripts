using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Playables;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Each row of spikes has unique hitBoxId (e.g. eileen-mind_energy-spikeXX)
/// </summary>
public class Script_AlternatingSpikesAttack : Script_EnergySpikeAttack
{
    [SerializeField] private Transform spikeParentsParent;
    [SerializeField] private Script_EnergySpikeParent[] spikeParents;

    protected override void OnValidate()
    {
        spikeParents = spikeParentsParent.transform.GetChildren<Script_EnergySpikeParent>();
        
        base.OnValidate();

        for (int i = 0; i < spikeParents.Length; i++)
        {
            Script_EnergySpike[] spikeChildren = spikeParents[i].transform.GetChildren<Script_EnergySpike>();
            foreach (Script_EnergySpike spikeChild in spikeChildren)
            {
                spikeChild.hitBox.Id = string.IsNullOrEmpty(hitBoxId) ? spikeParents[i].hitBoxId : hitBoxId;
            }
        }
    }
    
    protected override Script_EnergySpike[] GetSpikeChildren()
    {
        Script_EnergySpike[] spikes = new Script_EnergySpike[]{};
        foreach (Script_EnergySpikeParent spikeParent in spikeParents)
        {
            Script_EnergySpike[] childrenSpikes = spikeParent.transform.GetChildren<Script_EnergySpike>();
            spikes = spikes.Concat(childrenSpikes).ToArray();
        }

        return spikes;
    }
    
    public void AlternatingSpikes()
    {
        didHit = false;
        if (isInUse)     return;
        isInUse = true;

        SpikeSequence();
    }

    public override void CollisionedWith(Collider collider, Script_HitBox hitBox)
    {
        Script_HurtBox hurtBox = collider.GetComponent<Script_HurtBox>();
        if (hurtBox != null && !didHit)
        {
            int dmg = GetAttackStat().GetVal();
            print($"CollisionedWith with {hurtBox} inflicting dmg: {dmg}");
            
            /// Only hit if did damage
            if (hurtBox.Hurt(dmg, hitBox) > 0)
            {
                HitSFX();
                didHit = true;
            }


            if (hitBoxBehavior != null)     hitBoxBehavior.Hit(collider);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_AlternatingSpikesAttack))]
public class Script_AlternatingSpikesAttackTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_AlternatingSpikesAttack attack = (Script_AlternatingSpikesAttack)target;
        if (GUILayout.Button("InitialState()"))
        {
            attack.InitialState();
        }
    }
}
#endif