using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shake image should have canvas group on same level and children Images.
/// Note: currently, no extra shake functionality has been implemented; the image distorter will handle all shaking.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class Script_ShakeImage : MonoBehaviour
{
    // ------------------------------------------------------------
    // Unity Events
    
    // TheEndCliff CanvasGroup: Image Distorter Controller
    public void Open()
    {
        gameObject.SetActive(true);
    }

    // TheEndCliff CanvasGroup: Image Distorter Controller
    public void Close()
    {
        gameObject.SetActive(false);
    }

    // ------------------------------------------------------------
}
