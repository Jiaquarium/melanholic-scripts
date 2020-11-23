using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Script_VCamera : MonoBehaviour
{
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
}
