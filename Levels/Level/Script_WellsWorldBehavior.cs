using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Script_WellsWorldBehavior : Script_WorldTileBehavior
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    public CinemachineVirtualCamera VirtualCamera
    {
        get => virtualCamera;
    }
}
