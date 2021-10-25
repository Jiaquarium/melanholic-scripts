using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TeletypeDialogueContainer : MonoBehaviour
{
    [SerializeField] private Script_TeletypeTextContainer[] texts;
    
    // Start is called before the first frame update
    void OnValidate()
    {
        FindTexts();
    }

    void FindTexts()
    {
        texts = GetComponentsInChildren<Script_TeletypeTextContainer>(true);
    }

    public void InitialState()
    {
        texts = GetComponentsInChildren<Script_TeletypeTextContainer>(true);

        foreach (var t in texts)
            t.Close();
    }
}
