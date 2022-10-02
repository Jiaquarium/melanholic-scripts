using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_ImageDistorterController : MonoBehaviour
{
    [SerializeField] private Image outlineX;
    [SerializeField] private bool oscillateXOn;
    [SerializeField] private float oscillateXTime;
    [SerializeField] private float oscillateXMagnitude;

    [SerializeField] private Image outlineY;
    [SerializeField] private bool oscillateYOn;
    [SerializeField] private float oscillateYTime;
    [SerializeField] private float oscillateYMagnitude;

    [SerializeField] private List<Image> smearOutlines;
    [SerializeField] private List<Image> smearForceOutlines;
    [SerializeField] private bool smearOn;
    [SerializeField] private float smearTime;

    private Vector3 outlineXInitialLocation;
    private float oscillateXTimer;
    private float isOscillateXSwitchDir;
    private int oscillateXCount;

    private Vector3 outlineYInitialLocation;
    private float oscillateYTimer;
    private float isOscillateYSwitchDir;
    private int oscillateYCount;

    private float smearTimer;
    private int smearCount;
    private float smearForceTimer;
    private int smearForceCount;

    private Script_CanvasAdjuster canvasAdjusterX;
    private Script_CanvasAdjuster canvasAdjusterY;

    public bool SmearForceOn { get; set; }
    
    void Awake()
    {
        canvasAdjusterX = outlineX.GetComponent<Script_CanvasAdjuster>();
        canvasAdjusterY = outlineY.GetComponent<Script_CanvasAdjuster>();
    }
    
    void OnEnable()
    {
        InitialState();
    }

    void OnDisable()
    {
        ResetOutlines();
    }

    // Do calcs in LateUpdate in case canvases need to be adjusted beforehand.
    void LateUpdate()
    {
        if (oscillateXOn)
            HandleOscillateX();
        
        if (oscillateYOn)
            HandleOscillateY();

        if (smearOn)
            HandleSmear();

        if (SmearForceOn)
            HandleSmearForce();
    }

    /// <summary>
    /// Will oscillate in +X direction once, then -X direction twice to give staticy effect.
    /// </summary>
    private void HandleOscillateX()
    {
        // Uses the canvas adjuster position when available
        Vector3 xLocation = canvasAdjusterX == null
            ? outlineXInitialLocation
            : canvasAdjusterX.MyPosition;
        
        oscillateXTimer += Time.unscaledDeltaTime;
        
        if (oscillateXTimer >= oscillateXTime)
        {
            oscillateXCount++;
            oscillateXTimer = 0f;
            outlineX.rectTransform.anchoredPosition = new Vector3(
                xLocation.x + (oscillateXMagnitude * isOscillateXSwitchDir),
                xLocation.y,
                xLocation.z
            );

            // Switch direction
            if ((oscillateXCount - 1) % 2 == 0)
                isOscillateXSwitchDir = isOscillateXSwitchDir * -1;
        }
        else
        {
            float xDelta = (oscillateXTimer / oscillateXTime) * oscillateXMagnitude;
            
            outlineX.rectTransform.anchoredPosition = new Vector3(
                xLocation.x + (xDelta * isOscillateXSwitchDir),
                xLocation.y,
                xLocation.z
            );
        }
    }

    private void HandleOscillateY()
    {
        // Uses the canvas adjuster position when available
        Vector3 yLocation = canvasAdjusterY == null
            ? outlineYInitialLocation
            : canvasAdjusterY.MyPosition;
        
        oscillateYTimer += Time.unscaledDeltaTime;
        
        if (oscillateYTimer >= oscillateYTime)
        {
            oscillateYCount++;
            oscillateYTimer = 0f;
            outlineY.rectTransform.anchoredPosition = new Vector3(
                yLocation.x,
                yLocation.y + (oscillateYMagnitude * isOscillateYSwitchDir),
                yLocation.z
            );

            // Switch direction
            if ((oscillateYCount - 1) % 2 == 0)
                isOscillateYSwitchDir = isOscillateYSwitchDir * -1;
        }
        else
        {
            float yDelta = (oscillateYTimer / oscillateYTime) * oscillateYMagnitude;
            
            outlineY.rectTransform.anchoredPosition = new Vector3(
                yLocation.x,
                yLocation.y + (yDelta * isOscillateYSwitchDir),
                yLocation.z
            );
        }
    }

    private void HandleSmear()
    {
        if (smearCount >= smearOutlines.Count)
            return;
        
        smearTimer -= Time.unscaledDeltaTime;

        if (smearTimer <= 0f)
        {
            smearTimer = smearTime;
            smearOutlines[smearCount].gameObject.SetActive(true);
            smearCount++;
        }        
    }

    /// <summary>
    /// Callable to start smearing via script instead of OnEnable
    /// </summary>
    public void HandleSmearForce()
    {
        if (smearForceCount >= smearForceOutlines.Count)
            return;
        
        smearForceTimer -= Time.unscaledDeltaTime;

        if (smearForceTimer <= 0f)
        {
            smearForceTimer = smearTime;
            smearForceOutlines[smearForceCount].gameObject.SetActive(true);
            smearForceCount++;
        }        
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    
    public void StartSmearForceOn()
    {
        SmearForceOn = true;
    }

    // ------------------------------------------------------------------

    private void SetOutlines()
    {
        outlineXInitialLocation = outlineX.rectTransform.anchoredPosition;
        outlineYInitialLocation = outlineY.rectTransform.anchoredPosition;
    }
    
    private void ResetOutlines()
    {
        outlineX.rectTransform.anchoredPosition = outlineXInitialLocation;
        outlineY.rectTransform.anchoredPosition = outlineYInitialLocation;
    }
    
    private void InitialState()
    {
        isOscillateXSwitchDir = 1f;
        oscillateXCount = 0;

        if (oscillateXOn && outlineX != null)
            outlineX.gameObject.SetActive(true);

        isOscillateYSwitchDir = 1f;
        oscillateYCount = 0;

        if (oscillateYOn && outlineY != null)
            outlineY.gameObject.SetActive(true);

        smearTimer = smearTime;
        smearCount = 0;
        smearOutlines.ForEach(outline => outline.gameObject.SetActive(false));
        
        smearForceTimer = smearTime;
        smearForceCount = 0;
        smearForceOutlines.ForEach(outline => outline.gameObject.SetActive(false));
        SmearForceOn = false;

        SetOutlines();
    }
}
