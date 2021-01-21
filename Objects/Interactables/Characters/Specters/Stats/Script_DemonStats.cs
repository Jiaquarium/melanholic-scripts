using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_DemonStats : Script_CharacterStats
{
    protected override void Die(Script_GameOverController.DeathTypes deathType)
    {
        Debug.Log($"{transform.name} DEMON OVERRIDE Die() called");
        GetComponent<Script_Demon>().Die();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_DemonStats)), CanEditMultipleObjects]
public class Script_DemonStatsTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_DemonStats stats = (Script_DemonStats)target;
        if (GUILayout.Button("InitialState()"))
        {
            stats.InitialState();
        }
        if (GUILayout.Button("Hurt(1)"))
        {
            stats.Hurt(1, null);
        }
    }
}
#endif