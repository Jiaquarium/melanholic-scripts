using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To be attached to a Graphics Transform you need to adjust based on
/// which direction animator state is (sometimes need to adjust Sprites
/// that take up more than 1 space)
/// </summary>
[RequireComponent(typeof(Transform))]
public class Script_AnimatorPositionAdjuster : MonoBehaviour
{
    [SerializeField] private bool isUpAdjustment;
    [SerializeField] private bool isDownAdjustment;
    [SerializeField] private bool isLeftAdjustment;
    [SerializeField] private bool isRightAdjustment;
    
    [SerializeField] private Vector3 upAdjustedPosition;
    [SerializeField] private Vector3 downAdjustedPosition;
    [SerializeField] private Vector3 leftAdjustedPosition;
    [SerializeField] private Vector3 rightAdjustedPosition;

    private Vector3 originalLocalPosition;
    private Transform myTransform;

    public bool IsDisabled { get; set; }

    void Awake()
    {
        myTransform = GetComponent<Transform>();
        originalLocalPosition = myTransform.localPosition;
    }

    public void Adjust(Directions dir)
    {
        if (myTransform == null || IsDisabled)
            return;
        
        myTransform.localPosition = originalLocalPosition;
        
        switch (dir)
        {
            case (Directions.Up):
                if (isUpAdjustment)
                    myTransform.localPosition = upAdjustedPosition;
                break;
            case (Directions.Down):
                if (isDownAdjustment)
                    myTransform.localPosition = downAdjustedPosition;
                break;
            case (Directions.Left):
                if (isLeftAdjustment)
                    myTransform.localPosition = leftAdjustedPosition;
                break;
            case (Directions.Right):
                if (isRightAdjustment)
                    myTransform.localPosition = rightAdjustedPosition;
                break;
            default:
                break;
        }
    }

    public void InitialState()
    {
        myTransform.localPosition = originalLocalPosition;
    }
}
