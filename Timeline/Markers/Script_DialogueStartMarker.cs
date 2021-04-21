using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class Script_DialogueStartMarker : Marker, INotification
{
    public PropertyName id { get; }

    public int dialogueNodeIndex;
    public bool isSilent;
}
