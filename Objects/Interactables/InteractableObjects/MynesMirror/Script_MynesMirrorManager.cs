using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for Mynes Mirror Game Objects
/// States will be handled by Scarlet Cipher 
/// </summary>
public class Script_MynesMirrorManager : MonoBehaviour
{
    public static Script_MynesMirrorManager Control;
    
    /// <summary>
    /// Call this from Mynes Mirror End of Timeline so Mynes Mirrors can react to it
    /// </summary>
    public void OnEndTimeline()
    {
        Script_MynesMirrorEventsManager.EndTimeline();
    }

    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }   
    }
}
