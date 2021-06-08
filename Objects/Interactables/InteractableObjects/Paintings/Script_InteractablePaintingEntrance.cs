using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_InteractablePaintingEntrance : Script_QuestPainting
{
    static readonly private string BoarNeedle = Const_Items.BoarNeedleId; 
    static readonly private float BoarNeedleWaitTime = 0.5f; 
    
    [SerializeField] private Script_ExitMetadataObject exit;
    public Script_DialogueNode[] paintingDialogueNodes;
    
    [SerializeField] private SpriteRenderer paintingGraphics;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite disabledSprite;

    [SerializeField] private bool isAllowDisabledDialogue;

    private int paintingDialogueIndex;
    
    public override States State
    {
        get => _state;
        set
        {
            _state = value;

            HandlePaintingSprite(_state);
        }
    }
    
    // Painting Entrance have option to allow text interaction.
    public override void HandleAction(string action)
    {
        Debug.Log($"{name} HandleAction action: {action}");
        bool isDisabled = !isAllowDisabledDialogue && State == States.Disabled;

        if (action == Const_KeyCodes.Action1 && !isDisabled)
        {
            ActionDefault();
        }
    }
    
    public override void ActionDefault()
    {
        if (
            isDialogueCoolDown
            || CheckDisabledDirections()
        )
        {
            return;
        }
        
        // If already talking to the Painting, then just continue dialogue.
        if (
            Script_ActiveStickerManager.Control.IsActiveSticker(BoarNeedle)
            && Script_Game.Game.GetPlayer().State == Const_States_Player.Dialogue
        )
        {
            ContinueDialogue();
        }
        else
        {
            base.ActionDefault();
        }
    }

    public void InitiatePaintingEntrance()
    {
        Script_Game.Game.GetPlayer().SetIsStandby();
        StartCoroutine(WaitToStartEntranceNode());

        IEnumerator WaitToStartEntranceNode()
        {
            yield return new WaitForSeconds(BoarNeedleWaitTime);

            if (State == States.Disabled)
            {
                Debug.Log($"{name} State: {State}");
                Script_Game.Game.GetPlayer().SetIsInteract();
            }
            else
            {
                // Script_Game.Game.GetPlayer().SetIsInteract();
                Debug.Log("starting dialogue node in painting");
                /// Player state is set to dialogue by DM but just to be safe
                Script_Game.Game.GetPlayer().SetIsTalking();
                Script_DialogueManager.DialogueManager.StartDialogueNode(
                    paintingDialogueNodes[paintingDialogueIndex],
                    SFXOn: true,
                    type: Const_DialogueTypes.Type.PaintingEntrance,
                    this
                );
                HandlePaintingDialogueNodeIndex();
            }
        }
    }

    public void HandleExit()
    {
        Script_Game.Game.Exit(
            exit.data.level,
            exit.data.playerSpawn,
            exit.data.facingDirection,
            true
        );   
    }

    // ------------------------------------------------------------------
    // Timeline Signal Reactions

    public void SetStateActive()
    {
        State = States.Active;
    }
    // ------------------------------------------------------------------

    private void HandlePaintingDialogueNodeIndex()
    {
        if (paintingDialogueIndex == paintingDialogueNodes.Length - 1)
        {
            paintingDialogueIndex = 0;    
        }
        else
        {
            paintingDialogueIndex++;
        }
    }

    private void HandlePaintingSprite(States state)
    {
        if (isDonePainting) return;
        
        switch (state)
        {
            case (States.Active):
                if (paintingGraphics != null && activeSprite != null)
                    paintingGraphics.sprite = activeSprite;
                break;
            
            case (States.Disabled):
                if (paintingGraphics != null && disabledSprite != null)
                    paintingGraphics.sprite = disabledSprite;
                break;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_InteractablePaintingEntrance))]
public class Script_InteractablePaintingEntranceTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_InteractablePaintingEntrance t = (Script_InteractablePaintingEntrance)target;
        if (GUILayout.Button("Done Painting"))
        {
            t.DonePainting();
        }

        if (GUILayout.Button("Default Painting"))
        {
            t.DefaultPainting();
        }

        if (GUILayout.Button("Set State Disabled"))
        {
            t.State = Script_InteractablePaintingEntrance.States.Disabled;
        }

        if (GUILayout.Button("Set State Active"))
        {
            t.State = Script_InteractablePaintingEntrance.States.Active;
        }
    }
}
#endif