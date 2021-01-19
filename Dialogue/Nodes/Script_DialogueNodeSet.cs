using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use to specify a full set of Nodes we want to switch out
/// </summary>
public class Script_DialogueNodeSet : MonoBehaviour
{
    [SerializeField] private Script_DialogueNode[] _nodes;

    public Script_DialogueNode[] Nodes
    {
        get => _nodes;
    }
}
