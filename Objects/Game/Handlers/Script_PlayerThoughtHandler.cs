using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerThoughtHandler : MonoBehaviour
{
    /// <summary>
    /// Inserts to the front of thoughts List
    /// </summary>
    public void AddPlayerThought(
        Model_Thought thought,
        Model_PlayerThoughts thoughts
    )
    {
        thoughts.uglyThoughts.Insert(0, thought);
    }

    public void RemovePlayerThought(
        Model_Thought thought,
        Model_PlayerThoughts thoughts
    )
    {
        thoughts.uglyThoughts.Remove(thought);
    }

    public int GetThoughtsCount(Model_PlayerThoughts thoughts)
    {
        return thoughts.uglyThoughts.Count;
    }
}
