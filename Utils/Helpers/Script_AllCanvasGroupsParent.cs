using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AllCanvasGroupsParent : MonoBehaviour
{
    [SerializeField] private CanvasGroup wordsEffectsCanvasGroup;

    public void Setup()
    {
        gameObject.SetActive(true);
        wordsEffectsCanvasGroup.gameObject.SetActive(true);
    }
}
