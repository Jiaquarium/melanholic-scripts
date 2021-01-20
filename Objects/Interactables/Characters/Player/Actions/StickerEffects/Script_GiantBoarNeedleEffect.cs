using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(PlayableDirector))]
public class Script_GiantBoarNeedleEffect : Script_StickerEffect
{
    [SerializeField] private TimelineAsset timeline;
    private PlayableDirector myDirector;

    void Awake()
    {
        myDirector = GetComponent<PlayableDirector>();
    }

    public override void Effect()
    {
        Debug.Log($"{name} Effect()");
        Play();
    }

    void Play()
    {
        myDirector.Play(timeline);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_GiantBoarNeedleEffect))]
public class Script_GiantBoarNeedleEffectTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_GiantBoarNeedleEffect bn = (Script_GiantBoarNeedleEffect)target;
        if (GUILayout.Button("Play Boar Needle Effect"))
        {
            bn.Effect();
        }
    }
}
#endif
