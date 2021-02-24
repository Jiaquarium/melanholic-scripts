using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_42 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    
    // ==================================================================
    [SerializeField] private Script_WellsPuzzleController wellsPuzzleController;
    [SerializeField] private Script_FrozenWell frozenWell;

    protected override void OnEnable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess += OnPuzzleSuccess;
    }

    protected override void OnDisable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess -= OnPuzzleSuccess;
    }

    private void OnPuzzleSuccess(string puzzleId)
    {
        if (puzzleId == wellsPuzzleController.PuzzleId)
        {
            Debug.Log("PUZZLE COMPLETED!!! FREEZE WELL!!!");

            frozenWell.Freeze();
        }
    }

    public override void Setup()
    {
        wellsPuzzleController.InitialState();
    }
}