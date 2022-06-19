using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_NotesTallyTracker : MonoBehaviour
{
    [SerializeField] private List<Script_NotesTally> notesTallies;
    [SerializeField] private Script_ScarletCipherManager scarletCipherManager;
    
    public void UpdateNotesTallyUI()
    {
        for (var i = 0; i < notesTallies.Count; i++)
        {
            // Start at index 4 Scarlet Cipher because we only track the last 6 with tallies. 
            var scarletCipherIdx = i + Script_ScarletCipherManager.IntroRoomNotesCount;
            var isRevealed = scarletCipherManager.ScarletCipherVisibility[scarletCipherIdx];
            
            notesTallies[i].Mark(isRevealed);
        }
    }

    public void Setup()
    {

    }
}
