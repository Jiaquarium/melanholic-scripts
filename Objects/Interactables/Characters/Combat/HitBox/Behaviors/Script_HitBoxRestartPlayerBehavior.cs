﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Script_HitBoxRestartPlayerBehavior : Script_HitBoxBehavior
{
    private static int ExitFaderSortingOrder = 999;

    [SerializeField] private Transform restartDestination;
    [SerializeField] private Directions facingDirection = Directions.Up;

    public override void Hit(Collider col)
    {
        Dev_Logger.Debug(col.tag);
        if (col.tag == Const_Tags.Player)
        {
            Dev_Logger.Debug($"{name} Player hit: {col}");
            Dev_Logger.Debug($"Time Left (s): {Script_ClockManager.Control.TimeLeft}");
            
            if (Const_Dev.IsDevMode && Debug.isDebugBuild)
                return;

            var game = Script_Game.Game;
            
            // Ignore this behavior if the hit caused Time to run out.
            if (
                Script_ClockManager.Control.ClockState == Script_Clock.States.Done
                || Script_ClockManager.Control.TimeLeft == 0
                || game.GetPlayer().isInvincible
            )
            {
                return;
            }
            
            game.ChangeStateCutScene();
            Script_HUDManager.Control.IsForceUp = true;
            
            // Force Time HUD to appear above fader
            Script_HUDManager.Control.TimeHUDSortingOrder = ExitFaderSortingOrder + 1;
            
            StartCoroutine(game.TransitionFadeIn(
                Script_TransitionManager.RestartPlayerFadeInTime, () =>
                {
                    Script_Player p = col.transform.parent.GetComponent<Script_Player>();
                    
                    Vector3 prevPlayerPos = p.transform.position;
                    
                    p.Teleport(restartDestination.position);
                    p.FaceDirection(facingDirection);

                    Script_HurtBoxEventsManager.PlayerRestartTeleport(col);
                    
                    game.SnapActiveCam(prevPlayerPos);
                    FadeOut();
                }
            ));
            
            void FadeOut()
            {
                StartCoroutine(game.TransitionFadeOut(
                    Script_TransitionManager.RestartPlayerFadeOutTime, () =>
                    {
                        game.ChangeStateInteract();

                        Script_HUDManager.Control.IsForceUp = false;
                        Script_HUDManager.Control.SetTimeHUDSortingOrderDefault();

                        Script_HurtBoxEventsManager.PlayerRestart(col);
                    }
                ));
            }
        }
    }
}
