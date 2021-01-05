using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_HUDManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup HUDCanvasGroup;
    
    public void Setup()
    {
        HUDCanvasGroup.gameObject.SetActive(true);
    }
}
