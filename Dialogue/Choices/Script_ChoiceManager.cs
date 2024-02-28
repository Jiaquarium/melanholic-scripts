using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_ChoiceManager : MonoBehaviour
{
    public CanvasGroup choiceCanvasTop;
    public Script_DialogueChoice[] choicesTop;
    public CanvasGroup choiceCanvasBottom;
    public Script_DialogueChoice[] choicesBottom;
    public CanvasGroup activeCanvas;
    public Script_DialogueChoice[] activeChoices;

    
    private Script_DialogueManager dialogueManager;

    public void StartChoiceMode(Script_DialogueNode node)
    {
        if (node.data.locationType == "top")
        {
            activeCanvas = choiceCanvasTop;
            activeChoices = choicesTop;
        }
        else
        {
            activeCanvas = choiceCanvasBottom;
            activeChoices = choicesBottom;
        }

        // to get rid of flash at beginning and hide choice buttons
        foreach(Script_DialogueChoice choice in activeChoices)
        {
            choice.cursor.enabled = false;
            choice.gameObject.SetActive(false);
        }

        for (int i = 0; i < node.data.children.Length; i++)
        {
            activeChoices[i].Id = i;
            TextMeshProUGUI text = Script_Utils.FindComponentInChildWithTag<TextMeshProUGUI>(
                activeChoices[i].gameObject,
                Const_Tags.DialogueChoice
            );
            Script_DialogueNode childNode = node.data.children[i];

            // Ensure choice text is updated for language change
            childNode.Refresh();

            string unformattedText = childNode.data.choiceText;
            text.text = Script_Utils.FormatString(unformattedText);

            // show choice buttons with data
            activeChoices[i].gameObject.SetActive(true);
        }

        activeCanvas.gameObject.SetActive(true);
    }

    // Must wait for next frame or could reinteract with interactable (e.g. interactable text).
    public void InputChoice(int Id)
    {
        StartCoroutine(WaitEndChoices());

        IEnumerator WaitEndChoices()
        {
            yield return null;
            
            EndChoiceMode();

            if (!dialogueManager.isInputDisabled)
            {
                Dev_Logger.Debug("Submit UI Choice");
                Script_SFXManager.SFX.PlayUIChoiceSubmit();
            }

            dialogueManager.NextDialogueNode(Id);
        }
    }

    void EndChoiceMode()
    {
        activeCanvas.gameObject.SetActive(false);
    }

    public void Setup()
    {
        choiceCanvasTop.gameObject.SetActive(false);
        choiceCanvasBottom.gameObject.SetActive(false);
        dialogueManager = GetComponent<Script_DialogueManager>();
    }
}
