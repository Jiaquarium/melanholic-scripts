using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerCameraTargetFollower : MonoBehaviour
{

    [Range(0, 20)]
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer graphics;
    
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_PixelTargetFollower pixelTargetFollower;

    [SerializeField] private AnimationCurve progressCurve;

    [SerializeField] private Vector3 targetPosition;
    private float progress;

    public Script_PixelTargetFollower PixelTargetFollower
    {
        get => pixelTargetFollower;
    }
    
    void Awake()
    {
        graphics.gameObject.SetActive(Const_Dev.IsCamGuides);
    }
    
    void Update()
    {
        StartCoroutine(MoveTowardsPlayer());
    }
    
    public void MatchPlayer()
    {
        transform.position = game.GetPlayer().FocalPoint.position;
        pixelTargetFollower.Move(transform.position);
    }
    
    // Making this a coroutine forces it to happen after Player movement in execution loop.
    public IEnumerator MoveTowardsPlayer()
    {
        yield return null;

        Script_Player player = game.GetPlayer();
        targetPosition = player.FocalPoint.position;
        Vector3 myPosition = transform.position;

        if (!myPosition.IsSame(targetPosition, 4))
            progress = 0f;

        if (progress < 1f)
        {
            progress += speed * Time.smoothDeltaTime;

            if (progress > 1f)
                progress = 1f;

            Vector3 newPosition = Vector3.Lerp(
                myPosition,
                targetPosition,
                progressCurve.Evaluate(progress)
            );

            transform.position = newPosition;
            pixelTargetFollower.Move(transform.position);
        }
    }
}
