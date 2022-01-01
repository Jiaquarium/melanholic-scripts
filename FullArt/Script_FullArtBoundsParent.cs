using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_FullArtBoundsParent : MonoBehaviour
{
    [SerializeField] private Script_ScalingBounds bounds;
    
    void OnValidate()
    {
        SetBounds();
    }

    private void SetBounds()
    {
        Script_FullArt[] fullArts = GetComponentsInChildren<Script_FullArt>(true);
        foreach (var fullArt in fullArts)
        {
            fullArt.CustomBounds = bounds;
        }
    }
}
