using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the Puppeteer Transform animations.
/// </summary>
[RequireComponent(typeof(Script_TimelineController))]
public class Script_PuppetPuzzleController : Script_PuzzleController
{
    [SerializeField] protected bool isDone;
    [SerializeField] protected List<Script_Puppet> buffPuppets;
    [SerializeField] private Script_PostProcessingSettings postProcessingPuppeteer;
    
    [SerializeField] protected Script_Game game;


    protected Script_TimelineController timelineController;
    
    public bool IsDone
    {
        get => isDone;
        set => isDone = value;
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();

        Script_PlayerEventsManager.OnPuppeteerActivate      += OnPuppeteerActivate;
        Script_PlayerEventsManager.OnPuppeteerDeactivate    += OnPuppeteerDeactivate;

        // Handle Urselks Saloon Hallway having slightly lighter vignette, since room is already very dark
        if (game.levelBehavior == game.UrsaSaloonHallwayBehavior)
            postProcessingPuppeteer.SetVignettePuppeteerSaloonHallway();
        else
            postProcessingPuppeteer.SetVignettePuppeteerDefault();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Script_PlayerEventsManager.OnPuppeteerActivate      -= OnPuppeteerActivate;
        Script_PlayerEventsManager.OnPuppeteerDeactivate    -= OnPuppeteerDeactivate;
    }    

    protected virtual void Awake()
    {
        timelineController = GetComponent<Script_TimelineController>();
    }

    protected virtual void OnPuppeteerActivate()
    {
        if (!IsDone)
        {
            Dev_Logger.Debug($"{name} OnPuppeteerActivate");
            
            game.ChangeStateCutScene();
            
            Script_SFXManager.SFX.PlayPuppeteerEffectOn();
            
            // This Timeline is also shared with Urselks Saloon Hallway Puppet Puzzle.
            timelineController.PlayableDirectorPlayFromTimelines(0, 0);
        }
    }

    protected virtual void OnPuppeteerDeactivate()
    {
        if (!IsDone)
        {
            Dev_Logger.Debug($"{name} OnPuppeteerDeactivate");
            
            game.ChangeStateCutScene();
            
            Script_SFXManager.SFX.PlayPuppeteerEffectOff();
            
            // This Timeline is also shared with Urselks Saloon Hallway Puppet Puzzle.
            timelineController.PlayableDirectorPlayFromTimelines(0, 1);
        }   
    }

    // ------------------------------------------------------------------
    // Timeline Signals

    public virtual void PuppeteerActivateTimelinePlayerBuff()
    {
        Script_Game.Game.GetPlayer().SetBuffEffectActive(true);
    }
    
    public virtual void OnPuppeteerActivateTimelineDone()
    {
        Script_Game.Game.GetPlayer().SetBuffEffectActive(false);
        
        // Wait a frame before interacting / must wait at least a frame after stopping timeline to modify its objects
        StartCoroutine(WaitToInteract());
        
        IEnumerator WaitToInteract()
        {
            yield return null;
            
            game.ChangeStateInteract();
            PostProcessingInitialize();
        }
    }
    
    public virtual void PuppeteerDeactivateTimelinePuppetBuffs()
    {
        buffPuppets.ForEach(puppet => puppet.SetBuffEffectActive(true));
    }

    public virtual void OnPuppeteerDeactivateTimelineDone()
    {
        buffPuppets.ForEach(puppet => puppet.SetBuffEffectActive(false));
        
        StartCoroutine(WaitToInteract());
        
        IEnumerator WaitToInteract()
        {
            yield return null;
            
            game.ChangeStateInteract();
            PostProcessingInitialize();
        }
    }

    protected void PostProcessingInitialize()
    {
        postProcessingPuppeteer.gameObject.SetActive(false);
        postProcessingPuppeteer.InitialStateWeight();
    }
}
