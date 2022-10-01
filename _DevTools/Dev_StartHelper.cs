using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Dev_StartHelper : MonoBehaviour
{
    [SerializeField] Script_CanvasGroupController fader;
    [SerializeField] Script_CanvasGroupController bgBlack;
    [SerializeField] Script_CanvasGroupController initFader;
    [SerializeField] Script_UIAspectRatioEnforcerFrame UIAspectRatioEnforcerFrame;
    
    private void BuildSetup()
    {
        fader.gameObject.SetActive(false);
        bgBlack.gameObject.SetActive(false);
        initFader.gameObject.SetActive(false);
        UIAspectRatioEnforcerFrame.gameObject.SetActive(true);
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Dev_StartHelper))]
    public class Dev_StartHelperTester : Editor
    {
        private static bool showBuildSettings;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Dev_StartHelper t = target as Dev_StartHelper;

            var style = EditorStyles.foldoutHeader;

            showBuildSettings = EditorGUILayout.Foldout(showBuildSettings, "Build Settings", style);
            if (showBuildSettings)
            {
                if (GUILayout.Button("Build Setup", GUILayout.Height(32)))
                {
                    t.BuildSetup();

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(t);
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                    }
                }
            }
        }
    }
    #endif
}
