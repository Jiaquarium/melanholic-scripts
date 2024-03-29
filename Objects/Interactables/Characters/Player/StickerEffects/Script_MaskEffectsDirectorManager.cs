using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_MaskEffectsDirectorManager : MonoBehaviour
{
    public enum Effects
    {
        MyMask = 8
    }
    
    public static Script_MaskEffectsDirectorManager Instance;

    [SerializeField] private Script_TimelineController timelineController;
    [SerializeField] private Script_Game game;
    
    [Header("My Mask Timeline")]
    [SerializeField] private Script_LevelBehavior_48 GrandMirror;
    [SerializeField] private List<GameObject> myMaskInitialBindings;
    [SerializeField] private bool isMyMaskMutationOff;
    [SerializeField] private float myMaskBgmFadeOutTime;
    [SerializeField] private float myMaskBgmFadeInTime;
    [SerializeField] private Vector3 screenShakeVals;
    [SerializeField] private Script_CameraShake activeZoomCamera;

    private Coroutine extraBG2Coroutine;

    public bool IsMyMaskMutationOff
    {
        get => isMyMaskMutationOff;
        set => isMyMaskMutationOff = value;
    }

    public void PlayMyMaskEffect(List<GameObject> bindings)
    {
        int myMaskIdx = (int)Effects.MyMask;
        int directorIdx = 0;

        TimelineAsset myMaskTimeline = timelineController.timelines[myMaskIdx];
        PlayableDirector director = timelineController.playableDirectors[directorIdx];
        
        var myMaskBindings = new List<GameObject>();

        // Leave initial bindings as null to retain Timeline bindings
        myMaskBindings.AddRange(myMaskInitialBindings);
        myMaskBindings.AddRange(bindings);
        
        director.BindTimelineTracks(myMaskTimeline, myMaskBindings, true);
        timelineController.PlayableDirectorPlayFromTimelines(directorIdx, myMaskIdx);
    }

    public void StopForceSheepFaceDirection()
    {
        Script_StickerEffectEventsManager.MyMaskStopFaceDir();
    }

    // ------------------------------------------------------------------
    // Timeline Signals

    public void MyMaskEquipEffectTimeline()
    {
        game.GetPlayer().MyMaskEquipEffectTimeline();
        GrandMirror.SetPlayerMutationActive(false);
    }

    public void MyMaskFraming()
    {
        // Framing (Thin because ConstantDefault hides too much of scene)
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: true,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
            isNoAnimation: false
        );
    }
    
    // Mask Effect timeline: very beginning
    public void MyMaskFadeOutBgm()
    {
        GrandMirror.StopSheepBleats(Directions.None);
        
        var bgm = Script_BackgroundMusicManager.Control;

        bgm.FadeOut(null, myMaskBgmFadeOutTime, Const_AudioMixerParams.ExposedBGVolume);
        bgm.FadeOutExtra(out extraBG2Coroutine, null, myMaskBgmFadeOutTime, Const_AudioMixerParams.ExposedBG2Volume);
    }

    public void StartScreenShake()
    {
        activeZoomCamera.Shake(screenShakeVals.x, screenShakeVals.y, screenShakeVals.z, null);
    }

    // MyMask Effect Timeline on white screen up. Also preemptively return all Ids back to default.
    public void StopScreenShake()
    {
        activeZoomCamera.InitialState();

        Script_NPCEventsManager.NPCRandomAnimatorForceDefault();
    }

    // Mask Effect timeline: towards the end
    public void MyMaskFadeInBgm()
    {
        var bgm = Script_BackgroundMusicManager.Control;
        
        bgm.Stop();
        bgm.Play(bgm.CurrentClipIndex, true, bgm.myMaskDoneSongStartTime);
        
        bgm.FadeIn(null, myMaskBgmFadeInTime, Const_AudioMixerParams.ExposedBGVolume);
        bgm.FadeInExtra(out extraBG2Coroutine, null, myMaskBgmFadeInTime, Const_AudioMixerParams.ExposedBG2Volume);
    }

    public void ForceSheepFaceDirection()
    {
        Script_StickerEffectEventsManager.MyMaskForceFaceDir(Directions.Up);
        Script_NPCEventsManager.NPCRandomAnimatorForceDefault();
        game.GetPlayer().FaceDirection(Directions.Up);
    }

    public void MyMaskEffectTimelineDone()
    {
        // Remove framing from PlayMyMaskEffect
        Script_UIAspectRatioEnforcerFrame.Control.EndingsLetterBox(
            isOpen: false,
            framing: Script_UIAspectRatioEnforcerFrame.Framing.ConstantThin,
            cb: game.ChangeStateInteract
        );
    }

    // ------------------------------------------------------------------
    
    public void Setup()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_MaskEffectsDirectorManager))]
    public class Script_MaskEffectsDirectorManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_MaskEffectsDirectorManager t = (Script_MaskEffectsDirectorManager)target;
            if (GUILayout.Button("Equip Effect Timeline"))
            {
                t.MyMaskEquipEffectTimeline();
            }

            if (GUILayout.Button("Force Sheep Face Direction"))
            {
                t.ForceSheepFaceDirection();
            }

            if (GUILayout.Button("Stop Sheep Face Direction"))
            {
                t.StopForceSheepFaceDirection();
            }
        }
    }
    #endif
}
