using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class Script_MelancholyPianoEffect : Script_StickerEffect
{
    [SerializeField] private List<Script_ExitMetadataObject> pianoSpawns;
    
    [SerializeField] private float disabledReactionWaitTime;
    
    private int pianoIdx;
    private AudioSource audioSource;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();        
    }
    
    public override void Effect()
    {
        Debug.Log($"{name} Effect()");

        // Check Level Behavior to ensure Piano effect is not disabled in current room.
        if (Script_Game.Game.levelBehavior.IsMelancholyPianoDisabled)
        {
            Script_Game.Game.ChangeStateCutScene();
            audioSource.PlayOneShot(Script_SFXManager.SFX.piano, Script_SFXManager.SFX.pianoVol);

            StartCoroutine(WaitToReact());
            
            return;
        }
        
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

        IEnumerator WaitToReact()
        {
            yield return new WaitForSeconds(disabledReactionWaitTime);

            Script_DialogueManager.DialogueManager.DisabledMelancholyPianoReaction();
        }
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