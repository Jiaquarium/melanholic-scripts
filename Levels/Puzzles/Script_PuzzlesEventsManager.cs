using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// level behaviors can subscribe to these events for puzzle events
/// </summary>
public class Script_PuzzlesEventsManager : MonoBehaviour
{
    public delegate void PuzzleProgressAction();
    public static event PuzzleProgressAction OnPuzzleProgress;
    public static void PuzzleProgress() { if (OnPuzzleProgress != null) OnPuzzleProgress(); }
    /// <summary>
    /// used for a different progress feedback, e.g. more dramatic or indication of almost done
    /// </summary>
    public delegate void PuzzleProgressAction2();
    public static event PuzzleProgressAction OnPuzzleProgress2;
    public static void PuzzleProgress2() { if (OnPuzzleProgress2 != null) OnPuzzleProgress2(); }
    public delegate void PuzzleSuccessAction(string arg);
    public static event PuzzleSuccessAction OnPuzzleSuccess;
    public static void PuzzleSuccess(string arg)
    {
        if (OnPuzzleSuccess != null)
        {
            Debug.Log("Puzzle success event triggered----------------------------------");
            OnPuzzleSuccess(arg);
        }
    }
    public delegate void PuzzleResetAction();
    public static event PuzzleResetAction OnPuzzleReset;
    public static void PuzzleReset() { if (OnPuzzleReset != null) OnPuzzleReset(); }
}
