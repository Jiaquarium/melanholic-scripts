using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When game begins, this will initialize a new "seed" to answer Myne's question as 
/// the final quest
/// </summary>
public class Script_ScarletCipher : MonoBehaviour
{
    [SerializeField] private int[] _scarletCipher;
    [SerializeField] private Script_DialogueNode[] myneDialogues;
    public int[] ScarletCipher {
        get
        {
            return _scarletCipher;
        }
        private set
        {
            _scarletCipher = value;
        }
    }

    public void Initialize()
    {
        int[] newCipher = new int[myneDialogues.Length];

        for (int i = 0; i < newCipher.Length; i++)
        {
            int choicesCount = myneDialogues[i].data.children.Length;
            
            /// Choose a random choice for the node Random.Range(inclusive, exclusive)
            int choice = Random.Range(0, choicesCount);
            
            Debug.Log($"choice: {choice}");
            
            newCipher[i] = choice;
        }

        ScarletCipher = newCipher;
    }
}
