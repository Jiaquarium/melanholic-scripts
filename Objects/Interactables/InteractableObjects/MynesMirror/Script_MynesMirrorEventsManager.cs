using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MynesMirrorEventsManager : MonoBehaviour
{
    public delegate void OnEndTimelineDelegate();
    public static event OnEndTimelineDelegate OnEndTimeline;
    public static void EndTimeline() {
        if (OnEndTimeline != null) OnEndTimeline();
    }

    public delegate void InteractionNodeDone();
    public static event InteractionNodeDone OnInteractionNodeDone;
    public static void EndInteractionNode()
    {
        if (OnInteractionNodeDone != null) OnInteractionNodeDone();
    }
}
