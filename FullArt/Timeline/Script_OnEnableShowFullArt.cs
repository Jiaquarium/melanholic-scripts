using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NOTE: if continuing into Full Art dialogue, ensure to put fadeIn: None for that Node
/// </summary>
public class Script_OnEnableShowFullArt : MonoBehaviour
{
    [SerializeField] private Script_FullArt fullArt;
    [SerializeField] private FadeSpeeds fadeInSpeed;

    void OnEnable()
    {
        Debug.Log($"{this.name} setting full art animation triggers to prepare entrance from right");

        Script_FullArtManager.Control.ShowFullArt(
            fullArt,
            fadeInSpeed,
            null,
            Script_FullArtManager.FullArtState.Timeline
        );
    }
}
