using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_DaysTextContainer : MonoBehaviour
{
    [SerializeField] private Image activeImage;
    [SerializeField] private Script_TMProPopulator TMProPopulator;

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

    public void UpdateTMPId(string newId)
    {
        if (TMProPopulator != null)
            TMProPopulator.UpdateTextId(newId);   
    }
}
