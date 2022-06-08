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
    
    [SerializeField] private bool didSpeakWithEileenToday;

    [SerializeField] private Script_LevelBehavior_25 LB25;
    [SerializeField] private Script_LevelBehavior_26 LB26;
    
    [SerializeField] private Script_DemonNPC Eileen;
    [SerializeField] private Script_DemonNPC EileenElleniaHurt;
    
    /// <summary>
    /// This room uses a global BgThemePlayer to not be too repetitive when entering / exiting.
    /// </summary>
    [SerializeField] private Script_BgThemePlayer EileenThemePlayer;
    
    [SerializeField] private Script_InteractableObjectTextParent textParent;
    [SerializeField] private Transform fullArtParent;
    [SerializeField] private Script_InteractableFullArt noteFullArt;
    
    [SerializeField] private TimelineAsset playerDropTimeline;
    
    [SerializeField] private Script_PlayerDropSFXOnEnable dropSFX;
    [SerializeField] private Script_UrselkAttacks urselkAttacks;
    [SerializeField] private float onEntranceAttackFreezeTime;
    
    private List<GameObject> playerObjsToBind = new List<GameObject>();

    private bool isOnEntranceAttackFrozen;
    private PlayableDirector playerPlayableDirector;
    
    private bool isInitialize = true;
    
    private bool isTimelineControlled = false;

    // ------------------------------------------------------------------
    // Dev Only
    [SerializeField] private bool isHideSpikes;
    [SerializeField] private List<Script_DemonIntervalAttackController> spikeControllers;


    protected override void OnEnable()
    {
        Script_GameEventsManager.OnLevelInitComplete    += OnEntranceAttack;
        Script_CombatEventsManager.OnEnemyAttackEnd     += OnEntranceAttackEnd;
        
        playerPlayableDirector = game.GetPlayer().GetComponent<PlayableDirector>();
        playerPlayableDirector.stopped                  += OnDropTimelineDone;
        
        if (didSpeakWithEileenToday)
            OnFinishedTalking();
    }

    protected override void OnDisable()
    {
        Script_GameEventsManager.OnLevelInitComplete    -= OnEntranceAttack;
        Script_CombatEventsManager.OnEnemyAttackEnd     -= OnEntranceAttackEnd;
        
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
        Debug.Log("OnFinishedTalking() switching out Eileen's dialogue nodes now...");
        spokenWithEileen = true;
        didSpeakWithEileenToday = true;
        Eileen.MyDialogueState = Script_DemonNPC.DialogueState.Talked;
    }
    
    // ----------------------------------------------------------------------
    // Auto Choice Action
    public void OnDidTalkToEllenia(OnNextNodeChoiceArgs modifiableEventArgs)
    {
        Debug.Log("Spoken with Ellenia: " + LB25.spokenWithEllenia);

        if (LB25.spokenWithEllenia)     modifiableEventArgs.choice = 1;
        else                            modifiableEventArgs.choice = 0;
    }
    // ----------------------------------------------------------------------

    public void SetNewElleniaPassword()
    {
        string newPassword = GetComponent<Script_PasswordChooser>().GetRandomWord();
        Script_Names.UpdateElleniaPassword(newPassword);
        print($"Set ElleniaPassword to {Script_Names.ElleniaPassword}");
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

    /// <summary>
    /// Do one initial spike attack when Player first enters to ensure player is
    /// dealt damage as "victim"
    /// 
    /// Changes state to interact 
    /// </summary>
    private void OnEntranceAttack()
    {
        if (didOnEntranceAttack)    return;
        game.ChangeStateCutScene();
        
        urselkAttacks.AlternatingSpikesAttack();
        
        didOnEntranceAttack = true;
        isOnEntranceAttackFrozen = true;

        StartCoroutine(WaitToInteractSafe());
        
        /// To be extra safe so player doesn't freeze on entrance
        /// if OnEntranceAttackEnd doesn't fire
        /// 9/29 playtest: frozen even after hit 
        IEnumerator WaitToInteractSafe()
        {
            yield return new WaitForSeconds(onEntranceAttackFreezeTime);

            Debug.Log("Safe set to interact after OnEntranceAttack");

            if (isOnEntranceAttackFrozen)
            {
                game.ChangeStateInteract();
                isOnEntranceAttackFrozen = false;
            }
        }
    }

    private void OnEntranceAttackEnd(string hitBoxId)
    {
        if (hitBoxId == Const_HitBox.EileenEnergySpikeEntrance)
        {
            game.ChangeStateInteract();
            isOnEntranceAttackFrozen = false;
            Debug.Log("Entrance attack is over");
        }
    }

    private void BaseSetup()
    {
        EileenElleniaHurt.gameObject.SetActive(false);
        
        game.SetupInteractableFullArt(fullArtParent, isInitialize);
        
        // Handle Eileen leaving the room after you "fix" her mind
        if (LB26.isCurrentPuzzleComplete)
        {
            HandleEileenDefault(isPuzzleDone: true);
        }
        else
        {
            HandleEileenDefault(isPuzzleDone: false);
            
            if (didSpeakWithEileenToday)
                Eileen.MyDialogueState = Script_DemonNPC.DialogueState.Talked;
            
            // We need to set a new password, since the talked nodes don't call the password
            // set function and we are not saving the password from run to run
            if (LB26.isPuzzleComplete)  OnElleniaPassword(); 

            // Eileen Ellenia Hurt Event Cycle.
            if (game.RunCycle == Script_RunsManager.Cycle.Weekend)
            {
                HandleEileenWeekend(Script_EventCycleManager.Control.IsElleniaHurt());
            }
        }
        
        isInitialize = false;

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
        
        // Handle saving BGM played state.
        game.PauseBgMusic();
        AudioSource audio = EileenThemePlayer.GetComponent<AudioSource>();
        audio.volume = 1f;
        audio.gameObject.SetActive(true);
        if (!audio.isPlaying) audio.UnPause();
        
        // Handle coming from Eileen Mind painting
        Debug.Log($"LB21: Last LB is {game.LastLevelBehavior}");
        if (game.LastLevelBehavior == LB26)
        {
            Debug.Log("Player coming from LB26_EileensMind");
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