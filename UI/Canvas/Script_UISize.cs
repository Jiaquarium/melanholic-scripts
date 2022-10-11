using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_UISize : MonoBehaviour
{
    [SerializeField] private Vector2 demoSize;

    [SerializeField] private List<RectTransform> childrenRects;

    private RectTransform myRect;
    
    void Awake()
    {
        myRect = GetComponent<RectTransform>();
        
        if (Const_Dev.IsDemo)
        {
            Vector2 newSize = new Vector2(
                demoSize.x > 0f ? demoSize.x : myRect.sizeDelta.x,
                demoSize.y > 0f ? demoSize.y : myRect.sizeDelta.y
            );
            myRect.sizeDelta = newSize;

            childrenRects.ForEach(rect => rect.sizeDelta = newSize);
        }
    }
}
