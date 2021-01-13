using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_DisablerController : MonoBehaviour
{
    [SerializeField] private Script_CollisionBox BoxL;
    [SerializeField] private Script_CollisionBox BoxR;
    [SerializeField] private Script_CollisionBox BoxU;
    [SerializeField] private Script_CollisionBox BoxD;

    public bool GetPlayerInBox(Directions dir)
    {
        Script_CollisionBox box;

        switch (dir)
        {
            case (Directions.Left):
                box = BoxL;
                break;
            case (Directions.Right):
                box = BoxR;
                break;
            case (Directions.Up):
                box = BoxU;
                break;
            default:
                box = BoxD;
                break;
        }

        box.ExposeBox();

        foreach (Collider col in box.Colliders)
        {
            if (col.tag == Const_Tags.Player)
            {
                return true;
            }
        }

        return false;
    }
}
