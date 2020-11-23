﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class Script_TileMapFadeIn : MonoBehaviour
{
    public float fadeInTime;
    public Tilemap tm;
    public List<Vector3Int> tileLocs = new List<Vector3Int>();
    public Color tmpColor;
    public void SetTileMapTransparent()
    {
        tm = GetComponent<Tilemap>();
        tmpColor = tm.color;
        tmpColor.a = 0f;

        // first find all tile locations and set to transparent
        tileLocs.Clear();
        tileLocs = Script_Utils.AddTileLocs(tileLocs, tm,
            tileLoc => {
                tm.SetTileFlags(tileLoc, TileFlags.None);
                tm.SetColor(tileLoc, tmpColor);
            }
        );
    }

    public IEnumerator FadeInCo(Action cb)
    {
        // need to set transparent first and populate tileLocs
        SetTileMapTransparent();
        while (tmpColor.a < 1f)
        {
            tmpColor.a += Time.deltaTime / fadeInTime;
            
            if (tmpColor.a > 1f)
            {
                tmpColor.a = 1f;
            }

            foreach (Vector3Int loc in tileLocs)
            {
                tm.SetTileFlags(loc, TileFlags.None);
                tm.SetColor(loc, tmpColor);
            }

            yield return null;
        }

        foreach (Vector3Int loc in tileLocs)
        {
            tm.SetTileFlags(loc, TileFlags.None);
            tm.SetColor(loc, tmpColor);
        }
        if (cb != null)    cb();
    }
}
