using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_GameEventsManager : MonoBehaviour
{
    public delegate void LevelInitCompleteAction();
    public static event LevelInitCompleteAction OnLevelInitComplete;
    public static void LevelInitComplete() { if (OnLevelInitComplete != null) OnLevelInitComplete(); }

    /// <summary>
    /// Event fires before we inactivate the current level game object
    /// </summary>
    public delegate void LevelBeforeDestroyDelegate();
    public static event LevelBeforeDestroyDelegate OnLevelBeforeDestroy;
    public static void LevelBeforeDestroy() { if (OnLevelBeforeDestroy != null) OnLevelBeforeDestroy(); }
}
