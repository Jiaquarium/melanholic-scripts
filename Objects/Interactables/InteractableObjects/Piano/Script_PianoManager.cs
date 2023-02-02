using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Script_PianoManager : MonoBehaviour
{
    public static Script_PianoManager Control;
    public const int NumPianos = 5;

    [SerializeField] private Script_Piano[] pianos = new Script_Piano[NumPianos];
    
    [SerializeField] private Script_CanvasGroupController pianosCanvasGroup;
    [SerializeField] private Image choicesR1;
    [SerializeField] private Image choicesR2;
    [SerializeField] private GameObject choicesR1FirstSelected;
    [SerializeField] private GameObject choicesR2FirstSelected;
    [SerializeField] private EventSystem choicesEventSystem;
    [SerializeField] private Script_DialogueNode disabledMelancholyPianoReactionNode;

    [SerializeField] Script_Game game;
    [SerializeField] Script_DialogueManager dialogueManager;

    private AudioSource audioSource;
    
    // ------------------------------------------------------------------
    // For OnHitCancelUI Handling
    private bool isPianoEffect;
    private Coroutine disabledReactionCoroutine;
    private bool isDisabledReaction;
    private bool isDisabledDialogueRunning;
    private bool isCanceledDialogue;
    // ------------------------------------------------------------------

    public Script_Piano[] Pianos
    {
        get => pianos;
        set => pianos = value;
    }
    
    void OnEnable()
    {
        Script_CombatEventsManager.OnHitCancelUI += OnHitCancelUI;
    }

    void OnDisable()
    {
        Script_CombatEventsManager.OnHitCancelUI -= OnHitCancelUI;
    }
    
    public string GetPianoMapName(int idx)
    {
        return pianos[idx].MapName;
    }
    
    public bool GetPianoIsRemembered(int idx)
    {
        return pianos[idx].IsRemembered;
    }

    public void DisabledMelancholyPianoReaction(float time)
    {
        isDisabledReaction = true;
        disabledReactionCoroutine = StartCoroutine(WaitToReact());

        IEnumerator WaitToReact()
        {
            yield return new WaitForSeconds(time);

            dialogueManager.StartDialogueNode(disabledMelancholyPianoReactionNode, SFXOn: false);
            isDisabledDialogueRunning = true;
            StopMyCoroutines();
        }
    }
    
    // ------------------------------------------------------------------
    // UI / Unity Event

    public void SetPianosCanvasGroupActive(bool isActive)
    {
        if (isActive)
        {
            bool isR2 = game.RunCycle == Script_RunsManager.Cycle.Weekend;

            choicesR1.gameObject.SetActive(!isR2);
            choicesR2.gameObject.SetActive(isR2);

            choicesEventSystem.firstSelectedGameObject = isR2
                ? choicesR2FirstSelected
                : choicesR1FirstSelected;

            pianosCanvasGroup.Open();

            isPianoEffect = true;
        }
        else
        {
            pianosCanvasGroup.Close();

            isPianoEffect = false;
        }
    }

    /// <summary>
    /// When called via next node action in default case, change state to interact after. Otherwise, if this is
    /// being called to cut dialogue short in Spike Room, allow the restart behavior to control game state.
    /// </summary>
    public void OnDisabledReactionDone()
    {
        Dev_Logger.Debug($"{name} OnDisabledReactionDone isCanceledDialogue {isCanceledDialogue}");
        
        if (!isCanceledDialogue)
            game.ChangeStateInteract();

        InitialState();
    }

    // ------------------------------------------------------------------
    // Unity Event OnClick Handlers
    
    public void ExitPianoChoices()
    {
        InitialState();

        Script_SFXManager.SFX.PlayPianoNote();
        
        game.NextFrameChangeStateInteract();
    }
    
    // Called from Piano UI Choices
    public void PianoExit(int idx)
    {
        Script_Piano piano = pianos[idx];
        
        if (!piano.IsRemembered)
        {
            audioSource.PlayOneShot(Script_SFXManager.SFX.UIErrorSFX, Script_SFXManager.SFX.UIErrorSFXVol);
            return;
        }
        
        game.Exit(
            piano.Level,
            piano.PlayerSpawn,
            piano.FacingDirection,
            isExit: true,
            isSilent: false,
            exitType: Script_Exits.ExitType.Piano
        );

        InitialState();
    }
    // ------------------------------------------------------------------

    private void OnHitCancelUI(Script_HitBox hitBox, Script_HitBoxBehavior hitBoxBehavior)
    {
        // Note: this case will only happen in Spike Room, where Piano is disabled and hit with spike
        if (isDisabledReaction)
        {
            Dev_Logger.Debug($"{name} OnHitCancelUI isDisabledReaction");
            
            // In the case we're still waiting, stop coroutine, allow Restart behavior to set state
            StopMyCoroutines();

            // If dialogue is up, end dialogue, allow Restart behavior to set state
            if (isDisabledDialogueRunning)
            {
                Dev_Logger.Debug($"{name} OnHitCancelUI forcing end dialogue");

                // Let Restart Behavior handle game state
                isCanceledDialogue = true;
                
                // Ensure to prevent stacked calls, and wait for OnDisabledReactionDone to set InitialState
                isDisabledReaction = false;
                
                // This will then fire the next node action OnDisabledReactionDone
                dialogueManager.HandleEndDialogue();
            }
            else
            {
                InitialState();
            }
        }
        else if (isPianoEffect)
        {
            bool isStateHandled = (hitBoxBehavior != null && hitBoxBehavior.IsHitBoxBehaviorStateChanging())
                || Script_ClockManager.Control.IsClockDoneState;
            
            Dev_Logger.Debug($"{name} OnHitCanceLUI isStateHandled {isStateHandled}");

            CancelToInitialState(!isStateHandled);
        }
    }
    
    private void CancelToInitialState(bool isInteractAfter)
    {
        InitialState();
        
        if (isInteractAfter)
            game.ChangeStateInteract();
    }
    
    private void StopMyCoroutines()
    {
        if (disabledReactionCoroutine != null)
        {
            StopCoroutine(disabledReactionCoroutine);
            disabledReactionCoroutine = null;
        }
    }
    
    private void InitialState()
    {
        SetPianosCanvasGroupActive(false);

        isDisabledReaction = false;
        isDisabledDialogueRunning = false;
        isCanceledDialogue = false;
    }
    
    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        pianosCanvasGroup.Close();
    }
}
