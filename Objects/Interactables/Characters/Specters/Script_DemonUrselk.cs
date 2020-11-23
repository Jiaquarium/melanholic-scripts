using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_UrselkAttacks))]
public class Script_DemonUrselk : Script_Demon
{
    public override void Attack()
    {
        GetComponent<Script_UrselkAttacks>().RandomSpikes();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_DemonUrselk))]
public class Script_DemonUrselkTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_DemonUrselk urselk = (Script_DemonUrselk)target;
        if (GUILayout.Button("Attack()"))
        {
            urselk.Attack();
        }
    }
}
#endif
