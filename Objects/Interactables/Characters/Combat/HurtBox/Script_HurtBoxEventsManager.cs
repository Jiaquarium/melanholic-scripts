using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_HurtBoxEventsManager : MonoBehaviour
{
    public delegate void OnHurtDelegate(string hurtBoxTag, Script_HitBox hitBox);
    public static event OnHurtDelegate OnHurt;
    public static void Hurt(string hurtBoxTag, Script_HitBox hitBox)
    {
        Debug.Log($"Hurt event: hurtBoxTag {hurtBoxTag}, hitBox {hitBox.tag}");
        
        if (OnHurt != null)
            OnHurt(hurtBoxTag, hitBox);
    }

    public delegate void OnPlayerRestartTeleportDelegate(Collider col);
    public static event OnPlayerRestartTeleportDelegate OnPlayerRestartTeleport;
    public static void PlayerRestartTeleport(Collider col)
    {
        if (OnPlayerRestartTeleport != null)
            OnPlayerRestartTeleport(col);
    }

    public delegate void PlayerRestartDelegate(Collider col);
    public static event PlayerRestartDelegate OnPlayerRestart;
    public static void PlayerRestart(Collider col)
    {
        if (OnPlayerRestart != null)
            OnPlayerRestart(col);
    }
}
