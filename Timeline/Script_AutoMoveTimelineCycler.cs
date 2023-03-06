using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// This works with Moving NPC's auto timeline. Set this as the Director.
/// 
/// On each timeline's stopped event (end) this will set up a coroutine to play the next timeline
/// (and face specified direction / handle a waiting direction).
/// 
/// If the NPC's dialogue is triggered, then cache the set up'ed coroutine and play for when dialogue
/// ends. Note, this plays the timeline immediately after the dialogue always.
/// </summary>
public class Script_AutoMoveTimelineCycler : MonoBehaviour
{
    [SerializeField] private PlayableDirector myDirector;
    [SerializeField] private Script_MovingNPC myNPC;
    [SerializeField] private List<TimelineAsset> timelines;
    [SerializeField] private List<Directions> directions;

    [Space][Header("Next Timeline Params")][Space]
    // Specify params for facing the following specified direction on wait
    
    [SerializeField] private Vector2 randomDelayBounds;
    
    // The direction to face on wait
    [SerializeField] private Directions waitFaceDirection;
    // Chance to face the waitFaceDirection
    [SerializeField][Range(0f, 1f)] private float faceDirProbability;
    
    private int timelineIdx;
    private Directions currentDir;
    private Coroutine nextTimelineCoroutine;
    private bool isCachedNextTimeline;

    void OnEnable()
    {
        myDirector.stopped += NextTimeline;
        Script_NPCEventsManager.OnMovingNPCTriggerDialogue += CacheNextTimelineOnDialogue;
        Script_NPCEventsManager.OnMovingNPCEndDialogue += PlayCachedNextTimeline;
  
        timelineIdx = 0;

        // Start at frame 0 of timeline. Prevents teleporting to this frame when the timeline
        // begins after the initial wait period (if any)
        myDirector.playableAsset = timelines[timelineIdx];
        myNPC.FaceDirection(directions[timelineIdx]);
        myDirector.time = 0f;
        myDirector.Evaluate();
        
        WaitToPlayTimeline(timelineIdx, Directions.None);
    }

    void OnDisable()
    {
        myDirector.stopped -= NextTimeline;
        Script_NPCEventsManager.OnMovingNPCTriggerDialogue -= CacheNextTimelineOnDialogue;
        Script_NPCEventsManager.OnMovingNPCEndDialogue -= PlayCachedNextTimeline;
        
        StopMyCoroutines();
    }

    private void NextTimeline(PlayableDirector director)
    {
        // Stop AutoMove
        myNPC.IsAutoMoveTimelineForcePaused = true;
        
        timelineIdx++;

        if (timelineIdx >= timelines.Count)
            timelineIdx = 0;
        
        WaitToPlayTimeline(timelineIdx, waitFaceDirection);
    }

    private void WaitToPlayTimeline(int i, Directions waitFaceDir)
    {
        if (myDirector == null)
            return;
        
        myDirector.playableAsset = timelines[i];
        currentDir = directions[i];
        
        nextTimelineCoroutine = StartCoroutine(WaitToPlay());

        HandleFaceDirection();

        IEnumerator WaitToPlay()
        {
            float delay = Random.Range(randomDelayBounds.x, randomDelayBounds.y);
            
            yield return new WaitForSeconds(delay);

            Dev_Logger.Debug($"{name} Play timeline {timelineIdx} with delay {delay}");
            PlayCurrentState();

            nextTimelineCoroutine = null;
        }

        void HandleFaceDirection()
        {
            // Handle facing specified direction on wait, based on probability
            if (waitFaceDir != Directions.None)
            {
                float randomChance = Random.Range(0f, 1f);
                
                if (randomChance <= faceDirProbability)
                    myNPC.FaceDirection(waitFaceDir);
            }
        }
    }

    // -------------------------------------------------------------------------------------
    // Next Node Actions
    
    private void CacheNextTimelineOnDialogue(Script_MovingNPC npc)
    {
        if (npc == myNPC && nextTimelineCoroutine != null)
        {
            StopMyCoroutines();
            isCachedNextTimeline = true;

            Dev_Logger.Debug($"{name} Cached next timeline");
        }
    }
    
    private void PlayCachedNextTimeline(Script_MovingNPC npc)
    {
        if (npc == myNPC && isCachedNextTimeline)
        {
            PlayCurrentState();

            isCachedNextTimeline = false;

            Dev_Logger.Debug($"{name} Play next timeline with idx: {timelineIdx}");
        }
    }

    // -------------------------------------------------------------------------------------

    private void PlayCurrentState()
    {
        myNPC.FaceDirection(currentDir);
        myDirector.Play();
        myNPC.IsAutoMoveTimelineForcePaused = false;
    }
    
    public void StopMyCoroutines()
    {
        if (nextTimelineCoroutine != null)
        {
            StopCoroutine(nextTimelineCoroutine);
            nextTimelineCoroutine = null;
        }
    }
}
