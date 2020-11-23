using System.Collections;
using System.Collections.Generic;
using UnityEngine;
     
public class Dev_Logger : MonoBehaviour
{
    static string myLog = "";
    private string output;
    private string stack;

    void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

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

    void OnGUI()
    {
        int w = Screen.width / 2;
        int h = Screen.height - 10;

        GUIStyle style = new GUIStyle();
        style.fontSize = h * 1 / 100;
        style.normal.textColor = new Color (255f, 255f, 255f, 1.0f);

        myLog = GUI.TextArea(new Rect(10, 10, w, h), myLog, 5000, style);
    }
}