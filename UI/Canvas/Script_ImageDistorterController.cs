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
    
    // Start is called before the first frame update
    void OnEnable()
    {
        InitialState();
    }

    // Update is called once per frame
    void Update()
    {
        if (oscillateXOn)
            HandleOscillateX();
        
        if (oscillateYOn)
            HandleOscillateY();

        if (smearOn)
            HandleSmear();
    }

    /// <summary>
    /// Will oscillate in +X direction once, then -X direction twice to give staticy effect.
    /// </summary>
    private void HandleOscillateX()
    {
        oscillateXTimer += Time.unscaledDeltaTime;
        
        if (oscillateXTimer >= oscillateXTime)
        {
            oscillateXCount++;
            oscillateXTimer = 0f;
            outlineX.rectTransform.anchoredPosition = new Vector3(
                outlineXInitialLocation.x + (oscillateXMagnitude * isOscillateXSwitchDir),
                outlineXInitialLocation.y,
                outlineXInitialLocation.z
            );

            // Switch direction
            if ((oscillateXCount - 1) % 2 == 0)
                isOscillateXSwitchDir = isOscillateXSwitchDir * -1;
        }
        else
        {
            float xDelta = (oscillateXTimer / oscillateXTime) * oscillateXMagnitude;
            
            outlineX.rectTransform.anchoredPosition = new Vector3(
                outlineXInitialLocation.x + (xDelta * isOscillateXSwitchDir),
                outlineXInitialLocation.y,
                outlineXInitialLocation.z
            );
        }
    }

    private void HandleOscillateY()
    {
        oscillateYTimer += Time.unscaledDeltaTime;
        
        if (oscillateYTimer >= oscillateYTime)
        {
            oscillateYCount++;
            oscillateYTimer = 0f;
            outlineY.rectTransform.anchoredPosition = new Vector3(
                outlineYInitialLocation.x,
                outlineYInitialLocation.y + (oscillateYMagnitude * isOscillateYSwitchDir),
                outlineYInitialLocation.z
            );

            // Switch direction
            if ((oscillateYCount - 1) % 2 == 0)
                isOscillateYSwitchDir = isOscillateYSwitchDir * -1;
        }
        else
        {
            float yDelta = (oscillateYTimer / oscillateYTime) * oscillateYMagnitude;
            
            outlineY.rectTransform.anchoredPosition = new Vector3(
                outlineYInitialLocation.x,
                outlineYInitialLocation.y + (yDelta * isOscillateYSwitchDir),
                outlineYInitialLocation.z
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

    private void InitialState()
    {
        outlineXInitialLocation = outlineX.rectTransform.anchoredPosition;
        isOscillateXSwitchDir = 1f;
        oscillateXCount = 0;

        if (oscillateXOn && outlineX != null)
            outlineX.gameObject.SetActive(true);

        outlineYInitialLocation = outlineY.rectTransform.anchoredPosition;
        isOscillateYSwitchDir = 1f;
        oscillateYCount = 0;

        if (oscillateYOn && outlineY != null)
            outlineY.gameObject.SetActive(true);

        smearTimer = smearTime;
        smearCount = 0;
        smearOutlines.ForEach(outline => outline.gameObject.SetActive(false));
    }
}
