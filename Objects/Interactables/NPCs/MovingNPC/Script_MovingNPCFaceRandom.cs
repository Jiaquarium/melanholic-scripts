using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_MovingNPC))]
public class Script_MovingNPCFaceRandom : MonoBehaviour
{
    [SerializeField] private float changeDirectionTime;
    [SerializeField] private bool isFaceDirectionOnMyMask;

    [SerializeField] private Directions faceDirectionOnMyMask;

    private Script_MovingNPC npc;
    private Script_ActiveStickerManager activeStickerManager;
    
    private float timer;
    private bool isFacingMyMaskDirection;

    void Awake()
    {
        npc = GetComponent<Script_MovingNPC>();
        activeStickerManager = Script_ActiveStickerManager.Control;
    }
    
    void Update()
    {
        if (isFaceDirectionOnMyMask)
        {
            var maskEffectsManager = Script_MaskEffectsDirectorManager.Instance;
            var forceFaceDirection = maskEffectsManager.IsForceSheepFaceDirection;
            
            if (forceFaceDirection)
            {
                npc.FaceDirection(faceDirectionOnMyMask);
                isFacingMyMaskDirection = true;
                return;
            }

            // Immediately face another direction if coming out of Facing My Mask state.
            if (!forceFaceDirection && isFacingMyMaskDirection)
            {
                timer = 0f;
                isFacingMyMaskDirection = false;
            }
        }
        
        timer = Mathf.Max(0, timer - Time.deltaTime);
        
        if (timer <= 0f)
        {
            FaceRandomDirection();
            timer = changeDirectionTime;
        }
    }

    private void FaceRandomDirection()
    {
        int i = Random.Range(1, 5);

        Directions direction = (Directions)i;

        npc.FaceDirection(direction);
    }
}
