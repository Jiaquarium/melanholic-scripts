using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LanguageChoice : MonoBehaviour
{
    [SerializeField] private Script_LocalizationUtils.LangEnum langEnum;
    
    [SerializeField] private Script_StartOverviewController startOverviewController;

    // OnClick
    public void UpdateLang()
    {
        startOverviewController.HandleNewLangStartup(langEnum);
        
        PlaySFX();
    }

    private void PlaySFX()
    {
        var sfxManager = Script_SFXManager.SFX;
        sfxManager.Play(sfxManager.PencilEditShort, sfxManager.PencilEditShortVol);
    }
}
