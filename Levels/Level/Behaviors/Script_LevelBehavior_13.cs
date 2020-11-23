using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_13 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool didPickUpAutumnStone;
    
    /* ======================================================================= */

    [SerializeField] private Script_CollectibleObject autumnStone;

    protected override void OnEnable()
    {
        Script_ItemsEventsManager.OnItemPickUp += OnItemPickUp;
    }

    protected override void OnDisable()
    {
        Script_ItemsEventsManager.OnItemPickUp -= OnItemPickUp;
    }
    
    private void OnItemPickUp(string itemId)
    {
        if (itemId == autumnStone.GetItem().id)
        {
            didPickUpAutumnStone = true;
        }
    }

    void Start()
    {
        if (didPickUpAutumnStone)
        {
            if (autumnStone != null)    autumnStone.gameObject.SetActive(false);
        }
        else                            autumnStone.gameObject.SetActive(true);
    }
}
