using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_PushableCheckCollisions))]
[RequireComponent(typeof(Script_InteractionBoxController))]
public class Script_Pushable : Script_InteractableObject
{
    public int pushableId;
    public float speed;
    [SerializeField] private float progress;
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector3 spawnLocation;
    [SerializeField] private Vector3 startLocation;
    [SerializeField] private Vector3 endLocation;
    [SerializeField] private bool _isDisabled;
    private bool isHiddenAfterMove;
    

    void Awake()
    {
        spawnLocation = transform.position;
    }

    void OnValidate() {
        spawnLocation = transform.position;
    }
    
    /// Using fixedUpdate to allow to work at lower framerates
    void FixedUpdate()
    {
        if (isMoving)   ActuallyMove();
    }
    
    public void Push(Directions dir)
    {
        if (isMoving)       return;
        if (_isDisabled)    return;
        
        Vector3 desiredDir = Script_Utils.GetDirectionToVectorDict()[dir];
        // check for collisions
        // if no collisions then able to push, return true
        GetComponent<Script_InteractionBoxController>().HandleActiveInteractionBox(dir);
        bool isCollision = GetComponent<Script_CheckCollisions>().CheckCollisions(
            transform.position, dir
        );
        if (isCollision)
        {
            return;
        }

        startLocation = transform.position;
        endLocation = transform.position + desiredDir;
        Move();
    }

    public bool IsDisabled
    {
        get { return _isDisabled; }
        set { _isDisabled = value; }
    }

    public void HideAfterMove()
    {
        isHiddenAfterMove = true;
    }

    public void SetActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }
    
    void Respawn()
    {
        transform.position = spawnLocation;
        SetActive(true);

        Debug.Log($"Pushable {this.name} respawned at {spawnLocation}");
    }

    void Move()
    {
        progress = 0f;
        isMoving = true;
    }

    void ActuallyMove()
    {
        progress += speed * Time.fixedDeltaTime;
        transform.position = Vector3.Lerp(startLocation, endLocation, progress);

        if (progress >= 1f)
        {
            progress = 1f;
            isMoving = false;
            if (isHiddenAfterMove)
            {
                SetActive(false);
                isHiddenAfterMove = false;
            }
        }
    }

    public virtual void InitialState()
    {
        Respawn();
        isHiddenAfterMove = false;
    }

    public void Setup()
    {
        InitialState();
    }
}
