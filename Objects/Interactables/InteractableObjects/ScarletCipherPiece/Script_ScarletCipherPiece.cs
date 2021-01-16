using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ScarletCipherPiece : Script_InteractableObject
{
    [SerializeField] private int _scarletCipherId;
    
    private int ScarletCipherId
    {
        get => _scarletCipherId;
        set => _scarletCipherId = value;
    }

    protected override void Start()
    {
        base.Start();
        // Singletons hooked up in Awake() 
        if (Script_ScarletCipherManager.Control.ScarletCipherVisibility[ScarletCipherId])
        {
            Hide();
        }
    }

    public override void ActionDefault()
    {
        Script_ScarletCipherManager.Control.RevealScarletCipherSlot(ScarletCipherId);
        Hide();
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
