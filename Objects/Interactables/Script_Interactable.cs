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

    protected bool DisableL {
        get { return _disableL; }
    }

    protected bool DisableR {
        get { return _disableR; }
    }

    protected bool DisableU {
        get { return _disableU; }
    }

    protected bool DisableD {
        get { return _disableD; }
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

            timer = 0f;
            isDialogueCoolDown = false;
        }
    }

    public virtual bool CheckDisabledDirections()
    {
        Directions directionToPlayer = Script_Utils.GetDirectionToTarget(
                                            transform.position,
                                            Script_Game.Game.GetPlayer().transform.position
                                        );
        
        if (
            directionToPlayer == Directions.Left && DisableL
            || directionToPlayer == Directions.Up && DisableU
            || directionToPlayer == Directions.Right && DisableR
            || directionToPlayer == Directions.Down && DisableD
        )
        {
            return true;
        }

        return false;
    }
}
