using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_45 : Script_LevelBehavior
{
    // ==================================================================
    // State Data

    // ==================================================================

    [SerializeField] private Light directionalLight;
    [SerializeField] private Script_CollectibleObject lastSpellRecipeBook;

    [SerializeField] private bool isFinalTrueEndingTimeline;

    [SerializeField] private Transform cursedOnes;

    // Should not be saved in state because this quest should be repeatable on
    // subsequent runs.
    private bool didPickUpLastSpellRecipeBook;

    public bool IsFinalTrueEndingTimeline
    {
        get => isFinalTrueEndingTimeline;
        set => isFinalTrueEndingTimeline = value;
    }
    
    protected override void OnEnable()
    {
        Script_ItemsEventsManager.OnItemPickUp      += OnItemPickUp;

        if (IsFinalTrueEndingTimeline)
            HandleLanternReactions(true);
        else
            HandleLanternReactions(game.GetPlayer().IsLightOn);
    }

    protected override void OnDisable()
    {
        Script_ItemsEventsManager.OnItemPickUp      -= OnItemPickUp;

        Debug.Log($"On Disable: Setting IsFinalTrueEndingTimeline {IsFinalTrueEndingTimeline} to false");
        IsFinalTrueEndingTimeline = false;
    }
    
    protected override void Update()
    {
        base.Update();

        if (IsFinalTrueEndingTimeline)
            HandleLanternReactions(true);
        else
            HandleLanternReactions(game.GetPlayer().IsLightOn);
    }

    private void OnItemPickUp(string itemId)
    {
        if (itemId == lastSpellRecipeBook.Item.id)
        {
            didPickUpLastSpellRecipeBook = true;
        }
    }

    private void HandleLanternReactions(bool isLightOn)
    {
        directionalLight.gameObject.SetActive(isLightOn);
    }

    public override void Setup()
    {
        if (lastSpellRecipeBook != null)
        {
            if (didPickUpLastSpellRecipeBook)   lastSpellRecipeBook.gameObject.SetActive(false);
            else                                lastSpellRecipeBook.gameObject.SetActive(true);
        }
    }
}