using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ScarletCipherEventsManager : MonoBehaviour
{
    public delegate void ScarletCipherPiecePickUpDelegate(int Id);
    public static event ScarletCipherPiecePickUpDelegate OnScarletCipherPiecePickUp;
    public static void ScarletCipherPiecePickUp(int Id)
    {
        if (OnScarletCipherPiecePickUp != null)   OnScarletCipherPiecePickUp(Id);
    }
}
