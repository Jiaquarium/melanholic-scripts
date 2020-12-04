using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Script_TimelineController))]
[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_27 : Script_LevelBehavior
{
    [SerializeField] private Script_SavePoint sp;
    [SerializeField] private PlayableDirector initialFadeOutDirector;
    [SerializeField] private Script_DialogueNode monologueNode;
    [SerializeField] private EventSystem eventSystemMain;
    [SerializeField] private float amnesiaSFXWaitTime;
    [SerializeField] private Script_HitBox[] hitBoxes;
    [SerializeField] private Script_HitBox gameOverHitBox;
    
    private bool isTrackingInventoryClose;

    private bool isInit = true;
    
    protected override void OnEnable()
    {
        initialFadeOutDirector.stopped += OnDirectorStopped;
        Script_MenuEventsManager.OnExitMenu += OnExitMenuAfterCutScene;
    }

    protected override void OnDisable()
    {
        initialFadeOutDirector.stopped -= OnDirectorStopped;
        Script_MenuEventsManager.OnExitMenu -= OnExitMenuAfterCutScene;
    }

    private void OnDirectorStopped(PlayableDirector aDirector)
    {
        if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[0])
            OnInitialFadeOutDone();
        else if (aDirector.playableAsset == GetComponent<Script_TimelineController>().timelines[1])
            OpenThoughtsSequence();
    }

    /// <summary>
    /// NextNodeAction Handler; called from SavePoint SaveCompleteNode
    /// </summary>
    public void FadeOutBg()
    {
        game.ChangeStateCutScene();
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
        
        StartCoroutine(WaitAmnesiaSFX());

        IEnumerator WaitAmnesiaSFX()
        {
            yield return new WaitForSeconds(amnesiaSFXWaitTime);
            GetComponent<AudioSource>().PlayOneShot(
                Script_SFXManager.SFX.shellShock, Script_SFXManager.SFX.shellShockVol
            );
        }
    }
    public void FadeOutToCrunch()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);
    }

    private void OnInitialFadeOutDone()
    {
        game.GetPlayer().GetComponent<Script_PlayerStats>().Hurt(1, hitBoxes[0]);
        game.GetPlayer().GetComponent<Script_PlayerStats>().Hurt(1, hitBoxes[1]);
        game.GetPlayer().GetComponent<Script_PlayerStats>().Hurt(1, hitBoxes[2]);
        game.GetPlayer().GetComponent<Script_PlayerStats>().Hurt(1, hitBoxes[3]);
        Script_DialogueManager.DialogueManager.StartDialogueNode(monologueNode);
    }
    
    private void OpenThoughtsSequence()
    {
        game.InitializeMenuState(eventSystemMain);
        game.OpenInventory();

        // TODO: clean up, set the thought UI button to highlighted
        
        isTrackingInventoryClose = true;
    }

    private void OnExitMenuAfterCutScene()
    {
        if (isTrackingInventoryClose)
        {
            isTrackingInventoryClose = false;
            Debug.Log("Just closed main menu");

            StartCoroutine(WaitGoToTitle());
        }

        IEnumerator WaitGoToTitle()
        {
            yield return new WaitForSeconds(1.5f);
            Script_SceneManager.ToTitleScene();
        }
    }

    protected override void HandleAction()
    {
        // for cutScene dialogue
        base.HandleDialogueAction();
    }
    
    public override void Setup()
    {
        // game.SetupSavePoint(sp, isInit);
        
        /// go to game over screen
        // game.GetPlayer().GetComponent<Script_PlayerStats>().Hurt(999, gameOverHitBox);

        isInit = false;
    }
}