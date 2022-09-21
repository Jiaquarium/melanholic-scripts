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
    
    [Tooltip("Define explicit colliders to disable player interaction. Default behavior checks Player's facing direction.")]
    [SerializeField] private Script_DisablerController disablerController;

    [SerializeField] private Script_InteractableBox[] extraInteractableBoxes;
    
    [Tooltip("Explicit colliders to hide and/or reveal.")]
    [SerializeField] private Collider[] targetColliders;

    public bool DisableL {
        get => _disableL;
        set => _disableL = value;
    }

    public bool DisableR {
        get => _disableR;
        set => _disableR = value;
    }

    public bool DisableU {
        get => _disableU;
        set => _disableU = value;
    }

    public bool DisableD {
        get => _disableD;
        set => _disableD = value;
    }

    protected virtual void OnDisable()
    {
        DialogueCoolDownReset();
    }

    protected virtual void OnValidate() { }
    
    protected virtual void Awake() { }

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
            
            Dev_Logger.Debug($"Left {isPlayerInLeftBox}");
            Dev_Logger.Debug($"Right {isPlayerInRightBox}");
            Dev_Logger.Debug($"Up {isPlayerInUpBox}");
            Dev_Logger.Debug($"Down {isPlayerInDownBox}");
            
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
                Script_Game.Game.GetPlayer().FacingDirection == Directions.Right && DisableL
                || Script_Game.Game.GetPlayer().FacingDirection == Directions.Down && DisableU
                || Script_Game.Game.GetPlayer().FacingDirection == Directions.Left && DisableR
                || Script_Game.Game.GetPlayer().FacingDirection == Directions.Up && DisableD
            );
        }
    }

    // Useful if we need to change the size of interactable area (e.g. after a dialogue).
    public void SetExtraInteractableBoxes(bool isActive)
    {
        foreach (var box in extraInteractableBoxes)
        {
            box.gameObject.SetActive(isActive);
        }
    }

    public void SetTargetColliders(bool isActive)
    {
        foreach (var collider in targetColliders)
            collider.gameObject.SetActive(isActive);
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
