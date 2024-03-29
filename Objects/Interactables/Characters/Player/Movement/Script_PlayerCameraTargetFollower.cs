using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerCameraTargetFollower : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float weight;
    
    [SerializeField] private SpriteRenderer graphics;   
    [SerializeField] private Script_Game game;

    public bool IsFollowing { get; set; }
    
    void Awake()
    {
        graphics.gameObject.SetActive(Const_Dev.IsCamGuides);
    }
    
    void LateUpdate()
    {
        if (IsFollowing)
            DampTowardsPlayer();
    }
    
    public void MatchPlayer()
    {
        transform.position = game.GetPlayer().FocalPoint.position;
    }
    
    // Making this a coroutine forces it to happen after Player movement in execution loop.
    public void MoveTowardsPlayer()
    {
        Script_Player player = game.GetPlayer();

        Vector3 playerPosition = player.FocalPoint.position;
        Vector3 myPosition = transform.position;

        Vector3 newPosition = (playerPosition * weight) + (myPosition * (1 - weight));

        transform.position = newPosition;
    }

    /// <summary>
    /// Frame rate independent damping
    /// https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/
    /// </summary>
    public void DampTowardsPlayer()
    {
        Script_Player player = game.GetPlayer();

        Vector3 playerPosition = player.FocalPoint.position;
        Vector3 myPosition = transform.position;

        Vector3 newPosition = myPosition.FrameRateAwareDamp(playerPosition, weight, Time.deltaTime);

        transform.position = newPosition;
    }
}
