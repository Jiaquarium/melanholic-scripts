using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Script_SettingsResolutionChoice : MonoBehaviour
{
    public Vector2Int resolution;
    
    [SerializeField] private TextMeshProUGUI myText;
    [SerializeField] private Script_SettingsSystemController systemController;

    public Button MyButton => GetComponent<Button>(); 
    public Script_ButtonHighlighter ButtonHighlighter => GetComponent<Script_ButtonHighlighter>(); 
    
    void Awake()
    {
        UpdateText();
    }

    void OnValidate()
    {
        UpdateText();
    }

    // ------------------------------------------------------------
    // Unity Events

    public void SetResolution()
    {
        systemController.SetResolution(this);
    }

    // ------------------------------------------------------------

    private void UpdateText()
    {
        myText.text = $"{resolution.x} x {resolution.y}";
    }
}
