using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_MeshLightAdjustController : MonoBehaviour
{
    private static float throttleTimer = 0.5f;
    
    [SerializeField] private Vector2 alphaRange;
    [SerializeField] private string alphaPropRefName;
    [SerializeField] private Renderer rend;

    [SerializeField] private Script_LightFXManager lightFXManager;

    private float timer;
    
    void OnEnable()
    {
        timer = throttleTimer;
    }
    
    void Update()
    {
        if (timer <= 0f)
        {
            float aDelta = alphaRange.y - alphaRange.x;
            float currentAlpha = alphaRange.x + (lightFXManager.LightCurvePercent * aDelta);

            SetMaterialAlpha(currentAlpha);
            timer = throttleTimer;
        }
        else
        {
            timer -= Time.deltaTime;
            
            if (timer < 0f)
                timer = 0f;
        }
    }
    
    // Note: this modifies just the material instance
    private void SetMaterialAlpha(float a)
    {
        rend.material.SetFloat(alphaPropRefName, a);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_MeshLightAdjustController))]
    public class Script_MeshLightAdjustControllerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_MeshLightAdjustController t = (Script_MeshLightAdjustController)target;
            if (GUILayout.Button("Set Alpha 0.5f"))
            {
                t.SetMaterialAlpha(0.5f);
            }
        }
    }
#endif
}
