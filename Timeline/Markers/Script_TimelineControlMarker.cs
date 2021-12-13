using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class Script_TimelineControlMarker : Marker, INotification
{
    public PropertyName id { get; }

    public bool isPauseTimelineWaitForInput;

    public bool isAction = false;
    public int pauseActionIndex = -1;
}
