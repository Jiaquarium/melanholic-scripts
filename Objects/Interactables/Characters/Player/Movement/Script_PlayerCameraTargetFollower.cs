using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerCameraTargetFollower : MonoBehaviour
{
    [SerializeField] private Script_Game game;

    [Range(0, 1)]
    [SerializeField] private float asymptoticTargetWeight = 0.1f;
    [SerializeField] private SpriteRenderer graphics;

    void Awake()
    {
        graphics.gameObject.SetActive(Const_Dev.IsCamGuides);
    }
    
    void Update()
    {
        StartCoroutine(MoveTowardsPlayerGhost());
    }
    
    public void MatchPlayerGhost()
    {
        transform.position = game.GetPlayerGhost().FocalPoint.position;
    }
    
    // Making this a coroutine forces it to happen after Player movement in execution loop.
    public IEnumerator MoveTowardsPlayerGhost()
    {
        yield return null;

        Vector3 playerGhostPosition = game.GetPlayerGhost().FocalPoint.position;
        Vector3 myPosition = transform.position;

        Vector3 newPosition = (playerGhostPosition * asymptoticTargetWeight) +
            (myPosition * (1 - asymptoticTargetWeight));

        transform.position = newPosition;
    }
}
