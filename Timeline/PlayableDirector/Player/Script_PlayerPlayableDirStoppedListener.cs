using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Script_PlayerPlayableDirStoppedListener : Script_PlayableDirStoppedListener
{
    [SerializeField] private Script_Player player;
    
    protected override void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            Script_Game.Game.HandlePlayableDirectorStopped(aDirector);
            player.UpdateLocation();
        }
    }
}
