using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text.RegularExpressions;
     
public class Dev_Logger : MonoBehaviour
{
    static private int maxChars = 10000;
    static private int maxLinesPerDisplay = 80;
    static string myLog = new string(' ', maxChars);

    [SerializeField] private CanvasGroup devLoggerCanvasGroup; 
    [SerializeField] private TextMeshProUGUI displayText0;
    [SerializeField] private TextMeshProUGUI displayText1;
    
    // State
    [SerializeField] private bool isActive;

    private string output;
    private string stack;

    void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    void Awake()
    {
        if (!Const_Dev.IsDevMode)
            Debug.LogWarning($"{name} is active in this production build");
        
        devLoggerCanvasGroup.gameObject.SetActive(isActive);
        
        if (!Const_Dev.IsLoggerAvailable)
        {
            isActive = false;
            gameObject.SetActive(false);
            devLoggerCanvasGroup.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    void Update()
    {
        HandleInput();

        if (isActive)
        {
            // Put in displays Lines they can hold.
            string[] logs = Regex.Split(myLog, "\r\n|\r|\n");
            
            displayText0.text = String.Join("\n", logs, 0, maxLinesPerDisplay);
            displayText1.text = String.Join("\n", logs, maxLinesPerDisplay, maxLinesPerDisplay);
        }
    }

    // Always log in background unless release builds.
    public void Log(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
        myLog = output + "\n" + myLog;
        
        // Shorten log to what Displays can hold.
        myLog = myLog.Substring(0, maxChars);
    }

    void HandleInput()
    {
        if (Input.GetButtonDown(Const_KeyCodes.Dev))
        {
            isActive = !isActive;
            devLoggerCanvasGroup.gameObject.SetActive(isActive);
        }
    }
}