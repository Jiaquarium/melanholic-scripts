using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_HideGameObjects : MonoBehaviour
{
    public void HideAfterNum(int num, GameObject[] objs)
    {
        for (int i = 0; i < objs.Length; i++)
        {
            if (i < num)    objs[i].SetActive(true);
            else            objs[i].SetActive(false);
        }
    }
}
