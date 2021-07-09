using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_TimelineController))]
public class Script_Moose : Script_DemonNPC
{
    [SerializeField] private Script_DemonNPC Suzette;
    
    // Also sets Suzette Inactive.
    public void FinalSpellExit()
    {
        GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 0);
    }

    // ----------------------------------------------------------------------
    // Timeline Signals
    
    public void SuzetteExit()
    {
        Suzette.gameObject.SetActive(false);
    }

    // ----------------------------------------------------------------------
}
