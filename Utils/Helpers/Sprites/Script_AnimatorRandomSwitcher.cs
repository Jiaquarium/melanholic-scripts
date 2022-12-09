using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AnimatorRandomSwitcher : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController animatorOverride;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Script_StaticNPC npc;
    [SerializeField] private Script_AnimatorPositionAdjuster animatorPositionAdjuster;
    [SerializeField] private bool isDisableAdjusterOnSwitch;

    private RuntimeAnimatorController defaultAnimator;
    
    void OnEnable()
    {
        Script_NPCEventsManager.OnNPCRandomAnimatorSwitch += SwitchAnimator;
        Script_NPCEventsManager.OnNPCRandomAnimatorDefault += DefaultAnimator;
    }

    void OnDisable()
    {
        Script_NPCEventsManager.OnNPCRandomAnimatorSwitch -= SwitchAnimator;
        Script_NPCEventsManager.OnNPCRandomAnimatorDefault -= DefaultAnimator;
    }
    
    void Awake()
    {
        defaultAnimator = myAnimator.runtimeAnimatorController;
    }

    public void SwitchAnimator()
    {
        myAnimator.runtimeAnimatorController = animatorOverride;
        myAnimator.AnimatorSetDirection(npc.FacingDirection);

        if (isDisableAdjusterOnSwitch && animatorPositionAdjuster != null)
        {
            animatorPositionAdjuster.IsDisabled = true;
            animatorPositionAdjuster.InitialState();
        }
    }

    public void DefaultAnimator()
    {
        myAnimator.runtimeAnimatorController = defaultAnimator;
        myAnimator.AnimatorSetDirection(npc.FacingDirection);

        if (isDisableAdjusterOnSwitch && animatorPositionAdjuster != null)
        {
            animatorPositionAdjuster.IsDisabled = false;
            animatorPositionAdjuster.Adjust(npc.FacingDirection);
        }
    }
}
