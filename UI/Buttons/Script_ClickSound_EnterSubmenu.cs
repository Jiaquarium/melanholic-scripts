using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Gives us the option to use a different click SFX vs. other Select buttons
/// </summary>
[RequireComponent(typeof(Button))]
public class Script_ClickSound_EnterSubmenu : MonoBehaviour, ISubmitHandler
{
    public Script_InventoryAudioSettings settings;
    [SerializeField] private AudioSource source;

    void Awake()
    {
        source = settings.clickEnterSubmenuAudioSource;
    }

    public void OnSubmit(BaseEventData e)
    {
        source.PlayOneShot(settings.clickSFX, settings.clickVolume);
    }
}
