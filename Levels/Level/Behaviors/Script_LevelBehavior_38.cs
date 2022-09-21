using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_TimelineController))]
public class Script_LevelBehavior_38 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */

    /* ======================================================================= */
    [SerializeField] private Script_DemonNPC Ids;

    [SerializeField] private Script_VCamera scareZoomVCam;
    [SerializeField] private float scareWaitTime;
    [SerializeField] private float zoomOutCameraWaitTime;
    [SerializeField] private bool isScareDay;

    [SerializeField] private Script_HUDManager HUDManager;
    [SerializeField] private Script_StickerHolsterManager stickerHolsterManager;

    [SerializeField] private Script_TriggerReliableStay emphasizeWalkTrigger;

    private bool didIdsRun;
       
    protected override void OnEnable() {
        base.OnEnable();
        
        Script_GameEventsManager.OnLevelInitComplete    += OnLevelInitCompleteEvent;
    }

    protected override void OnDisable() {
        base.OnDisable();
        
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
    }    
    
    void Start()
    {
        // Always scare the very first time (Player must pass this on the first Monday)
        // and then 1/3 chance to scare after that.
        bool isFirstMonday = Script_Game.Game.CycleCount == 0
            && Script_Game.Game.IsRunDay(Script_Run.DayId.mon);
        bool isRandomDay = Random.Range(0, 3) == 0 ? true : false;
        
        isScareDay = isFirstMonday || isRandomDay;
    }

    public void OnTriggerWallTransition()
    {
        Dev_Logger.Debug("Change wall sprites");
    }

    public void ScareSFX()
    {
        if (isScareDay)
        {
            HUDManager.FadeSpeed = FadeSpeeds.None;
            stickerHolsterManager.FadeSpeed = FadeSpeeds.None;
            
            game.ChangeStateCutScene();

            Script_VCamManager.VCamMain.SetNewVCam(scareZoomVCam);
            
            Script_SFXManager.SFX.PlayHallwayScare();

            StartCoroutine(WaitAfterScare());

            emphasizeWalkTrigger.gameObject.SetActive(false);
        }

        IEnumerator WaitAfterScare()
        {
            yield return new WaitForSeconds(scareWaitTime);

            Script_VCamManager.VCamMain.SwitchToMainVCam(scareZoomVCam);
            
            yield return new WaitForSeconds(zoomOutCameraWaitTime);

            HUDManager.SetFadeSpeedDefault();
            stickerHolsterManager.SetFadeSpeedDefault();            
            
            game.ChangeStateInteract();
        }
    }
    
    public override void InitialState()
    {
    }

    // ------------------------------------------------------------------
    // Timeline Signal Reactions
    public void OnIdsRunAwayTimelineDone()
    {
        game.ChangeStateInteract();
        didIdsRun = true;
    }

    // ------------------------------------------------------------------
    // Unity Events
    
    public void OnTriggerPlayerEnter()
    {
        var player = game.GetPlayer();
        
        player.IsEmphasizeWalk = true;
    }

    public void OnTriggerPlayerExit()
    {
        var player = game.GetPlayer();
        
        player.IsEmphasizeWalk = false;
    }

    // ------------------------------------------------------------------
    
    private void OnLevelInitCompleteEvent()
    {
        HandlePlayIdsTimeline();
    }
    
    private void HandlePlayIdsTimeline()
    {
        if (ShouldPlayIdsIntro())
        {
            game.ChangeStateCutScene();
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
        }
    }

    private bool ShouldPlayIdsIntro()
    {
        return !didIdsRun && Script_EventCycleManager.Control.IsLastElevatorTutorialRun();
    }

    public override void Setup()
    {
        if (ShouldPlayIdsIntro())
            Ids.gameObject.SetActive(true);
        else
            Ids.gameObject.SetActive(false);
        
        if (!game.IsFirstMonday)
            emphasizeWalkTrigger.gameObject.SetActive(false);
    }        
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LevelBehavior_38))]
public class Script_LevelBehavior_38Tester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LevelBehavior_38 t = (Script_LevelBehavior_38)target;
        if (GUILayout.Button("InitalState()"))
        {
            t.InitialState();
        }
    }
}
#endif