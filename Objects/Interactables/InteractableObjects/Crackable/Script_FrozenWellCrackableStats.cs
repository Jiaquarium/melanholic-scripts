using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For Frozen Well
[RequireComponent(typeof(Script_TimelineController))]
public class Script_FrozenWellCrackableStats : Script_CrackableStats
{
    [SerializeField] private List<GameObject> wellObjects;
    
    public override int Hurt(int dmg, Script_HitBox hitBox, Script_HitBoxBehavior hitBoxBehavior)
    {
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);

        // reduce health
        currentHp -= dmg;
        currentHp = Mathf.Clamp(currentHp, 0, int.MaxValue);
        
        Dev_Logger.Debug($"{transform.name} took damage {dmg}. currentHp: {currentHp}");
        
        if (currentHp == 0)
        {
            CrackedAction.SafeInvokeDynamic(this);

            Script_Game.Game.ChangeStateCutScene();
            
            // Play the Frozen Well Shattering Timeline
            // This Timeline Flow Should Call the following functions:
            // 1. DiagonalCut
            // 2. Shatter & HideWell
            // 3. AllowPlayerInteract
            // 4. OnFrozenWellShatterTimelineDone
            GetComponent<Script_TimelineController>().PlayableDirectorPlayFromTimelines(0, 1);   
        }

        return dmg;
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void HideWell()
    {
        wellObjects.ForEach(obj => obj.SetActive(false));
        
        // Preemptively hide all other Frozen Wells.
        Script_InteractableObjectEventsManager.FrozenWellDie(this);
        
        // Hide Ice on disable in case player exits before the Shattering Timeline completes.
        // (OnFrozenWellShatterTimelineDone's HideIce will not be called in this case).
        isHideOnDisable = true;
    }

    public void OnFrozenWellShatterTimelineDone(Script_CrackableStats ice)
    {
        Script_InteractableObjectEventsManager.IceCrackingTimelineDone(this);
        
        // Hide Ice Remains.
        HideIce();
    }

    // ------------------------------------------------------------------
}
