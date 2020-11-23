using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// May be used on a parent of sprites or on the sprite itself
/// </summary>
/// <param name="t">AUTO-SET to children or the Transform of which script is attached</param>
[RequireComponent(typeof(Script_SpriteFadeOut))]
public class Script_SpriteVisibilityController : MonoBehaviour
{
    public Vector3 targetLoc;
    public bool isAxisZ = true;
    public float timer;
    public float maxTimer;
    private Script_Game g;
    [SerializeField] private bool isParent;
    [SerializeField] private Script_SpriteFadeOut spriteFader;
    [SerializeField] private FadeSpeeds fadeSpeed;

    private Coroutine fadeOutCoroutine;
    private Coroutine fadeInCoroutine;
    private bool isFadedOut;
    private bool isFadedIn;
    
    
    void Awake() {
        spriteFader = GetComponent<Script_SpriteFadeOut>();
        if (isParent)   spriteFader.isParent = true;
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
        spriteFader = GetComponent<Script_SpriteFadeOut>();
        if (isParent)   spriteFader.isParent = true;
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

    void HandleVisibility()
    {
        if (g.GetPlayer() != null)
        {
            HandleFadeOut(CheckShouldFadeIn());
        }
    }

    private bool CheckShouldFadeIn()
    {
        Vector3 playerLoc = g.GetPlayerLocation();

        if (isAxisZ)    return !(playerLoc.z >= targetLoc.z);
        else            return !(playerLoc.x >= targetLoc.x);
    }

    private void HandleFadeOut(bool isFadeIn)
    {
        if (isFadeIn)  FadeIn();
        else           FadeOut();

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
            
            Debug.Log("Fading out");

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
    }
}
