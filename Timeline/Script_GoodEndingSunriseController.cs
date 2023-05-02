using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_GoodEndingSunriseController : MonoBehaviour
{
    [SerializeField] private Script_GoodEndingController goodEndingController;
    
    // Sunrise GoodEnding Timeline
    // Start static to prep for screen blackout, indicating something is awry
    public void StartStatic()
    {
        goodEndingController.StartStaticFX();
    }

    // Sunrise GoodEnding Timeline
    public void StopStatic()
    {
        goodEndingController.StopStaticFX();
    }
}
