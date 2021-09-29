using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PixelTargetFollower : MonoBehaviour
{
    [SerializeField] private bool isFollowPixel = true;

    public void Move(Vector3 position)
    {
        transform.position = isFollowPixel ? RoundToPixel(position) : position;
    }
    
    // https://github.com/Unity-Technologies/Graphics/blob/9fe6c70ad13301eac50341847ef1fe0463100288/com.unity.render-pipelines.universal/Runtime/2D/PixelPerfectCamera.cs#L222
    private Vector3 RoundToPixel(Vector3 position)
    {
        // https://github.com/Unity-Technologies/Graphics/blob/737a4369e21f754df8e79610019da127e2b4e4c2/com.unity.render-pipelines.universal/Runtime/2D/PixelPerfectCameraInternal.cs#L165
        float unitsPerPixel = 1.0f /  Script_GraphicsManager.AssetsPPU;

        if (unitsPerPixel == 0f)
            return position;
        
        Vector3 result;
        result.x = Mathf.Round(position.x / unitsPerPixel) * unitsPerPixel;
        result.y = Mathf.Round(position.y / unitsPerPixel) * unitsPerPixel;
        result.z = Mathf.Round(position.z / unitsPerPixel) * unitsPerPixel;

        return result;
    }
}
