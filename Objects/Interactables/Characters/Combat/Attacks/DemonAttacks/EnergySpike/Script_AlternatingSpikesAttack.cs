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
/// Used in Eileen's Mind.
/// </summary>
public class Script_AlternatingSpikesAttack : Script_EnergySpikeAttack
{
    [SerializeField] private Transform spikeParentsParent;
    [SerializeField] private Script_EnergySpikeParent[] spikeParents;
    [SerializeField] private PlayableDirector director;
    
    protected override void OnValidate()
    {
        // Only need reference to hitboxes
        Script_EnergySpike[] spikeObjs = GetSpikeChildren();
        
        spikes = new Transform[0];
        hitBoxes = new Script_HitBox[spikeObjs.Length];
        
        for (int i = 0; i < spikeObjs.Length; i++)
        {
            hitBoxes[i] = spikeObjs[i].hitBox;
        }
    }
    
    public void AlternatingSpikes()
    {
        didHit = false;
        if (isInUse)     return;
        isInUse = true;

        SpikeSequence();
    }

    protected override void SpikeSequence()
    {
        SetHitBoxes();
        SpikesSFX();
        
        director.Play();
        
        StartCoroutine(WaitToEndSpike());
    }

    public override void CollisionedWith(Collider collider, Script_HitBox hitBox)
    {
        Script_HurtBox hurtBox = collider.GetComponent<Script_HurtBox>();
        if (hurtBox != null && !didHit)
        {
            int dmg = GetAttackStat().GetVal();
            Dev_Logger.Debug($"CollisionedWith with {hurtBox} inflicting dmg: {dmg}");
            
            /// Only hit if did damage
            if (hurtBox.Hurt(dmg, hitBox) > 0)
            {
                HitSFX();
                didHit = true;
            }

            if (hitBoxBehavior != null)
                hitBoxBehavior.Hit(collider);
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