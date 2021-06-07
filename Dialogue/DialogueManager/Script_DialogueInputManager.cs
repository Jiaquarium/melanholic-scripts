using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_DialogueInputManager : MonoBehaviour
{
    [SerializeField] private Script_Game game;
    
    void Update()
    {
        HandleCutSceneDialogueAction();
    }
    
    protected virtual void HandleCutSceneDialogueAction()
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
}
