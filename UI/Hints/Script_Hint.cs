using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Hint : MonoBehaviour
{
    public FadeSpeeds fadeSpeed;
    [SerializeField] private float uptime = 2.5f; 
    private float time;
    private bool isHiding;

    void OnEnable() {
        time = uptime;    
    }

    void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0f)
        {
            time = 0f;
            Hide();
        }
    }

    public void Show()
    {
        Dev_Logger.Debug("Show hint");
        Script_HintManager.Control.FadeIn(this);
    }
    
    public void Hide()
    {
        if (isHiding)   return;
        
        isHiding = true;
        Script_HintManager.Control.FadeOut(this, () => {
            isHiding = false;
        });
    }
}
