using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Collider))]
public class Script_BoundingVolume : MonoBehaviour
{
    private static float OrthoUpperBound = 9f;
    
    [SerializeField] private bool confineScreenEdges;
    
    [SerializeField] private Script_GraphicsManager graphicsManager;
    [SerializeField] private Camera mainCamera;
    
    private Vector3 defaultScale;

    public bool ConfineScreenEdges
    {
        get => confineScreenEdges;
    }

    public Collider BoundingVolumeCollider
    {
        get => GetComponent<Collider>();
    }
    
    void Awake()
    {
        defaultScale = transform.localScale;
    }
    
    void Start()
    {
        HideRenderer();
    }

    private void HideRenderer()
    {
        var r = GetComponent<Renderer>();

        if (r != null)
            r.enabled = false;
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

