using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ExitToWeekendCutScene : MonoBehaviour
{
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_PRCSManager PRCSManager;
    [SerializeField] private Script_PRCS toWeekendPRCS;
    
    // Show Cut Scene
    public void Play()
    {
        PRCSManager.OpenPRCSCustom(Script_PRCSManager.CustomTypes.ToWeekend);
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void OnCutSceneDone()
    {
        game.ShowSaveAndStartWeekendMessage();
        game.StartWeekendCycleSaveInitialize();   
    }
}
