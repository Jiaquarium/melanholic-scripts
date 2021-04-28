using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_DaysTextContainer : MonoBehaviour
{
    [SerializeField] private Image activeImage;
    [SerializeField] private Script_StringFormatTMP text;

    private bool isCurrentDay;

    public bool IsCurrentDay
    {
        get => isCurrentDay;
        set
        {
            activeImage.gameObject.SetActive(value);
            
            isCurrentDay = value;
        }
    }

    public string Text
    {
        get => text.DynamicText;
        set => text.DynamicText = value;
    }
}
