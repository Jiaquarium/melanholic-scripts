using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_MelancholyPianoEffect : Script_StickerEffect
{
    [SerializeField] private List<Script_ExitMetadataObject> pianoSpawns;
    private int pianoIdx;
    
    public override void Effect()
    {
        Debug.Log($"{name} Effect()");

        var pianoSpawn = pianoSpawns[pianoIdx];

        Script_Game.Game.Exit(
            pianoSpawn.data.level,
            pianoSpawn.data.playerSpawn,
            pianoSpawn.data.facingDirection,
            isExit: true,
            isSilent: false,
            exitType: Script_Exits.ExitType.Piano
        );

        pianoIdx++;
        if (pianoIdx >= pianoSpawns.Count)  pianoIdx = 0;
    }

    public void Setup()
    {
        pianoIdx = 0;
        pianoSpawns = Script_Game.Game.PianoSpawns;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_MelancholyPianoEffect))]
public class Script_MelancholyPianoEffectTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_MelancholyPianoEffect t = (Script_MelancholyPianoEffect)target;
        if (GUILayout.Button("Effect()"))
        {
            t.Effect();
        }
    }
}
#endif