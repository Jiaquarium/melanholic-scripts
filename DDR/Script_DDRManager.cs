using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Arrows move towards Outlines. On input, check the current direction
/// and report its rating.
/// </summary>
public class Script_DDRManager : MonoBehaviour
{
    public enum DDRState
    {
        Inactive = 0,
        Active = 1
    }
    
    public Script_Game game;
    [SerializeField] private DDRState _state;
    
    [SerializeField] private float bpm;

    public CanvasGroup DDRCanvasGroup;
    
    public Script_Arrow ArrowPrefabLeft;
    public Script_Arrow ArrowPrefabDown;
    public Script_Arrow ArrowPrefab;
    public Script_Arrow ArrowPrefabRight;
    
    public Transform ArrowOutlineLeft;
    public Transform ArrowOutlineDown;
    public Transform ArrowOutline;
    public Transform ArrowOutlineRight;
    public Transform ArrowsContainer;
    
    public Script_TierComment Tier1Comment;
    public Script_TierComment Tier2Comment;
    public Script_TierComment Tier3Comment;


    public Model_SongMoves songMoves;
    public float tierNeg1Buffer;
    public float tier1Buffer;
    public float tier2Buffer;
    public float tier3Buffer;
    public float focusTimeLength;
    public float tierCommentActivationLength;


    public Script_Arrow[] activeLeftArrows      = new Script_Arrow[0];
    public Script_Arrow[] activeDownArrows      = new Script_Arrow[0];
    public Script_Arrow[] activeUpArrows        = new Script_Arrow[0];
    public Script_Arrow[] activeRightArrows     = new Script_Arrow[0];

    public int leftArrowsCounter;
    public int downArrowsCounter;
    public int upArrowsCounter;
    public int rightArrowsCounter;
    public int nextLeftArrowIndex;
    public int nextDownArrowIndex;
    public int nextUpArrowIndex;
    public int nextRightArrowIndex;

    private int leftMoveCount;
    private int upMoveCount;
    private int downMoveCount;
    private int rightMoveCount;
    private Script_ArrowOutline ScriptArrowOutlineLeft;
    private Script_ArrowOutline ScriptArrowOutlineDown;
    private Script_ArrowOutline ScriptArrowOutlineUp;
    private Script_ArrowOutline ScriptArrowOutlineRight;
    public int mistakes;
    public int mistakesAllowed;


    public float timeToRise;
    public bool didFail;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private Script_DDRConductor conductor;   
    private Script_SFXManager sfxManager;
    private int lastBeat;
    public DDRState State
    {
        get => _state;
        private set => _state = value;
    }

    public bool IsPlaying
    {
        get => musicSource.isPlaying;
    }
    
    void Awake()
    {
        ScriptArrowOutlineLeft      = ArrowOutlineLeft.GetComponent<Script_ArrowOutline>();
        ScriptArrowOutlineDown      = ArrowOutlineDown.GetComponent<Script_ArrowOutline>();
        ScriptArrowOutlineUp        = ArrowOutline.GetComponent<Script_ArrowOutline>();
        ScriptArrowOutlineRight     = ArrowOutlineRight.GetComponent<Script_ArrowOutline>();
    }
    
    void Start()
    {
        sfxManager = Script_SFXManager.SFX;
    }
    
    void Update()
    {
        HandleBpmPulse();
        
        HandleLeftArrowSpawn();
        HandleDownArrowSpawn();
        HandleUpArrowSpawn();
        HandleRightArrowSpawn();
        
        if (State == DDRState.Active)
        {
            HandleInput();
            HandleMistakes();
            HandleSongFinish();
        }
    }
    
    public void Activate()
    {
        this.gameObject.SetActive(true);
        game.ManagePlayerViews(Const_States_PlayerViews.DDR);

        DDRCanvasGroup.GetComponent<Script_CanvasGroupController>().Open();
    }

    public void Deactivate()
    {
        if (State != DDRState.Active)   return;

        InitialState();

        this.gameObject.SetActive(false);
        game.ManagePlayerViews(Const_States_PlayerViews.Health);

        /// Fire done event
        Script_DDREventsManager.DDRDone();
    }

    void ClearState()
    {
        DestroyArrows(activeLeftArrows);
        DestroyArrows(activeDownArrows);
        DestroyArrows(activeUpArrows);
        DestroyArrows(activeRightArrows);
        
        activeLeftArrows        = null;
        activeDownArrows        = null;
        activeUpArrows          = null;
        activeRightArrows       = null;

        leftMoveCount           = 0;
        downMoveCount           = 0;
        upMoveCount             = 0;
        rightMoveCount          = 0;
        
        leftArrowsCounter       = 0;
        downArrowsCounter       = 0;
        upArrowsCounter         = 0;
        rightArrowsCounter      = 0;

        nextLeftArrowIndex      = 0;
        nextDownArrowIndex      = 0;
        nextUpArrowIndex        = 0;
        nextRightArrowIndex     = 0;

        mistakes = 0;
    }

    void DestroyArrows(Script_Arrow[] arrows)
    {
        if (arrows == null)     return;
        
        foreach (Script_Arrow a in arrows)
        {
            if (a != null)  Destroy(a.gameObject);
        }
    }

    void HandleLeftArrowSpawn()
    {
        if (leftMoveCount >= songMoves.leftMoveTimes.Length)
            return;
        
        float nextLeftMoveTime = songMoves.leftMoveTimes[leftMoveCount];
        
        if (conductor.SongPosition >= nextLeftMoveTime - timeToRise)
        {
            StartLeftArrow();
            leftMoveCount++;
        }
    }
    
    void HandleDownArrowSpawn()
    {
        if (downMoveCount >= songMoves.downMoveTimes.Length)
            return;

        float nextDownMoveTime = songMoves.downMoveTimes[downMoveCount];
        
        if (conductor.SongPosition >= nextDownMoveTime - timeToRise)
        {
            StartDownArrow();
            downMoveCount++;
        }
    }

    void HandleUpArrowSpawn()
    {
        if (upMoveCount >= songMoves.upMoveTimes.Length)
            return;

        float nextUpMoveTime = songMoves.upMoveTimes[upMoveCount];
        
        if (conductor.SongPosition >= nextUpMoveTime - timeToRise)
        {
            StartUpArrow();
            upMoveCount++;
        }
    }

    void HandleRightArrowSpawn()
    {
        if (rightMoveCount >= songMoves.rightMoveTimes.Length)
            return;

        float nextRightMoveTime = songMoves.rightMoveTimes[rightMoveCount];
        
        if (conductor.SongPosition >= nextRightMoveTime - timeToRise)
        {
            StartRightArrow();
            rightMoveCount++;
        }
    }

    void StartLeftArrow()
    {
        Script_Arrow arrow = Instantiate(ArrowPrefabLeft) as Script_Arrow;
        activeLeftArrows[leftArrowsCounter] = arrow;
        arrow.transform.SetParent(ArrowsContainer, false);
        SetupArrow(arrow, ArrowOutlineLeft);
        leftArrowsCounter++;
    }

    void StartDownArrow()
    {
        Script_Arrow arrow = Instantiate(ArrowPrefabDown) as Script_Arrow;
        activeDownArrows[downArrowsCounter] = arrow;
        arrow.transform.SetParent(ArrowsContainer, false);
        SetupArrow(arrow, ArrowOutlineDown);
        downArrowsCounter++;
    }
    
    void StartUpArrow()
    {
        Script_Arrow arrow = Instantiate(ArrowPrefab) as Script_Arrow;
        activeUpArrows[upArrowsCounter] = arrow;
        arrow.transform.SetParent(ArrowsContainer, false);
        SetupArrow(arrow, ArrowOutline);
        upArrowsCounter++;
    }

    void StartRightArrow()
    {
        Script_Arrow arrow = Instantiate(ArrowPrefabRight) as Script_Arrow;
        activeRightArrows[rightArrowsCounter] = arrow;
        arrow.transform.SetParent(ArrowsContainer, false);
        SetupArrow(arrow, ArrowOutlineRight);
        rightArrowsCounter++;
    }

    void SetupArrow(Script_Arrow arrow, Transform arrowOutline)
    {
        arrow.Setup(
            arrowOutline.GetComponent<RectTransform>().localPosition,
            timeToRise,
            tierNeg1Buffer,
            tier1Buffer,
            tier2Buffer,
            tier3Buffer,
            this,
            conductor
        );
        arrow.BeginRising();
    }

    void HandleInput()
    {
        // ask arrow where it is, and manager will post that event
        // for game -> level behavior to handle
        if (
            Input.GetButtonDown(Const_KeyCodes.Left)
        )
        {
            ScriptArrowOutlineLeft.Focus();
            
            if (
                nextLeftArrowIndex < activeLeftArrows.Length
                && activeLeftArrows[nextLeftArrowIndex] != null
                && !activeLeftArrows[nextLeftArrowIndex].isClicked
            )
            {
                int tier = ReportArrowTier(activeLeftArrows[nextLeftArrowIndex]);
                if (tier > 0)
                {
                    HandleTierFlash(tier, Directions.Left);
                    nextLeftArrowIndex++;
                }
            } 
        }
        if (Input.GetButtonDown(Const_KeyCodes.Down))
        {
            ScriptArrowOutlineDown.Focus();
            
            if (
                nextDownArrowIndex < activeDownArrows.Length
                && activeDownArrows[nextDownArrowIndex] != null
                && !activeDownArrows[nextDownArrowIndex].isClicked
            )
            {
                int tier = ReportArrowTier(activeDownArrows[nextDownArrowIndex]);
                if (tier > 0)
                {
                    HandleTierFlash(tier, Directions.Down);
                    nextDownArrowIndex++;
                }
            } 
        }
        if (Input.GetButtonDown(Const_KeyCodes.Up))
        {
            ScriptArrowOutlineUp.Focus();
            
            if (
                nextUpArrowIndex < activeUpArrows.Length
                && activeUpArrows[nextUpArrowIndex] != null
                && !activeUpArrows[nextUpArrowIndex].isClicked
            )
            {
                int tier = ReportArrowTier(activeUpArrows[nextUpArrowIndex]);
                if (tier > 0)
                {
                    HandleTierFlash(tier, Directions.Up);
                    nextUpArrowIndex++;
                }
            } 
        }
        if (Input.GetButtonDown(Const_KeyCodes.Right))
        {
            ScriptArrowOutlineRight.Focus();
            
            if (
                nextRightArrowIndex < activeRightArrows.Length
                && activeRightArrows[nextRightArrowIndex] != null
                && !activeRightArrows[nextRightArrowIndex].isClicked
            )
            {
                int tier = ReportArrowTier(activeRightArrows[nextRightArrowIndex]);
                if (tier > 0)
                {
                    HandleTierFlash(tier, Directions.Right);
                    nextRightArrowIndex++;
                }
            }
        }
    }

    private void HandleTierFlash(int tier, Directions dir)
    {
        switch (tier)
        {
            case 1:
                switch (dir)
                {
                    case Directions.Left:
                        ScriptArrowOutlineLeft.FlashTier1();
                        break;
                    case Directions.Down:
                        ScriptArrowOutlineDown.FlashTier1();
                        break;
                    case Directions.Up:
                        ScriptArrowOutlineUp.FlashTier1();
                        break;
                    case Directions.Right:
                        ScriptArrowOutlineRight.FlashTier1();
                        break;
                }
                break;
            
            case 2:
                switch (dir)
                {
                    case Directions.Left:
                        ScriptArrowOutlineLeft.FlashTier2();
                        break;
                    case Directions.Down:
                        ScriptArrowOutlineDown.FlashTier2();
                        break;
                    case Directions.Up:
                        ScriptArrowOutlineUp.FlashTier2();
                        break;
                    case Directions.Right:
                        ScriptArrowOutlineRight.FlashTier2();
                        break;
                }
                break;
        }
    }

    public int ReportArrowTier(Script_Arrow arrow)
    {
        float t = arrow.ReportTier();
        
        if (t <= tier1Buffer)
        {
            // handle case if it's passed the outline and not tier1
            if (t < 0 && Mathf.Abs(t) > tierNeg1Buffer)
            {
                // report tier3 to game
                arrow.isClicked = true;
                game.HandleDDRArrowClick(3);
                
                Tier3Comment.Activate();
                mistakes++;
                MistakeSFX();

                return 3;
            }
            
            // report tier1 to game
            arrow.isClicked = true;
            arrow.DestroyArrow();
            game.HandleDDRArrowClick(1);
            
            Tier1Comment.Activate();
            OnBeatEffect();
            
            return 1;
        }
        else if (t <= tier2Buffer)
        {
            // report tier2 to game
            arrow.isClicked = true;
            arrow.DestroyArrow();
            game.HandleDDRArrowClick(2);
            
            Tier2Comment.Activate();
            OnBeatEffect();

            return 2;
        }
        else if (t <= tier3Buffer)
        {
            // report tier3 to game
            arrow.isClicked = true;
            game.HandleDDRArrowClick(3);
            
            Tier3Comment.Activate();
            mistakes++;
            MistakeSFX();

            return 3;
        }

        return -1;
    }

    private void HandleMistakes()
    {
        if (mistakesAllowed != -1 && mistakes >= mistakesAllowed)
        {
            didFail = true;
            Deactivate();
        }
    }

    private void HandleSongFinish()
    {
        if (!IsPlaying)
        {
            didFail = false;
            Deactivate();
        }
    }

    public void StartMusic(
        Model_SongMoves _songMoves,
        Script_BgThemePlayer bgThemePlayer,
        int _mistakesAllowed // set to -1 to ignore mistakes
    )
    {
        upMoveCount = 0;
        downMoveCount = 0;
        mistakes = 0;
        didFail = false;

        songMoves = _songMoves;
        mistakesAllowed = _mistakesAllowed;

        leftArrowsCounter = 0;
        downArrowsCounter = 0;
        upArrowsCounter = 0;
        rightArrowsCounter = 0;
        
        activeLeftArrows            = new Script_Arrow[songMoves.leftMoveTimes.Length];
        activeDownArrows            = new Script_Arrow[songMoves.downMoveTimes.Length];
        activeUpArrows              = new Script_Arrow[songMoves.upMoveTimes.Length];
        activeRightArrows           = new Script_Arrow[songMoves.rightMoveTimes.Length];
        
        ScriptArrowOutlineLeft.Setup(focusTimeLength);
        ScriptArrowOutlineDown.Setup(focusTimeLength);
        ScriptArrowOutlineUp.Setup(focusTimeLength);
        ScriptArrowOutlineRight.Setup(focusTimeLength);

        Tier1Comment.Setup(tierCommentActivationLength);
        Tier2Comment.Setup(tierCommentActivationLength);
        Tier3Comment.Setup(tierCommentActivationLength);

        musicSource.time = 0f;
        musicSource.Play();
        
        conductor.SetDspTimeStart();
        lastBeat = 0;
        Pulse();

        State = DDRState.Active;
    }

    private void HandleBpmPulse()
    {
        if (!musicSource.isPlaying)
            return;
        
        float secPerBeat = 60f / bpm;

        int currentBeat = (int)(conductor.SongPosition / secPerBeat);

        if (currentBeat > lastBeat)
        {
            Pulse();
            lastBeat = currentBeat;
        }
    }
    
    private void Pulse()
    {
        ScriptArrowOutlineLeft.FlashBpm();
        ScriptArrowOutlineDown.FlashBpm();
        ScriptArrowOutlineUp.FlashBpm();
        ScriptArrowOutlineRight.FlashBpm();
    }

    private void OnBeatEffect()
    {
        // sfxSource.PlayOneShot(sfxManager.DDRStep, sfxManager.DDRStepVol);
    }

    private void MistakeSFX()
    {
        sfxSource.PlayOneShot(sfxManager.DDRMistake, sfxManager.DDRMistakeVol);
    }

    private void InitialState()
    {
        State = DDRState.Inactive;
        game.StopMovingNPCThemes();
        ClearState();

        DDRCanvasGroup.GetComponent<Script_CanvasGroupController>().Close();
    }

    public void Setup()
    {
        InitialState();
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_DDRManager))]
    public class Script_DDRManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_DDRManager t = (Script_DDRManager)target;
            if (GUILayout.Button("Flash Tier1"))
            {
                t.HandleTierFlash(1, Directions.Left);
                t.HandleTierFlash(1, Directions.Down);
                t.HandleTierFlash(1, Directions.Up);
                t.HandleTierFlash(1, Directions.Right);
            }

            if (GUILayout.Button("Flash Tier2"))
            {
                t.HandleTierFlash(2, Directions.Left);
                t.HandleTierFlash(2, Directions.Down);
                t.HandleTierFlash(2, Directions.Up);
                t.HandleTierFlash(2, Directions.Right);
            }
        }
    }
    #endif
}
