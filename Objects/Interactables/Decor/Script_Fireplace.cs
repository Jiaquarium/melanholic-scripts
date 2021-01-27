using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Fireplace : Script_Interactable
{
    [SerializeField] private Script_Flame[] flames;
    [SerializeField] private bool isDisabled;
    private int fireIdx;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeFire();
    }

    public void InitializeFire()
    {
        if (isDisabled)     return;
        
        fireIdx = 0;
        for (int i = 1; i < flames.Length; i++)
            flames[i].gameObject.SetActive(false);

        flames[fireIdx].gameObject.SetActive(true);
    }

    public void GrowFire()
    {
        if (isDisabled)     return;
        
        // if end of array then stay there
        if (fireIdx == flames.Length - 1)  return;

        fireIdx++;
        flames[fireIdx].gameObject.SetActive(true);
    }

    private void Extinguish()
    {
        fireIdx = 0;
        foreach (var f in flames) f.gameObject.SetActive(false);
    }

    public void Disable()
    {
        Extinguish();
        isDisabled = true;
    }

    public void Enable()
    {
        isDisabled = false;
        InitializeFire();
    }
}
