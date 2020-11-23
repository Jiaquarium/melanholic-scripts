using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Script_LevelBehavior : MonoBehaviour
{
    public Script_Game game;

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
        HandlePuzzle();
        HandleOnEntrance();
    }

    protected virtual void HandleAction() {}
    protected virtual void HandlePuzzle() {}
    protected virtual void HandleOnEntrance() {}
    /// <summary>
    /// this is for moving forward in dialogue during cut scenes
    /// best place is here because this could be separate of player logic
    /// (e.g. narration scenes, etc.)
    /// </summary>
    protected virtual void HandleDialogueAction()
    {
        if (
            Input.GetButtonDown(Const_KeyCodes.Action1)
            && game.state == Const_States_Game.CutScene
        )
        {
            Debug.Log("Script_LevelBehavior: HandleDialogueAction()");
            game.HandleContinuingDialogueActions(Const_KeyCodes.Action1);
        }
    }

    protected virtual void OnDisable() {}
    protected virtual void OnEnable() {}
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
    public virtual void Setup()
    {
        // game.CreateNPCs();
        // game.CreateInteractableObjects();
        // game.CreateDemons();
    }
}
