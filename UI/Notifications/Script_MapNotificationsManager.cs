using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_MapNotificationsManager : MonoBehaviour
{
    public static Script_MapNotificationsManager Control;
    
    [SerializeField] private Script_MapNotification mapNotification;
    
    [SerializeField] private float duration;
    
    [SerializeField] private Script_Game game;
    
    /// <summary>
    /// NOTE: Ensure this happens before other cut scenes if any in rooms
    /// in Awake or After Init functions.
    /// </summary>
    public void PlayMapNotification(
        string mapName,
        Action cb = null,
        bool isInteractAfter = true
    )
    {
        game.ChangeStateCutScene();

        mapNotification.Open(mapName);

        StartCoroutine(WaitToEndNotification());

        IEnumerator WaitToEndNotification()
        {
            yield return new WaitForSeconds(duration);

            mapNotification.Close(() => {
                if (isInteractAfter)
                    game.ChangeStateInteract();

                if (cb != null) cb();
            });
        }
    }

    public void Setup()
    {
        if (Control == null)
        {
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(this.gameObject);
        }

        mapNotification.Setup();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_MapNotificationsManager))]
public class Script_MapNotificationsManagerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_MapNotificationsManager t = (Script_MapNotificationsManager)target;
        if (GUILayout.Button("Play Map Notification"))
        {
            t.PlayMapNotification("Test Map Notification");
        }
    }
}
#endif