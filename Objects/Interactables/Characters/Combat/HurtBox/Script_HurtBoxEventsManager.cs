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
}
