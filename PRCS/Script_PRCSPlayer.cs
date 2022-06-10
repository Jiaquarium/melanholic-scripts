using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;

/// <summary>
/// Plays PRCS using 1 myTimeline and lets you define a finished state (e.g. myTimeline done and/or nodes done)
/// and this'll fire a finished event once reached which gameObjects can react to
/// 
/// If you don't need this extra behavior and just need to play a Timeline, consider just using Timeline + Signals
/// instead
/// 
/// Detects when playable is done
/// Can hook up a PRCSLastNode to do isNodesDone
/// 
/// NOTE: ONLY REFERENCE 1 Director HERE; USE ANOTHER GAMEOBJECT/COMPONENT FOR ADDT'L
/// 
/// This uses events and not signals because we need to wait to detect things outside the Timeline
/// (e.g. Return or End of Node)
/// </summary>
public class Script_PRCSPlayer : MonoBehaviour
{
    public enum DoneStates
    {
        TimelineAndNodes = 0,
        TimelineOnly = 1,
        TimelineAndReturn = 2
    }
    [SerializeField] private DoneStates DoneCondition;
    [SerializeField] private Script_PRCS PRCS;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private TimelineAsset myTimeline;
    
    [SerializeField] private FadeSpeeds fadeInSpeed;
    [SerializeField] private FadeSpeeds fadeOutSpeed;
    [Tooltip("isContinuation, will not fade in the PRCS")]
    [SerializeField] private bool isContinuation;

    private bool isDone;
    private bool isTimelineDone;
    // To detect end of all dialogue nodes used during PRCS.
    private bool isNodesDone;
    private bool isReturnPressedDone;
    private bool isDetectingReturn;
    private Script_Game game;
    
    void OnEnable()
    {
        director.stopped += PRCSDone;
    }

    void OnDisable()
    {
        director.stopped -= PRCSDone;
    }

    void Start()
    {
        game = Script_Game.Game;
    }

    void Update()
    {
        if (isDetectingReturn)
        {
            if (game.GetPlayer().MyPlayerInput.actions[Const_KeyCodes.Interact].WasPressedThisFrame())
            {
                isReturnPressedDone         = true;
                isDetectingReturn           = false;
            }
        }
    }

    void LateUpdate()
    {
        if (isDone)     return;
        
        switch (DoneCondition)
        {
            case (DoneStates.TimelineAndNodes):
                if (isTimelineDone && isNodesDone)          FireDoneEvent();
                break;
            case (DoneStates.TimelineOnly):
                if (isTimelineDone)                         FireDoneEvent();
                break;
            case (DoneStates.TimelineAndReturn):
                if (isTimelineDone && isReturnPressedDone)  FireDoneEvent();
                break;
            default:
                break;
        }

        void FireDoneEvent()
        {
            Debug.Log($"!!!Firing PRCS Done for Director {director}, this: {this}");
            Script_PRCSEventsManager.PRCSDone(this);
            isDone = true;
        }
    }
    
    public void Play(Action cb = null)
    {
        Debug.Log("Play(): Starting PRCS Scene!!!");

        isDone              = false;
        isNodesDone         = false;
        isTimelineDone      = false;
        isReturnPressedDone = false;

        if (!isContinuation)
        {
            Script_PRCSManager.Control.ShowPRCS(PRCS, fadeInSpeed, cb);
        }
        
        Debug.Log($"Playing myTimeline asset: {myTimeline}");
        director.Play(myTimeline);
    }

    public void PlayCustom(Script_PRCSManager.CustomTypes type)
    {
        isDone              = false;
        isNodesDone         = false;
        isTimelineDone      = false;
        isReturnPressedDone = false;
        
        if (!isContinuation)
        {
            Script_PRCSManager.Control.OpenPRCSCustom(type);
        }
    }

    public void Stop(Action cb = null)
    {
        Script_PRCSManager.Control.HidePRCS(PRCS, fadeOutSpeed, cb);
    }

    public void CloseCustom(Script_PRCSManager.CustomTypes type, Action cb)
    {
        Script_PRCSManager.Control.ClosePRCSCustom(type, () => {
            if (cb != null)     cb();
        });
    }

    /// <summary>
    /// Call this from the last node to notify we've finished dialogue
    /// </summary>
    public void NodeDone()
    {
        isNodesDone = true;
    }

    private void PRCSDone(PlayableDirector aDirector)
    {
        if (aDirector.playableAsset == myTimeline)
        {
            isTimelineDone      = true;

            if (DoneCondition == DoneStates.TimelineAndReturn)
                isDetectingReturn   = true;
        }        
    }
}
