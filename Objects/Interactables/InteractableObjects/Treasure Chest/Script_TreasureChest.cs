using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NOTE: on Item's NextNodeAction
/// NOTE: Default behavior is only interactable from the front; change the disabler to reflect this
/// </summary>
public class Script_TreasureChest : Script_InteractableObject
{
    [SerializeField] private bool _isOpen;
    
    [Tooltip("The item inside the chest.")]
    [SerializeField] private Script_ItemObject item;
    
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;

    [SerializeField] private bool useMesh;
    [SerializeField] private Script_Interactable openMesh;
    [SerializeField] private Script_Interactable closedMesh;

    public bool IsOpen
    {
        get => _isOpen;
        set
        {
            _isOpen = value;
            HandleGraphics(_isOpen);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        Script_ItemsEventsManager.OnItemStash += OnItemStash;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Script_ItemsEventsManager.OnItemStash -= OnItemStash;
    }

    protected override void Start()
    {
        base.Start();
        HandleGraphics(IsOpen);
    }

    protected override void ActionDefault()
    {
        Debug.Log($"{name}: Action default called in TreasureChest");
        
        if (CheckDisabledDirections())
        {
            Debug.Log($"{name}: Action default from Disabled Direction");
            return;
        }
        
        if (!IsOpen)
        {
            Script_Game.Game.HandleItemReceive(item);
            IsOpen = true;
        }
    }

    private void OnItemStash(string itemId)
    {
        if (item.Item.id == itemId)
        {
            Debug.Log("ON STASH ITEM called from treasure chest");
        }
    }

    private void HandleGraphics(bool isOpen)
    {
        if (useMesh)
        {
            openMesh.gameObject.SetActive(isOpen);
            closedMesh.gameObject.SetActive(!isOpen);
        }
        else
        {
            var sprite = isOpen ? openSprite : closedSprite;
            rendererChild.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
