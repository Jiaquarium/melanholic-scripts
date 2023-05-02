using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    [Space]
    [Header("Randomizer")]
    [Tooltip("Turn on to randomize oscillating X and Y directions")]
    [SerializeField] private bool isRandomOscillateOn;
    [Tooltip("Example: 3 is one third change, higher value, less probability")]
    [SerializeField] private int randomProbabilityDivisor;
    [Space]
    [Header("Interval")]
    [SerializeField] private bool isInterval;
    [SerializeField] private float intervalDisabledTime;
    [SerializeField] private float intervalActiveTime;
    [SerializeField] private List<Script_TMProRandomizer> TMProRandomizers;
    [SerializeField] private UnityEvent onActiveIntervalStart;
    [SerializeField] private UnityEvent onActiveIntervalEnd;
    private float intervalTimer;
    private bool isActiveInterval;

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
    public float OscillateXTime { get => oscillateXTime; }
    public float OscillateXMagnitude { get => oscillateXMagnitude; }
    public float OscillateYTime { get => oscillateYTime; }
    public float OscillateYMagnitude { get => oscillateYMagnitude; }
    
    void Awake()
    {
        if (outlineX != null)
            canvasAdjusterX = outlineX.GetComponent<Script_CanvasAdjuster>();
        
        if (outlineY != null)
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
        if (isInterval && HandleIntervalDisable())
            return;
        
        if (isRandomOscillateOn)
            HandleOscillateSwitch();
        
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
    /// The oscillation works by moving incrementally up to the X mag. Then back to almost center
    /// (center + 1 frame of movement)
    /// </summary>
    private void HandleOscillateX()
    {
        if (outlineX == null)
            return;
        
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
        if (outlineY == null)
            return;
        
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

    public void SwitchSettings(Vector3 xProps, Vector3 yProps)
    {
        oscillateXTime = xProps.x;
        oscillateXMagnitude = xProps.y;
        oscillateYTime = yProps.x;
        oscillateYMagnitude = yProps.y;
    }

    /// <summary>
    /// When the Image has moved to one extreme (e.g. all the way to the right) it will
    /// do the probability test to switch directions.
    /// </summary>
    private void HandleOscillateSwitch()
    {
        if (oscillateXOn && oscillateXTimer == 0)
        {
            if (UnityEngine.Random.Range(0, randomProbabilityDivisor) == 0)
            {
                oscillateXOn = false;
                oscillateYOn = true;
            }
        }
        else if (oscillateYOn && oscillateYTimer == 0)
        {
            if (UnityEngine.Random.Range(0, randomProbabilityDivisor) == 0)
            {
                oscillateXOn = true;
                oscillateYOn = false;
            }
        }
    }

    /// <summary>
    /// Returns true if currently this distorter should be disabled.
    /// </summary>
    private bool HandleIntervalDisable()
    {
        intervalTimer -= Time.unscaledDeltaTime;
        
        if (intervalTimer <= 0f)
        {
            intervalTimer = 0f;
            isActiveInterval = !isActiveInterval;

            if (isActiveInterval)
                onActiveIntervalStart.SafeInvoke();
            else
                onActiveIntervalEnd.SafeInvoke();

            if (isActiveInterval)
            {
                // Handle setting outlines active for an active interval
                OutlinesInitialState();
                intervalTimer = intervalActiveTime;

                if (TMProRandomizers.Count > 0)
                    TMProRandomizers.ForEach(randomizer => randomizer.enabled = true);
            }
            // if last wasn't disabled, then isIntervalDisabled set to true, disable outlines,
            else
            {
                if (outlineX != null)
                    outlineX.gameObject.SetActive(false);
                
                if (outlineY != null)
                    outlineY.gameObject.SetActive(false);
                
                intervalTimer = intervalDisabledTime;

                if (TMProRandomizers.Count > 0)
                    TMProRandomizers.ForEach(randomizer => randomizer.enabled = false);
            }
        }
        
        return !isActiveInterval;
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
        if (outlineX != null)
            outlineXInitialLocation = outlineX.rectTransform.anchoredPosition;
        
        if (outlineY != null)
            outlineYInitialLocation = outlineY.rectTransform.anchoredPosition;
    }
    
    private void ResetOutlines()
    {
        if (outlineX != null)
            outlineX.rectTransform.anchoredPosition = outlineXInitialLocation;
        
        if (outlineY != null)
            outlineY.rectTransform.anchoredPosition = outlineYInitialLocation;
    }
    
    private void OutlinesInitialState()
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
    
    private void InitialState()
    {
        OutlinesInitialState();

        intervalTimer = 0f;
        isActiveInterval = false;
    }
}
