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

    private static readonly int IsActive = Animator.StringToHash("IsActive");
    private static readonly int Sketch = Animator.StringToHash("Sketch");
    
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

    protected override void OnEnable()
    {
        HandlePaintingSprite(State);
        
        base.OnEnable();
    }

    // Painting Entrance have option to allow text interaction.
    public override void HandleAction(string action)
    {
        Dev_Logger.Debug($"{name} HandleAction action: {action}");
        bool isDisabled = !isAllowDisabledDialogue && State == States.Disabled;

        if (action == Const_KeyCodes.InteractAction && !isDisabled)
        {
            ActionDefault();
        }
    }
    
    protected override void ActionDefault()
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
        StartCoroutine(WaitToStartEntranceNode());

        IEnumerator WaitToStartEntranceNode()
        {
            yield return new WaitForSeconds(BoarNeedleWaitTime);
            
            Script_Game.Game.GetPlayer().SetIsStandby();

            if (State == States.Disabled)
            {
                var paintingEntranceManager = Script_DialogueManager.DialogueManager.paintingEntranceManager;
                
                yield return new WaitForSeconds(paintingEntranceManager.BeforeDisabledReactionWaitTime);
                
                if (paintingEntranceManager.DidTryDisabledEntrance)
                {
                    Dev_Logger.Debug($"{name} State: {State}");
                    Script_SFXManager.SFX.PlayTVChannelChangeStatic(
                        Script_Game.Game.GetPlayer().SetIsInteract
                    );
                }
                else
                {
                    Script_Game.Game.GetPlayer().SetIsInteract();
                    Script_Game.Game.ChangeStateCutScene();
                    Script_DialogueManager.DialogueManager.StartDialogueNode(
                        paintingEntranceManager.disabledPaintingEntranceReactionNode,
                        SFXOn: true
                    );

                    // Play SFX on dialogue done.
                }

                paintingEntranceManager.DidTryDisabledEntrance = true;
            }
            else
            {
                // Script_Game.Game.GetPlayer().SetIsInteract();
                Dev_Logger.Debug("starting dialogue node in painting");
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

    public void SetSketchAnimation()
    {
        if (myAnimator != null)
            myAnimator.SetTrigger(Sketch);
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
                {
                    Dev_Logger.Debug($"{name} Setting to States.Active");
                    
                    paintingGraphics.sprite = activeSprite;
                    if (myAnimator != null)
                        myAnimator.SetBool(IsActive, true);
                }
                break;
            
            case (States.Disabled):
                if (paintingGraphics != null && disabledSprite != null)
                {
                    Dev_Logger.Debug($"{name} Setting to States.Disabled");
                    
                    paintingGraphics.sprite = disabledSprite;
                    if (myAnimator != null)
                        myAnimator.SetBool(IsActive, false);
                }
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

        if (GUILayout.Button("Sketch"))
        {
            t.SetSketchAnimation();
        }
    }
}
#endif