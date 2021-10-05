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

    private bool didIdsRun;
       
    protected override void OnEnable() {
        base.OnEnable();
        
        // Disable Pixel Perfect for now, because it's causing really bad shaking with the Screen Space
        // bg Canvas not set to World Space. We want it to be screen space so it gives a different effect.
        game.PixelPerfectEnable(false);
        
        Script_GameEventsManager.OnLevelInitComplete    += OnLevelInitCompleteEvent;
    }

    protected override void OnDisable() {
        base.OnDisable();
        game.PixelPerfectEnable(true);
        Script_GameEventsManager.OnLevelInitComplete    -= OnLevelInitCompleteEvent;
    }    
    
    public void OnTriggerWallTransition()
    {
        Debug.Log("Change wall sprites");
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