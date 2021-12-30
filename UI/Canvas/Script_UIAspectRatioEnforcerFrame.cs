using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Adds a frame over the empty black rect space of the camera to cover overflowing UI elements (e.g. Full Art)
/// Resizing in the editor window will freeze these stretching borders, but in a build, the Update Loop
/// still runs, thus upsizing will stretch the borders properly.
/// </summary>
[ExecuteInEditMode]
public class Script_UIAspectRatioEnforcerFrame : MonoBehaviour
{
    [SerializeField] private RectTransform topRect;
    [SerializeField] private RectTransform bottomRect;
    [SerializeField] private RectTransform leftRect;
    [SerializeField] private RectTransform rightRect;
    [SerializeField] private Camera cam;

    private Vector2 cameraRect = Vector2.zero;

    // Editor only
    [SerializeField] private Color devColor = new Color(0f, .47f, 0f);
    [SerializeField] private Color prodColor = new Color(0f, 0f, 0f);
    
    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 _cameraRect = new Vector2(cam.pixelRect.x, cam.pixelRect.y);
        if (_cameraRect != cameraRect)
        {
            MatchBorders();
            cameraRect = _cameraRect;
        }
    }

    private void MatchBorders()
    {
        Vector2 topBorderHeight = new Vector2(0f, cam.pixelRect.y);
        Vector2 sideBorderHeight = new Vector2(cam.pixelRect.x, 0f);

        topRect.sizeDelta = topBorderHeight;
        bottomRect.sizeDelta = topBorderHeight;
        leftRect.sizeDelta = sideBorderHeight;
        rightRect.sizeDelta = sideBorderHeight;

        ForceUpdate();
    }

    private void ChangeBordersColor(bool isProd)
    {
        Image[] borders = GetComponentsInChildren<Image>();
        foreach (var border in borders)
        {
            border.color = isProd ? prodColor : devColor;
        }

        ForceUpdate();
    }

    private void ForceUpdate()
    {
        topRect.ForceUpdateRectTransforms();
        bottomRect.ForceUpdateRectTransforms();
        leftRect.ForceUpdateRectTransforms();
        rightRect.ForceUpdateRectTransforms();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_UIAspectRatioEnforcerFrame))]
    public class Script_UIAspectRatioEnforcerFrameTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_UIAspectRatioEnforcerFrame t = (Script_UIAspectRatioEnforcerFrame)target;
            
            if (GUILayout.Button("Match Borders"))
            {
                t.MatchBorders();
            }

            if (GUILayout.Button("Borders Black"))
            {
                t.ChangeBordersColor(isProd: true);
            }

            if (GUILayout.Button("Borders Dev"))
            {
                t.ChangeBordersColor(isProd: false);
            }
        }
    }
#endif
}
