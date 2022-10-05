using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SpritesChildrenController : MonoBehaviour
{
    [SerializeField] private float alpha;
    [SerializeField] SpriteRenderer[] childrenSprites;
    
    public float SetAlpha
    {
        set
        {
            alpha = value;
            UpdateAlpha();
        }
    }
    
    void OnValidate()
    {
        childrenSprites = transform.GetComponentsInChildren<SpriteRenderer>(true);
        UpdateAlpha();
    }

    void Update()
    {
        UpdateAlpha();
    }

    private void UpdateAlpha()
    {
        foreach (SpriteRenderer sr in childrenSprites)
        {
            Color newColor = sr.color;
            newColor.a = alpha;

            sr.color = newColor;
        }
    }
}
