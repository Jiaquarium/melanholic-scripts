using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_CameraCrop : MonoBehaviour {

    // Set this to your target aspect ratio, eg. (16, 9) or (4, 3).
    public Vector2 targetAspect = new Vector2(16, 9);
    [SerializeField] private Camera _camera;

    void OnValidate()
    {
    }
    
    void Start()
    {
    }

    // Call this method if your window size or target aspect change.
    public void Crop()
    {
        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_CameraCrop))]
public class Script_CameraCropTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_CameraCrop t = (Script_CameraCrop)target;
        if (GUILayout.Button("Crop Camera"))
        {
            t.Crop();
        }
    }
}
#endif