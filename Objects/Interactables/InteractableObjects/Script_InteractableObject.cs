using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Script_InteractableObject : Script_Interactable
{
    [SerializeField] protected States _state;

    public int Id;
    public string nameId;
    protected bool isActive = true;
    [SerializeField] protected Transform rendererChild;
    protected Script_Game game;
    [SerializeField] private UnityEvent action;
    [Tooltip("Easier way to reference Game if we don't care about Setup()")] [SerializeField] protected bool autoSetup;

    private bool didAutoSetupFail;

    public enum States
    {
        Active = 0,
        Disabled = 1
    }

    public virtual States State
    {
        get => _state;
        set => _state = value;
    }

    protected UnityEvent MyAction
    {
        get => action;
    }

    protected virtual void OnEnable()
    {
        if (autoSetup)
        {
            AutoSetup();
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }
    
    protected virtual void Start()
    {
        if (didAutoSetupFail)
        {
            game = Script_Game.Game;
            game.AddInteractableObject(this);
            
            didAutoSetupFail = false;
        }
    }

    protected virtual void AutoSetup()
    {
        try
        {
            game = Script_Game.Game;
            game.AddInteractableObject(this);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Game is not instantiated yet: {e}");
            didAutoSetupFail = true;
        }
    }
    
    // Update is called once per frame
    protected virtual void Update() {}

    public virtual void HandleAction(string action)
    {
        print($"Handling action: {action}");
        if (action == Const_KeyCodes.Action1 && State == States.Active)
        {
            ActionDefault();
        }
    }
    
    protected virtual void ActionDefault()
    {
        Debug.Log($"{name} Action default called in InteractableObject");
        InvokeAction();
    }

    public void SetInteractionActive(bool isActive)
    {
        if (isActive)   SetActive();
        else            SetDisabled();
    }

    protected virtual void SetActive()
    {
        State = States.Active;
    }

    protected virtual void SetDisabled()
    {
        State = States.Disabled;
    }

    protected void InvokeAction()
    {
        if (MyAction.CheckUnityEventAction()) MyAction.Invoke();
    }
    
    public virtual void ActionB() {}
    
    public virtual void ActionC() {}
    
    public virtual void Setup(
        bool isForceSortingLayer,
        bool isAxisZ,
        int offset
    )
    {
        game = Script_Game.Game;
        if (isForceSortingLayer)    EnableSortingOrder(isAxisZ, offset); 
    }
    
    public virtual void SetupSwitch(
        bool isOn,
        Sprite onSprite,
        Sprite offSprite
    ) {}
    
    public virtual void SetupLights(
        Light[] lights,
        float onIntensity,
        float offIntensity,
        bool isOn,
        Sprite onSprite,
        Sprite offSprite
    ){}
    
    public virtual void SetupText(
        Script_DialogueManager dm,
        Script_Player p,
        Model_Dialogue d
    ){}

    public virtual void SwitchDialogueNodes(Script_DialogueNode[] nodes, bool isReset = true){}

    public virtual void EnableSortingOrder(bool isAxisZ, int offset)
    {
        rendererChild.GetComponent<Script_SortingOrder>().EnableWithOffset(offset, isAxisZ);
    }

    public Transform GetRendererChild()
    {
        return rendererChild;
    }

    public void SetAlpha(float alpha)
    {
        try
        {
            var graphics = rendererChild.GetComponent<SpriteRenderer>();
            Color newColor = graphics.color;
            newColor.a = alpha;
            graphics.color = newColor;
        }
        catch (System.ArgumentNullException e)
        {
            Debug.LogError($"You need to add a Sprites Renderer component to rendererChild {rendererChild}; error: {e}");
        }
    }

    public virtual void InitializeState() {}
}
