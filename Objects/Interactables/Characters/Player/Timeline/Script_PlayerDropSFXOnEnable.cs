using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerDropSFXOnEnable : MonoBehaviour
{
    void OnEnable()
    {
        Script_Game.Game.GetPlayer().DropSFX();
        this.gameObject.SetActive(false);
    }
}
