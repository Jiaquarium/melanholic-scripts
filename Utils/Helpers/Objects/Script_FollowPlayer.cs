using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For use on multiple sprites, define children sprites in Script_SpriteFadeOut
/// Use innerBound flag and a fadeOutAxis to define where the follower fades out
/// </summary>
public class Script_FollowPlayer : MonoBehaviour
{
    [SerializeField] private AnimationCurve progressCurve;
    [SerializeField] private float speed;
    [SerializeField] private FollowVector followVector; // based on player
    [SerializeField] private FadeOutAxis fadeOutAxis;
    [SerializeField] private Vector3 fadeOutVectorLowerBound;
    [SerializeField] private Vector3 fadeOutVectorUpperBound;
    [SerializeField] private bool innerFlag = true;
    [SerializeField] private Script_SpriteFadeOut spriteFader;
    [SerializeField] private FadeSpeeds fadeSpeed;
    [SerializeField] private Vector3 offset;
    private Coroutine fadeOutCoroutine;
    private Coroutine fadeInCoroutine;
    private bool isFadedOut;
    private bool isFadedIn;
    private enum FadeOutAxis { None, x, z }
    private enum FollowVector { x, y, z }
    private float progress;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 lastEndPosition;
    
    private Script_Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = Script_Game.Game.GetPlayer();
        UpdateFollowTarget();
        transform.position = endPosition;
        
        if (CheckBounds()) spriteFader.SetVisibility(true);
        else               spriteFader.SetVisibility(false);
    }

    void Update()
    {
        UpdateFollowTarget();
        ActuallyMove();
        HandleFadeOut();
    }

    private void UpdateFollowTarget()
    {
        Vector3 newPos;
        switch (followVector)
        {
            case (FollowVector.z):
                // get new position with Player's z value
                newPos = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    player.transform.position.z
                );
                break;
            case (FollowVector.y):
                newPos = new Vector3(
                    transform.position.x,
                    player.transform.position.y,
                    transform.position.z
                );
                break;
            default:
                newPos = new Vector3(
                    player.transform.position.x,
                    transform.position.y,
                    transform.position.z
                );
                break;
        }

        // update position
        endPosition = newPos + offset;
    }

    private void ActuallyMove()
    {
        // restart Lerp'ing if target changed
        if (lastEndPosition != endPosition || progress >= 1f)
            InitializeState();
        
        progress += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(
            startPosition,
            endPosition,
            progressCurve.Evaluate(progress)
        );

        lastEndPosition = endPosition;
    }

    private void HandleFadeOut()
    {
        if (fadeOutAxis == FadeOutAxis.None)    return;

        if (CheckBounds())  FadeIn();
        else                FadeOut();

        /// <summary>
        /// Ensure other coroutine stops running
        /// Check if we're already fading out
        /// </summary>
        void FadeOut()
        {
            if (fadeInCoroutine != null)
            {
                StopCoroutine(fadeInCoroutine);
                fadeInCoroutine = null;
            }
            if (isFadedOut || fadeOutCoroutine != null)     return;
            
            float t = Script_GraphicsManager.GetFadeTime(fadeSpeed);
            isFadedIn = false;
            fadeOutCoroutine = StartCoroutine(spriteFader.FadeOutCo(
                () => {
                    isFadedOut = true;
                }, t
            ));
        }

        void FadeIn()
        {
            // if is fully faded out, then upon fading in, match up with player position    
            if (isFadedOut)     InitializeState();
            
            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine);
                fadeOutCoroutine = null;
            }
            if (isFadedIn || fadeInCoroutine != null)       return;
            
            float t = Script_GraphicsManager.GetFadeTime(fadeSpeed);
            isFadedOut = false;
            fadeInCoroutine = StartCoroutine(spriteFader.FadeInCo(
                () => {
                    isFadedIn = true;
                }, t
            ));
        }
    }

    /// <summary>
    /// Check where to fade in
    /// </summary>
    private bool CheckBounds()
    {
        float z = player.transform.position.z;
        float x = player.transform.position.x;
        
        if (fadeOutAxis == FadeOutAxis.z)
        {
            if (innerFlag)  return (z >= fadeOutVectorLowerBound.z && z <= fadeOutVectorUpperBound.z);
            else            return (z <= fadeOutVectorLowerBound.z || z >= fadeOutVectorUpperBound.z);
        }
        else
        {
            if (innerFlag)  return (x >= fadeOutVectorLowerBound.x && x <= fadeOutVectorUpperBound.x);
            else            return (x <= fadeOutVectorLowerBound.x || x >= fadeOutVectorUpperBound.x);
        }
    }

    private void InitializeState()
    {
        startPosition = transform.position;
        // endPosition = startPosition;
        progress = 0;
    }
}
