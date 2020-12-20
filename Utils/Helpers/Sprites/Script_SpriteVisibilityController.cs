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
    [Tooltip("Hide until meets the Transform")][SerializeField] private bool isShowOnReachedTarget;
    [SerializeField] private Script_SpriteFadeOut spriteFader;
    [SerializeField] protected FadeSpeeds fadeSpeed;

    private Coroutine fadeOutCoroutine;
    private Coroutine fadeInCoroutine;
    private bool isFadedOut;
    private bool isFadedIn;
    
    
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
            isFadedIn = true;
            isFadedOut = false;
        }
        else
        {
            spriteFader.SetVisibility(false);
            isFadedIn = false;
            isFadedOut = true;
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
    /// </summary>
    public void FadeOut()
    {
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = null;
        }
        if (isFadedOut || fadeOutCoroutine != null)     return;
        
        Debug.Log("Fading out");

        float t = Script_GraphicsManager.GetFadeTime(fadeSpeed);
        isFadedIn = false;
        fadeOutCoroutine = StartCoroutine(spriteFader.FadeOutCo(
            () => {
                isFadedOut = true;
            }, t
        ));
    }

    public void FadeIn()
    {
        // if is fully faded out, then upon fading in, match up with player position    
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }
        if (isFadedIn || fadeInCoroutine != null)       return;
        
        Debug.Log("Fading in");

        float t = Script_GraphicsManager.GetFadeTime(fadeSpeed);
        isFadedOut = false;
        fadeInCoroutine = StartCoroutine(spriteFader.FadeInCo(
            () => {
                isFadedIn = true;
            }, t
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
        Vector3 playerLoc = g.GetPlayerLocation();

        bool isPlayerReachedTarget;

        if (isAxisZ)    isPlayerReachedTarget = playerLoc.z >= targetLoc.z;
        else            isPlayerReachedTarget = playerLoc.x >= targetLoc.x;

        if (isShowOnReachedTarget)  return isPlayerReachedTarget;
        else                        return !isPlayerReachedTarget;
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
