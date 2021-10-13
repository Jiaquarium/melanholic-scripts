using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TimeManager : MonoBehaviour
{
    [SerializeField] private Script_Game game;
    [SerializeField] private float timeSinceLastPlayed;

    void Update()
    {
        UpdateTotalPlayTime();
    }
    
    public float TotalPlayTime {
        get => game.totalPlayTime;
        set => game.totalPlayTime = value;
    }

    public float UpdateTotalPlayTime()
    {
        float timePlayed = Time.time - timeSinceLastPlayed;
        TotalPlayTime += timePlayed;
        timeSinceLastPlayed = Time.time;
        return TotalPlayTime;
    }
    
    public void Setup()
    {
        timeSinceLastPlayed = Time.time;
    }
}
