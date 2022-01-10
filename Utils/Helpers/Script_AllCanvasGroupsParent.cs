using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AllCanvasGroupsParent : MonoBehaviour
{
    [SerializeField] private Script_WordsEffects wordsEffects;

    public void Setup()
    {
        gameObject.SetActive(true);
        wordsEffects.Setup();
    }
}
