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
    private static readonly float defaultMaxTimer = 0.2f;
    
    [SerializeField] private Transform target;
    public Vector3 targetLoc;
    
    [Tooltip("Define an adjustment to targetLoc used to test for visibility comparison")]
    [SerializeField] private Vector3 targetLocAdjustment;

    public bool isAxisZ = true;
    public float timer;
    
    [Tooltip("Interval for testing visibility")]
    public float maxTimer;
    [SerializeField] private bool isUseDefaultMaxTimer;
    
    
    [SerializeField] private bool isParent;
    [Tooltip("Hide until meets the Transform")]
    [SerializeField] private bool isShowOnReachedTarget;
    [SerializeField] private Script_SpriteFadeOut spriteFader;
    [SerializeField] protected FadeSpeeds fadeSpeed;

    protected Script_Game g;
    private Coroutine fadeOutCoroutine;
    private Coroutine fadeInCoroutine;

    private float MaxTimer => isUseDefaultMaxTimer ? defaultMaxTimer : maxTimer;
    
    void Awake() {
        if (spriteFader == null)    spriteFader = GetComponent<Script_SpriteFadeOut>();
        if (isParent)   spriteFader.isParent = true;
        
        SetVisibilityTargetByTransform();
    }
    
    void Start()
    {
        g = Script_Game.Game;
        
        if (targetLoc == Vector3.zero)
            targetLoc = GetComponent<Transform>().position;

        targetLoc += targetLocAdjustment;

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
            timer = MaxTimer;
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
        
        if (fadeOutCoroutine != null)
            return;
        
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
        
        if (fadeInCoroutine != null)
            return;

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
