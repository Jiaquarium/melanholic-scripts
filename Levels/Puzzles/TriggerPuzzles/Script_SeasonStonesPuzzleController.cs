using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Script_SeasonStonesPuzzleController : Script_TriggerPuzzleController
{
    public Script_PushableTriggerStay rockTrigger;
    [SerializeField] private Script_LevelBehavior_20 LB20;
    [SerializeField] private List<int> stoneIds;

    private void Update()
    {
        
    }
    
    private Dictionary<string, int> SeasonStonesIntIds = new Dictionary<string, int>{
        {"collectible_winter-stone",    0},
        {"collectible_spring-stone",    1},
        {"collectible_summer-stone",    2},
        {"collectible_autumn-stone",    3},
    };
    
    public override void TriggerActivated(string Id, Collider other)
    {
        Debug.Log($"Activating with collectible {Id}; collider {other}");
        
        /// If it's a success case, we'll handle the season change here instead
        /// to be sure to prevent any race cases
        if (CheckSuccessCase())
        {
            // Success by moving pushable Rock
            if (Id == rockTrigger.Id)
            {
                // pass the rock trigger Id
                Script_PuzzlesEventsManager.PuzzleSuccess(Id);
            }
            // Success by dropping a stone
            else
            {
                // otherwise give the season stone Id to know which season to change to 
                if (other.transform.parent.GetComponent<Script_CollectibleObject>() != null)
                {
                    Script_PuzzlesEventsManager.PuzzleSuccess(
                        other.transform.parent
                            .GetComponent<Script_CollectibleObject>().GetItem().id
                    );
                }
            }
        }
        /// Feedback for Rock Pushable
        else if (Id == rockTrigger.Id)
        {
            Script_PuzzlesEventsManager.PuzzleProgress();
        }
        /// Change the season based on the season stone dropped, but ONLY if it'll lead to solution
        else if (CheckForSeasonsChange())
        {
            // change garden sprite & SFX
            if (other.transform.parent.GetComponent<Script_CollectibleObject>() != null)
            {
                LB20.ChangeSeason(
                    other.transform.parent
                        .GetComponent<Script_CollectibleObject>().GetItem().id
                );
            }   
        }
    }

    private bool CheckSuccessCase()
    {
        return !LB20.isPuzzleComplete
            && CheckTriggersAllOccupied()
            && CheckForSeasonsOrdering()
            && rockTrigger.isOn;
    }

    protected override bool CheckTriggersAllOccupied()
    {
        foreach (
            Script_CollectibleTriggerStay trigger in
            transform.GetChildren<Script_CollectibleTriggerStay>()
        )
        {
            Debug.Log("CHECKING SUCCESS, TRIGGER NOT ALL OCCUPIED");
            if (!trigger.isOn)  return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if correct ordering by clockwise
    /// </summary>
    private bool CheckForSeasonsOrdering()
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            Debug.Log($"checking trigger[{i}]");

            int IdDiff;
            int currIdx = i;
            int nextIdx = i == triggers.Length - 1 ? 0 : i + 1;
            
            // don't check if there are multiple drops in space
            if (triggers[currIdx].GetComponent<Script_CollectibleTriggerStay>().collectibles.Count > 1)
                return false;

            Debug.Log($"current trigger collectible: " + triggers[currIdx].GetComponent<Script_CollectibleTriggerStay>());
            string currId = triggers[currIdx]
                .GetComponent<Script_CollectibleTriggerStay>().collectibles[0].GetItem().id;
            string nextId = triggers[nextIdx]
                .GetComponent<Script_CollectibleTriggerStay>().collectibles[0].GetItem().id;
 
            int currSeasonStoneId;
            int nextSeasonStoneId;
            if (!SeasonStonesIntIds.TryGetValue(currId, out currSeasonStoneId))
                return false;
            if (!SeasonStonesIntIds.TryGetValue(nextId, out nextSeasonStoneId))
                return false;

            IdDiff = nextSeasonStoneId - currSeasonStoneId;
            // if our Id is 3, the diff should winter:0 so 0 - 3
            if (currSeasonStoneId == 3)
            {
                 if (Mathf.Abs(IdDiff) != 3)    return false;
            }
            // if diff isn't 1, it's not a subsequent season
            else if (IdDiff != 1)    return false;
        }
        
        Debug.Log("CHECKING SUCCESS, STONES ORDERED CORRECTLY");
        return true;
    }

    /// <summary>
    /// We test the solution with the current stone laid down
    /// </summary>
    private bool CheckForSeasonsChange()
    {
        Debug.Log("Checking for Season Change animation FEEDBACK");
        /// Map the stones to the array that corresponds to the triggers [S, W, N, E]
        stoneIds = new List<int>{-1, -1, -1, -1};
        if (triggers.Length != stoneIds.Count) Debug.LogError("You need to match trigger length with number of stoneIds");
        for (int i = 0; i < stoneIds.Count; i++)
        {
            int stoneId;
            Script_CollectibleTriggerStay t = triggers[i].GetComponent<Script_CollectibleTriggerStay>();
            
            if (t.collectibles.Count == 0)  continue;
            
            string currId = t.collectibles[0]?.GetItem()?.id;
            if (!SeasonStonesIntIds.TryGetValue(currId, out stoneId))   continue;
            stoneIds[i] = stoneId;
        }

        /// Fill in nulls between the Ids (emulates if you were to start at first trigger (S) and work your
        /// way around by laying the proper stones)
        /// Don't fill in triggers you already have a stone in
        int lastId = -1;
        for (int i = 1; i < stoneIds.Count; i++)
        {
            if (i == 1) lastId = stoneIds[0];

            // if last & current null, skip
            if (lastId == -1 && stoneIds[i] == -1)  continue;
            // if last not null but current null, set current to what it should be
            else if (lastId != -1 && stoneIds[i] == -1)
            {
                stoneIds[i] = HandleLastOutOfIndex(lastId + 1);
            }
            
            lastId = stoneIds[i];
        }

        /// Now all -1 in between spaces with stones are filled
        /// Remove all -1 on both sides and ensure what we have left is consecutive
        /// Which could lead to a solution
        stoneIds = stoneIds.Where(x => x != -1).ToList();
        
        // /// Finally check the list for correct ordering
        if (stoneIds.Count == 1)    return true;
        for (int i = 1; i < stoneIds.Count; i++)
        {
            int currentShouldBe = HandleLastOutOfIndex(stoneIds[i - 1] + 1);
            
            Debug.Log($"currentShouldBe: {currentShouldBe}; actually: {stoneIds[i]}");

            if (currentShouldBe != stoneIds[i])     return false;
        }

        return true;

        int HandleLastOutOfIndex(int num)
        {
            if (num == triggers.Length)     return 0;
            else                            return num; 
        }
    }
}
