using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_IdsDeadController : MonoBehaviour
{
    [SerializeField] private Canvas idsDeadCanvas;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Script_GlitchFXManager glitchFXManager;
    
    public void StartGlitchEffect()
    {
        // Change canvas to Screen Space - Camera
        idsDeadCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        idsDeadCanvas.worldCamera = mainCamera;
        idsDeadCanvas.planeDistance = Script_GraphicsManager.CamCanvasPlaneDistance;

        // Start static high screen FX
        glitchFXManager.SetMCDrowning();
        glitchFXManager.SetBlend(1f);
    }

    public void StopGlitchEffect()
    {
        idsDeadCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        glitchFXManager.SetBlend(0f);
    }
}
