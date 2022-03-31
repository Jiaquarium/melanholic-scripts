using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TextureScroller : MonoBehaviour
{
    [SerializeField] private string MainTex = "_BaseMap";
    [SerializeField] private float scrollSpeed = .01f;

    private Renderer graphics;
    private float timeStart;

    public float ScrollSpeed
    {
        get => scrollSpeed;
        set => scrollSpeed = value;
    }

    void Awake ()
    {
        UpdateMaterial();
    }

    void Update ()
    {
        UpdateOffset();
    }

    public void UpdateMaterial()
    {
        graphics = GetComponent<Renderer>();
        timeStart = Time.time;
    }

    private void UpdateOffset()
    {
        float x = Mathf.Repeat((Time.time - timeStart) * scrollSpeed, 1f);
        Vector2 offset = new Vector2(x, 0f);
        graphics.material.SetTextureOffset(MainTex, offset);
    }
}
