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
            Debug.Log($"DEV MODE (IsNoTimeHurt): Player {name} would take damage {sec} sec. Time: {Script_ClockManager.Control.ClockTime}");
        }
        else
        {
            HandleTakeDamage(sec);
            Debug.Log($"Player {name} took damage {sec} sec. Time: {Script_ClockManager.Control.ClockTime}");
        }

        return sec;
    }

    /// <summary>
    /// TODO: Reduce current time
    /// Return the new time
    /// </summary>
    private float HandleTakeDamage(int secAfterDef)
    {
        // Reduce time
        Script_ClockManager.Control.FastForwardTime(secAfterDef);
        
        // Return the new Time
        return Script_ClockManager.Control?.ClockTime ?? 0f;
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
