using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class Script_TilemapHidden : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Debug.isDebugBuild && Const_Dev.IsDevMode)  return;
        
        Tilemap tilemap = GetComponent<Tilemap>();
        Color newColor = tilemap.color;
        newColor.a = 0f; 
        tilemap.color = newColor;

        TilemapRenderer renderer = GetComponent<TilemapRenderer>();
        if (renderer != null)
            renderer.enabled = false;
    }
}
