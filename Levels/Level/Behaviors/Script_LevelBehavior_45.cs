using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_45 : Script_LevelBehavior
{
    // ==================================================================
    // State Data

    // ==================================================================

    [SerializeField] private Light directionalLight;
    
    private Script_LanternFollower lanternFollower;
    
    protected override void Update()
    {
        base.Update();

        if (lanternFollower.IsLightOn)  directionalLight.gameObject.SetActive(true);
        else                            directionalLight.gameObject.SetActive(false);
    }


    public override void Setup()
    {
        lanternFollower = game.LanternFollower;
    }
}