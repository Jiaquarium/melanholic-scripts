using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_8 : Script_LevelBehavior
{
    public const string MapName = Script_Names.HallwayToBasement;
    
    [SerializeField] private Script_LevelBehavior_6 mirrorPuzzleBehavior;
    [SerializeField] private Script_TileMapExitEntrance entrance;

    public override void Setup()
    {
        // If mirror puzzle isn't done, cannot go backwards (e.g. tunneled here via
        // Piano Sticker)
        entrance.IsDisabled = !mirrorPuzzleBehavior.isPuzzleCompleted;
    }
}
