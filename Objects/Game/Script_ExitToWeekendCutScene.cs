using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ExitToWeekendCutScene : MonoBehaviour
{
    [SerializeField] private Script_Game game;
    [SerializeField] private Script_PRCSManager PRCSManager;
    [SerializeField] private float keepToWeekendTextUpTime;
    [SerializeField] private float waitInBlackTime;
    
    // Show Cut Scene
    public void Play()
    {
        PRCSManager.OpenPRCSCustom(Script_PRCSManager.CustomTypes.ToWeekend);
    }

    // ------------------------------------------------------------------
    // Unity Event
    
    // On ToWeekend Text done typing
    public void OnCutSceneDone()
    {
        StartCoroutine(WaitToClosePRCS());

        IEnumerator WaitToClosePRCS()
        {
            yield return new WaitForSeconds(keepToWeekendTextUpTime);

            Script_PRCSManager.Control.ClosePRCSCustom(Script_PRCSManager.CustomTypes.ToWeekend);

            yield return new WaitForSeconds(waitInBlackTime);
            
            game.ShowSaveAndStartWeekendMessage();
            game.StartWeekendCycleSaveInitialize();
        }
    }
}
