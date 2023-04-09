using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Reference: https://github.com/rlabrecque/Steamworks.NET-Example/blob/master/Assets/Scripts/SteamStatsAndAchievements.cs
/// We keep a local state version of the achievements that is the source of truth.
/// On Update() Steamworks API calls are made to match this state, and we set currentAchievements
/// to match this, despite if the API calls worked or not
/// 
/// (e.g. Game is up with Steam not running, achievements will be stored locally but nothing will happen in Steamworks;
/// then, next time Game is running with Steam, once this Manager's Update() runs, API calls will be made to match local state.)
/// </summary>
public class Script_AchievementsManager : MonoBehaviour
{
    private const int Interval = 4;
    
    public const string ACH_PSY_CONN = "ACH_PSY_CONN";
    public const string ACH_WORD = "ACH_WORD";
    public const string ACH_DANCE_PERFECT = "ACH_DANCE_PERFECT";
    public const string ACH_SPIKE_PERFECT = "ACH_SPIKE_PERFECT";
    public const string ACH_BREAK_ICE = "ACH_BREAK_ICE";
    public const string ACH_WELL = "ACH_WELL";
    public const string ACH_SIN = "ACH_SIN";
    public const string ACH_RAVE_STAGE = "ACH_RAVE_STAGE";
    public const string ACH_CCTV_CODE = "ACH_CCTV_CODE";
    public const string ACH_NAUTICAL_DAWN = "ACH_NAUTICAL_DAWN";
    public const string ACH_SEALING = "ACH_SEALING";
    public const string ACH_SADIST = "ACH_SADIST";

    public static string[] achievementIds = new string[]
    {
        ACH_PSY_CONN,
        ACH_WORD,
        ACH_DANCE_PERFECT,
        ACH_SPIKE_PERFECT,
        ACH_BREAK_ICE,
        ACH_WELL,
        ACH_SIN,
        ACH_RAVE_STAGE,
        ACH_CCTV_CODE,
        ACH_NAUTICAL_DAWN,
        ACH_SEALING,
        ACH_SADIST
    };

    public static Script_AchievementsManager Instance;

    public enum CursedCutScenes
    {
        ElderEclaire,
        Ids,
        Ellenia,
        Eileen
    }
    
    public Model_Achievements achievementsState = new Model_Achievements();
    [SerializeField] Model_Achievements currentAchievements = new Model_Achievements();

    [SerializeField] private Script_Game game;

    private bool m_bStoreStats;

	private CGameID m_GameID;
    private bool m_bRequestedStats;
    private bool m_bStatsValid;
    
    protected Callback<UserStatsReceived_t> m_UserStatsReceived;
    protected Callback<UserAchievementStored_t> m_UserAchievementStored;

    void OnEnable()
    {
		if (!SteamManager.Initialized || Const_Dev.IsDemo)
        {
            Debug.LogWarning($"{name} SteamManager is not inited");
			return;
        }

        Dev_Logger.Debug($"{name} setting callbacks. SteamManager is inited");

		// Cache the GameID for use in the Callbacks
		m_GameID = new CGameID(SteamUtils.GetAppID());

		m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
	}

    void Update()
    {
        if (!SteamManager.Initialized || Const_Dev.IsDemo)
			return;

        if (!m_bRequestedStats)
        {
			// Is Steam Loaded? if no, can't get stats, done.
			if (!SteamManager.Initialized || Const_Dev.IsDemo)
            {
				m_bRequestedStats = true;
				return;
			}
			
			// If yes, request our stats.
			bool bSuccess = SteamUserStats.RequestCurrentStats();

			// This function should only return false if we weren't logged in, and we already checked that.
			// But handle it being false again anyway, just ask again later.
			m_bRequestedStats = bSuccess;
		}
        
        // Do not start updating Steamworks' Achievements state until we've received the stats
        // callback, since Achievement API calls will not work until then.
        if (!m_bStatsValid)
            return;
        
        if (achievementsState.achPsyConn && !currentAchievements.achPsyConn)
        {
            currentAchievements.achPsyConn = true;
            UnlockAchievement(ACH_PSY_CONN);
        }
        
        if (achievementsState.achWord && !currentAchievements.achWord)
        {
            currentAchievements.achWord = true;
            UnlockAchievement(ACH_WORD);
        }

        if (achievementsState.achDancePerfect && !currentAchievements.achDancePerfect)
        {
            currentAchievements.achDancePerfect = true;
            UnlockAchievement(ACH_DANCE_PERFECT);
        }

        if (achievementsState.achSpikePerfect && !currentAchievements.achSpikePerfect)
        {
            currentAchievements.achSpikePerfect = true;
            UnlockAchievement(ACH_SPIKE_PERFECT);
        }

        if (achievementsState.achBreakIce && !currentAchievements.achBreakIce)
        {
            currentAchievements.achBreakIce = true;
            UnlockAchievement(ACH_BREAK_ICE);
        }

        if (achievementsState.achWell && !currentAchievements.achWell)
        {
            currentAchievements.achWell = true;
            UnlockAchievement(ACH_WELL);
        }

        if (achievementsState.achSin && !currentAchievements.achSin)
        {
            currentAchievements.achSin = true;
            UnlockAchievement(ACH_SIN);
        }

        if (achievementsState.achRaveStage && !currentAchievements.achRaveStage)
        {
            currentAchievements.achRaveStage = true;
            UnlockAchievement(ACH_RAVE_STAGE);
        }

        if (achievementsState.achCctvCode && !currentAchievements.achCctvCode)
        {
            currentAchievements.achCctvCode = true;
            UnlockAchievement(ACH_CCTV_CODE);
        }

        if (achievementsState.achNauticalDawn && !currentAchievements.achNauticalDawn)
        {
            currentAchievements.achNauticalDawn = true;
            UnlockAchievement(ACH_NAUTICAL_DAWN);
        }

        if (achievementsState.achSealing && !currentAchievements.achSealing)
        {
            currentAchievements.achSealing = true;
            UnlockAchievement(ACH_SEALING);
        }

        if (achievementsState.achSadist && !currentAchievements.achSadist)
        {
            currentAchievements.achSadist = true;
            UnlockAchievement(ACH_SADIST);
        }

        SyncCursedCutSceneState();

        // Throttle to every 4 frames; Steamworks recommends to rate limit.
        // https://partner.steamgames.com/doc/api/ISteamUserStats#StoreStats
        if (Time.frameCount % Interval == 0)
        {
            // If SteamManager is not inited, keep on trying on interval.
            if (m_bStoreStats)
            {
                bool bSuccess = SteamUserStats.StoreStats();
                
                // If this failed, we never sent anything to the server, try again later.
                m_bStoreStats = !bSuccess;
            }
        }
        
        // Keeps currentAchievements in sync with achievementState when loading achievements from state
        // (Note: currentAchievements cursed cut scene states aren't ever used, only achievementState's
        // are used to check for sadist achievement)
        void SyncCursedCutSceneState()
        {
            currentAchievements.didElderSealing = achievementsState.didElderSealing;
            currentAchievements.didIdsR2 = achievementsState.didIdsR2;
            currentAchievements.didElleniaR2 = achievementsState.didElleniaR2;
            currentAchievements.didEileenR2 = achievementsState.didEileenR2;
        }
    }

    public void UnlockPsyConn()
    {
        if (Const_Dev.IsDemo)
            return;
        
        if (!achievementsState.achPsyConn)
        {
            achievementsState.achPsyConn = true;
            SaveLocal();
        }
    }
    
    public void UnlockWord()
    {
        if (Const_Dev.IsDemo)
            return;
        
        if (!achievementsState.achWord)
        {
            achievementsState.achWord = true;
            SaveLocal();
        }
    }
    
    public void UnlockDancePerfect()
    {
        if (Const_Dev.IsDemo)
            return;
        
        if (!achievementsState.achDancePerfect)
        {
            achievementsState.achDancePerfect = true;
            SaveLocal();
        }
    }
    
    public void UnlockSpikePerfect()
    {
        if (Const_Dev.IsDemo)
            return;
        
        if (!achievementsState.achSpikePerfect)
        {
            achievementsState.achSpikePerfect = true;
            SaveLocal();
        }
    }
    
    public void UnlockBreakIce()
    {
        if (Const_Dev.IsDemo)
            return;
        
        if (!achievementsState.achBreakIce)
        {
            achievementsState.achBreakIce = true;
            SaveLocal();
        }
    }
    
    public void UnlockWell()
    {
        if (Const_Dev.IsDemo)
            return;
        
        if (!achievementsState.achWell)
        {
            achievementsState.achWell = true;
            SaveLocal();
        }
    }
    
    public void UnlockSin()
    {
        if (Const_Dev.IsDemo)
            return;
        
        if (!achievementsState.achSin)
        {
            achievementsState.achSin = true;
            SaveLocal();
        }
    }
    
    public void UnlockRaveStage()
    {
        if (Const_Dev.IsDemo)
            return;
        
        if (!achievementsState.achRaveStage)
        {
            achievementsState.achRaveStage = true;
            SaveLocal();
        }
    }
    
    public void UnlockCctvCode()
    {
        if (Const_Dev.IsDemo)
            return;
        
        if (!achievementsState.achCctvCode)
        {
            achievementsState.achCctvCode = true;
            SaveLocal();
        }
    }
    
    public void UnlockNauticalDawn()
    {
        if (Const_Dev.IsDemo)
            return;
        
        if (!achievementsState.achNauticalDawn)
        {
            achievementsState.achNauticalDawn = true;
            SaveLocal();
        }
    }
    
    public void UnlockSealing()
    {
        if (Const_Dev.IsDemo)
            return;
        
        if (!achievementsState.achSealing)
        {
            achievementsState.achSealing = true;
            SaveLocal();
        }
    }
    
    public void UnlockSadist()
    {
        if (Const_Dev.IsDemo)
            return;
        
        if (!achievementsState.achSadist)
        {
            achievementsState.achSadist = true;
            SaveLocal();
        }
    }

    // Set the Cursed Cut Scene, and save Achievements to keep up to date
    public void UpdateCursedCutScene(CursedCutScenes cutScene)
    {
        if (Const_Dev.IsDemo)
            return;
        
        bool didStateUpdate = false;
        
        switch (cutScene)
        {
            case CursedCutScenes.ElderEclaire:
                if (!achievementsState.didElderSealing)
                {
                    achievementsState.didElderSealing = true;
                    currentAchievements.didElderSealing = true;
                    didStateUpdate = true;
                }
                break;
            case CursedCutScenes.Ids:
                if (!achievementsState.didIdsR2)
                {
                    achievementsState.didIdsR2 = true;
                    currentAchievements.didIdsR2 = true;
                    didStateUpdate = true;
                }
                break;
            case CursedCutScenes.Ellenia:
                if (!achievementsState.didElleniaR2)
                {
                    achievementsState.didElleniaR2 = true;
                    currentAchievements.didElleniaR2 = true;
                    didStateUpdate = true;
                }
                break;
            case CursedCutScenes.Eileen:
                if (!achievementsState.didEileenR2)
                {
                    achievementsState.didEileenR2 = true;
                    currentAchievements.didEileenR2 = true;
                    didStateUpdate = true;
                }
                break;
        }

        // If all 4 are done, set achievementsState.achSadist, which will unlock it on the next available frame.
        if (
            achievementsState.didElderSealing
            && achievementsState.didIdsR2
            && achievementsState.didElleniaR2
            && achievementsState.didEileenR2
        )
        {
            UnlockSadist();
        }
        // Save if an update occured to track Cursed cut scenes watched.
        else if (didStateUpdate)
        {
            SaveNextAvailableFrame();
        }
    }
    
    // Keep local state as source of truth.
    private void SaveLocal() => game.SaveAchievements();
    
    private void UnlockAchievement(string achievementId)
    {
        Dev_Logger.Debug($"Unlocking Achievement: {achievementId}, SteamManager.Initialized {SteamManager.Initialized}");
        
        if (SteamManager.Initialized)
        {
            SteamUserStats.SetAchievement(achievementId);
        }
        
        SaveNextAvailableFrame();
    }

    private void SaveNextAvailableFrame()
    {
        m_bStoreStats = true;
    }

    private void OnUserStatsReceived(UserStatsReceived_t pCallback) {
        if (!SteamManager.Initialized || Const_Dev.IsDemo)
        	return;

        // We may get callbacks for other games' stats arriving, ignore them.
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
        	if (EResult.k_EResultOK == pCallback.m_eResult)
            {
        		m_bStatsValid = true;

        		if (Debug.isDebugBuild && Const_Dev.IsDevMode)
                {
                    // Load and print achievements
                    foreach (var achId in achievementIds)
                    {
                        bool isAchieved = false;
                        bool ret = SteamUserStats.GetAchievement(achId, out isAchieved);
                        
                        if (ret)
                        {
                            string achName = SteamUserStats.GetAchievementDisplayAttribute(achId, "name");
                            Dev_Logger.Debug($"Achievement {achName}: {isAchieved}");
                        }
                        else
                            Debug.LogWarning($"SteamUserStats.GetAchievement failed for Achievement {achId} \nIs it registered in the Steam Partner site?");
                    }
                }
        	}
        	else
            {
        		Dev_Logger.Debug($"RequestStats - failed, {pCallback.m_eResult}");
        	}
        }
	}
    
    // Dev only
    private void OnAchievementStored(UserAchievementStored_t pCallback)
    {
		if (!Debug.isDebugBuild)
            return;
        
        if (!SteamManager.Initialized || Const_Dev.IsDemo)
        	return;
        
        // We may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID) {
			if (0 == pCallback.m_nMaxProgress) {
				Dev_Logger.Debug($"Name: {pCallback.m_rgchAchievementName}");
			}
			else {
				Dev_Logger.Debug("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
			}
		}
	}

    // Dev only
    private void ResetSteamStatsAndAchievements()
    {
        if (!SteamManager.Initialized || Const_Dev.IsDemo)
        	return;
        
        Dev_Logger.Debug("Resetting Steam Stats & Achievements");
        
        achievementsState.InitialState();
        currentAchievements.InitialState();
        
        SteamUserStats.ResetAllStats(true);
        
        SaveNextAvailableFrame();
        game.SaveAchievements();
    }

    public void Setup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        } 
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_AchievementsManager))]
    public class Script_AchievementsManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_AchievementsManager t = (Script_AchievementsManager)target;
            if (GUILayout.Button("Unlock ACH_PSY_CONN"))
                t.UnlockPsyConn();

            if (GUILayout.Button("Unlock ACH_WORD"))
                t.UnlockWord();
            
            if (GUILayout.Button("Unlock ACH_DANCE_PERFECT"))
                t.UnlockDancePerfect();

            if (GUILayout.Button("Unlock ACH_SPIKE_PERFECT"))
                t.UnlockSpikePerfect();
            
            if (GUILayout.Button("Unlock ACH_BREAK_ICE"))
                t.UnlockBreakIce();

            if (GUILayout.Button("Unlock ACH_WELL"))
                t.UnlockWell();
            
            if (GUILayout.Button("Unlock ACH_SIN"))
                t.UnlockSin();
            
            if (GUILayout.Button("Unlock ACH_RAVE_STAGE"))
                t.UnlockRaveStage();
            
            if (GUILayout.Button("Unlock ACH_CCTV_CODE"))
                t.UnlockCctvCode();
            
            if (GUILayout.Button("Unlock ACH_NAUTICAL_DAWN"))
                t.UnlockNauticalDawn();
            
            if (GUILayout.Button("Unlock ACH_SEALING"))
                t.UnlockSealing();
            
            if (GUILayout.Button("Unlock ACH_SADIST"))
                t.UnlockSadist();
            
            EditorGUILayout.LabelField("Other", EditorStyles.miniLabel);

            if (GUILayout.Button("Reset All"))
                t.ResetSteamStatsAndAchievements();
        }
    }
    #endif
}
