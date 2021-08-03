using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// NOTE: Setup is called on EVERY level change
/// </summary>
public class Script_LevelBehavior : MonoBehaviour
{
    public Script_Game game;
    
    [SerializeField] private Script_Snow snowEffect;
    [SerializeField] private Script_VCamera levelVCam;

    public bool IsMelancholyPianoDisabled { get; set; }

    public Script_VCamera LevelVCam
    {
        get => levelVCam;
    }

    void Awake()
    {
        /// NOTE: Awake() will occur before Setup()
        /// Will only happen once when level is set to active
        /// useful for init
    }
    
    void Start()
    {
        /// NOTE: Awake() will occur before Setup()
        /// "" same as Awake() otherwise
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleAction();
    }

    /// <summary>
    /// Meant to be overriden
    /// Do any on start reactions here to when level has finished fading in
    /// </summary>
    public virtual void OnLevelInitComplete()
    {
    }

    protected virtual void HandleAction() {}

    protected virtual void OnDisable()
    {
        if (levelVCam != null)
        {
            if (Script_VCamManager.VCamMain != null)
            {
                Script_VCamManager.VCamMain.DisableLevelVCam(levelVCam);

                game.SnapCam(
                    levelVCam.transform.position,
                    Script_VCamManager.VCamMain.transform,
                    Script_VCamManager.VCamMain.VCamera.CinemachineVirtualCamera
                );
            }
            
        }
    }
    
    protected virtual void OnEnable()
    {
        if (levelVCam != null)
        {
            Script_VCamManager.VCamMain.SetNewVCam(levelVCam);
            game.ForceCutBlend();
        }
    }
    
    public virtual void EatDemon(int Id) {}
    
    public virtual void SetSwitchState(int Id, bool isOn) {
        Debug.LogError($"You must override SetSwitchState(int Id, bool isOn) when using switches.");
    }
    
    public virtual void HandleMovingNPCCurrentMovesDone() {}
    
    public virtual void HandleMovingNPCAllMovesDone() {}
    
    public virtual void HandleDDRArrowClick(int t) {}
    
    public virtual void OnDoorLockUnlock(int id) {}
    
    public virtual void OnCloseInventory() {}
    
    public virtual void HandleExitCutScene() {}
    
    public virtual void HandleMovingNPCOnApproachedTarget(int i) {}
    
    public virtual void HandleDialogueNodeAction(string a) {}
    
    public virtual void HandleDialogueNodeUpdateAction(string a) {}
    
    public virtual bool ActivateTrigger(string Id) { return true; }
    
    public virtual void Cleanup() {}
    
    public virtual int OnSubmit(string s) { return -1; }
    
    public virtual void HandlePlayableDirectorStopped(PlayableDirector aDirector) {}
    
    /// <summary>
    /// This will be called on Level Behavior initialization by WeatherManager
    /// When Level Behavior is set inactive, Script_Snow gameObject itself will
    /// handle setting inactive again
    /// </summary>
    public void HandleSnowFallStart(bool isSnowDay)
    {
        if (snowEffect == null)
        {
            string warning = $"{name} has no Snow Effect defined";
            if (!Const_Dev.IsDevMode)       Debug.LogWarning(warning);
            else                            Debug.Log(warning);

            return;
        }
        
        if (isSnowDay)      snowEffect.gameObject.SetActive(true);
        else                snowEffect.gameObject.SetActive(false);
    }
    
    public virtual void InitialState() { }
    
    /// <summary>
    /// Called on EVERY level init
    /// </summary>
    public virtual void Setup()
    {
        // game.CreateNPCs();
        // game.CreateInteractableObjects();
        // game.CreateDemons();
    }
}
