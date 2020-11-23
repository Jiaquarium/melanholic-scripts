using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_StartEventsManager : MonoBehaviour
{
    public delegate void ExitSubmenuAction();
    public static event ExitSubmenuAction OnExitSubmenu;
    public static void ExitSubmenu() { if (OnExitSubmenu != null) OnExitSubmenu(); }

    public delegate void ExitFileActionsAction();
    public static event ExitFileActionsAction OnExitFileActions;
    public static void ExitFileActions() { if (OnExitFileActions != null) OnExitFileActions(); }
}
