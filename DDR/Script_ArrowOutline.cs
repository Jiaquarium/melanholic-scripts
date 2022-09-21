using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_ArrowOutline : MonoBehaviour
{
    private static int Flash = Animator.StringToHash("flash");
    
    public Sprite defaultSprite;
    
    [SerializeField] private Animator tier1Animator;
    [SerializeField] private Animator tier2Animator;
    [SerializeField] private Animator bpmAnimator;
    [SerializeField] private Animator focusAnimator;
    
    public void FlashTier1()
    {
        tier1Animator.SetTrigger(Flash);
    }

    public void FlashTier2()
    {
        tier2Animator.SetTrigger(Flash);
    }

    public void FlashBpm()
    {
        bpmAnimator.SetTrigger(Flash);
    }
    
    public void Focus()
    {
        focusAnimator.SetTrigger(Flash);
    }

    public void Setup()
    {
    }
}
