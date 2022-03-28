using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// TIME: 1s
/// Acts as controller / manager for attack
/// SET activeHitBox MANUALLY FOR THIS ATTACK
/// this acts as the manager for the energy spikes
/// 
/// Will set all hitBoxes under this attack with its hitBoxId, or if empty
/// the hitBoxParent id (in case we want to specify different hitboxes under a single atk)
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
public class Script_EnergySpikeAttack : Script_Attack
{
    /// IMPORTANT: MAKE SURE THIS MATCHES THE TIMELINE LENGTH FOR ACCURATE ENDATTACK EVENTS
    [SerializeField] private float spikeTime;
    [SerializeField] protected Transform[] spikes;
    [SerializeField] protected Script_HitBox[] hitBoxes;
    [SerializeField] private bool noSFXWhenTalking;
    [SerializeField] protected bool isInUse;

    [Tooltip("Specify an alternate speaker to play Hit SFX (e.g. spike SFX is proximity effected but Hit SFX is not")]
    [SerializeField] private AudioSource hitSpeaker;

    private Grid attackGrid;
    protected bool didHit;
    
    protected virtual void OnValidate()
    {
        Script_EnergySpike[] spikeObjs = GetSpikeChildren();
        
        spikes = new Transform[spikeObjs.Length];
        hitBoxes = new Script_HitBox[spikeObjs.Length];
        
        GetComponent<Script_TimelineController>().playableDirectors.Clear();
        
        for (int i = 0; i < spikeObjs.Length; i++)
        {
            spikes[i]   = spikeObjs[i].transform;
            hitBoxes[i] = spikeObjs[i].hitBox;
            hitBoxes[i].Id = hitBoxId;
            GetComponent<Script_TimelineController>().playableDirectors.Add(
                spikeObjs[i].GetComponent<PlayableDirector>()
            );
        }
    }

    protected virtual Script_EnergySpike[] GetSpikeChildren()
    {
        return this.transform.GetComponentsInChildren<Script_EnergySpike>(includeInactive: true);
    }

    private void OnDisable()
    {
        InitialState();
    }

    private void OnEnable()
    {
        InitialState();    
    }
    
    protected virtual void Awake()
    {
        // ShowSpikes(false);
    }

    protected void SetHitBoxes()
    {
        foreach (Script_HitBox hBox in hitBoxes)
        {
            hBox.SetResponder(this);
            hBox.StartCheckingCollision();
        }
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

            if (hitBoxBehavior != null)
                hitBoxBehavior.Hit(collider);
        }
    }

    /// <summary>
    /// Position and activate the spike
    /// </summary>
    public void RandomSpikes(
        List<Vector3Int> attackTileLocs,
        Tilemap tilemap,
        Grid _attackGrid
    )
    {
        didHit = false;
        attackGrid = _attackGrid;
        if (isInUse)     return;
        isInUse = true;

        if (Debug.isDebugBuild && Const_Dev.IsDevMode)  attackGrid.gameObject.SetActive(true);
        
        // get all available attack tilemap positions
        attackTileLocs.Clear();
        attackTileLocs = Script_Utils.AddTileLocs(attackTileLocs, tilemap, null);

        // set each spike to a random position to attack
        for (int i = 0; i < spikes.Length; i++)
        {
            int randomIdx = Random.Range(0, attackTileLocs.Count);
            Vector3Int tileToAttackLoc = attackTileLocs[randomIdx];
            // this is what is affecting Timeline to not offset correctly
            Vector3 spikeAttackPos = attackGrid.CellToWorld(tileToAttackLoc);
            spikes[i].position = spikeAttackPos;

            attackTileLocs.RemoveAt(randomIdx);
        }
        
        SpikeSequence();
    }

    public virtual void Spike(Directions dir) {}

    protected void SpikesSFX()
    {
        if (noSFXWhenTalking && Script_Game.Game.GetPlayer().State == Const_States_Player.Dialogue)
            return;

        GetComponent<AudioSource>().PlayOneShot(
            Script_SFXManager.SFX.EnergySpikeAttack,
            Script_SFXManager.SFX.EnergySpikeAttackVol
        );
    }

    protected override void HitSFX()
    {
        AudioSource hitAudioSource;

        if (hitSpeaker != null)
            hitAudioSource = hitSpeaker;
        else
            hitAudioSource = GetComponent<AudioSource>();
        
        hitAudioSource.PlayOneShot(
            Script_SFXManager.SFX.EnergySpikeHurt,
            Script_SFXManager.SFX.EnergySpikeHurtVol
        );
    }

    protected IEnumerator WaitToEndSpike()
    {
        yield return new WaitForSeconds(spikeTime);
        EndSpike();
    }

    protected virtual void EndSpike()
    {
        // ShowSpikes(false);
        Script_CombatEventsManager.EnemyAttackEnd(hitBoxId);
        isInUse = false;

        if (Debug.isDebugBuild && Const_Dev.IsDevMode)
        {
          if (attackGrid != null) attackGrid.gameObject.SetActive(true);
        }
    }

    protected void ResetSpikesElevation()
    {
        foreach (Transform spike in spikes)
        {
            spike.position = new Vector3(spike.position.x, 0f, spike.position.z);
        }
    }

    protected virtual void SpikeSequence()
    {
        SetHitBoxes();
        SpikesSFX();
        // animation to move spikes up
        ResetSpikesElevation();
        GetComponent<Script_TimelineController>().PlayAllPlayables();

        StartCoroutine(WaitToEndSpike());
    }

    public void InitialState()
    {
        isInUse = false;
        GetComponent<Script_TimelineController>().StopAllPlayables();
        ResetSpikesElevation();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_EnergySpikeAttack))]
public class Script_EnergySpikeAttackTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_EnergySpikeAttack attack = (Script_EnergySpikeAttack)target;
        if (GUILayout.Button("InitialState()"))
        {
            attack.InitialState();
        }
        if (GUILayout.Button("Spike(Up)"))
        {
            attack.Spike(Directions.Up);
        }
    }
}
#endif