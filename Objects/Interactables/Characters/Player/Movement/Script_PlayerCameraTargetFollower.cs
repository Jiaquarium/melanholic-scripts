using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerCameraTargetFollower : MonoBehaviour
{
    [SerializeField] private Script_Player player;

    [SerializeField] private float speed;

    private Vector3 startLocation;
    private Vector3 endLocation;
    private Vector3 newEndLocation;
    private float progress;
    
    public Script_Player Player
    {
        get => player;
        set => player = value;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
