using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_Arrow : MonoBehaviour
{
    public string type;
    public Vector3 startLocation;
    public Vector3 endLocation;
    public Vector3 secondEndLocation;
    [SerializeField] private float progress1;
    [SerializeField] private float progress2;
    public bool isClicked = false;
    public Sprite clickedSprite;
    
    
    private Script_DDRManager DDRManager;
    private Script_DDRConductor conductor;
    
    private float timeToReachEndLocation;
    private bool isMoving;
    private bool isPassingOutline;
    private bool isReported;
    private float tierNeg1Buffer;
    private float tier1Buffer;
    private float tier2Buffer;
    private float tier3Buffer;

    private float startSongPosition;

    void Update()
    {
        if (isMoving)
        {
            if (isPassingOutline)
            {
                SecondRise();
                return;
            }

            Rise();
        }

        if (isClicked)
        {
            GetComponent<Image>().sprite = clickedSprite; 
        }
    }
    
    void Rise()
    {
        var currentTime = conductor.SongPosition - startSongPosition;
        progress1 = currentTime / timeToReachEndLocation;

        GetComponent<RectTransform>().localPosition = Vector3.Lerp(
            startLocation,
            endLocation,
            progress1
        );

        if (Debug.isDebugBuild && Const_Dev.IsDevMode)
        {
            ChangeColorOnRise();
        }

        // continue to move arrow off screen when done lerping
        if (progress1 >= 1f)
        {
            Debug.Log($"{this} {type} arrow reached end at time {conductor.SongPosition} TOTAL TIME: {currentTime}");
            
            isPassingOutline = true;
            startLocation = GetComponent<RectTransform>().localPosition;
        }
    }

    void ChangeColorOnRise()
    {
        if (1f - progress1 <= tier1Buffer)
        {
            GetComponent<Image>().color = Color.green;
        }
        else if (1f - progress1 <= tier2Buffer)
        {
            GetComponent<Image>().color = Color.yellow;
        }
        else if (1f - progress1 <= tier3Buffer)
        {
            GetComponent<Image>().color = Color.red;
        }
    }

    // to continue to move offscreen
    void SecondRise()
    {
        var currentTime = conductor.SongPosition - startSongPosition;
        progress2 = currentTime / timeToReachEndLocation - 1f;
        
        GetComponent<RectTransform>().localPosition = Vector3.Lerp(
            startLocation,
            secondEndLocation,
            progress2
        );

        if (Debug.isDebugBuild && Const_Dev.IsDevMode)
        {
            ChangeColorOnSecondRise();
        }

        // case where arrow passes outline and is not clicked in time
        if (progress2 > tierNeg1Buffer)
        {
            if (!isClicked)
            {
                Debug.Log($"Reporting no click from arrow {this} at TOTAL TIME 2: {currentTime}");

                DDRManager.ReportArrowTier(this);
                if (type == "left")             DDRManager.nextLeftArrowIndex++;
                else if (type == "down")        DDRManager.nextDownArrowIndex++;
                else if (type == "up")          DDRManager.nextUpArrowIndex++;
                else if (type == "right")       DDRManager.nextRightArrowIndex++;
            }
        }

        // destroy arrow when done with second lerp
        if (progress2 >= 1f)
        {
            DestroyArrow();
        }
    }

    public float ReportTier()
    {
        if (isPassingOutline)
        {
            return -progress2;
        }
        else
        {
            return 1f - progress1;
        }
    }

    void ChangeColorOnSecondRise()
    {
        if (progress2 > tierNeg1Buffer)
        {
            GetComponent<Image>().color = Color.red;
        }
    }

    public void BeginRising()
    {
        isMoving = true;
    }

    public void DestroyArrow()
    {
        if (this != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
    
    public void Setup(
        Vector3 target,
        float time,
        float _tierNeg1Buffer,
        float _tier1Buffer,
        float _tier2Buffer,
        float _tier3Buffer,
        Script_DDRManager _DDRManager,
        Script_DDRConductor _conductor
    )
    {
        tierNeg1Buffer          = _tierNeg1Buffer;
        tier1Buffer             = _tier1Buffer;
        tier2Buffer             = _tier2Buffer;
        tier3Buffer             = _tier3Buffer;
        DDRManager              = _DDRManager;
        conductor               = _conductor;
        progress1               = 0;
        startLocation           = GetComponent<RectTransform>().localPosition;
        timeToReachEndLocation  = time;
        endLocation             = target;
        secondEndLocation       = endLocation + (endLocation - startLocation);

        startSongPosition       = conductor.SongPosition;
    }
}
