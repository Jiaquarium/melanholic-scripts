using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_MovingNPC))]
public class Script_MovingNPCFaceRandom : MonoBehaviour
{
    [SerializeField] private float changeDirectionTime;
    [SerializeField] private bool isFaceDirectionOnMyMask;

    private Script_MovingNPC npc;
    private Script_ActiveStickerManager activeStickerManager;
    
    private float timer;
    private bool isForceFaceDirection;

    void OnEnable()
    {
        Script_StickerEffectEventsManager.OnMyMaskForceFaceDir += FaceForcedDirection;
        Script_StickerEffectEventsManager.OnMyMaskStopFaceDir += ResumeFaceRandomDirection;
    }

    void OnDisable()
    {
        Script_StickerEffectEventsManager.OnMyMaskForceFaceDir -= FaceForcedDirection;
        Script_StickerEffectEventsManager.OnMyMaskStopFaceDir -= ResumeFaceRandomDirection;
    }
    
    void Awake()
    {
        npc = GetComponent<Script_MovingNPC>();
        activeStickerManager = Script_ActiveStickerManager.Control;
    }
    
    void Update()
    {
        if (isFaceDirectionOnMyMask && isForceFaceDirection)
            return;
        
        timer = Mathf.Max(0, timer - Time.deltaTime);
        
        if (timer <= 0f)
        {
            FaceRandomDirection();
            timer = changeDirectionTime;
        }
    }

    private void FaceForcedDirection(Directions dir)
    {
        npc.FaceDirection(dir);
        timer = 0f;
        isForceFaceDirection = true;
    }

    private void ResumeFaceRandomDirection()
    {
        FaceRandomDirection();
        timer = changeDirectionTime;
        isForceFaceDirection = false;
    }

    private void FaceRandomDirection()
    {
        int i = Random.Range(1, 5);

        Directions direction = (Directions)i;

        npc.FaceDirection(direction);
    }
}
