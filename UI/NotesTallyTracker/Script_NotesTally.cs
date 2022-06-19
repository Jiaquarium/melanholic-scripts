using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_NotesTally : MonoBehaviour
{
    [SerializeField] private GameObject note;
    
    public void Mark(bool isFound)
    {
        note.gameObject.SetActive(isFound);
    }
}
