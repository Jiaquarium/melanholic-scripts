using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TierComment : MonoBehaviour
{
    private static string Emphasize = "emphasize";
    
    [SerializeField] private Animator TMProAnimator;
    
    private IEnumerator co;    
    
    private float activateTimeLength;

    public void Activate()
    {
        this.gameObject.SetActive(true);
        TMProAnimator.SetTrigger(Emphasize);
        
        if (co != null)
        {
            StopCoroutine(co);
        }

        co = WaitToDeactivate();
        StartCoroutine(co);
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator WaitToDeactivate()
    {
        yield return new WaitForSeconds(activateTimeLength);

        Deactivate();
    }

    public void Setup(float t)
    {
        activateTimeLength = t;

        Deactivate();
    }
}
