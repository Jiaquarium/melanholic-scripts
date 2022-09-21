using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animator must be set to Unscaled time because these are affecting disabled game objects
/// 
/// NOTE: if continuing into Full Art dialogue, ensure to put fadeIn: None for that Node
/// </summary>
public class Script_OnEnableEntranceFromRightTimelineHelper : MonoBehaviour
{
    [SerializeField] private Script_FullArt fullArt;
    [SerializeField] private FadeSpeeds fadeInSpeed;

    void OnEnable()
    {
        Dev_Logger.Debug($"{this.name} setting full art animation triggers to prepare entrance from right");

        Script_FullArtManager.Control.EntranceFromRight(fullArt);
    }
}
