using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_DialogueManager))]
public class Script_DialogueManagerCanvasHandler : MonoBehaviour
{
    public CanvasGroup dialogueCanvasGroup;
    [SerializeField] private Script_CanvasGroupController[] dialogueCanvases;

    private void OnValidate() {
        dialogueCanvases = dialogueCanvasGroup.transform.GetChildren<Script_CanvasGroupController>();
    }

    /// <summary>
    /// Disable all, if only disabling inactive ones, we'll see a flash if
    /// we're changing canvas types
    /// </summary>
    public void DisableCanvases()
    {
        foreach (Script_CanvasGroupController canvasGroup in dialogueCanvases)
            canvasGroup?.Close();
    }
    
    public void DisableInactiveCanvases()
    {
        foreach (Script_CanvasGroupController canvasGroup in dialogueCanvases)
        {
            if (canvasGroup != GetComponent<Script_DialogueManager>().activeCanvas)
                canvasGroup.Close();
        }
    }

    /// <summary>
    /// For use to keep canvases up that have writing on them
    /// </summary>
    public void DisableOnlyEmptyCanvases()
    {
        foreach (Script_CanvasGroupController canvasGroup in dialogueCanvases)
        {
            if (canvasGroup.canvasChild.IsTextEmpty())
                canvasGroup.Close();
        }
    }

    public void DisableInactiveContinuationIcons()
    {
        foreach (Script_CanvasGroupController canvasGroup in dialogueCanvases)
        {
            if (canvasGroup != GetComponent<Script_DialogueManager>().activeCanvas)
                canvasGroup.canvasChild.DisableContinuationIcon();
        }
    }

    public void Setup()
    {
        foreach (Script_CanvasGroupController canvasGroup in dialogueCanvases)
            canvasGroup.canvasChild?.gameObject.SetActive(true);
        
        DisableCanvases();
    }
}
