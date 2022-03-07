using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Current default way forced the attached to also fade
/// Use isParent flag to override this
/// </summary>
public class Script_SpriteFadeOut : MonoBehaviour
{
    public float fadeOutTime;
    public float fadeInTime;
    [SerializeField] SpriteRenderer[] childrenSprites;
    public bool isParent;

    void OnValidate()
    {
        if (isParent)
        {
            // if there is a sprite component on here too, consider it a child as well
            childrenSprites = transform.GetComponentsInChildren<SpriteRenderer>(true);
            
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
                childrenSprites = childrenSprites.AddItemToArray(sr);    
        }    
    }

    public IEnumerator FadeOutCo(Action cb, float? t = null, float minAlpha = 0f)
    {
        fadeOutTime = t ?? fadeOutTime;
        
        SpriteRenderer sr;
        Color tmpColor;
        
        /// Use the color of the first child
        if (isParent)   sr = childrenSprites[0];
        else            sr = GetComponent<SpriteRenderer>();
        tmpColor = sr.color;

        if (fadeOutTime <= 0)     tmpColor.a = minAlpha;

        while (tmpColor.a > minAlpha)
        {
            tmpColor.a -= Time.deltaTime / fadeOutTime;
            if (tmpColor.a <= minAlpha)   tmpColor.a = minAlpha;

            sr.color = tmpColor;
            foreach (SpriteRenderer childSprite in childrenSprites)
                childSprite.color = tmpColor;

            yield return null;
        }

        sr.color = tmpColor;
        foreach (SpriteRenderer childSprite in childrenSprites)
            childSprite.color = tmpColor;
        
        if (cb != null)    cb();
    }

    public IEnumerator FadeInCo(Action cb, float? t = null, float maxAlpha = 1f)
    {
        fadeInTime = t ?? fadeInTime;
        SpriteRenderer sr;
        Color tmpColor;

        if (isParent)   sr = childrenSprites[0];
        else            sr = GetComponent<SpriteRenderer>();
        tmpColor = sr.color;

        if (fadeInTime <= 0)     tmpColor.a = maxAlpha;

        while (tmpColor.a < maxAlpha)
        {
            tmpColor.a += Time.deltaTime / fadeInTime;
            if (tmpColor.a > maxAlpha)   tmpColor.a = maxAlpha;

            sr.color = tmpColor;
            foreach (SpriteRenderer childSprite in childrenSprites)
                childSprite.color = tmpColor;

            yield return null;
        }

        sr.color = tmpColor;
        foreach (SpriteRenderer childSprite in childrenSprites)
            childSprite.color = tmpColor;
        
        if (cb != null)    cb();
    }

    /// <summary>
    /// NOT COROUTINE
    /// </summary>
    /// <param name="isTransparent"></param>
    public void SetVisibility(bool isVisible)
    {
        SpriteRenderer sr;
        
        if (isParent)
        {
            sr = childrenSprites[0];
        }
        else
        {
            sr = GetComponent<SpriteRenderer>();
        }

        Color tmpColor = sr.color;
        tmpColor.a = isVisible ? 1f : 0f;
        
        sr.color = tmpColor;
        foreach (SpriteRenderer childSprite in childrenSprites)
            childSprite.color = tmpColor;
    }
}
