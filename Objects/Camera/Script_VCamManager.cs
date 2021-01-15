using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_VCamManager : MonoBehaviour
{
    // Singleton
    public static Script_VCamManager VCamMain;
    public static readonly float defaultBlendTime = 2f;


    public static Script_VCamera ActiveVCamera
    {
        get {
            CinemachineVirtualCamera CVCam = Script_Game.Game.GetComponent<CinemachineBrain>()
                .ActiveVirtualCamera as CinemachineVirtualCamera;
            return CVCam.GetComponent<Script_VCamera>();
        }
    }

    public static void SetActiveCameraOrthoSize(float size)
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
    /// switching to a vCam but then returning back to main
    /// </summary>
    public void SetNewVCam(Script_VCamera VCam)
    {
        VCam.SetPriority(1);
        GetComponent<Script_VCamera>().SetPriority(0);
    }

    /// <summary>
    /// switching back to main VCam
    /// </summary>
    public void SwitchToMainVCam(Script_VCamera otherVCam)
    {
        GetComponent<Script_VCamera>()?.SetPriority(1);
        otherVCam.SetPriority(0);
    }

    /// <summary>
    /// switching to from non-main VCam to another non-main VCam
    /// </summary>
    public void SwitchBetweenVCams(Script_VCamera vCam1, Script_VCamera vCam2)
    {
        vCam2.SetPriority(1);
        vCam1.SetPriority(0);
    }

    public void GetAllVCamPrioritiesNotEqual(int i)
    {
        List<CinemachineVirtualCamera> vCams = new List<CinemachineVirtualCamera>();

        foreach (CinemachineVirtualCamera vCam in Resources.FindObjectsOfTypeAll<CinemachineVirtualCamera>())
        {
            if (vCam.Priority != i)
            {
                Debug.Log($"{vCam.name} != {i}; priority: {vCam.Priority} ");
            }
        }
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