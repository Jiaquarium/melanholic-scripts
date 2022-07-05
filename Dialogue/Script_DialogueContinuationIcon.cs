using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_DialogueContinuationIcon : MonoBehaviour
{
    public Script_DialogueManager dm;
    public Image defaultIcon;
    public Image endIcon;

    
    [SerializeField] private float timer;
    [SerializeField] private bool isOn;
    [SerializeField] private bool isDisabled;
    private bool isLastDialogueSection;
    private bool isInitialFlicker;
    private Image activeImg;

    void Start()
    {
        dm = Script_DialogueManager.DialogueManager;
    }
    
    void Update() {
        if (dm.currentNode.data.children.Length == 0 && dm.dialogueSections.Count == 0)
            isLastDialogueSection = true;
        else    
            isLastDialogueSection = false;
        
        if (
            !dm.isRenderingDialogueSection
            && !dm.isInputMode
            && !dm.noContinuationIcon
            && !isDisabled
            && dm.IsActive()
        )
        {
            if (isLastDialogueSection)  SetImageLastNode();
            else                        SetImageDefaultContinuation();

            HandleFlicker();
            timer += Time.deltaTime;
        }
        else
        {
            TurnOffFlicker();
        }
    }

    void HandleFlicker()
    {        
        // keeping dialogue up, freeze UI animation to react to player command
        if (dm.isKeepingDialogueUp)
        {
            SetImage(true);
            return;
        }
        
        // first on flick only wait half flicker time
        if (isInitialFlicker)
        {
            Flicker();
            isInitialFlicker = false;
        }
        else if (timer >= Script_DialogueManager.DialogueContinuationFlickerInterval)
        {
            Flicker();
        }
    }

    void Flicker()
    {
        if (isOn)   SetImage(false);
        else        SetImage(true);

        timer = 0;
    }

    void TurnOffFlicker()
    {
        if (!isOn && timer == 0)    return;
        InitialOffState();
        isInitialFlicker = true;
    }

    void SetImage(bool _isOn)
    {
        isOn = _isOn;
        activeImg.enabled = isOn;
    }

    void SetImageLastNode()
    {
        activeImg = endIcon;
        defaultIcon.enabled = false;
    }

    void SetImageDefaultContinuation()
    {
        activeImg = defaultIcon;
        endIcon.enabled = false;
    }

    void InitialOffState()
    {
        defaultIcon.enabled = false;
        endIcon.enabled = false;
        isOn = false;
        timer = 0;
        isInitialFlicker = true;
    }

    public void Disable()
    {
        isDisabled = true;
    }
    
    public void Setup()
    {
        InitialOffState();
        isDisabled = false;
    }
}
