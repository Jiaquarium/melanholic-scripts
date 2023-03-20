using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This component is meant to be attached onto Button Outline, and will be able to adjust font style
/// whenevre the outline is activated by ButtonHighlighter
/// </summary>
[RequireComponent(typeof(Image))]
public class Script_ButtonHighlighterOutlineHelper : MonoBehaviour
{
    [SerializeField] private FontStyles myStyle;
    [SerializeField] private FontStyles defaultStyle;
    [SerializeField] private TextMeshProUGUI TMP;

    void OnEnable()
    {
        TMP.fontStyle = myStyle;
    }

    void OnDisable()
    {
        TMP.fontStyle = defaultStyle;
    }
}
