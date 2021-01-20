using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Script_InteractableObject : Script_Interactable
{
    public int Id;
    public string nameId;
    protected bool isActive = true;
    [SerializeField] protected Transform rendererChild;
    protected Script_Game game;
    [SerializeField] private UnityEvent action;
    [Tooltip("Easier way to reference Game if we don't care about Setup()")] [SerializeField] protected bool autoSetup;
    
    protected virtual void OnEnable()
    {
        if (autoSetup)
        {
            AutoSetup();
        }
    }
    
    protected virtual void Start()
    {
        
    }

    protected virtual void AutoSetup()
    {
        game = Script_Game.Game;
        Script_Game.Game.AddInteractableObject(this);
    }
    
    // Update is called once per frame
    protected virtual void Update() {}

    public virtual void HandleAction(string action)
    {
        print($"Handling action: {action}");
        if (action == Const_KeyCodes.Action1)
        {
            ActionDefault();
        }
    }
    
    public virtual void ActionDefault()
    {
        Debug.Log($"{name} Action default called in InteractableObject");
        if (action.CheckUnityEventAction()) action.Invoke();
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

    public virtual void SwitchDialogueNodes(Script_DialogueNode[] nodes){}

    public virtual void EnableSortingOrder(bool isAxisZ, int offset)
    {
        rendererChild.GetComponent<Script_SortingOrder>().EnableWithOffset(offset, isAxisZ);
    }

    public Transform GetRendererChild()
    {
        return rendererChild;
    }

    public virtual void InitializeState() {}
}
