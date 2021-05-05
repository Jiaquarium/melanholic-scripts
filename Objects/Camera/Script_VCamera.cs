using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_VCamera : MonoBehaviour
{
    [SerializeField] private bool isFollowPlayer;
    
    public Transform Follow
    {
        get => GetComponent<CinemachineVirtualCamera>().Follow;
    }

    public CinemachineVirtualCamera CinemachineVirtualCamera
    {
        get => GetComponent<CinemachineVirtualCamera>();
    }
    
    public void FollowTarget(Transform target)
    {
        GetComponent<CinemachineVirtualCamera>().Follow = target;
    }

    public void SetPriority(int priority)
    {
        GetComponent<CinemachineVirtualCamera>().Priority = priority;
    }

    public void SetOrthoSize(float size)
    {
        GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = size;
    }
    
    void Awake()
    {
        if (isFollowPlayer)
        {
            Debug.Log($"{name} Set to follow Player");
            FollowTarget(Script_Game.Game?.GetPlayer().FocalPoint);
        }
    }
}