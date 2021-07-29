using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
     
public class Dev_Logger : MonoBehaviour
{
    static string myLog = "";
    
    [SerializeField] private CanvasGroup devLoggerCanvasGroup; 
    [SerializeField] private TextMeshProUGUI TMP;
    
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
            TMP.text = myLog;
    }

    // Always log in background unless release builds.
    public void Log(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
        myLog = output + "\n" + myLog;
        if (myLog.Length > 5000)
        {
            myLog = myLog.Substring(0, 4000);
        }
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