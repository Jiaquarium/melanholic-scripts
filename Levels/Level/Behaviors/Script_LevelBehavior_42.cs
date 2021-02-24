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
    public bool didPickUpLastWellMap;    
    // ==================================================================
    [SerializeField] private Script_WellsPuzzleController wellsPuzzleController;
    [SerializeField] private Script_FrozenWell frozenWell;
    [SerializeField] private Script_CollectibleObject lastWellMap;

    protected override void OnEnable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess += OnPuzzleSuccess;
        Script_ItemsEventsManager.OnItemPickUp      += OnItemPickUp;
    }

    protected override void OnDisable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess -= OnPuzzleSuccess;
        Script_ItemsEventsManager.OnItemPickUp      -= OnItemPickUp;
    }

    private void OnItemPickUp(string itemId)
    {
        if (itemId == lastWellMap.Item.id)
        {
            didPickUpLastWellMap = true;
        }
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

        // Only Spawn Last Well Map if Player has not picked it up.
        if (lastWellMap != null)
        {
            if (didPickUpLastWellMap)
                lastWellMap.gameObject.SetActive(false);
            else
                lastWellMap.gameObject.SetActive(true);
        }
    }
}