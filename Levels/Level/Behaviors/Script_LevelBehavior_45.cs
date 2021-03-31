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
    
    private Script_LanternFollower lanternFollower;
    
    // Should not be saved in state because this quest should be repeatable on
    // subsequent runs.
    private bool didPickUpLastSpellRecipeBook;
    
    protected override void OnEnable()
    {
        Script_ItemsEventsManager.OnItemPickUp      += OnItemPickUp;
    }

    protected override void OnDisable()
    {
        Script_ItemsEventsManager.OnItemPickUp      -= OnItemPickUp;
    }
    
    protected override void Update()
    {
        base.Update();

        HandleLanternReactions(lanternFollower.IsLightOn);
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

        if (lastSpellRecipeBook != null)
        {
            if (!didPickUpLastSpellRecipeBook)
                lastSpellRecipeBook.gameObject.SetActive(isLightOn);
        }
    }

    public override void Setup()
    {
        lanternFollower = game.LanternFollower;

        if (lastSpellRecipeBook != null)
        {
            if (didPickUpLastSpellRecipeBook)   lastSpellRecipeBook.gameObject.SetActive(false);
            else                                lastSpellRecipeBook.gameObject.SetActive(true);
        }
    }
}