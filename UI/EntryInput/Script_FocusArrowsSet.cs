using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_FocusArrowsSet : MonoBehaviour
{
    public static readonly int ClickTrigger = Script_SettingsController.ClickTrigger;

    [SerializeField] private List<Animator> arrows;
    
    public void OnClickUpArrow() => arrows[0].SetTrigger(ClickTrigger);
    
    public void OnClickDownArrow() => arrows[1].SetTrigger(ClickTrigger);
}
