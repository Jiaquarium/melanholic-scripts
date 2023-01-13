using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// NOTE: Keep max HP equal to number of thought Slots
/// </summary>
[RequireComponent(typeof(Script_Player))]
public class Script_PlayerStats : Script_CharacterStats
{
    public string defaultHitBoxMessage;
    
    /// TODO: REMOVE ISSWALLEDDEMON, give demons hitboxes and they pass it in
    public override int Hurt(int sec, Script_HitBox hitBox)
    {
        if (GetComponent<Script_Player>().isInvincible)
            return 0;

        // reduce dmg by defense
        sec -= stats.defense.GetVal();
        sec = Mathf.Clamp(sec, 0, int.MaxValue);
        
        if (Const_Dev.IsNoTimeHurt)
        {
            Dev_Logger.Debug($"DEV MODE (IsNoTimeHurt): Player {name} would take damage {sec} sec. Time: {Script_ClockManager.Control.ClockTime}");
        }
        else
        {
            HandleTakeDamage(sec);
            Dev_Logger.Debug($"Player {name} took damage {sec} sec. Time: {Script_ClockManager.Control.ClockTime}");
        }

        return sec;
    }

    private void HandleTakeDamage(int secAfterDef)
    {
        // Reduce time
        Script_ClockManager.Control.FastForwardTime(secAfterDef);

        if (
            Script_ClockManager.Control.ClockState == Script_Clock.States.Done
            || Script_ClockManager.Control.TimeLeft == 0
        )
        {
            // Event to notify time has hit 0 because of a Fast Forward (Hit)
            Dev_Logger.Debug($"{name} Reaching time from hit");
            Script_ClockEventsManager.FastForwardTimesUp();
        }
    }

    public override int Heal(int healHp)
    {
        Debug.LogWarning("Heal not implemented yet.");
        return 0;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_PlayerStats)), CanEditMultipleObjects]
public class Script_PlayerStatsTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_PlayerStats stats = (Script_PlayerStats)target;
        if (GUILayout.Button("Hurt(1)"))
        {
            stats.Hurt(1, null);
        }

        if (GUILayout.Button("Heal(1)"))
        {
            stats.Heal(1);
        }

        if (GUILayout.Button("FullHeal()"))
        {
            stats.FullHeal();
        }
    }
}
#endif
