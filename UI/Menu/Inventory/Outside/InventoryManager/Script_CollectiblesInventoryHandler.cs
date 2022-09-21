using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Script_CollectiblesInventoryHandler : MonoBehaviour
{
    [SerializeField] private Script_UIState mainController;
    [SerializeField] private Script_ItemsController ItemsController;
    [SerializeField] private Script_FullArtDictionary fullArtDictionary;
    private Script_InventoryAudioSettings settings;
    private bool isFullArtMode;
    private bool isInputDisabled; // to prevent from cancelling before fullart is shown and stacking requests
    private Script_FullArt fullArt;
    private Script_Collectible collectible;

    private void Update()
    {
        if (!isFullArtMode || isInputDisabled)      return;

        var playerInput = Script_PlayerInputManager.Instance.MyPlayerInput;
        
        if (
            playerInput.actions[Const_KeyCodes.Inventory].WasPressedThisFrame()
            || playerInput.actions[Const_KeyCodes.UICancel].WasPressedThisFrame()
            || playerInput.actions[Const_KeyCodes.Interact].WasPressedThisFrame()
            || playerInput.actions[Const_KeyCodes.UISubmit].WasPressedThisFrame()
        )
        {
            isInputDisabled = true;
            /// Need to disable exit input managers bc if we exit too fast before fadeOut cb is finished
            /// ItemChoicesInputManager will exit before this can
            mainController.state = UIState.Disabled;

            if (fullArt.nextFullArt == null)
            {
                Dev_Logger.Debug($"End fullart detected; {this.name} attempting to hide collectible full art");

                Script_Game.Game.fullArtManager.HideFullArt(fullArt, collectible.fadeOutSpeed, () =>
                {
                    print("isplayer state interact: " + Script_Game.Game.GetPlayer().State == Const_States_Player.Interact);
                    isInputDisabled = false;
                    mainController.state = UIState.Interact;
                    isFullArtMode = false;
                    collectible = null;

                    // reactivate EventSystemMain and get back to inventory slots
                    ItemsController.ExitFullArt();
                });
            }
            else
            {
                ContinueExamine();
            }
        }
    }
    
    public void Examine(Script_Collectible _collectible)
    {
        if (isInputDisabled)    return;
        print("trying to examine:" + _collectible.name);
        collectible = _collectible;

        // show full art and set it via out
        if (!fullArtDictionary.myDictionary.TryGetValue(collectible.fullArtId, out fullArt))
        {
            // the key isn't in the dictionary.
            print("value of fullArt" + fullArt);
            Debug.LogError($"Key:{collectible.fullArtId} is not in dictionary");
            ItemsController.ExitFullArt();
            return;
        }
        
        isInputDisabled = true;
        ItemsController.EnterFullArt();
        Script_Game.Game.fullArtManager.ShowFullArt(
            fullArt,
            collectible.fadeInSpeed,  // Use collectible fadeIn speed so fullArt can be extensible
            () =>
                {
                    isFullArtMode = true;
                    isInputDisabled = false;
                },
            Script_FullArtManager.FullArtState.Inventory
        );
        // on space or enter exit, reactivate eventSystem
    }

    /// <summary>
    /// TODO: function to move to next full art that belongs to the collectible if any
    /// </summary>
    public void ContinueExamine()
    {
        Debug.LogError("You need to implement ContinueExamine() in Examine in Script_CollectiblesInventoryHandler");
    }

    public void Setup(
        Script_InventoryAudioSettings _settings
    )
    {
        settings = _settings;
    }
}
