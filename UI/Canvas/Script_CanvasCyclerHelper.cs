using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Script_CanvasCyclerHelper : MonoBehaviour
{
    [SerializeField] private float cycleTime = 0.025f;

    [SerializeField] private List<Script_CanvasGroupController> canvasGroups;
    [SerializeField] private UnityEvent onStopEvent;
    [SerializeField] private UnityEvent onEnableEvent;

    private float timer;
    private int idx;

    void OnEnable()
    {
        InitialState();

        onEnableEvent.SafeInvoke();
    }

    void OnDisable()
    {
        onStopEvent.SafeInvoke();
    }
    
    void Update()
    {
        if (timer <= 0f)
        {
            idx++;
            if (idx >= canvasGroups.Count)
                idx = 0;

            CycleCanvas(idx);
            timer = cycleTime;
            return;
        }

        timer -= Time.deltaTime;
        if (timer < 0f)
            timer = 0f;
    }

    public void Start()
    {
        gameObject.SetActive(true);
    }

    public void Stop()
    {
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Show the current idx canvas group by adjusting their alphas
    /// </summary>
    private void CycleCanvas(int targetIdx)
    {
        for (var i = 0; i < canvasGroups.Count; i++)
            canvasGroups[i].Alpha = i == targetIdx ? 1f : 0f;
    }

    private void InitialState()
    {
        // Always start with first canvas showing
        idx = 0;
        timer = cycleTime;
    }
}
