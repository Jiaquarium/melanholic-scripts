using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ScarletCipherPiece : Script_InteractableObject
{
    [SerializeField] private int _scarletCipherId;
    
    public int ScarletCipherId
    {
        get => _scarletCipherId;
        private set => _scarletCipherId = value;
    }

    public bool DidPickUp()
    {
        return Script_ScarletCipherManager.Control.ScarletCipherVisibility[ScarletCipherId];
    }

    protected override void Start()
    {
        base.Start();
        // Singletons hooked up in Awake() 
        if (DidPickUp())
        {
            Hide();
        }
    }

    public override void ActionDefault()
    {
        Script_ScarletCipherManager.Control.RevealScarletCipherSlot(ScarletCipherId);
        Hide();

        game.GetPlayer().ScarletCipherPickUpSFX();
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
