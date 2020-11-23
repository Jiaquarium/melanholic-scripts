using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_DialogueImageBackground : MonoBehaviour
{
    public TextMeshProUGUI extraText;
    
    void Start()
    {
        HandleImageEnabled();
    }

    void Update()
    {
        HandleImageEnabled();
    }

    private void HandleImageEnabled()
    {
        GetComponent<Image>().enabled = !string.IsNullOrEmpty(extraText.text);
    }
}
