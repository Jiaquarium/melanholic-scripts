using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_UsableKeyTarget_MasterSeasonsKey : Script_UsableKeyTarget
{
    [SerializeField] private PlayableDirector masterLockUnlockDirector;
    
    void OnEnable()
    {
        masterLockUnlockDirector.stopped += OnUnlockedDone;
    }

    void OnDisable()
    {
        masterLockUnlockDirector.stopped -= OnUnlockedDone;
    }
    
    protected override void OnUnlock(Script_UsableKey key)
    {
        Script_Game.Game.ChangeStateCutScene();
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
        
        base.OnUnlock(key);
    }

    private void OnUnlockedDone(PlayableDirector aDirector)
    {
        Script_Game.Game.ChangeStateInteract();
    }
}
