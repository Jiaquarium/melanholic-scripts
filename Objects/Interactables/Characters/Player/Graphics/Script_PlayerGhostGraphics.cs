using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerGhostGraphics : MonoBehaviour
{
    [SerializeField] private Renderer graphics;

    public void SetHidden(bool isHidden)
    {
        graphics.enabled = !isHidden;
    }
}
