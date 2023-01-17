using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(PlayableDirector))]
[RequireComponent(typeof(Script_CanvasGroupController))]
public class Script_ScarletCipherLastOnes : MonoBehaviour
{
    const string HiddenSymbol = "?";
    
    // The time to be left up (less than timeline, because timeline is more impactful / will
    // happen before)
    [SerializeField] private float leaveUpTime;
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float afterFadeOutWaitTime;
    
    [SerializeField] private Canvas worldSpaceCanvas;
    [SerializeField] private Script_CanvasGroupController textCanvasGroup;
    
    [Tooltip("Offset to player position")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private Script_Game game;
    [SerializeField] private TextMeshProUGUI textTMP;

    private PlayableDirector playableDirector;
    private Script_CanvasGroupController canvasGroupController;
    
    void OnEnable()
    {
        textCanvasGroup.Close();
        UpdateNumbers(Script_ScarletCipherManager.Control.ScarletCipherPublic);
        TeleportToPlayer();
    }

    /// <summary>
    /// Will update the current TMP string which is a space delimited representation of the currenet cipher.
    /// Hidden values specified by HiddenSymbol.
    /// </summary>
    /// <param name="scarletCipherPublic">The current scarlet cipher with hidden values as -1</param>
    public void UpdateNumbers(int[] scarletCipherPublic)
    {
        string[] numbers = new string[10];

        for (var i = 0; i < scarletCipherPublic.Length; i++)
        {
            string currentSymbol;
            
            if (scarletCipherPublic[i] == -1)
                currentSymbol = HiddenSymbol;
            else
                currentSymbol = scarletCipherPublic[i].ToString();

            numbers[i] = currentSymbol;
        }

        string newText = string.Join(" ", numbers);
        Dev_Logger.Debug($"newText: {newText}");
        
        textTMP.text = newText;
    }
    
    public void PlayLastOnesTimeline()
    {
        canvasGroupController.Open();
        playableDirector.Play();
    }

    // For use when not activating via Timeline
    public void HandleShow(Action actionBeforeClosing)
    {
        canvasGroupController.Open();

        textCanvasGroup.FadeIn(fadeInTime, () => {
            StartCoroutine(WaitToFadeOut());
        });

        IEnumerator WaitToFadeOut()
        {
            yield return new WaitForSeconds(leaveUpTime);

            textCanvasGroup.FadeOut(fadeOutTime, () => {
                if (actionBeforeClosing == null)
                {
                    InitialState();
                    game.ChangeStateInteract();
                }
                else
                    StartCoroutine(WaitToCallback());
            });
        }

        IEnumerator WaitToCallback()
        {
            yield return new WaitForSeconds(afterFadeOutWaitTime);
            
            // Note: must complete everything syncronously after calling InitialState since
            // will deactivate this gameobject and further coroutines won't be played then
            InitialState();
            actionBeforeClosing();
        }
    }

    private void TeleportToPlayer()
    {
        var playerPosition = game.GetPlayer().transform.position;

        transform.position = Vector3.zero;
        worldSpaceCanvas.transform.position = playerPosition + offset;
    }

    // ------------------------------------------------------------------
    // Timeline Signals

    // Last Ones Timeline
    public void OnLastOnesTimelineDone()
    {
        InitialState();
        game.ChangeStateInteract();
    }

    // ------------------------------------------------------------------

    private void InitialState()
    {
        canvasGroupController.Close();
        textCanvasGroup.Close();
    }
    
    public void Setup()
    {
        playableDirector = GetComponent<PlayableDirector>();
        canvasGroupController = GetComponent<Script_CanvasGroupController>();
        InitialState();
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_ScarletCipherLastOnes))]
    public class Script_ScarletCipherLastOnesTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_ScarletCipherLastOnes t = (Script_ScarletCipherLastOnes)target;
            if (GUILayout.Button("Play LastOnesTimeline"))
            {
                t.PlayLastOnesTimeline();
            }
        }
    }
    #endif
}
