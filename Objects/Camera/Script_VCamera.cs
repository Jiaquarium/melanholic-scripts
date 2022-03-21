using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_VCamera : MonoBehaviour
{
    [SerializeField] private bool isFollowPlayer = true;
    
    [SerializeField] private Vector3 offset = new Vector3(10.8f, 21f, -22f);
    
    public Transform Follow
    {
        get => GetComponent<CinemachineVirtualCamera>().Follow;
        set => GetComponent<CinemachineVirtualCamera>().Follow = value;
    }

    public CinemachineVirtualCamera CinemachineVirtualCamera
    {
        get => GetComponent<CinemachineVirtualCamera>();
    }

    public Vector3 OffsetTargetPosition
    {
        get => Follow.transform.position + offset;
    }

    public Vector3 Offset
    {
        get => offset;
    }

    void Start()
    {
        if (isFollowPlayer)
        {
            Debug.Log($"{name} Set to follow Player");
            SetFollow();
            transform.position = OffsetTargetPosition;
        }
    }

    public void SetFollow()
    {
        Follow = Script_Game.Game?.GetPlayer().FocalPoint;
    }

    public void SetPriority(int priority)
    {
        GetComponent<CinemachineVirtualCamera>().Priority = priority;
    }

    public void SetOrthoSize(float size)
    {
        GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = size;
    }

    public void InvalidateConfinerCache()
    {
        GetComponent<CinemachineConfiner>()?.InvalidatePathCache();
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_VCamera))]
    public class Script_VCameraTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_VCamera t = (Script_VCamera)target;
            if (GUILayout.Button("Invalidate Confiner Cache"))
            {
                t.InvalidateConfinerCache();
            }
        }
    }
    #endif
}