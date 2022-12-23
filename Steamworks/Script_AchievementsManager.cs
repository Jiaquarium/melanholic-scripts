using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    private CGameID gameID;
    private bool isStoreStatsThisFrame;
    private bool isStoreLocalStatsThisFrame;

    protected Callback<UserAchievementStored_t> userAchievementStored;

    void OnEnable()
    {
		if (!SteamManager.Initialized)
        {
            Debug.LogError($"{name} SteamManager is not inited");
			return;
        }

		// Cache the GameID for use in the Callbacks
		gameID = new CGameID(SteamUtils.GetAppID());

		userAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
	}

    void Update()
    {
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
            HandleSavingSteam();
        
        // Local state can save immediately since we only try once
        HandleSavingLocalState();

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

    public void UnlockPsyConn() => achievementsState.achPsyConn = true;
    public void UnlockWord() => achievementsState.achWord = true;
    public void UnlockDancePerfect() => achievementsState.achDancePerfect = true;
    public void UnlockSpikePerfect() => achievementsState.achSpikePerfect = true;
    public void UnlockBreakIce() => achievementsState.achBreakIce = true;
    public void UnlockWell() => achievementsState.achWell = true;
    public void UnlockSin() => achievementsState.achSin = true;
    public void UnlockRaveStage() => achievementsState.achRaveStage = true;
    public void UnlockCctvCode() => achievementsState.achCctvCode = true;
    public void UnlockNauticalDawn() => achievementsState.achNauticalDawn = true;
    public void UnlockSealing() => achievementsState.achSealing = true;
    public void UnlockSadist() => achievementsState.achSadist = true;

    // Set the Cursed Cut Scene, and save Achievements to keep up to date
    public void UpdateCursedCutScene(CursedCutScenes cutScene)
    {
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
    
    private void HandleSavingSteam()
    {
        if (isStoreStatsThisFrame)
        {
			bool bSuccess = SteamUserStats.StoreStats();
			
            // If this failed, we never sent anything to the server, try again later.
			isStoreStatsThisFrame = !bSuccess;
		}
    }
    
    private void HandleSavingLocalState()
    {
        if (isStoreLocalStatsThisFrame)
        {
            // Keep local state in sync with Steam, just try once, since not vital to have
            // this perfectly up to date (since the Achievement will be saved in Steamworks).
            game.SaveAchievements();
            isStoreLocalStatsThisFrame = false;
        }
    }
    
    private void UnlockAchievement(string achievementId)
    {
        Dev_Logger.Debug($"Unlocking Achievement: {achievementId}");
        
        SteamUserStats.SetAchievement(achievementId);
        SaveNextAvailableFrame();
    }

    private void SaveNextAvailableFrame()
    {
        isStoreStatsThisFrame = true;
        isStoreLocalStatsThisFrame = true;
    }

    // Dev only
    private void OnAchievementStored(UserAchievementStored_t pCallback)
    {
		if (!Debug.isDebugBuild)
            return;
        
        // We may get callbacks for other games' stats arriving, ignore them
		if ((ulong)gameID == pCallback.m_nGameID) {
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
