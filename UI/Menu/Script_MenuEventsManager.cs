using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MenuEventsManager : MonoBehaviour
{
    public delegate void ExitSubmenuAction();
    public static event ExitSubmenuAction OnExitSubmenu;
    public static void ExitSubmenu()
    {
        if (OnExitSubmenu != null)
        {
            Dev_Logger.Debug("ExitSubmenu event");
            OnExitSubmenu();
        }
    }

    public delegate void ExitMenuAction();
    public static event ExitMenuAction OnExitMenu;
    public static void ExitMenu()
    {
        if (OnExitMenu != null)
        {
            Dev_Logger.Debug("ExitMenu event");
            OnExitMenu();
        }
    }
}
