using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_RunsManager : MonoBehaviour
{
    public static readonly int EroIntroRun = -1;
    public static readonly int LightupPaintingsPuzzleRun = -1;
    public static readonly int MelzIntroRun = -1;
    public static Script_RunsManager Control;
    
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
