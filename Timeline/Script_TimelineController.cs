using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_TimelineController : MonoBehaviour
{
    public List <PlayableDirector> playableDirectors;
    public List <TimelineAsset> timelines;

    /// <summary>
    /// used when the gameObject is the playerDirector 
    /// </summary>
    public void PlayableDirectorPlay(int i)
    {
        playableDirectors[i].Play();
    }

    public void PlayAllPlayables()
    {
        foreach (PlayableDirector playable in playableDirectors)
        {
            playable.Play();
        }
    }

    public void StopAllPlayables()
    {
        foreach (PlayableDirector playable in playableDirectors)
        {
            if (playable == null)
                return;

            if (playable.state == PlayState.Playing)
            {
                Dev_Logger.Debug($"playable {playable} is playing, stopping now.");
                playable.time = 0f;
                playable.Stop();
            }
        }
    }

    /// <summary>
    /// used when we have multiple timelines we want to manage
    /// e.g. KTVApproach, KTV Exit
    /// </summary>
    public PlayableDirector PlayableDirectorPlayFromTimelines(int playableDirectorIdx, int timelineIdx)
    {
        TimelineAsset selectedAsset;

        if (timelines.Count <= timelineIdx)     selectedAsset = timelines[timelines.Count - 1];
        else                                    selectedAsset = timelines[timelineIdx];

        Dev_Logger.Debug("playing asset: " + selectedAsset);
        playableDirectors[playableDirectorIdx].Play(selectedAsset);

        return playableDirectors[playableDirectorIdx];
    }

    public void BindVirtualCameraAndPlayFromDirector(int i, int j, CinemachineVirtualCamera virtualCamera)
    {
        if (playableDirectors.Count <= i || playableDirectors[i] == null)
        {
            Debug.LogWarning($"{name} playableDirector at current idx {i} is null");
            return;
        }
        
        if (timelines.Count <= j || timelines[j] == null)
        {
            Debug.LogWarning($"{name} timeline at current idx {j} is null");
            return;
        }
        
        PlayableDirector director = playableDirectors[i];
        TimelineAsset timeline = timelines[j];

        foreach (var track in timeline.GetOutputTracks())
        {
            var clips = track.GetClips();

            foreach (TimelineClip clip in clips)
            {
                var shot = clip.asset as CinemachineShot;
                
                if (shot == null)
                    continue;
                
                director.SetReferenceValue(shot.VirtualCamera.exposedName, virtualCamera);
            }
        }
        
        PlayableDirectorPlayFromTimelines(i, j);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_TimelineController))]
public class Script_TimelineControllerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_TimelineController timelineController = (Script_TimelineController)target;
        if (GUILayout.Button("Test Binding"))
        {
            // timelineController.BindTimelineTracks(timelineController.timelines[1]);
        }
    }
}
#endif

