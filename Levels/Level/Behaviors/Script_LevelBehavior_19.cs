using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Exit is disabled until the drop spot is figured out
/// </summary>
public class Script_LevelBehavior_19 : Script_LevelBehavior
{
    [SerializeField] private Script_DialogueNode[] dialogueNodes;
    [SerializeField] private Script_DialogueManager dm;
    [SerializeField] private Script_BgThemePlayer MBgThemePlayerPrefab;
    [SerializeField] private Transform Myne;
    [SerializeField] private Script_VCamera staticZoomOutVCam;
    [SerializeField] private Script_GardenLightsController gardenLightsController;
    [SerializeField] private Script_Item requiredItem;
    [SerializeField] private Script_CollectibleTriggerStay lightsTrigger;
    [SerializeField] private Script_LightsController[] lightControllers;
    [SerializeField] private float lightFadeTime;
    [SerializeField] private Transform coneLight;
    [SerializeField] private Script_FullArtParent fullArtParent;

    private bool shouldInitialize = true;
    
    protected override void OnDisable()
    {
        if (Script_VCamManager.VCamMain != null)
            Script_VCamManager.VCamMain.SwitchToMainVCam(staticZoomOutVCam);
        
        game.ForceCutBlend();
    }

    public void DropSpotActivated()
    {
        UndimLights();
    }

    /// When P reenters and stone is on drop spot
    public void DropSpotReactivated()
    {
        UndimLights();
        game.ForceCutBlend();
    }

    public void DropSpotDeactivated()
    {
        DimLights();
    }

    private void UndimLights()
    {   
        Script_VCamManager.VCamMain.SetNewVCam(staticZoomOutVCam);
        
        foreach (Script_LightsController l in lightControllers)
        {       
            l.FadeIn(lightFadeTime, null);
        }
        
        coneLight.gameObject.SetActive(false);
    }

    private void DimLights()
    {
        Script_VCamManager.VCamMain.SwitchToMainVCam(staticZoomOutVCam);
        
        foreach (Script_LightsController l in lightControllers)
        {
            l.FadeOut(lightFadeTime, null);
        }

        /// Wait for lights to fully dim until putting back on cone light
        StartCoroutine(WaitToShowVolLight());
        IEnumerator WaitToShowVolLight()
        {
            yield return new WaitForSeconds(lightFadeTime);
            coneLight.gameObject.SetActive(true);
        }
    }

    private void ForceLightsOff()
    {
        foreach (Script_LightsController l in lightControllers)
        {
            l.FadeOut(0, null);
        }
    }

    void Awake()
    {
        requiredItem = gardenLightsController.requiredItem;
        
        ForceLightsOff();
    }

    public override void Setup()
    {
        Myne.gameObject.SetActive(true);
        game.SetupInteractableFullArt(fullArtParent.transform, shouldInitialize);

        shouldInitialize = false;
    }
}
