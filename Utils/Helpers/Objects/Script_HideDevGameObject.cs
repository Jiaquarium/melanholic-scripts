using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_HideDevGameObject : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);    
    }
}
