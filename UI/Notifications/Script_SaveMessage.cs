using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_SaveMessage : MonoBehaviour
{
    [SerializeField] private GameObject progressContainer;
    [SerializeField] private GameObject doneContainer;

    void OnEnable()
    {
        InitialState();
    }

    public void Done()
    {
        progressContainer.gameObject.SetActive(false);
        doneContainer.gameObject.SetActive(true);
    }

    private void InitialState()
    {
        progressContainer.gameObject.SetActive(true);
        doneContainer.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_SaveMessage))]
    public class Script_SaveMessageTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_SaveMessage t = (Script_SaveMessage)target;
            if (GUILayout.Button("Initial State"))
            {
                t.InitialState();
            }

            if (GUILayout.Button("Done State"))
            {
                t.Done();
            }
        }
    }
#endif
}
