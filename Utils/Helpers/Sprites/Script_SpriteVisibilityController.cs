using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// May be used on a parent of sprites or on the sprite itself
/// Used to hide the sprite when Player passes a certain location
/// </summary>
/// <param name="t">AUTO-SET to children or the Transform of which script is attached</param>
[RequireComponent(typeof(Script_SpriteFadeOut))]
public class Script_SpriteVisibilityController : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Vector3 targetLoc;
    public bool isAxisZ = true;
    public float timer;
    public float maxTimer;
    protected Script_Game g;
    [SerializeField] private bool isParent;
    [Tooltip("Hide until meets the Transform")]
    [SerializeField] private bool isShowOnReachedTarget;
    [SerializeField] private Script_SpriteFadeOut spriteFader;
    [SerializeField] protected FadeSpeeds fadeSpeed;

    private Coroutine fadeOutCoroutine;
    private Coroutine fadeInCoroutine;
    
    void Awake() {
        if (spriteFader == null)    spriteFader = GetComponent<Script_SpriteFadeOut>();
        if (isParent)   spriteFader.isParent = true;
        
        SetVisibilityTargetByTransform();
    }
    
    void Start()
    {
        g = Script_Game.Game;
        
        if (targetLoc == Vector3.zero)
        {
            targetLoc = GetComponent<Transform>().position;
        }

        if (CheckShouldFadeIn())
        {
            spriteFader.SetVisibility(true);
        }
        else
        {
            spriteFader.SetVisibility(false);
        }
    }

    void OnValidate() {
        if (spriteFader == null)    spriteFader = GetComponent<Script_SpriteFadeOut>();
        if (isParent)   spriteFader.isParent = true;
        SetVisibilityTargetByTransform();
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0f)
        {
            timer = maxTimer;
            HandleVisibility();
        }
    }

    /// <summary>
    /// Ensures other coroutine stops running
    /// Check if we're already fading out
    /// 
    /// Note: This gameobject must be active to call this method (to run coroutine).
    /// </summary>
    public void FadeOut()
    {
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }
        if (fadeOutCoroutine != null)     return;
        
        Debug.Log("Fading out");

        float t = Script_Utils.GetFadeTime(fadeSpeed);
        fadeOutCoroutine = StartCoroutine(spriteFader.FadeOutCo(
            null,
            t
        ));
    }

    /// <summary>
    /// Note: This gameobject must be active to call this method (to run coroutine).
    /// </summary>
    public void FadeIn()
    {
        // if is fully faded out, then upon fading in, match up with player position    
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }
        if (fadeInCoroutine != null)       return;
        
        Debug.Log("Fading in");

        float t = Script_Utils.GetFadeTime(fadeSpeed);
        fadeInCoroutine = StartCoroutine(spriteFader.FadeInCo(
            null,
            t
        ));
    }
    
    void HandleVisibility()
    {
        if (g.GetPlayer() != null)
        {
            HandleFadeOut(CheckShouldFadeIn());
        }
    }

    protected virtual bool CheckShouldFadeIn()
    {
        var player = g.GetPlayer();

        if (player == null)
            return false;

        Vector3 playerLoc = player.transform.position;

        bool isPlayerReachedTarget;

        if (isAxisZ)    isPlayerReachedTarget = playerLoc.z >= targetLoc.z;
        else            isPlayerReachedTarget = playerLoc.x >= targetLoc.x;

        if (isShowOnReachedTarget)
            return isPlayerReachedTarget;
        else
            return !isPlayerReachedTarget;
    }

    private void HandleFadeOut(bool isFadeIn)
    {
        if (isFadeIn)  FadeIn();
        else           FadeOut();
    }

    private void SetVisibilityTargetByTransform()
    {
        if (target != null)
        {
            targetLoc = target.transform.position;           
        }
    }
}
