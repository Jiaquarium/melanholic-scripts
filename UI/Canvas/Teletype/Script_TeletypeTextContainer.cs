using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TeletypeTextContainer : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
