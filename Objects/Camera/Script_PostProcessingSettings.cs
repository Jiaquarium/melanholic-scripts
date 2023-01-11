using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_PostProcessingSettings : MonoBehaviour
{
    private Volume volume;

    void Awake()
    {
        volume = GetComponent<Volume>();        
    }
    
    public void Vignette(bool isActive)
    {
        Vignette vignette;
        volume.profile.TryGet(out vignette);
        
        if (vignette != null)
            vignette.active = isActive;
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_PostProcessingSettings))]
    public class Script_PostProcessingSettingsTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_PostProcessingSettings t = (Script_PostProcessingSettings)target;
            if (GUILayout.Button("Vignette On"))
            {
                t.Vignette(true);
            }

            if (GUILayout.Button("Vignette Off"))
            {
                t.Vignette(false);
            }
        }
    }
    #endif
}