using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// this child of demon specifies the demon's particular attacks set
/// potentially could just be one attack
/// </summary>
public class Script_UrselkAttacks : Script_DemonAttacks
{
    [SerializeField] private Script_EnergySpikeAttack randomSpikesAttack;
    [SerializeField] private Script_AlternatingSpikesAttack alternatingSpikesAttack;
    [SerializeField] private Grid attackGrid;
    [SerializeField] private Tilemap attackTilemap;
    private List<Vector3Int> attackTileLocs = new List<Vector3Int>();

    void Awake()
    {
        if (attackGrid != null)     attackGrid.gameObject.SetActive(false);
    }
    
    public void RandomSpikes()
    {
        randomSpikesAttack.RandomSpikes(attackTileLocs, attackTilemap, attackGrid);
    }

    public void AlternatingSpikesAttack()
    {
        alternatingSpikesAttack.AlternatingSpikes();
    }
}
