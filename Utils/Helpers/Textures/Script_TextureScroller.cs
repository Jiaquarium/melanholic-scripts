using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TextureScroller : MonoBehaviour
{
    [SerializeField] private string MainTex = "_BaseMap";
    [SerializeField] private float scrollSpeed = .01f;

    private Renderer graphics;

    void Awake ()
    {
        graphics = GetComponent<Renderer>();
    }

    void Update ()
    {
        float x = Mathf.Repeat(Time.time * scrollSpeed, 1f);
        Vector2 offset = new Vector2(x, 0f);
        graphics.material.SetTextureOffset(MainTex, offset);
    }
}
