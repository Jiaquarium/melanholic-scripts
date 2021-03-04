using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TriggerStay should send this the objects inside the Trigger, and
/// this controller will check to ensure the required objects are present.
/// </summary>
public class Script_MeetupPuzzleController : Script_PuzzleController
{
    public List<Script_Player> playersOnTrigger;
    [SerializeField] private List<Script_Player> targetPlayersOnTrigger;
    [SerializeField] private bool isDone;
    
    void Update()
    {
        if (CheckMatchingPlayersOnTrigger())
            CompleteState();
    }
    
    public override void CompleteState()
    {
        if (isDone)     return;

        Debug.Log("PUZZLE IS DONE!!!!!!!!! BOTH TARGETS ON PLATFORM");
        
        isDone = true;
    }

    private bool CheckMatchingPlayersOnTrigger()
    {
        if (playersOnTrigger.Count != targetPlayersOnTrigger.Count)     return false;
        
        foreach (Script_Player player in targetPlayersOnTrigger)
        {
            if (playersOnTrigger.Find(target => player == target) == null)    return false;
        }

        return true;
    }
}
