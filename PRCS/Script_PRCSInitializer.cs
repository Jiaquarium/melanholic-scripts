using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PRCSInitializer : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] canvasGroups;
    
    public void Initialize()
    {
        foreach (CanvasGroup canvasGroup in canvasGroups)
        {
            canvasGroup.alpha = 0f;
        }
    }
}
