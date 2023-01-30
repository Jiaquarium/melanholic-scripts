using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This will hide all children lights. Set a reference for the desired location to compare with player's.
/// </summary>
[DisallowMultipleComponent]
public class Script_LightProximityHider : MonoBehaviour
{
    private static int FrameInterval = 4;

    [SerializeField] private float maxDistance;
    [SerializeField] private Transform lightLocation; 
    [SerializeField] private List<Light> myLights;

    private Vector2 myLocationXZ;
    private float groundDistance;
    private Script_Game game;

    void OnValidate()
    {
        PopulateLights();
    }
    
    void Awake()
    {
        PopulateLights();
        myLocationXZ = new Vector2(lightLocation.position.x, lightLocation.position.z);
    }
    
    void Start()
    {
        game = Script_Game.Game;
    }
    
    void Update()
    {
        // Throttle
        if (Time.frameCount % FrameInterval == 0)
            HandleHide();
    }

    private void HandleHide()
    {
        if (
            game == null
            || !game.GetPlayerIsSpawned()
        )
        {
            return;
        }

        Vector3 playerLocation = Script_Game.Game.GetPlayerLocation();
        
        // Calculate distance ignoring non-2D axis (gizmos are accurate this way)
        Vector2 playerLocationXZ = new Vector2(playerLocation.x, playerLocation.z);
        groundDistance = Vector2.Distance(playerLocationXZ, myLocationXZ);

        bool isOutOfRange = groundDistance > maxDistance;
        myLights.ForEach(l => l.enabled = !isOutOfRange);
    }

    private void PopulateLights()
    {
        myLights = GetComponentsInChildren<Light>(true).ToList();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var position = lightLocation.position;
        Handles.color = Color.green;
        
        Handles.DrawWireDisc(position, new Vector3(0, 1, 0), maxDistance);
    }
#endif
}
