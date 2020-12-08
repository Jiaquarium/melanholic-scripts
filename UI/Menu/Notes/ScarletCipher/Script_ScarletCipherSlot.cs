using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_ScarletCipherSlot : MonoBehaviour
{
    public int Id; 
    public TextMeshProUGUI TMP;
    [TextArea(1,1)][SerializeField] private string defaultValue;

    public void Setup(int? dialogueChoiceIdx)
    {
        TMP.text = dialogueChoiceIdx == null
            ? defaultValue
            : dialogueChoiceIdx.ToString();
    }
}
