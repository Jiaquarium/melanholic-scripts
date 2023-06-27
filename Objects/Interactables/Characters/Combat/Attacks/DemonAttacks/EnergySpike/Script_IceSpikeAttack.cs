using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Player Spike Attack
/// spikes [0, 1, 2, 3] => [N, E, S, W] depending on how it's laid out in Inspector
/// </summary>
public class Script_IceSpikeAttack : Script_EnergySpikeAttack
{
    // Should equal Player Ice Spike Timeline duration
    private const float SnowWomanSpikeDisabledAnimationTime = 1f;
    
    [SerializeField] private Script_Player player;
    [SerializeField] private Script_EnergySpike spikeN;
    [SerializeField] private Script_EnergySpike spikeE;
    [SerializeField] private Script_EnergySpike spikeS;
    [SerializeField] private Script_EnergySpike spikeW;

    private Directions currentAttackDir;
    
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
        
        // Set current direction of this attack, so SpikeSequence can access
        currentAttackDir = dir;
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
        // Get current tilemap player is on
        // Player location will always be the destination tile if does Spike mid-move (movement updates player.location 1st)
        Vector3 playerLocation = player.location;
        Script_PlayerCheckCollisions playerCheckCollisions = player.PlayerCheckCollisions;
        
        if (
            // Check tilemaps with isNoSnowWomanSpike flag
            (Script_Game.Game.GetIsTileMapMetaDataMap() && GetIsNoSnowWomanSpike())
            // Check doing spike off tilemaps
            || playerCheckCollisions.CheckAttackOffTilemap(currentAttackDir, playerLocation)
        )
        {
            StartCoroutine(WaitToEndSnowWomanAnimation());
            return;
        }

        Script_VCamManager.VCamMain.Shake(
            Script_IceSpikeEffect.ShakeTime,
            Script_IceSpikeEffect.ShakeAmp,
            Script_IceSpikeEffect.ShakeFreq,
            null
        );
        
        SetSpikeActive(currentAttackDir);
        SetHitBoxes();
        SpikesSFX();
        ResetSpikesElevation();
        GetComponent<Script_TimelineController>().PlayAllPlayables();

        IEnumerator WaitToEndSnowWomanAnimation()
        {
            yield return new WaitForSeconds(SnowWomanSpikeDisabledAnimationTime);
            EndPlayerIceSpike();
        }

        bool GetIsNoSnowWomanSpike()
        {
            bool isNoSnowWomanSpike = false;
            Vector3Int tileWorldLocation = playerLocation.ToVector3Int();
            Tilemap currentTileMap = playerCheckCollisions.GetCurrentTileMapOn(tileWorldLocation);
            Script_TileMapMetaData tileMapMetaData = null;

            if (currentTileMap != null)
            {
                tileMapMetaData = currentTileMap.GetComponent<Script_TileMapMetaData>();
                if (tileMapMetaData != null)
                    isNoSnowWomanSpike = tileMapMetaData.IsNoSnowWomanSpike;
            }
            
            return isNoSnowWomanSpike;
        }
    }

    private void SetSpikeActive(Directions dir)
    {
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
    }

    private void EndPlayerIceSpike()
    {
        HideSpikes();
        isInUse = false;

        player.SetIsInteract();

        base.EndAttack();
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
        
        EndPlayerIceSpike();   
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
