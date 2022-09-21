using System.Diagnostics;

public static class Dev_Logger
{
    [Conditional("ENABLE_LOGS")]
    public static void Debug(string logMsg)
    {
        UnityEngine.Debug.Log(logMsg);
    }
}
