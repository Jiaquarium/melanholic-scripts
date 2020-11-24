using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Script_Entry : Script_Slot
{
    // public int Id;
    public string text;
    public string nameId;
    public DateTime timestamp;
    public string headline;

    [SerializeField] private TextMeshProUGUI timestampText;
    [SerializeField] private TextMeshProUGUI headlineText;

    public void Edit(string _text, DateTime _timestamp)
    {
        text = _text;
        timestamp = _timestamp;

        timestampText.text = _timestamp.FormatDateTime();
    }
    
    public void Setup(
        int _Id,
        string _nameId,
        string _text,
        DateTime _timestamp,
        string _headline
    )
    {
        Id = _Id;
        nameId = _nameId;
        text = _text;
        timestamp = _timestamp;
        headline = _headline;

        timestampText.text = _timestamp.FormatDateTime();
        headlineText.text = _headline;
    }
}
