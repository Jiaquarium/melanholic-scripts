using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Script_CameraPixelMovement : MonoBehaviour
{
    [SerializeField] private Vector3 camPosition;
    [SerializeField] private Vector3 screenPoint;
    [SerializeField] private Vector3 pixelRoundedScreenPoint;
    [SerializeField] private Vector3 pixelRoundedWorldPoint;
    [SerializeField] private Vector3 pixelOffset;

    [SerializeField] private Script_GraphicsManager graphicsManager;
    [SerializeField] private Camera cam;
    
    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        camPosition = cam.transform.position;
        
        // convert camera position to screen space
        screenPoint = cam.WorldToScreenPoint(camPosition);
        pixelRoundedScreenPoint = RoundToPixel(screenPoint);
        
        pixelRoundedWorldPoint = cam.ScreenToWorldPoint(pixelRoundedScreenPoint);
        
        pixelOffset = pixelRoundedWorldPoint - camPosition;

        pixelOffset.z = -pixelOffset.z;
        Matrix4x4 offsetMatrix = Matrix4x4.TRS(-pixelOffset, Quaternion.identity, new Vector3(1.0f, 1.0f, -1.0f));
        cam.worldToCameraMatrix = offsetMatrix * cam.transform.worldToLocalMatrix;
    }

    private Vector3 RoundToPixel(Vector3 position)
    {
        float pixelRatio = graphicsManager.PixelRatio;
        
        Vector3 result;
        result.x = Mathf.Round(position.x / pixelRatio) * pixelRatio;
        result.y = Mathf.Round(position.y / pixelRatio) * pixelRatio;
        result.z = Mathf.Round(position.z / pixelRatio) * pixelRatio;

        return result;
    }
}
