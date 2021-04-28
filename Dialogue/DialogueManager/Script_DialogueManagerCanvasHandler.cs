using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_DialogueManager))]
public class Script_DialogueManagerCanvasHandler : MonoBehaviour
{
    public CanvasGroup dialogueCanvasGroup;
    public Canvas[] dialogueCanvases;

    private void OnValidate() {
        dialogueCanvases = dialogueCanvasGroup.transform.GetChildren<Canvas>();
    }

    /// <summary>
    /// Disable all, if only disabling inactive ones, we'll see a flash if
    /// we're changing canvas types
    /// </summary>
    public void DisableCanvases()
    {
        foreach (Canvas canvas in dialogueCanvases) canvas?.gameObject.SetActive(false);
    }
    public void DisableInactiveCanvases()
    {
        foreach (Canvas canvas in dialogueCanvases)
        {
            if (canvas != GetComponent<Script_DialogueManager>().activeCanvas)
                canvas.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// For use to keep canvases up that have writing on them
    /// </summary>
    public void DisableOnlyEmptyCanvases()
    {
        foreach (Canvas canvas in dialogueCanvases)
        {
            if (canvas.GetComponent<Script_Canvas>().IsTextEmpty())
                canvas.gameObject.SetActive(false);
        }
    }

    public void DisableInactiveContinuationIcons()
    {
        foreach (Canvas canvas in dialogueCanvases)
        {
            if (canvas != GetComponent<Script_DialogueManager>().activeCanvas)
            {
                if (canvas.GetComponent<Script_Canvas>().ContinuationIcon != null)
                    canvas.GetComponent<Script_Canvas>().ContinuationIcon.Disable();
            }
        }
    }
}
