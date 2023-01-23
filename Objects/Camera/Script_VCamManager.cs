using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// The entry point to handle VCams through code.
/// 
/// Transposer Body
/// Enum:Lock To Target With World Up
/// Vector3(10.8, 21, -22)
/// </summary>

[RequireComponent(typeof(Script_CameraShake))]
[RequireComponent(typeof(Script_VCamera))]
public class Script_VCamManager : MonoBehaviour
{
    // Singleton
    public static Script_VCamManager VCamMain;
    public static readonly float defaultBlendTime = 2f;

    [SerializeField] private CinemachineBrain brain;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Script_GraphicsManager graphics;
    [SerializeField] private Script_VCamera vCamera;

    [SerializeField] private CinemachineConfiner cinemachineConfiner;

    [SerializeField] private CinemachineBrain.BrainUpdateMethod defaultBlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.FixedUpdate;

    public CinemachineBrain Brain
    {
        get => brain;
    }
    
    public Script_VCamera ActiveVCamera
    {
        get {
            CinemachineVirtualCamera CVCam = Brain.ActiveVirtualCamera as CinemachineVirtualCamera;
            return CVCam?.GetComponent<Script_VCamera>();
        }
    }

    public Script_VCamera VCamera
    {
        get => vCamera;
    }

    public Collider BoundingVolume
    {
        get => GetComponent<CinemachineConfiner>().m_BoundingVolume;
        set => GetComponent<CinemachineConfiner>().m_BoundingVolume = value;
    }

    public bool ConfineScreenEdges
    {
        get => GetComponent<CinemachineConfiner>().m_ConfineScreenEdges;
        set => GetComponent<CinemachineConfiner>().m_ConfineScreenEdges = value;
    }

    public Script_CameraShake CameraShake => GetComponent<Script_CameraShake>();

    void LateUpdate()
    {
        HandleConfineCamera();
    }
    
    public void SetActiveCameraOrthoSize(float size)
    {
        Script_VCamera VCam = ActiveVCamera;
        VCam.SetOrthoSize(size);
        Debug.LogWarning($"Ensure you Default size value of {VCam} onValidate");
    }

    public static void SetCameraOrthoSize(Script_VCamera VCam, float size)
    {
        VCam.SetOrthoSize(size);
        Debug.LogWarning($"Ensure you Default size value of {VCam} onValidate");
    }
    
    /// <summary>
    /// Switching to a vCam but then returning back to main
    /// </summary>
    public void SetNewVCam(Script_VCamera VCam)
    {
        VCam.SetPriority(1);
        ActiveVCamera.SetPriority(0);
    }

    /// <summary>
    /// Switching back to main VCam. The level VCam will serve as the Main VCam if one is defined.
    /// </summary>
    public void SwitchToMainVCam(Script_VCamera otherVCam)
    {
        // Always treat the Level VCam as the Main when it is defined.
        Script_VCamera mainVCam;
        if (Script_Game.Game.levelBehavior.LevelVCam != null)
        {
            mainVCam = Script_Game.Game.levelBehavior.LevelVCam;
        }
        else
        {
            mainVCam = VCamera;
        }

        mainVCam?.SetPriority(1);
        
        if (mainVCam == otherVCam)
            return;
        
        otherVCam.SetPriority(0);
    }

    public void DisableLevelVCam(Script_VCamera levelVCam)
    {
        // Avoid erroring OnDisable.
        if (this == null || this?.VCamera == null)
            return;
        
        VCamera.SetPriority(1);
        levelVCam.SetPriority(0);
    }

    /// <summary>
    /// switching to from non-main VCam to another non-main VCam
    /// </summary>
    public void SwitchBetweenVCams(Script_VCamera vCam1, Script_VCamera vCam2)
    {
        vCam2.SetPriority(1);
        vCam1.SetPriority(0);
    }

    public void Shake(float duration, float amp, float freq, Action cb = null)
    {
        GetComponent<Script_CameraShake>().Shake(duration, amp, freq, cb);
    }

    public void StopShake()
    {
        GetComponent<Script_CameraShake>().InitialState();
    }

    public void SetCinemachineBlendUpdateMethod(CinemachineBrain.BrainUpdateMethod updateMethod)
    {
        brain.m_BlendUpdateMethod = updateMethod;
    }
    
    public void SetDefaultCinemachineBlendUpdateMethod()
    {
        brain.m_BlendUpdateMethod = defaultBlendUpdateMethod;
    }

    public void GetAllVCamPrioritiesNotEqual(int i)
    {
        List<CinemachineVirtualCamera> vCams = new List<CinemachineVirtualCamera>();

        foreach (CinemachineVirtualCamera vCam in Resources.FindObjectsOfTypeAll<CinemachineVirtualCamera>())
        {
            if (vCam.Priority != i)
            {
                Dev_Logger.Debug($"{vCam.name} != {i}; priority: {vCam.Priority} ");
            }
        }
    }

    /// <summary>
    /// Should only confine camera above a threshold, currently ~5.6x ortho size because trying
    /// to handle all ortho sizes doesn't look great, and ranges below ~6x are rarer / not our target.
    /// </summary>
    private void HandleConfineCamera()
    {
        // Maintain confinement state if we're using another VCam.
        if (
            ActiveVCamera != null && ActiveVCamera == VCamera
            && !Brain.IsBlending
        )
            cinemachineConfiner.enabled = mainCamera.orthographicSize >= graphics.MinOrthoSizeCameraConfinement;
    }

    public void Setup()
    {
        if (VCamMain == null)
        {
            VCamMain = this;
        }
        else if (VCamMain != this)
        {
            Destroy(this.gameObject);
            Debug.LogError("Script_VCamManager.VCamMain: You are trying to create multiple instances of me.");
        }
        
        // reset noise when starting game
        CinemachineBasicMultiChannelPerlin noise = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;

        defaultBlendUpdateMethod = brain.m_BlendUpdateMethod;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_VCamManager))]
public class Script_VCamManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_VCamManager t = (Script_VCamManager)target;
        if (GUILayout.Button("GetAllVCamPrioritiesNotEqual(0)"))
        {
            t.GetAllVCamPrioritiesNotEqual(0);
        }
    }
}
#endif