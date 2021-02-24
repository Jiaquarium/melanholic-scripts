using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_WellsPuzzleController : Script_PuzzleController
{
    private const int KeyWellsCount = 4;
    
    [SerializeField] private Script_Well[] keyWells = new Script_Well[KeyWellsCount];
    [SerializeField] private int currentWellIdx;
    [SerializeField] private bool isDone;

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
        
        if (well == keyWells[currentWellIdx])
        {
            Debug.Log($"CORRECT Well! {well}");

            if (currentWellIdx == KeyWellsCount - 1)
            {
                CompleteState();
                return;
            }

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
}