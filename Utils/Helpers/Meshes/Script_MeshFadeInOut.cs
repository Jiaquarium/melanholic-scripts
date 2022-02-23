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
        
        MeshRenderer meshRenderer;
        Color tmpColor;
        
        /// Use the color of the first child
        if (isParent)
            meshRenderer = childrenMeshes[0];
        else
            meshRenderer = GetComponent<MeshRenderer>();
        
        // Use color of the first material
        tmpColor = meshRenderer.materials[0].color;

        if (fadeOutTime <= 0)
            tmpColor.a = minAlpha;

        while (tmpColor.a > minAlpha)
        {
            tmpColor.a -= Time.deltaTime / fadeOutTime;
            
            if (tmpColor.a <= minAlpha)
                tmpColor.a = minAlpha;

            // Set all materials alpha
            foreach (var material in meshRenderer.materials)
                material.color = tmpColor;
            
            foreach (MeshRenderer childMesh in childrenMeshes)
            {
                foreach (var material in childMesh.materials)
                    material.color = tmpColor;
            }

            yield return null;
        }

        foreach (var material in meshRenderer.materials)
            material.color = tmpColor;
        
        foreach (MeshRenderer childMesh in childrenMeshes)
        {
            foreach (var material in childMesh.materials)
                material.color = tmpColor;
        }
        
        if (cb != null)
            cb();
    }

    public IEnumerator FadeInCo(Action cb, float? t = null, float maxAlpha = 1f)
    {
        Debug.Log("FadeInCo called");
        fadeInTime = t ?? fadeInTime;
        MeshRenderer meshRenderer;
        Color tmpColor;

        /// Use the color of the first child
        if (isParent)
            meshRenderer = childrenMeshes[0];
        else
            meshRenderer = GetComponent<MeshRenderer>();
        
        tmpColor = meshRenderer.materials[0].color;

        if (fadeInTime <= 0)
            tmpColor.a = maxAlpha;

        while (tmpColor.a < maxAlpha)
        {
            tmpColor.a += Time.deltaTime / fadeInTime;
            
            if (tmpColor.a > maxAlpha)
                tmpColor.a = maxAlpha;

            // Set all materials alpha
            foreach (var material in meshRenderer.materials)
                material.color = tmpColor;
            
            foreach (MeshRenderer childMesh in childrenMeshes)
            {
                foreach (var material in childMesh.materials)
                    material.color = tmpColor;
            }

            yield return null;
        }

        foreach (var material in meshRenderer.materials)
            material.color = tmpColor;
            
        foreach (MeshRenderer childMesh in childrenMeshes)
        {
            foreach (var material in childMesh.materials)
                material.color = tmpColor;
        }
        
        if (cb != null)
            cb();
    }

    /// <summary>
    /// Note: Not a coroutine.
    /// </summary>
    /// <param name="isTransparent"></param>
    public void SetVisibility(bool isVisible, float maxAlpha = 1f, float minAlpha = 0f)
    {
        MeshRenderer meshRenderer;
        
        if (isParent)
            meshRenderer = childrenMeshes[0];
        else
            meshRenderer = GetComponent<MeshRenderer>();

        Color tmpColor = meshRenderer.materials[0].color;
        tmpColor.a = isVisible ? maxAlpha : minAlpha;
        
        foreach (var material in meshRenderer.materials)
            material.color = tmpColor;
            
        foreach (MeshRenderer childMesh in childrenMeshes)
        {
            foreach (var material in childMesh.materials)
                material.color = tmpColor;
        }
    }
}