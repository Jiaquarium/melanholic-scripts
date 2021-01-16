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
    [SerializeField] private Script_ItemObject item;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;

    private bool IsOpen
    {
        get => _isOpen;
        set => _isOpen = value;
    }

    void OnEnable()
    {
        Script_ItemsEventsManager.OnItemStash += OnItemStash;
    }

    void OnDisable()
    {
        Script_ItemsEventsManager.OnItemStash -= OnItemStash;
    }

    protected override void Start()
    {
        base.Start();
        if (_isOpen)    ChangeSprite(openSprite);
        else            ChangeSprite(closedSprite);
    }

    public override void ActionDefault()
    {
        Debug.Log($"{name}: Action default called in TreasureChest");
        
        if (CheckDisabledDirections())  return;
        
        if (!IsOpen)
        {
            Script_Game.Game.HandleItemReceive(item);
            ChangeSprite(openSprite);
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

    private void ChangeSprite(Sprite img)
    {
        rendererChild.GetComponent<SpriteRenderer>().sprite = img;
    }
}
