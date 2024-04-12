using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TMProLocalizationActivator : MonoBehaviour
{
    [SerializeField] private GameObject EN_localizedObject;
    [SerializeField] private GameObject CN_localizedObject;
    [SerializeField] private GameObject JP_localizedObject;
    
#if UNITY_EDITOR
    // Revert any state changes back to default EN to clean up Editor View
    void OnApplicationQuit()
    {
        Script_Game.ChangeLangToEN();
        ActivateLocalizedObjects();
    }   
#endif
    
    void OnEnable()
    {
        ActivateLocalizedObjects();
    }

    private void ActivateLocalizedObjects()
    {
        Script_LocalizationUtils.SwitchActionOnLang(
            EN_action: EN_SetActive,
            CN_action: CN_SetActive,
            JP_action: JP_SetActive
        );
        
        void EN_SetActive() => SetObjectsActive(
            true,
            false,
            false
        );

        void CN_SetActive() => SetObjectsActive(
            false,
            true,
            false
        );

        void JP_SetActive() => SetObjectsActive(
            false,
            false,
            true
        );

        void SetObjectsActive(
            bool EN_isActive,
            bool CN_isActive,
            bool JP_isActive
        )
        {
            EN_localizedObject.SetActive(EN_isActive);
            CN_localizedObject.SetActive(CN_isActive);
            JP_localizedObject.SetActive(JP_isActive);
        }
    }
}
