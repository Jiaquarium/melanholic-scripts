﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_Camera : MonoBehaviour
{
    public Transform target;
    public Script_Game game;
    public float defaultOrthoSize;
    public Vector3 offset;
    public float defaultTrackingSpeed;
    public float moveToTargetSpeed;
    public float moveToTargetInstantlySpeed;
    public float timerMax;
    public float progress;
    public Vector3 endPosition;
    public Vector3 startPosition;
    public float cameraTrackedPlayerDistance;
    public Vector3 rotationAdjToFaceCamera;


    private Vector3 OffsetDefault;
    public float speed;
    private float timer;
    private bool isTrackingTarget = true;
    private bool shouldMoveToTarget = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        OffsetDefault = offset;
        speed = defaultTrackingSpeed;
        progress = 0f;
        timer = timerMax;
        startPosition = transform.position;
        endPosition = transform.position;     
    }

    // Update is called once per frame
    void Update()
    {
        if(
            target == null
            || !isTrackingTarget
        ) return;

        Timer();

        if (shouldMoveToTarget)
        {
            CheckMovedToTarget();
        }
        
        Move();
    }

    void Move()
    {
        if (progress >= 1f) return;
        progress += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(startPosition, endPosition, progress);
    }

    void Timer()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0f)
        {
            timer = timerMax;
            UpdateEndPosition();
        }
    }

    void UpdateEndPosition()
    {
        endPosition = target.position + offset;
        startPosition = transform.position;
        progress = 0f;
    }

    public void MatchPositionToTarget()
    {
        transform.position = target.position + offset;
        UpdateEndPosition();
    }

    public void ZoomSmooth(
        float targetOrthoSize,
        float zoomSmoothTime,
        Vector3 newPosition,
        Action cb
    )
    {
        StartCoroutine(UpdateZoomSmooth(
                targetOrthoSize,
                zoomSmoothTime,
                newPosition,
                cb
            )
        );
    }
    
    IEnumerator UpdateZoomSmooth(
        float targetOrthoSize,
        float zoomSmoothTime,
        Vector3 newPosition,
        Action cb
    )
    {
        float tmpSize = GetComponent<Camera>().orthographicSize;
        float sizeDiff = targetOrthoSize - tmpSize;
        float percentElapsed;

        Transform newTarget = new GameObject().transform;
        newTarget.position = newPosition  + offset;
        newTarget.SetParent(game.tmpTargetsContainer, false);
        target = newTarget;
        endPosition = target.position;

        // TODO SMOOTH ZOOM OUT
        while (GetComponent<Camera>().orthographicSize > targetOrthoSize)
        {
            percentElapsed = Time.deltaTime / zoomSmoothTime;
            tmpSize += percentElapsed * sizeDiff;
            if (GetComponent<Camera>().orthographicSize <= targetOrthoSize)
            {
                tmpSize = targetOrthoSize;
            }
            GetComponent<Camera>().orthographicSize = tmpSize;

            yield return null;
        }

        GetComponent<Camera>().orthographicSize = tmpSize;
        if (cb != null)    cb();
    }

    public void MoveToTargetSmooth(
        float moveTime,
        Vector3 newPosition,
        Action cb
    )
    {
        StartCoroutine(UpdateMoveToTargetSmooth(
                moveTime,
                newPosition,
                cb
            )
        );
    }
    
    IEnumerator UpdateMoveToTargetSmooth(
        float moveTime,
        Vector3 newPosition,
        Action cb
    )
    {
        float progress = 0;
        float percentElapsed = 0;
        Transform newTarget = new GameObject().transform;
        newTarget.SetParent(game.tmpTargetsContainer, false);
        newTarget.position = newPosition;
        target = newTarget;
        endPosition = target.position;

        // TODO SMOOTH ZOOM OUT
        while (progress < 1f)
        {
            percentElapsed = Time.deltaTime / moveTime;
            progress += percentElapsed;
            if (progress >= 1f)    progress = 1f;
            startPosition = transform.position;
            transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                progress
            );

            yield return null;
        }

        if (cb != null)    cb();
    }

    public void MoveToTarget()
    {
        shouldMoveToTarget = true;
    }

    void CheckMovedToTarget()
    {
        FastTrackSpeed();
        
        float cameraDistanceFromEndingPos = Vector3.Distance(transform.position, endPosition);

        if (cameraDistanceFromEndingPos <= cameraTrackedPlayerDistance)
        {
            speed = defaultTrackingSpeed;
            shouldMoveToTarget = false;
        }
    }

    public void SetOffsetToDefault()
    {
        offset = OffsetDefault;
    }

    public void SetOffset(Vector3 _offset)
    {
        offset = _offset;
    }

    /// <summary>
    /// set to false if don't want Camera to be auto-updating to target
    /// </summary>
    public void SetIsTrackingTarget(bool _isTrackingTarget)
    {
        isTrackingTarget = _isTrackingTarget;
    }

    public void SetTarget<T>(T gameObject) where T : Transform
    {
        target = gameObject;
    }

    public void FastTrackSpeed()
    {
        speed = moveToTargetSpeed;
    }

    public void InstantTrackSpeed()
    {
        speed = moveToTargetInstantlySpeed;
    }

    public void DefaultSpeed()
    {
        speed = defaultTrackingSpeed;
    }

    public Vector3 GetRotationAdjustment()
    {
        return rotationAdjToFaceCamera;
    }

    public void SetOrthographicSizeDefault()
    {
        GetComponent<Camera>().orthographicSize = defaultOrthoSize;
    }

    public void SetOrthographicSize(float size)
    {
        GetComponent<Camera>().orthographicSize = size;
    }

    public void Setup(Vector3 loc)
    {
        transform.position = loc;
        game = GetComponent<Script_Game>();
    }
}
