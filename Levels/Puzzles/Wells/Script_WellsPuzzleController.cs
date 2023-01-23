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
    
    [SerializeField] private Script_LevelBehavior_42 wellWorldBehavior;
    [SerializeField] private Script_Well[] keyWells = new Script_Well[KeyWellsCount];
    
    [SerializeField] private float beforePlaySecretSFXWaitTime;
    [SerializeField] private float afterPlaySecretSFXWaitTime;

    [SerializeField] private Script_DialogueNode wellTalkInitialDialogue;
    
    private int currentWellIdx;
    private bool isDone;
    private Script_Well currentWellTalking;

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
        Dev_Logger.Debug($"{well.name} IsCorrectWell: {IsCorrectWell(well)}");
        
        // Initial well talk, puzzle will be handled by next node action OnWellTalkInitialDialogueDone
        if (!wellWorldBehavior.didWellTalkInitialDialogue)
        {
            game.ChangeStateCutScene();

            currentWellTalking = well;
            
            Script_DialogueManager.DialogueManager.StartDialogueNode(
                wellTalkInitialDialogue,
                talkingInteractive: well
            );
            
            wellWorldBehavior.didWellTalkInitialDialogue = true;
            return;
        }
        
        // do well talk,
        well.WellTalk(() => {
            HandlePuzzle(well);
        });
    }

    private void HandlePuzzle(Script_Well well)
    {
        if (isDone)
            return;
    
        if (IsCorrectWell(well))
        {
            Dev_Logger.Debug($"CORRECT Well! {well}");

            // On Last Well
            if (currentWellIdx == KeyWellsCount - 1)
                ProgressNotification(true);
            else
                ProgressNotification(false);

            currentWellIdx++;
        }
        else
        {
            Dev_Logger.Debug($"WRONG Well! {well}... Restarting currentWellIdx");

            currentWellIdx = 0;
        }
    }

    public bool IsCorrectWell(Script_Well well) => well.Id == keyWells[currentWellIdx].Id;

    public override void CompleteState()
    {
        base.CompleteState();

        Dev_Logger.Debug($"PUZZLE COMPLETE!!!!!!!!!!!!!!!!!!!!!!!!");

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
    
    // ----------------------------------------------------------------------
    // Next Node Action
    
    // After well talk initial dialogue node
    public void OnWellTalkInitialDialogueDone()
    {
        currentWellTalking.WellTalk(() => {
            HandleWellAchievement();

            HandlePuzzle(currentWellTalking);
            currentWellTalking = null;
        });
        
        void HandleWellAchievement()
        {
            var achievementsManager = Script_AchievementsManager.Instance;
            Script_AchievementsManager.Instance.UnlockWell();
        }
    }

    // ----------------------------------------------------------------------
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