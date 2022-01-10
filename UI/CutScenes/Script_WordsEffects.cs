using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_WordsEffects : MonoBehaviour
{
    [SerializeField] private Canvas EileensMindDoubt;
    [SerializeField] private Canvas FireplaceInstantRegret;
    
    public void Setup()
    {
        gameObject.SetActive(true);
        EileensMindDoubt.gameObject.SetActive(false);
        FireplaceInstantRegret.gameObject.SetActive(false);
    }
}
