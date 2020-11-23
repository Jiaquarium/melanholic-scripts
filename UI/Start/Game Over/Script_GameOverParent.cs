using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_GameOverParent : MonoBehaviour
{
    [SerializeField] private Script_DeathByScreen[] deathByScreens;
    
    void OnValidate()
    {
        deathByScreens = GetComponentsInChildren<Script_DeathByScreen>();
    }
    
    public void Setup()
    {
        foreach (Script_DeathByScreen deathByScreen in deathByScreens)
        {
            deathByScreen.gameObject.SetActive(false);
        }
    }
}
