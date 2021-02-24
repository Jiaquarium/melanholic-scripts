using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PuzzleController : MonoBehaviour
{
    [SerializeField] private string puzzleId;
    
    public string PuzzleId
    {
        get => puzzleId;
    }

    protected virtual void OnEnable()
    {
        Script_PuzzlesEventsManager.OnPuzzleReset += InitialState;
    }

    protected virtual void OnDisable()
    {
        Script_PuzzlesEventsManager.OnPuzzleReset -= InitialState;
    }
    
    public virtual void InitialState() { }

    public virtual void CompleteState() { }

    public virtual void Setup() { }
}
