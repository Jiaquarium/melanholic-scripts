using UnityEngine;
using UnityEngine.Rendering;

public class Dev_DebugManager : MonoBehaviour
{
    private void Awake()
    {
        DebugManager.instance.enableRuntimeUI = false;
        Dev_Logger.Debug($"Preventing URP Debugger error messages, set DebugManager.instance.enableRuntimeUI to {DebugManager.instance.enableRuntimeUI}");
    }
}
