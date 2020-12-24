using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the Health View; actual death functionality in PlayerStats
/// 
/// NOTE: Should only be called from Stats
/// (is what actually handles health)
/// This only handles the view
/// </summary>
public class Script_HealthManager : MonoBehaviour
{
    public Vector3 heartSpawnLocation;
    public Script_HealthHeart heartPrefab;
    public Script_HealthHeartsHolder heartContainer;
    public CanvasGroup healthCanvas;
    

    [SerializeField] private List<Script_HealthHeart> hearts;
    // if this goes above slots - 1, then player loses
    [SerializeField] private int heartIndex;
    [SerializeField] private Script_Game game;
    
}
