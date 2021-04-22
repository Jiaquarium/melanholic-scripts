using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class Script_DialogueStartMarker : Marker, INotification
{
    public PropertyName id { get; }

    public int dialogueNodeIndex;
    
    [Tooltip(
        "Set to pause Timeline. Set in conjunction with 'Wait For Timeline' flag on Dialogue Section to wait"
        + " for Timeline Control instead of dialogue continuation input."
    )]
    public bool isPauseTimeline;
    public bool isSilent;
}
