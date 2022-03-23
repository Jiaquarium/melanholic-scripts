using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_BoundingVolume : MonoBehaviour
{
    private static float OrthoUpperBound = 9f;

    [Tooltip("Use to decrease size of Bounding Volume by a larger rate")]
    [SerializeField] private float scaleMultiplier = 1f;
    
    [SerializeField] private Script_GraphicsManager graphicsManager;
    [SerializeField] private Camera mainCamera;
    
    private Vector3 defaultScale;
    
    void Awake()
    {
        defaultScale = transform.localScale;
    }
    
    void Start()
    {
        HideRenderer();
    }

    void Update()
    {
        Resize();
    }

    private void HideRenderer()
    {
        var r = GetComponent<Renderer>();

        if (r != null)
            r.enabled = false;
    } 

    /// <summary>
    /// Resize the Bounding Volume based on the current Default Ortho Size with the minimum ortho.
    /// The larger Default Ortho Size the more the screen is showing and thus we can decrease the
    /// scale of our Bounding Volume.
    /// </summary>
    private void Resize()
    {
        // In case of unexpected ortho sizes, ignore resizing.
        if (
            mainCamera.orthographicSize > OrthoUpperBound
            || mainCamera.orthographicSize < graphicsManager.MinOrthoSizeCameraConfinement
        )
        {
            transform.localScale = defaultScale;
            return;
        }
        
        // Find ratio of DefaultOrthoSize compared with current
        var scaleFactor = mainCamera.orthographicSize / graphicsManager.MinOrthoSizeCameraConfinement;
        transform.localScale = defaultScale / (scaleFactor * scaleMultiplier);
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(Script_BoundingVolume))]
    public class Script_BoundingVolumeTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_BoundingVolume t = target as Script_BoundingVolume;
            if (GUILayout.Button("Invalidate Confiner Cache"))
            {
                Script_VCamManager.VCamMain.VCamera.InvalidateConfinerCache();
            }
        }
    }
#endif
}

