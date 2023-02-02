using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// While the Full Art is showing, disable menu UI state, so we're only listening from here.
/// </summary>
public class Script_CollectiblesInventoryHandler : MonoBehaviour
{
    [SerializeField] private Script_UIState mainController;
    [SerializeField] private Script_ItemsController ItemsController;
    [SerializeField] private Script_FullArtDictionary fullArtDictionary;
    
    private Script_InventoryAudioSettings settings;
    private Script_FullArt fullArt;
    private Script_Collectible collectible;
    
    public bool IsInputDisabled; // to prevent from cancelling before fullart is shown and stacking requests
    public bool IsFullArtMode { get; set; }

    private void Update()
    {
        if (!IsFullArtMode || IsInputDisabled)
            return;

        var playerInput = Script_PlayerInputManager.Instance.MyPlayerInput;
        
        if (
            playerInput.actions[Const_KeyCodes.Inventory].WasPressedThisFrame()
            || playerInput.actions[Const_KeyCodes.UICancel].WasPressedThisFrame()
            || playerInput.actions[Const_KeyCodes.Interact].WasPressedThisFrame()
            || playerInput.actions[Const_KeyCodes.UISubmit].WasPressedThisFrame()
        )
        {
            IsInputDisabled = true;
            
            /// Need to disable exit input managers bc if we exit too fast before fadeOut cb is finished
            /// ItemChoicesInputManager will exit before this can
            mainController.state = UIState.Disabled;

            if (fullArt.nextFullArt == null)
            {
                Dev_Logger.Debug($"End fullart detected; {this.name} attempting to hide collectible full art");

                Script_Game.Game.fullArtManager.HideFullArt(
                    fullArt,
                    // collectible.fadeOutSpeed,
                    FadeSpeeds.XXSlow,
                    () => {
                        InitialState();
                    }
                );
            }
            else
            {
                ContinueExamine();
            }
        }
    }
    
    public void Examine(Script_Collectible _collectible)
    {
        if (IsInputDisabled)
            return;

        Dev_Logger.Debug("trying to examine:" + _collectible.name);
        collectible = _collectible;

        // show full art and set it via out
        if (!fullArtDictionary.myDictionary.TryGetValue(collectible.fullArtId, out fullArt))
        {
            // the key isn't in the dictionary.
            Dev_Logger.Debug("value of fullArt" + fullArt);
            Debug.LogError($"Key:{collectible.fullArtId} is not in dictionary");
            ItemsController.ExitFullArt();
            return;
        }
        
        /// Disables exit input managers that would otherwise be listening for Cancel to fire ExitSubmenu & ExitMenu events
        mainController.state = UIState.Disabled;
        
        IsInputDisabled = true;
        ItemsController.EnterFullArt();
        Script_Game.Game.fullArtManager.ShowFullArt(
            fullArt,
            // collectible.fadeInSpeed,
            FadeSpeeds.XXSlow,
            () =>
                {
                    IsFullArtMode = true;
                    IsInputDisabled = false;
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

    public void CancelToInitialState()
    {
        Dev_Logger.Debug($"{name} Cancel to Initial State");

        Script_Game.Game.fullArtManager.CancelToInitialState(fullArt);
        InitialState();
    }
    
    private void InitialState()
    {
        Dev_Logger.Debug($"player state: {Script_Game.Game.GetPlayer().State}");
        
        mainController.state = UIState.Interact;

        IsInputDisabled = false;
        IsFullArtMode = false;
        collectible = null;

        // Reactivate EventSystemMain and get back to inventory item slots
        ItemsController.ExitFullArt();
    }

    public void Setup(
        Script_InventoryAudioSettings _settings
    )
    {
        settings = _settings;
    }
}
