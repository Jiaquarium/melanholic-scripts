using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_NotesHintManager : MonoBehaviour
{
    private enum HintType
    {
        SheepChase = 0,
        PaintingWords = 1,
        Chomp = 2,
        ThirdEye = 3,
        SnowWoman = 4,
        Act2Start = 5,
        Act2Default = 6,
        WellsWorldStart = 7,
        WellsWorldDone = 8,
        CelGardensStart = 9,
        CelGardensDone = 10,
        XXXWorldStart = 11,
        XXXWorldDone = 12,
        GoodEndingStart = 13,
        GoodEndingDone = 14,
        TrueEnding = 15
    }

    [Tooltip("These hint canvases should match up with HintType enums")]
    [SerializeField] private List<Script_NotesHint> notesHints;

    [SerializeField] private Script_Game game;

    [Space][Header("Game Section Cases")][Space]
    [SerializeField] private Script_LevelBehavior_25 elleniasRoom;
    [SerializeField] private Script_LevelBehavior_10 idsRoom;
    [SerializeField] private Script_LevelBehavior_26 spikeRoom;
    
    [SerializeField] private Script_ScarletCipherManager scarletCipherManager;
    [SerializeField] private Script_LevelBehavior_42 wellsWorld;
    [SerializeField] private Script_LevelBehavior_46 labyrinth;
    [SerializeField] private Script_LevelBehavior_24 ktv2;

    private HintType lastActiveHint;

    public void RenderHint()
    {
        HintType currentHintType = GetCurrentHintType();
        UpdateHintCanvases(currentHintType);
    }

    /// <summary>
    /// All test cases here to determine which part of game we're in
    /// </summary>
    private HintType GetCurrentHintType()
    {
        if (game.RunCycle == Script_RunsManager.Cycle.Sunday)
        {
            return HintType.TrueEnding;
        }
        else if (game.RunCycle == Script_RunsManager.Cycle.Weekend)
        {
            // Handle if did good ending but not True Ending yet
            if (game.didGoodEnding)
            {
                return HintType.GoodEndingDone;
            }
            // If all code has been found
            else if (IsAllScarletCipherRevealed())
            {
                return HintType.GoodEndingStart;
            }
            // Handle Wells World hints
            else if (IsInsideWellsWorld())
            {
                // Show hint based on completion state
                if (wellsWorld.isMooseQuestDone)
                    return HintType.WellsWorldDone;
                else
                    return HintType.WellsWorldStart;
            }
            // Handle Cel Garden World hints
            else if (IsInsideCelGardensWorld())
            {
                if (labyrinth.isPuzzleComplete)
                    return HintType.CelGardensDone;
                else
                    return HintType.CelGardensStart;
            }
            // Handle XXX World hints
            else if (IsInsideXXXWorld())
            {
                // If inside without completing both Wells World and Cel Gardens show Act2Default
                // to indicate this isn't a correct path
                if (!wellsWorld.isMooseQuestDone || !labyrinth.isPuzzleComplete)
                    return HintType.Act2Default;
                else if (ktv2.IsPuzzleComplete)
                    return HintType.XXXWorldDone;
                else
                    return HintType.XXXWorldStart;
            }
            else if (IsWeekendFirstDay())
            {
                return HintType.Act2Start;
            }
            else
            {
                return HintType.Act2Default;
            }
        }
        else
        {
            if (IsFirstMonday())
            {
                return HintType.SheepChase;
            }
            // After snow woman before Act II
            else if (GotSnowWoman())
            {
                return HintType.SnowWoman;
            }
            // After Third Eye before Snow Woman
            else if (GotThirdEye())
            {
                return HintType.ThirdEye;
            }
            // After Psychic Duck before Animal Within
            else if (GotAnimalWithin())
            {
                return HintType.Chomp;
            }
            // After Day 1, Player will have Psychic Duck mask
            else
            {
                return HintType.PaintingWords;
            }
        }
    }

    private bool IsFirstMonday() => game.IsFirstMonday;
    private bool GotAnimalWithin() => elleniasRoom.isPuzzleComplete;
    private bool GotThirdEye() => idsRoom.gotBoarNeedle;
    private bool GotSnowWoman() => spikeRoom.gotIceSpikeSticker;

    private bool IsWeekendFirstDay() => game.IsFirstThursday;
    private bool IsAllScarletCipherRevealed() => scarletCipherManager.ScarletCipherRemainingCount == 0;
    private bool IsInsideWellsWorld() => game.GetIsInsideWellsWorld();
    private bool IsInsideCelGardensWorld() => game.GetIsInsideCelestialGardensWorld();
    private bool IsInsideXXXWorld() => game.GetIsInsideXXXWorld();
    
    
    private void UpdateHintCanvases(HintType hintType)
    {
        int lastActiveIdx = (int)lastActiveHint;
        
        notesHints[lastActiveIdx].Close();
        notesHints[(int)hintType].Open();

        lastActiveHint = hintType;
    }

    public void Setup()
    {
        notesHints.ForEach(notesHint => notesHint.Close());
    }
}
