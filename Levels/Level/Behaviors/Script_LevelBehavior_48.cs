using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_48 : Script_LevelBehavior
{
    // ==================================================================
    // State Data
    [SerializeField] private bool isDone;
    // ==================================================================

    [SerializeField] private Script_Snow snowEffectAlways;
    [SerializeField] private Script_InteractablePaintingEntrance wellsWorldPainting;
    [SerializeField] private Script_InteractablePaintingEntrance celestialGardensWorldPainting;
    [SerializeField] private Script_InteractablePaintingEntrance xxxWorldPainting;

    public bool IsDone
    {
        get => isDone;
        set => isDone = value;
    }

    void Awake()
    {
        snowEffectAlways.gameObject.SetActive(true);
        
        IsMelancholyPianoDisabled = true;
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void SetWellsWorldPaintingStateActive()
    {
        wellsWorldPainting.SetStateActive();
    }

    public void SetCelestialGardensWorldPaintingStateActive()
    {
        celestialGardensWorldPainting.SetStateActive();
    }

    public void SetXXXWorldPaintingStateActive()
    {
        xxxWorldPainting.SetStateActive();
    }
    // ------------------------------------------------------------------
}