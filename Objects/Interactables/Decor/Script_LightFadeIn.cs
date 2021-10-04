using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_LightFadeIn : MonoBehaviour
{
    public Transform t;
    public Vector3 offset;
    
    [SerializeField] private float maxIntensity;
    
    private float tmpIntensity;
    private Light myLight;

    void Awake()
    {
        t = GetComponent<Transform>();
        myLight = GetComponent<Light>();
        
        if (maxIntensity == 0f)
            maxIntensity = myLight.intensity;
    }
    
    public IEnumerator FadeInLightOnTarget(
        float maxTime,
        Transform target,
        Action cb,
        float? newMaxIntensity = null
    )
    {
        if (newMaxIntensity != null)
            maxIntensity = (float)newMaxIntensity;
        
        if (target != null)
            t.position = target.position + offset;

        tmpIntensity = myLight.intensity;
        
        while (myLight.intensity < maxIntensity)
        {
            tmpIntensity += (Time.deltaTime / maxTime) * maxIntensity;

            if (tmpIntensity > maxIntensity)
            {
                tmpIntensity = maxIntensity;
            }

            myLight.intensity = tmpIntensity;
            yield return null;
        }

        if (cb != null)    cb();
    }

    public IEnumerator FadeOutLight(
        float maxTime,
        Action cb
    )
    {
        tmpIntensity = myLight.intensity;
        
        while (myLight.intensity > 0f)
        {
            tmpIntensity -= (Time.deltaTime / maxTime) * maxIntensity;

            if (tmpIntensity < 0f)
            {
                tmpIntensity = 0f;
            }

            myLight.intensity = tmpIntensity;
            yield return null;
        }

        if (cb != null)    cb();
    }
}
