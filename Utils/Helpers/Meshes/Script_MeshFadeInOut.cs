using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_MeshFadeInOut : MonoBehaviour
{
    public float fadeOutTime;
    public float fadeInTime;
    [SerializeField] MeshRenderer[] childrenMeshes;
    public bool isParent;

    void OnValidate()
    {
        if (isParent)
        {
            // if there is a sprite component on here too, consider it a child as well
            childrenMeshes = transform.GetComponentsInChildren<MeshRenderer>(true);
            
            MeshRenderer sr = GetComponent<MeshRenderer>();
            if (sr != null)
                childrenMeshes = childrenMeshes.AddItemToArray(sr);    
        }    
    }

    public IEnumerator FadeOutCo(Action cb, float? t = null, float minAlpha = 0f)
    {
        Debug.Log("FadeOutCo called");
        fadeOutTime = t ?? fadeOutTime;
        
        MeshRenderer sr;
        Color tmpColor;
        
        /// Use the color of the first child
        if (isParent)   sr = childrenMeshes[0];
        else            sr = GetComponent<MeshRenderer>();
        tmpColor = sr.material.color;

        if (fadeOutTime <= 0)     tmpColor.a = minAlpha;

        while (tmpColor.a > minAlpha)
        {
            tmpColor.a -= Time.deltaTime / fadeOutTime;
            if (tmpColor.a <= minAlpha)   tmpColor.a = minAlpha;

            sr.material.color = tmpColor;
            foreach (MeshRenderer childSprite in childrenMeshes)
                childSprite.material.color = tmpColor;

            yield return null;
        }

        sr.material.color = tmpColor;
        foreach (MeshRenderer childSprite in childrenMeshes)
            childSprite.material.color = tmpColor;
        if (cb != null)    cb();
    }

    public IEnumerator FadeInCo(Action cb, float? t = null, float maxAlpha = 1f)
    {
        Debug.Log("FadeInCo called");
        fadeInTime = t ?? fadeInTime;
        MeshRenderer sr;
        Color tmpColor;

        if (isParent)   sr = childrenMeshes[0];
        else            sr = GetComponent<MeshRenderer>();
        tmpColor = sr.material.color;

        if (fadeInTime <= 0)     tmpColor.a = maxAlpha;

        while (tmpColor.a < maxAlpha)
        {
            tmpColor.a += Time.deltaTime / fadeInTime;
            if (tmpColor.a > maxAlpha)   tmpColor.a = maxAlpha;

            sr.material.color = tmpColor;
            foreach (MeshRenderer childSprite in childrenMeshes)
                childSprite.material.color = tmpColor;

            yield return null;
        }

        sr.material.color = tmpColor;
        foreach (MeshRenderer childSprite in childrenMeshes)
            childSprite.material.color = tmpColor;
        if (cb != null)    cb();
    }

    /// <summary>
    /// Note: Not a coroutine.
    /// </summary>
    /// <param name="isTransparent"></param>
    public void SetVisibility(bool isVisible, float maxAlpha = 1f, float minAlpha = 0f)
    {
        MeshRenderer sr;
        
        if (isParent)
        {
            sr = childrenMeshes[0];
        }
        else
        {
            sr = GetComponent<MeshRenderer>();
        }

        Color tmpColor = sr.material.color;
        tmpColor.a = isVisible ? maxAlpha : minAlpha;
        
        sr.material.color = tmpColor;
        foreach (MeshRenderer childSprite in childrenMeshes)
            childSprite.material.color = tmpColor;
    }
}