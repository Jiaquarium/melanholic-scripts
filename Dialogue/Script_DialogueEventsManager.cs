using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_DialogueEventsManager : MonoBehaviour
{
    public delegate void DialogeEndAction();
    public static event DialogeEndAction OnDialogueEnd;
    public static void DialogueEndEvent() { if (OnDialogueEnd != null) OnDialogueEnd(); }
}
