using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Must interact with 3 wells in the order described by Map,
/// The order is denoted by the seasons on the Map, starting with Spring.
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class Script_WellsPuzzleController : Script_PuzzleController
{
    private const int KeyWellsCount = 3;
    
    [SerializeField] private Script_Well[] keyWells = new Script_Well[KeyWellsCount];
    
    [SerializeField] private float beforePlaySecretSFXWaitTime;
    [SerializeField] private float afterPlaySecretSFXWaitTime;
    
    private int currentWellIdx;
    private bool isDone;

    [SerializeField] private Script_Game game;

    protected override void OnEnable()
    {
        base.OnEnable();

        Script_InteractableObjectEventsManager.OnWellInteraction += OnWellInteraction;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Script_InteractableObjectEventsManager.OnWellInteraction -= OnWellInteraction;
    }

    public override void InitialState()
    {
        currentWellIdx = 0;
    }

    private void OnWellInteraction(Script_Well well)
    {
        if (isDone)     return;
        
        if (well.Id == keyWells[currentWellIdx].Id)
        {
            Debug.Log($"CORRECT Well! {well}");

            // On Last Well
            if (currentWellIdx == KeyWellsCount - 1)    ProgressNotification(true);
            else                                        ProgressNotification(false);

            currentWellIdx++;
        }
        else
        {
            Debug.Log($"WRONG Well! {well}... Restarting currentWellIdx");

            currentWellIdx = 0;
        }
    }

    public override void CompleteState()
    {
        base.CompleteState();

        Debug.Log($"PUZZLE COMPLETE!!!!!!!!!!!!!!!!!!!!!!!!");

        Script_PuzzlesEventsManager.PuzzleSuccess(PuzzleId);

        isDone = true;
    }

    private void ProgressNotification(bool isLastWell)
    {
        game.ChangeStateCutScene();

        StartCoroutine(WaitToPlayClip());

        IEnumerator WaitToPlayClip()
        {
            yield return new WaitForSeconds(beforePlaySecretSFXWaitTime);
            
            // Play Correct Partial Progress SFX if is correct well but not done yet. Play Secret SFX when done.
            if (isLastWell)
                GetComponent<AudioSource>().PlayOneShot(Script_SFXManager.SFX.Secret, Script_SFXManager.SFX.SecretVol);
            else
                GetComponent<AudioSource>().PlayOneShot(
                    Script_SFXManager.SFX.CorrectPartialProgress,
                    Script_SFXManager.SFX.CorrectPartialProgressVol
                );

            yield return new WaitForSeconds(afterPlaySecretSFXWaitTime);

            game.ChangeStateInteract();

            if (isLastWell)
                CompleteState();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_WellsPuzzleController))]
public class Script_WellsPuzzleControllerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_WellsPuzzleController t = (Script_WellsPuzzleController)target;
        if (GUILayout.Button("Complete Wells Puzzle & Freeze Wells"))
        {
            t.CompleteState();
        }
    }
}
#endif