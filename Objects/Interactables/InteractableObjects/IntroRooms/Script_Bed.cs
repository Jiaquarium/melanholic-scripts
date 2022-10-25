using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Bed : Script_Interactable
{
    [SerializeField] private GameObject emptyBed;
    [SerializeField] private GameObject occupiedBed;
    
    public void SwitchToBedIsOccupied(bool isOccupied)
    {
        occupiedBed.SetActive(isOccupied);
        emptyBed.SetActive(!isOccupied);
    }
}
