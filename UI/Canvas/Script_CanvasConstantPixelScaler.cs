using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

// Note that when we adjust the screen size, the PixelRatio is unchanged unless we
// disable and reenable Pixel Perfect Camera component.
[RequireComponent(typeof(CanvasScaler))]
public class Script_CanvasConstantPixelScaler : MonoBehaviour
{
    [SerializeField] private PixelPerfectCamera pixelPerfectCamera;
    
    void OnEnable()
    {
        SetScaleFactor();
    }

    void Start()
    {
        SetScaleFactor();
    }

    void Update()
    {
        SetScaleFactor();
    }

    private void SetScaleFactor()
    {
        // Catch when Singletons aren't set yet.
        try
        {
            if (pixelPerfectCamera == null)
                Setup();
            
            int scaleFactor = pixelPerfectCamera.pixelRatio;
            GetComponent<CanvasScaler>().scaleFactor = scaleFactor;
        }
        catch (System.Exception error)
        {
            Debug.LogWarning(error);
        }
    }

    public void Setup()
    {
        pixelPerfectCamera = Script_Game.Game.PixelPerfectCamera;
    }
}
