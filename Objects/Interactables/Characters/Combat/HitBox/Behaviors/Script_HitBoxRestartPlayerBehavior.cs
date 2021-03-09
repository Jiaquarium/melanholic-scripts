using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Script_HitBoxRestartPlayerBehavior : Script_HitBoxBehavior
{
    [SerializeField] private Transform restartDestination;

    public override void Hit(Collider col)
    {
        print(col.tag);
        if (col.tag == Const_Tags.Player)
        {
            print($"hit player");
            if (Const_Dev.IsDevMode && Debug.isDebugBuild)  return;

            Script_Game.Game.ChangeStateCutScene();
            
            // fade in black
            StartCoroutine(Script_Game.Game.TransitionFadeIn(
                Script_TransitionManager.RestartPlayerFadeInTime, () =>
                {
                    Script_Player p = col.transform.parent.GetComponent<Script_Player>();
                    
                    Vector3 prevPlayerPos = p.transform.position;
                    
                    p.Teleport(restartDestination.position);
                    
                    Script_Game.Game.SnapActiveCam(prevPlayerPos);
                    FadeOut();        
                }
            ));
        }

        void FadeOut()
        {
            StartCoroutine(Script_Game.Game.TransitionFadeOut(
                Script_TransitionManager.RestartPlayerFadeOutTime, () =>
                {
                    Script_Game.Game.ChangeStateInteract();
                }
            ));
        }
    }


}
