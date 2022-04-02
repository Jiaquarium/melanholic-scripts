using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPCs that always speak even without Psychic Duck.
/// </summary>
public class Script_PsychicNPC : MonoBehaviour
{
    [SerializeField] private bool isAlwaysPsychic;

    public bool IsAlwaysPsychic { get => isAlwaysPsychic; }
}
