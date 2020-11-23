using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_DisableGraphicRaycasters : MonoBehaviour
{
    void Awake()
    {
        GraphicRaycaster[] graphicRaycasters = transform
            .GetComponentsInChildren<GraphicRaycaster>(true);
        
        foreach (var g in graphicRaycasters)    g.enabled = false;
    }
}
