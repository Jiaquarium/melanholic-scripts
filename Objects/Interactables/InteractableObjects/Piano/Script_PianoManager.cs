using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_PianoManager : MonoBehaviour
{
    public static Script_PianoManager Control;

    [SerializeField] private Script_Piano[] pianos = new Script_Piano[3];
    
    [SerializeField] Script_CanvasGroupController pianosCanvasGroup;
    
    [SerializeField] Script_Game game;

    private AudioSource audioSource;

    public string GetPianoMapName(int idx)
    {
        return pianos[idx].MapName;
    }
    
    public bool GetPianoIsRemembered(int idx)
    {
        return pianos[idx].IsRemembered;
    }

    // ------------------------------------------------------------------
    // UI / Unity Event

    public void SetPianosCanvasGroupActive(bool isActive)
    {
        if (isActive)
        {
            pianosCanvasGroup.Open();
        }
        else
        {
            pianosCanvasGroup.Close();
        }
    }

    // ------------------------------------------------------------------
    // Unity Event OnClick Handlers
    
    public void ExitPianoChoices()
    {
        SetPianosCanvasGroupActive(false);
        
        game.NextFrameChangeStateInteract();
    }
    
    // Called from Piano UI Choices
    public void PianoExit(int idx)
    {
        Script_Piano piano = pianos[idx];
        
        if (!piano.IsRemembered)
        {
            audioSource.PlayOneShot(Script_SFXManager.SFX.UIErrorSFX, Script_SFXManager.SFX.UIErrorSFXVol);
            return;
        }
        
        game.Exit(
            piano.Level,
            piano.PlayerSpawn,
            piano.FacingDirection,
            isExit: true,
            isSilent: false,
            exitType: Script_Exits.ExitType.Piano
        );

        SetPianosCanvasGroupActive(false);
    }
    // ------------------------------------------------------------------

    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        pianosCanvasGroup.Close();
    }
}
