using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Tombstone : Script_InteractableObjectText
{
    // For Unity Event
    public void OnTombstoneInteract()
    {
        Script_BackgroundMusicManager.Control.FadeOutFast();
    }

    // For Unity Event
    public void OnTombstoneDone()
    {
        Script_BackgroundMusicManager.Control.FadeInSlow();
    }
}
