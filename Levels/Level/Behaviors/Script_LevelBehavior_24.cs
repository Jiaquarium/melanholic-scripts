using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_24 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    [SerializeField] private bool _isPuzzleComplete;
    public bool isPuzzleComplete {
        get { return _isPuzzleComplete; }
        set {_isPuzzleComplete = value; }
    }
    // alchemist circle persist
    public bool didPickUpSpringStone;

    /* ======================================================================= */
    [SerializeField] private Script_TrackedPushablesTriggerPuzzleController triggersPuzzleController;
    [SerializeField] private Transform pillarParent;
    [SerializeField] private Script_Tracker[] pillars;
    [SerializeField] private Transform transformingRock;
    [SerializeField] private Script_ItemObject springStone;
    [SerializeField] private Script_VCamera staticZoomOutVCam;
    [SerializeField] private float beforeSpawnWaitTime;
    [SerializeField] private float afterSpawnWaitTime;
    [SerializeField] private Transform interactableObjectsTextParent;
    [SerializeField] private Script_InteractableObjectText[] pillarTextObjs;
    [SerializeField] private Script_BgThemePlayer heartBeatBgThemePlayerPrefab;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private Script_LevelBehavior_23 LB23;
    [SerializeField] private SpriteRenderer alchemistCircle;
    [SerializeField] private float alchemistCircleCompleteAlpha;
    private Script_BgThemePlayer heartBeatBgThemePlayer;
    private bool isInit = true;
    
    private void OnValidate()
    {
        pillars     = pillarParent.GetChildren<Script_Tracker>();
    }
    
    protected override void OnEnable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess += OnPuzzleSuccess;
        /// use this to do onEnter actions, waiting until level is fully init'ed
        Script_GameEventsManager.OnLevelInitComplete += CheckSuccessCase;
        director.stopped += OnPlayableDirectorStopped;
        Script_ItemsEventsManager.OnItemPickUp += OnItemPickUp;

        game.PauseBgMusic();
        if (!isPuzzleComplete)
        {
            heartBeatBgThemePlayer = Instantiate(heartBeatBgThemePlayerPrefab, Vector3.zero, Quaternion.identity);
        }
        
        // enable pillars renderers
        SetPillarsVisibility(true);
        // set triggers and pillars to active
        ActivateTriggersAndPillars(true);
    }

    protected override void OnDisable()
    {
        Script_PuzzlesEventsManager.OnPuzzleSuccess -= OnPuzzleSuccess;
        Script_GameEventsManager.OnLevelInitComplete -= CheckSuccessCase;
        director.stopped -= OnPlayableDirectorStopped;
        Script_ItemsEventsManager.OnItemPickUp -= OnItemPickUp;

        if (heartBeatBgThemePlayer != null)
        {
            DestroyBgThemePlayer();
        }
        game.UnPauseBgMusic();

        ActivateTriggersAndPillars(false);
        SetPillarsVisibility(false);   
    }

    private void DestroyBgThemePlayer()
    {
        heartBeatBgThemePlayer.GetComponent<AudioSource>().volume = 0f;
        heartBeatBgThemePlayer.GetComponent<AudioSource>().Stop();
        Destroy(heartBeatBgThemePlayer.gameObject);
        heartBeatBgThemePlayer = null;
    }

    private void ActivateTriggersAndPillars(bool isActive)
    {
        // set triggers and pillars to active
        triggersPuzzleController.gameObject.SetActive(isActive);
        pillarParent.gameObject.SetActive(isActive);
    }

    private void SetPillarsVisibility(bool isOn)
    {
        foreach (Script_Tracker pillar in pillars)  pillar.SetVisibility(isOn);
    }

    private void CheckSuccessCase()
    {
        // check for completion state (only complete if still correct)
        // to ensure none were moved since player came from KTV Room 1
        triggersPuzzleController.CheckSuccessCase();
    }
    private void OnPuzzleSuccess(string Id)
    {
        if (!isPuzzleComplete)
        {
            print("PUZZLE SUCCESS SCENE!!!");
            isPuzzleComplete = true;
            
            // stop heartbeat bg music and stop pulsing animation of pillars
            if (heartBeatBgThemePlayer != null) DestroyBgThemePlayer();
            foreach (Script_Tracker tracker in pillars)
            {
                tracker.GetComponent<Script_StopAnimation>().StopAnimation();
                tracker.StopTracking();
            }
            
            game.ChangeStateCutScene();
            StartCoroutine(SpawnSummerStoneScene());
        }

        IEnumerator SpawnSummerStoneScene()
        {
            // camera to SummerStone via VCam, 2 sec
            Script_VCamManager.VCamMain.SetNewVCam(staticZoomOutVCam);
            yield return new WaitForSeconds(Script_VCamManager.defaultBlendTime);
            
            yield return new WaitForSeconds(beforeSpawnWaitTime);
            
            // spawn sunstone & SFX
            GetComponent<Script_TimelineController>().PlayableDirectorPlay(0);
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            print("DONE WITH ANIMATION");
            // then zoom back to default
            StartCoroutine(EndingAnimations());
        }

        IEnumerator EndingAnimations()
        {
            PuzzleFinishedState();
            yield return new WaitForSeconds(afterSpawnWaitTime);
            
            Script_VCamManager.VCamMain.SwitchToMainVCam(staticZoomOutVCam);
            yield return new WaitForSeconds(Script_VCamManager.defaultBlendTime);

            game.ChangeStateInteract();
        }
    }

    public void PuzzleFinishedState()
    {
        transformingRock.gameObject.SetActive(false);

        if (heartBeatBgThemePlayer != null) DestroyBgThemePlayer();
        
        triggersPuzzleController.CompleteState();
        LB23.CompletedState();
    }

    private void OnItemPickUp(string itemId)
    {
        if (itemId == springStone.GetItem().id)
        {
            didPickUpSpringStone = true;
        }
    }

    private void SetSpringStoneActive(bool isActive)
    {
        if (isActive)
        {
            springStone.gameObject.SetActive(true);
            springStone.SetAlpha(1f);
        }
        else
        {
            if (springStone != null)
                springStone.gameObject.SetActive(false);
        }
    }

    private void AlchemistCircleCompleteState()
    {
        Color newColor = alchemistCircle.color;
        newColor.a = alchemistCircleCompleteAlpha;

        alchemistCircle.color = newColor;
    }

    void Awake()
    {
        if (isPuzzleComplete)
        {
            PuzzleFinishedState();
            SetSpringStoneActive(!didPickUpSpringStone);
            AlchemistCircleCompleteState();
        }
        else
        {
            transformingRock.gameObject.SetActive(true);
            springStone.gameObject.SetActive(false);
        }
    }

    public override void Setup()
    {
        game.SetupInteractableObjectsText(interactableObjectsTextParent, isInit);
        game.SetupInteractableObjectsTextManually(pillarTextObjs, isInit);

        isInit = false;
    }
}
