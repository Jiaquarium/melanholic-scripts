using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Script_CrackableStats : Script_CharacterStats
{
    [SerializeField] private UnityEvent<Script_CrackableStats> crackedAction;
    [Tooltip("Does not Die on crack. All behavior will be specified by crackedAction.")]
    [SerializeField] private bool isHandleDieExplicit;    
    
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite lowHealthImage;
    [SerializeField] private int lowHealthThreshold;
    
    [SerializeField] private SpriteRenderer graphics;

    [SerializeField] private PlayableDirector crackingDirector;

    [SerializeField] private List<GameObject> visibleChildren;
    
    protected UnityEvent<Script_CrackableStats> CrackedAction
    {
        get => crackedAction;
    }
    
    public override int Hurt(int dmg, Script_HitBox hitBox)
    {
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);

        // reduce health
        currentHp -= dmg;
        currentHp = Mathf.Clamp(currentHp, 0, int.MaxValue);
        
        Debug.Log($"{transform.name} took damage {dmg}. currentHp: {currentHp}");
        
        if (currentHp == 0)
        {
            CrackedAction.SafeInvokeDynamic(this);

            if (crackingDirector != null)
                crackingDirector.Play();
            
            if (!isHandleDieExplicit)
                Die(Script_GameOverController.DeathTypes.Default);
        }
        else
        {
            // Show a different spirte depending on how hurt the Interactable is
            HandleHurtGraphics(currentHp);
        }

        return dmg;
    }
    
    protected override void Die(Script_GameOverController.DeathTypes deathType)
    {
        Debug.Log("**** CRACKABLE DIE() ****");
        gameObject.SetActive(false);
    }

    private void HandleHurtGraphics(int hp)
    {
        if (graphics == null)   return;
        
        if      (hp <= lowHealthThreshold)      graphics.sprite = lowHealthImage;
        else                                    graphics.sprite = defaultImage;
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void DieTimeline()
    {
        // Only turn off graphics so we can still play the rest of Timeline.
        foreach (var child in visibleChildren)
            child.SetActive(false);
    }

    public void OnIceBlockCrackingTimelineDone(Script_CrackableStats ice)
    {
        Debug.Log($"OnIceBlockCrackingTimelineDone ice <{ice}>");
        
        Script_InteractableObjectEventsManager.IceCrackingTimelineDone(this);

        gameObject.SetActive(false);
        
        // Revert children state from DieTimeline().
        foreach (var child in visibleChildren)
            child.SetActive(true);
    }
}
