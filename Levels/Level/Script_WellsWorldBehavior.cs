using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Script_WellsWorldBehavior : Script_WorldTileBehavior
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Script_Snow smallSnow;
    [SerializeField] private Script_Snow heavySnow;
    [SerializeField] private Script_DoorExitFireplace fireplaceExit;
    [SerializeField] private Script_InteractableObjectText fireText;
    [SerializeField] private Script_CollectibleObject lastWellMap;
    [SerializeField] private Script_CollectibleObject speedSeal;

    public CinemachineVirtualCamera VirtualCamera { get => virtualCamera; }

    public Script_Snow SmallSnow { get => smallSnow; }

    public Script_Snow HeavySnow { get => heavySnow; }

    public Script_DoorExitFireplace FireplaceExit { get => fireplaceExit; }

    public Script_InteractableObjectText FireText { get => fireText; }

    public Script_CollectibleObject LastWellMap { get => lastWellMap; }
    public Script_CollectibleObject SpeedSeal { get => speedSeal; }
}
