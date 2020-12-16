using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Script_PlayerInteractionBox : Script_InteractionBox
{
    /// public List<Script_Interactables> GetInteractables() default
    /// public List<Script_Pushable> GetPushables() default
    
    public override Script_StaticNPC GetNPC()
    {
        ExposeBox();
        foreach (Collider col in colliders)
        {
            if (col.tag == Const_Tags.NPC)
                return col.transform.parent.GetComponent<Script_StaticNPC>();
        }

        return null;
    }

    public override Script_InteractableObject[] GetInteractableObjects()
    {
        ExposeBox();
        
        Script_InteractableObject[] objs = new Script_InteractableObject[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == Const_Tags.InteractableObject)
            {
                objs[i] = colliders[i].transform.parent.GetComponent<Script_InteractableObject>();
            }
        }

        return objs.Where(x => x != null).ToArray();
    }
    
    public override Script_SavePoint GetSavePoint()
    {
        ExposeBox();
        foreach (Collider col in colliders)
        {
            if (col.tag == Const_Tags.SavePoint)
                return col.transform.parent.GetComponent<Script_SavePoint>();
        }

        return null;
    }

    public override Script_ItemObject GetItem()
    {
        ExposeBox();
        foreach (Collider col in colliders)
        {
            if (col.tag == Const_Tags.ItemObject)
                return col.transform.parent.GetComponent<Script_ItemObject>();
        }

        return null;
    }

    public override Script_UsableTarget GetUsableTarget()
    {
        ExposeBox();
        foreach (Collider col in colliders)
        {
            if (col.tag == Const_Tags.UsableTarget)
                return col.transform.parent.GetComponent<Script_UsableTarget>();
        }

        return null;
    }
}
