using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_PasswordChooser))]
public class Script_LevelBehavior_21 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    [Tooltip("Currently unused; for dialogue, only track current day talking state with didSpeakWithEileenToday")]
    public bool spokenWithEileen;
    public bool didOnEntranceAttack;
    /* ======================================================================= */
    
    // Got to Eileen PW Node
    [SerializeField] private bool didSpeakWithEileenToday;
    
    // If talked with Eileen but did not get to the PW node
    [SerializeField] private bool didInitiateWithEileenToday;

    [SerializeField] private Script_LevelBehavior_25 LB25;
    [SerializeField] private Script_LevelBehavior_26 LB26;
    
    [SerializeField] private Script_DemonNPC Eileen;
    [SerializeField] private Script_DemonNPC EileenElleniaHurt;
    [SerializeField] private Script_Bed EileensBed;
    
    /// <summary>
    /// This room uses a global BgThemePlayer to not be too repetitive when entering / exiting.
    /// </summary>
    [SerializeField] private Script_BgThemePlayer EileenThemePlayer;
    
    [SerializeField] private Script_InteractableObjectTextParent textParent;
    [SerializeField] private Transform fullArtParent;
    [SerializeField] private Script_InteractableFullArt noteFullArt;
    
    [SerializeField] private TimelineAsset playerDropTimeline;
    
    [SerializeField] private Script_PlayerDropSFXOnEnable dropSFX;
    
    [SerializeField] private float onEntranceActivateSpikesTime;
    [SerializeField] private float onEntranceAttackFreezeTime;
    
    [SerializeField] private Script_UrselkAttacks urselkAttacks;
    [SerializeField] private Script_DemonIntervalAttackController defaultEileenSpikeController;
    
    private List<GameObject> playerObjsToBind = new List<GameObject>();

    private bool isOnEntranceAttackFrozen;
    private PlayableDirector playerPlayableDirector;
    
    private bool isInitialize = true;
    private bool isElleniaHurt;
    private bool isTimelineControlled = false;

    // ------------------------------------------------------------------
    // Dev Only
    [SerializeField] private bool isHideSpikes;
    [SerializeField] private List<Script_DemonIntervalAttackController> spikeControllers;

    public bool DidSpeakWithEileenToday
    {
        get => didSpeakWithEileenToday;
    }
    
    public bool DidInitiateWithEileenToday
    {
        get => didInitiateWithEileenToday;
    }

    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete    += OnLevelInitCompleteEvent;
        Script_CombatEventsManager.OnEnemyAttackEnd     += HandleOnEntranceAttackDone;
        
        playerPlayableDirector = game.GetPlayer().GetComponent<PlayableDirector>();
        playerPlayableDirector.stopped                  += OnDropTimelineDone;
        
        // The first time player enters, need to trigger the Attack sequence
        if (!didOnEntranceAttack)
        {
            Script_HUDManager.Control.IsForceUp = true;
            defaultEileenSpikeController.IsDisabled = true;
        }
        
        if (didSpeakWithEileenToday)
            OnFinishedTalking();
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
        Script_CombatEventsManager.OnEnemyAttackEnd     -= HandleOnEntranceAttackDone;
        
        playerPlayableDirector.stopped -= OnDropTimelineDone;
        playerPlayableDirector = null;
        
        if (EileenThemePlayer != null)
        {
            AudioSource audio = EileenThemePlayer?.GetComponent<AudioSource>();
            audio.volume = 0f;
            audio.Pause();
        }

        if (!isTimelineControlled)
        {
            game.UnPauseBgMusic();
        }

        defaultEileenSpikeController.IsDisabled = false;

        isTimelineControlled = false;
    }
    
    // ----------------------------------------------------------------------
    // Next Node Action
    public void UpdateSistersNames()
    {
        Script_Names.UpdateEileen();
        Script_Names.UpdateEllenia();
    }
    
    // Node: she has talent
    public void OnElleniaPassword()
    {
        if (string.IsNullOrEmpty(Script_Names.ElleniaPassword))
        {
            SetNewElleniaPassword();
        }
    }

    // RevealPW Node
    public void OnFinishedTalking()
    {
        Dev_Logger.Debug("OnFinishedTalking() switching out Eileen's dialogue nodes now...");
        spokenWithEileen = true;
        didSpeakWithEileenToday = true;
        Eileen.MyDialogueState = Script_DemonNPC.DialogueState.Talked;
    }

    public void OnFirstPsychicNodeDone()
    {
        didInitiateWithEileenToday = true;
    }
    
    // ----------------------------------------------------------------------
    // Auto Choice Action
    public void OnDidTalkToEllenia(OnNextNodeChoiceArgs modifiableEventArgs)
    {
        Dev_Logger.Debug("Spoken with Ellenia: " + LB25.spokenWithEllenia);

        if (LB25.spokenWithEllenia)     modifiableEventArgs.choice = 1;
        else                            modifiableEventArgs.choice = 0;
    }
    // ----------------------------------------------------------------------

    public void SetNewElleniaPassword()
    {
        string newPassword = GetComponent<Script_PasswordChooser>().GetRandomWord();
        Script_Names.UpdateElleniaPassword(newPassword);
        Dev_Logger.Debug($"Set ElleniaPassword to {Script_Names.ElleniaPassword}");
    }
    
    /// <summary>
    /// Can only drop from rounded ints because we do rounding on entrance
    /// </summary>
    public void PlayerEntranceFromEileenMind()
    {
        Script_Player player = Script_Game.Game.GetPlayer();
        
        /// To prevent input until after animation is done
        player.SetIsStandby();

        playerObjsToBind.Add(player.gameObject);
        playerObjsToBind.Add(dropSFX.gameObject);
        
        playerPlayableDirector.BindTimelineTracks(playerDropTimeline, playerObjsToBind);
        playerPlayableDirector.Play(playerDropTimeline);
    }

    private void OnDropTimelineDone(PlayableDirector aDirector)
    {
        if (aDirector.playableAsset == playerDropTimeline)
        {
            Script_Game.Game.GetPlayer().UpdateLocation();
            Script_Game.Game.GetPlayer().SetIsInteract();
        }
    }

    // Initial spike attack when Player first enters
    private void OnLevelInitCompleteEvent()
    {
        if (isElleniaHurt)
        {
            // Track Eileen Cursed for Achievement after level init (black fade out) is done
            Script_AchievementsManager.Instance.UpdateCursedCutScene(
                Script_AchievementsManager.CursedCutScenes.Eileen
            );
        }
        
        if (didOnEntranceAttack)
            return;
        
        game.ChangeStateCutScene();
        urselkAttacks.AlternatingSpikesAttack();
        didOnEntranceAttack = true;
        isOnEntranceAttackFrozen = true;
        
        StartCoroutine(WaitToInteractSafe());
        StartCoroutine(WaitToEnableEileenSpikes());
        
        /// To be extra safe so player doesn't freeze on entrance
        /// if HandleOnEntranceAttackDone doesn't fire
        /// 9/29 playtest: frozen even after hit 
        IEnumerator WaitToInteractSafe()
        {
            yield return new WaitForSeconds(onEntranceAttackFreezeTime);

            Dev_Logger.Debug("Safe set to interact after Attack");

            if (isOnEntranceAttackFrozen)
                OnEntranceAttackDone();
        }

        IEnumerator WaitToEnableEileenSpikes()
        {
            yield return new WaitForSeconds(onEntranceActivateSpikesTime);
            defaultEileenSpikeController.IsDisabled = false;
        }
    }

    private void HandleOnEntranceAttackDone(string hitBoxId)
    {
        if (hitBoxId == Const_HitBox.EileenEnergySpikeEntrance)
        {
            OnEntranceAttackDone();
        }
    }

    private void OnEntranceAttackDone()
    {
        game.ChangeStateInteract();
        isOnEntranceAttackFrozen = false;
        Script_HUDManager.Control.IsForceUp = false;
    }

    private void BaseSetup()
    {
        EileenElleniaHurt.gameObject.SetActive(false);
        
        game.SetupInteractableFullArt(fullArtParent, isInitialize);
        
        // Handle Eileen leaving the room after you "fix" her mind
        if (LB26.isCurrentPuzzleComplete)
        {
            HandleEileenDefault(isPuzzleDone: true);
            EileensBed.SwitchToBedIsOccupied(isOccupied: false);
        }
        else
        {
            HandleEileenDefault(isPuzzleDone: false);
            
            if (didSpeakWithEileenToday)
                Eileen.MyDialogueState = Script_DemonNPC.DialogueState.Talked;
            
            // We need to set a new password, since the talked nodes don't call the password
            // set function and we are not saving the password from run to run
            if (LB26.isPuzzleComplete)
                OnElleniaPassword(); 

            // Eileen Ellenia Hurt Event Cycle.
            if (game.RunCycle == Script_RunsManager.Cycle.Weekend)
            {
                isElleniaHurt = Script_EventCycleManager.Control.IsElleniaHurt();
                
                HandleEileenWeekend(isElleniaHurt);
                EileensBed.SwitchToBedIsOccupied(isOccupied: isElleniaHurt);
            }
            else
                EileensBed.SwitchToBedIsOccupied(isOccupied: false);
        }
        
        isInitialize = false;

        /// <summary>
        /// After Weekend Cursed Time, Eileen should be sleeping.
        /// </summary>
        void HandleEileenWeekend(bool isElleniaHurt)
        {
            Eileen.gameObject.SetActive(!isElleniaHurt);
            EileenElleniaHurt.gameObject.SetActive(isElleniaHurt);
        }

        void HandleEileenDefault(bool isPuzzleDone)
        {
            Eileen.gameObject.SetActive(!isPuzzleDone);

            noteFullArt.gameObject.SetActive(isPuzzleDone);
        }
    }

    // ----------------------------------------------------------------------
    // Timeline Signals

    public void TimelineSetup()
    {
        BaseSetup();
        
        isTimelineControlled = true;
    }

    // ----------------------------------------------------------------------

    public override void Setup()
    {
        // Handle no spikes for Trailer
        if (Const_Dev.IsTrailerMode && isHideSpikes)
            spikeControllers.ForEach(spikeController => spikeController.IsDisabled = true);
        
        // Pause BGM without stopping the fade in Bgm coroutine.
        Script_BackgroundMusicManager.Control.PauseBgmOnSetup();
        
        AudioSource audio = EileenThemePlayer.GetComponent<AudioSource>();
        audio.volume = 1f;
        audio.gameObject.SetActive(true);
        
        if (!audio.isPlaying)
            audio.UnPause();
        
        // Handle coming from Eileen Mind painting
        Dev_Logger.Debug($"LB21: Last LB is {game.LastLevelBehavior}");
        if (game.LastLevelBehavior == LB26)
        {
            Dev_Logger.Debug("Player coming from LB26_EileensMind");
            PlayerEntranceFromEileenMind();
        }

        BaseSetup();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_21))]
public class Script_LevelBehavior_21Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_21 lb = (Script_LevelBehavior_21)target;
        if (GUILayout.Button("SetNewElleniaPassword()"))
        {
            lb.SetNewElleniaPassword();
        }

        if (GUILayout.Button("PlayerEntranceFromEileenMind()"))
        {
            lb.PlayerEntranceFromEileenMind();
        }
    }
}
#endif