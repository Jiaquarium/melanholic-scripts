using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Steamworks;

/// <summary>
/// Test if Steamworks API is activated.
/// </summary>
public class Dev_SteamworksTester : MonoBehaviour
{
    [SerializeField] private Script_CanvasGroupController steamTestCanvas;
    [SerializeField] private TextMeshProUGUI steamTestText;
    [SerializeField] private float canvasWaitToCloseTime;

    void Awake()
    {
        bool isTestingSteamworks = Const_Dev.IsSpecsDisplayOn || Debug.isDebugBuild;
        this.enabled = isTestingSteamworks;
        steamTestCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.L))
            ShowPersonaName();
        
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
            SetSteamAchievement();
        
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha0))
            ResetSteamStatsAndAchievements();
    }

    private void ShowPersonaName()
    {
        if (steamTestCanvas != null)
        {
            steamTestText.text = GetPersonaName();
            steamTestCanvas.Open();
            StartCoroutine(WaitToClose());
        }

        IEnumerator WaitToClose()
        {
            yield return new WaitForSeconds(canvasWaitToCloseTime);

            steamTestCanvas.Close();
        }

        string GetPersonaName() => SteamManager.Initialized
            ? SteamFriends.GetPersonaName()
            : $"NOT INITED";
    }

    private bool SetSteamAchievement()
    {
        Dev_Logger.Debug("Setting Steam Achievement");

        SteamUserStats.SetAchievement("ACH_PSY_CONN");
        bool isSuccess = SteamUserStats.StoreStats();

        return isSuccess;
    }

    private void ResetSteamStatsAndAchievements()
    {
        Dev_Logger.Debug("Resetting Steam Stats & Achievements");
        
        SteamUserStats.ResetAllStats(true);
    }
}
