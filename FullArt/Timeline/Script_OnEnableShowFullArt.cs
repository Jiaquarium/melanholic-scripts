using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NOTE: if continuing into Full Art dialogue, ensure to put fadeIn: None for that Node
/// NOTE: also to remove the Full Art call RemoveFullArt from a signal
/// </summary>
public class Script_OnEnableShowFullArt : MonoBehaviour
{
    [SerializeField] private Script_FullArt fullArt;
    [SerializeField] private FadeSpeeds fadeInSpeed;
    [SerializeField] private FadeSpeeds fadeOutSpeed;
    [SerializeField] private bool isUseOnce;

    private bool isDone;

    void OnEnable()
    {
        if (isDone)     return;
        
        Dev_Logger.Debug($"{this.name} setting full art animation triggers to prepare entrance from right");

        Script_FullArtManager.Control.ShowFullArt(
            fullArt,
            fadeInSpeed,
            null,
            Script_FullArtManager.FullArtState.Timeline
        );

        if (isUseOnce)  isDone = true;
    }

    // ------------------------------------------------------------------
    // Timeline Signal Receiver
    public void RemoveFullArt()
    {
        Script_FullArtManager.Control.HideFullArt(
            fullArt,
            fadeOutSpeed,
            null
        );
    }
}
