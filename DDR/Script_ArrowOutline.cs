using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_ArrowOutline : MonoBehaviour
{
    private static string Flash = "flash";
    
    public Sprite defaultSprite;
    public Sprite focusSprite;
    
    [SerializeField] private Animator tier1Animator;
    [SerializeField] private Animator tier2Animator;
    [SerializeField] private Animator bpmAnimator;
    
    private IEnumerator focusCo;
    private float focusTimeLength;
    private float lightUpTimeLength;

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
        GetComponent<Image>().sprite = focusSprite;

        if (focusCo != null)
        {
            StopCoroutine(focusCo);
        }

        focusCo = WaitToUnfocus();
        StartCoroutine(focusCo);
    }

    private IEnumerator WaitToUnfocus()
    {
        yield return new WaitForSeconds(focusTimeLength);

        Unfocus();
    }

    private void Unfocus()
    {
        GetComponent<Image>().sprite = defaultSprite;
    }

    public void Setup(float _focusTimeLength)
    {
        focusTimeLength = _focusTimeLength;

        Unfocus();
    }
}
