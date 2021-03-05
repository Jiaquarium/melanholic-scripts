using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class to trigger collisions
/// </summary>
public class Script_Interactable : MonoBehaviour
{
    private static float dialogueCoolDownTime = 0.6f;
    [SerializeField] protected bool isDialogueCoolDown = false;
    private float timer;
    
    [SerializeField] private bool _disableL;
    [SerializeField] private bool _disableR;
    [SerializeField] private bool _disableU;
    [SerializeField] private bool _disableD;
    [SerializeField] private Script_DisablerController disablerController;

    [SerializeField] private Script_InteractableBox[] extraInteractableBoxes;

    protected bool DisableL {
        get => _disableL;
    }

    protected bool DisableR {
        get => _disableR;
    }

    protected bool DisableU {
        get => _disableU;
    }

    protected bool DisableD {
        get => _disableD;
    }

    protected virtual void OnDisable()
    {
        DialogueCoolDownReset();
    }

    protected virtual void Awake()
    {
        
    }

    public virtual void ForcePush(Directions dir)
    {
        this.transform.position += dir.DirectionToVector();
    }
    
    /// <summary>
    /// Parent classes reference isDialogueCoolDown in ActionDefault() 
    /// to cool down after a dialogue end
    /// 
    /// This must be in a coroutine since Update() is frequently overriden
    /// </summary>
    public void StartDialogueCoolDown()
    {
        isDialogueCoolDown = true;
        timer = dialogueCoolDownTime;
        StartCoroutine(DialogueCoolDownCo());

        IEnumerator DialogueCoolDownCo()
        {
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            DialogueCoolDownReset();
        }
    }

    public bool CheckDisabledDirections()
    {
        if (disablerController != null) return GetDisablerControllerPlayerCollision();
        else                            return GetDisabledDirectionVector();        

        bool GetDisablerControllerPlayerCollision()
        {
            bool isPlayerInLeftBox = disablerController.GetPlayerInBox(Directions.Left);
            bool isPlayerInRightBox = disablerController.GetPlayerInBox(Directions.Right);
            bool isPlayerInUpBox = disablerController.GetPlayerInBox(Directions.Up);
            bool isPlayerInDownBox = disablerController.GetPlayerInBox(Directions.Down);
            
            Debug.Log($"Left {isPlayerInLeftBox}");
            Debug.Log($"Right {isPlayerInRightBox}");
            Debug.Log($"Up {isPlayerInUpBox}");
            Debug.Log($"Down {isPlayerInDownBox}");
            
            return (
                (isPlayerInLeftBox && DisableL)
                || (isPlayerInRightBox&& DisableR)
                || (isPlayerInUpBox && DisableU)
                || (isPlayerInDownBox&& DisableD)
            );
        }

        bool GetDisabledDirectionVector()
        {
            Directions directionToPlayer = Script_Utils.GetDirectionToTarget(
                                                transform.position,
                                                Script_Game.Game.GetPlayer().transform.position
                                            );
            
            return (
                directionToPlayer == Directions.Left && DisableL
                || directionToPlayer == Directions.Up && DisableU
                || directionToPlayer == Directions.Right && DisableR
                || directionToPlayer == Directions.Down && DisableD
            );
        }
    }

    // Useful if we need to change the size of interactable area (e.g. after a dialogue).
    public void RemoveExtraInteractableBoxes()
    {
        foreach (var box in extraInteractableBoxes)
        {
            box.gameObject.SetActive(false);
        }
    }

    protected virtual bool CheckDisabled()
    {
        return CheckDisabledDirections() || isDialogueCoolDown;
    }

    private void DialogueCoolDownReset()
    {
        timer = 0f;
        isDialogueCoolDown = false;
    }
}
